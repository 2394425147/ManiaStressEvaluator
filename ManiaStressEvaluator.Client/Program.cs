using ManiaStressEvaluator.Client.Evaluators;
using ManiaStressEvaluator.Interpreter;

namespace ManiaStressEvaluator.Client;

public static class Program
{
    public static async Task Main(string?[] args)
    {
        while (args.Length < 1)
        {
            Console.WriteLine("Please enter the path to the beatmap. (.osu)");
            args = new[] { Console.ReadLine() };
        }

        var rawChart = await File.ReadAllTextAsync(args[0] ?? throw new InvalidOperationException());
        var chart = OsuManiaInterpreter.Parse(rawChart);


        await new RealtimeEvaluator().Evaluate(chart);

        Console.ReadKey();
    }
}