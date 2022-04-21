namespace ManiaStressEvaluator.Interpreter.Data.Models.Muscles;

public class Human : MuscleBase
{
    public readonly Arm[] Arms = new Arm[2];

    public Human()
    {
        Arms[0] = new Arm();
        Arms[1] = new Arm();
    }
}