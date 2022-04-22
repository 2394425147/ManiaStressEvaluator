namespace ManiaStressEvaluator.Interpreter.Data.Beatmap;

public class NoteCollection : List<Note>
{
    public readonly float Time;

    public NoteCollection(float time)
    {
        Time = time;
    }
}