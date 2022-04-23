using ManiaStressEvaluator.Graph.Evaluators;
using ManiaStressEvaluator.Interpreter;
using Raylib_cs;

namespace ManiaStressEvaluator.Graph;

public static class Program
{
    public const int CanvasWidth = 2400;
    public const int CanvasHeight = 760;
    
    public static void Main(string?[] args)
    {
        Raylib.InitWindow(CanvasWidth, CanvasHeight, "Graph");

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
        
        var rawChart = File.ReadAllText(args[0] ?? throw new InvalidOperationException());
        var chart = OsuManiaInterpreter.Parse(rawChart);
        
        Console.WriteLine("Press any key to start");
        Console.ReadKey();
        
        var evaluator = new RealtimeEvaluator(chart);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            
            Raylib.ClearBackground(Color.BLACK);
            
            evaluator.Update();
            
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}


