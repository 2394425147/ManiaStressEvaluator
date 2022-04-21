using ManiaStressEvaluator.Interpreter.Data.Beatmap;
using ManiaStressEvaluator.Interpreter.Data.Models.Muscles;

namespace ManiaStressEvaluator.Client.Evaluators;

public class RealtimeEvaluator : IEvaluator
{
    public async Task Evaluate(Chart chart)
    {
        var startTime = DateTime.Now;
        var noteCopy = chart.Notes.OrderBy(n => n.Time).ToList();

        var player = new Human();
        var notesHit = 0;
        var totalStress = 0f;

        while (noteCopy.Count > 0)
        {
            #region Update

            var currentTime = DateTime.Now;
            var timeSinceStart = (currentTime - startTime).TotalMilliseconds / 1000f;

            Console.Title = $"{timeSinceStart:0.00}";

            foreach (var note in noteCopy.Where(n => n.Time < timeSinceStart).ToList())
            {
                notesHit++;
                
                player.Arms[note.Lane / 2].Wrist.Fingers[note.Lane % 2].React(note);

                var averageStress = (player.Arms[0].Wrist.Fingers[0].Stress +
                                   player.Arms[0].Wrist.Fingers[1].Stress +
                                   player.Arms[1].Wrist.Fingers[0].Stress +
                                   player.Arms[1].Wrist.Fingers[1].Stress) / 4f;
                
                totalStress += averageStress * 3.5f;
                
                Console.ForegroundColor = averageStress switch
                {
                    > 1.9f => ConsoleColor.Red,
                    > 1.4f => ConsoleColor.Yellow,
                    > 0.8f => ConsoleColor.White,
                    _ => ConsoleColor.Blue,
                };
                
                Console.Write($"{player.Arms[0].Wrist.Fingers[0].Stress:00.00}\t" +
                              $"{player.Arms[0].Wrist.Fingers[1].Stress:00.00}\t" +
                              $"{player.Arms[1].Wrist.Fingers[0].Stress:00.00}\t" +
                              $"{player.Arms[1].Wrist.Fingers[1].Stress:00.00}\t||\t" +
                              $"AVG: {averageStress * 3.5f:0.00}*\tTTL: {totalStress / notesHit * 1.5f:0.00}*\t||\t");


                Console.WriteLine(note.isHold
                    ? $"{note.Time:0.000}\t[{note.Length:0.00}]"
                    : $"{note.Time}");

                noteCopy.Remove(note);
            }

            #endregion

            await Task.Delay(new TimeSpan(60));
        }
    }
}