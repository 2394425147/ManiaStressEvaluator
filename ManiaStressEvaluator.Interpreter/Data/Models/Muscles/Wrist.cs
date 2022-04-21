namespace ManiaStressEvaluator.Interpreter.Data.Models.Muscles;

public class Wrist : MuscleBase
{
    public readonly List<Finger> Fingers = new();
    
    public Wrist()
    {
        Fingers.Add(new Finger(this));
        Fingers.Add(new Finger(this));
    }
}