using ManiaStressEvaluator.Interpreter.Data.Beatmap;
using ManiaStressEvaluator.Interpreter.Data.Models.Muscles;

namespace ManiaStressEvaluator.Client.Evaluators;

public class AheadOfTimeEvaluator : IEvaluator
{
    public Task Evaluate(Chart chart)
    {
        var noteCopy = chart.AsNoteCollections();

        var player = new Human();
        var notesHit = 0;
        var totalStress = 0f;

        foreach (var noteCollection in noteCopy)
        {
            notesHit++;

            player.React(noteCollection);

            var averageStress = player.GetAccumulativeStress();

            totalStress += averageStress;

            Console.ForegroundColor = averageStress switch
            {
                > 5.3f => ConsoleColor.Red,
                > 3.3f => ConsoleColor.Yellow,
                > 1.7f => ConsoleColor.White,
                _ => ConsoleColor.Blue
            };

            Console.WriteLine($"BDY: {player.Stress:0.0}\t|\t" +
                              $"RM0: {player.Arms[0].Stress:0.0}\t" +
                              $"RST: {player.Arms[0].Wrist.Stress:0.0}\t" +
                              $"FGR: {player.Arms[0].Wrist.Fingers[0].Stress:0.0} " +
                              $"{player.Arms[0].Wrist.Fingers[1].Stress:0.0}\t|\t" +
                              $"RM1: {player.Arms[1].Stress:0.0}\t" +
                              $"RST: {player.Arms[1].Wrist.Stress:0.0}\t" +
                              $"FGR: {player.Arms[1].Wrist.Fingers[0].Stress:0.0} " +
                              $"{player.Arms[1].Wrist.Fingers[1].Stress:0.0}\t||\t" +
                              $"FRM: {averageStress:0.00}*\tAVG: {totalStress / notesHit * 1.7f:0.000}*");
        }

        return Task.CompletedTask;
    }
}