namespace ManiaStressEvaluator.Interpreter.Data.Models.Muscles;

public class Arm : MuscleBase
{
    public Wrist Wrist;
    
    public Arm()
    {
        Wrist = new Wrist();
    }
}