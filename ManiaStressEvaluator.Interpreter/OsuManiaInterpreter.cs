using System.Text.RegularExpressions;
using ManiaStressEvaluator.Interpreter.Data.Beatmap;

namespace ManiaStressEvaluator.Interpreter;

public static class OsuManiaInterpreter
{
    private static string[] SplitByBeatmapSections(string text)
    {
        return Regex.Matches(text, @"\[\w+\]((\r\n)|.)*?(?=((\r\n)|$){2,})").Select(m => m.Value).ToArray();
    }

    public static Chart Parse(string rawChart, int laneCount = 4)
    {
        var sections = SplitByBeatmapSections(rawChart);
        var timingSection = sections.First(x => x.StartsWith("[TimingPoints]"))
            .Replace("[TimingPoints]\r\n",
                "")
            .Split(Environment.NewLine);

        var chart = new Chart();

        foreach (var section in timingSection)
        {
            var parameters = section.Split(',');

            var currentSection = new TempoSection
            {
                Time = int.Parse(parameters[0]) / 1000.0f
            };

            // 1 - Non-inherited
            // Other - Inherited
            if (parameters[6] == "1")
            {
                currentSection.Bpm = 1 / float.Parse(parameters[1]) * 1000 * 60;
                currentSection.Multiplier = chart.TempoSections.Count > 0 ? chart.TempoSections[^1].Multiplier : 1;
            }
            else
            {
                currentSection.Bpm = chart.TempoSections[^1].Bpm;
                currentSection.Multiplier = -(100.00f / float.Parse(parameters[1]));
            }

            chart.TempoSections.Add(currentSection);
        }

        if (chart.TempoSections[0].Time > -3)
            chart.TempoSections.Insert(0, new TempoSection
            {
                Time = -3,
                Bpm = chart.TempoSections[0].Bpm,
                Multiplier = 1
            });

        var noteSection = sections.First(x => x.StartsWith("[HitObjects]"))
            .Replace("[HitObjects]\r\n",
                "")
            .Split('\n');

        foreach (var section in noteSection)
        {
            var parameters = section.Split(',');

            var note = new Note
            {
                Time = int.Parse(parameters[2]) / 1000.0f,
                Lane = (int)Math.Floor(int.Parse(parameters[0]) * laneCount / 512.0f)
            };

            var holdParameters = parameters[^1].Split(':');

            if (holdParameters.Length > 5)
            {
                note.isHold = true;
                note.Length = float.Parse(holdParameters[0]) / 1000.0f - note.Time;
            }

            chart.Notes.Add(note);
        }

        return chart;
    }
}