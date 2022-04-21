using ManiaStressEvaluator.Interpreter.Data.Beatmap;

namespace ManiaStressEvaluator.Client.Evaluators;

public interface IEvaluator
{
    public Task Evaluate(Chart chart);
}