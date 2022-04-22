using ManiaStressEvaluator.Interpreter.Data.Beatmap;

namespace ManiaStressEvaluator.Interpreter.Data.Models.Muscles;

public class Human : MuscleBase
{
    public readonly List<Arm> Arms = new();

    public Human()
    {
        Arms.Add(new Arm(0));
        Arms.Add(new Arm(1));
    }

    public void React(NoteCollection notes)
    {
        Relax(notes);

        Stress += MathF.Max(0, 0.05f * (notes.Count - 2) * (1 / (notes.Time - LastHitTime)));

        var armMap = new Dictionary<int, List<Note>>();

        foreach (var note in notes)
        {
            if (!armMap.ContainsKey(Arm.Map(note.Lane)))
                armMap.Add(Arm.Map(note.Lane), new List<Note>());

            armMap[Arm.Map(note.Lane)].Add(note);
        }

        foreach (var arm in armMap) Arms[arm.Key].React(arm.Value);

        LastHitTime = notes.Time;
    }

    private void Relax(NoteCollection notes)
    {
        const float reliefMultiplier = 28f;
        Stress = MathF.Max(0,
            Stress - reliefMultiplier * MathF.Pow(notes.Time - LastHitTime, 1.5f));
    }

    public override float GetAccumulativeStress()
    {
        return Stress + Arms.Average(a => a.GetAccumulativeStress());
    }
}