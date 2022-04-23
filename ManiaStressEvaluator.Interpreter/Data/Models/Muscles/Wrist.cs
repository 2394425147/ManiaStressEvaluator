using ManiaStressEvaluator.Interpreter.Data.Beatmap;

namespace ManiaStressEvaluator.Interpreter.Data.Models.Muscles;

public class Wrist : MuscleBase
{
    private readonly Arm _parentArm;
    public readonly List<Finger> Fingers = new();

    private List<Note> _lastNotes = new();

    public Wrist(Arm arm)
    {
        _parentArm = arm;

        Fingers.Add(new Finger());
        Fingers.Add(new Finger());
    }

    public override float GetAccumulativeStress()
    {
        return Stress + Fingers.Average(x => x.GetAccumulativeStress());
    }

    public void React(List<Note> notes)
    {
        Relax(notes);

        if (MustRotate(notes)) Stress += 0.27f / (notes[0].Time - _lastNotes[0].Time);

        foreach (var note in notes) Fingers[_parentArm.Side < 1 ? note.Lane : note.Lane - 2].React(note);

        _lastNotes = notes;
    }

    private void Relax(List<Note> notes)
    {
        const float reliefMultiplier = 29f;
        Stress = MathF.Max(0,
            Stress - reliefMultiplier * MathF.Pow(notes[0].Time - LastHitTime, 1.5f));
    }


    private bool MustRotate(IEnumerable<Note> notes)
    {
        if (_lastNotes.Count < 1)
            return false;

        return !_lastNotes.Select(n => n.Lane).Intersect(notes.Select(n => n.Lane)).Any();
    }
}