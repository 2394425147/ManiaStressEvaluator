using ManiaStressEvaluator.Interpreter.Data.Beatmap;
using ManiaStressEvaluator.Interpreter.Data.Models.Muscles;

namespace ManiaStressEvaluator.Client.Evaluators;

public class AheadOfTimeEvaluator : IEvaluator
{
    public Task Evaluate(Chart chart)
    {
        var noteCopy = chart.Notes.OrderBy(n => n.Time).ToList();

        var player = new Human();
        var notesHit = 0;
        var totalStress = 0f;

        foreach (var note in noteCopy)
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
        }

        return Task.CompletedTask;
    }
}