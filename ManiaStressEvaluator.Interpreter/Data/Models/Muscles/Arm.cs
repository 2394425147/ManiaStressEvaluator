using ManiaStressEvaluator.Interpreter.Data.Beatmap;

namespace ManiaStressEvaluator.Interpreter.Data.Models.Muscles;

public class Arm : MuscleBase
{
    public readonly int Side;
    public readonly Wrist Wrist;

    private List<Note> _lastNotes = new();

    public Arm(int i)
    {
        Wrist = new Wrist(this);
        Side = i;
    }

    public override float GetAccumulativeStress()
    {
        return Stress + Wrist.GetAccumulativeStress();
    }

    public void React(List<Note> notes)
    {
        Relax(notes);

        if (MustPerformJack(notes)) Stress += 0.5f / (notes[0].Time - _lastNotes[0].Time);

        Wrist.React(notes);

        _lastNotes = notes;
    }

    private void Relax(List<Note> notes)
    {
        const float reliefMultiplier = 20f;
        Stress = MathF.Max(0,
            Stress - reliefMultiplier * MathF.Pow(notes[0].Time - LastHitTime, 1.5f));
    }


    private bool MustPerformJack(IEnumerable<Note> notes)
    {
        return _lastNotes.Count >= 1 &&
               _lastNotes.Select(n => n.Lane).Intersect(notes.Select(n => n.Lane)).Any();
    }

    public static int Map(int lane, int keyCount = 4)
    {
        return keyCount switch
        {
            4 => lane / 2,
            5 => lane < 3 ? 0 : 1,
            6 => lane / 3,
            _ => throw new ArgumentOutOfRangeException(nameof(keyCount), keyCount, null)
        };
    }
}