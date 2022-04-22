namespace ManiaStressEvaluator.Interpreter.Data.Models.Muscles;

public abstract class MuscleBase
{
    protected float LastHitTime;
    public float Stress;

    public abstract float GetAccumulativeStress();
}