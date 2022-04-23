using ManiaStressEvaluator.Client.Evaluators;
using ManiaStressEvaluator.Interpreter;

namespace ManiaStressEvaluator.Client;

public static class Program
{
    public static async Task Main(string?[] args)
    {
        if (args.Length < 1)
        {
            var osuFile = new DirectoryInfo("./").GetFiles().FirstOrDefault(f => f.Extension == ".osu");
            
            if (osuFile != null)
                args = new[] { osuFile.FullName };
            else while (args.Length < 1)
            {
                Console.WriteLine("Please enter the path to the beatmap. (.osu)");
                args = new[] { Console.ReadLine() };
            }
        }
        
        Console.WriteLine("Press any key to start");
        Console.ReadKey();

        var rawChart = await File.ReadAllTextAsync(args[0] ?? throw new InvalidOperationException());
        var chart = OsuManiaInterpreter.Parse(rawChart);


        await new RealtimeEvaluator().Evaluate(chart);

        Console.ReadKey();
    }
}