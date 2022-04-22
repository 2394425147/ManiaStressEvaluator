namespace ManiaStressEvaluator.Interpreter.Data.Beatmap;

public class Chart
{
    public List<Note> Notes = new();
    public List<TempoSection> TempoSections = new();

    /// <summary>
    ///     This game calculates position based on BPS.
    ///     For instance, the game scrolls 1BPS per unit, meaning that the game will scroll down exactly 1 unit for every BPS.
    /// </summary>
    /// <param name="tempoList">List of tempos to calculate on</param>
    /// <param name="time">Time position</param>
    /// <param name="scale">The scroll speed</param>
    /// <returns>Position of the game object (Scaled)</returns>
    public static float CalculatePosition(IEnumerable<TempoSection> tempoList, float time, float scale)
    {
        var validTempo = tempoList.Where(t => t.Time <= time).ToList();

        if (!validTempo.Any()) return 0;

        float position = 0;

        for (var i = 0; i < validTempo.Count - 1; i++)
        {
            var bps = TempoSection.BeatPerSecond(validTempo[i].Bpm * validTempo[i].Multiplier);
            position += bps * (validTempo[i + 1].Time - validTempo[i].Time);
        }

        var last = validTempo.Last();
        position += TempoSection.BeatPerSecond(last.Bpm) * (time - last.Time) * last.Multiplier;

        return position * scale;
    }

    public List<NoteCollection> AsNoteCollections()
    {
        var notes = Notes.OrderBy(n => n.Time).ToList();
        var returnList = new List<NoteCollection>();

        foreach (var t in notes)
        {
            if (returnList.Count < 1 || t.Time > returnList[^1].Time) returnList.Add(new NoteCollection(t.Time));

            returnList[^1].Add(t);
        }

        return returnList;
    }
}

public class TempoSection
{
    public float Bpm;
    public float Multiplier;
    public float Time;

    public static float BeatPerSecond(float bpm)
    {
        return bpm / 60;
    }
}