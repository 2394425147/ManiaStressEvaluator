using System.Globalization;
using ManiaStressEvaluator.Graph.Components;
using ManiaStressEvaluator.Interpreter.Data.Beatmap;
using ManiaStressEvaluator.Interpreter.Data.Models.Muscles;
using Raylib_cs;

namespace ManiaStressEvaluator.Graph.Evaluators;

public class RealtimeEvaluator
{
    private readonly DateTime _startTime;
    private readonly List<NoteCollection> _noteCopy;
    private readonly Human _player;
    private float frameStress;
    private float totalStress;
    private int notesHit;

    public RealtimeEvaluator(Chart chart)
    {
        _startTime = DateTime.Now;
        _noteCopy = chart.AsNoteCollections();

        _player = new Human();
        _lastPosition = new int[2];
    }

    private readonly List<LinePoint> _lineSegments = new();
    private readonly int[] _lastPosition;

    public void Update()
    {
        var currentTime = DateTime.Now;
        var timeSinceStart = (currentTime - _startTime).TotalMilliseconds / 1000f;

        foreach (var noteCollection in _noteCopy.Where(n => n.Time < timeSinceStart).ToList())
        {
            notesHit++;
            _player.React(noteCollection);

            frameStress = _player.GetAccumulativeStress();

            var lineColor = frameStress switch
            {
                > 5.6f => Color.RED,
                > 3.5f => Color.YELLOW,
                > 1.7f => Color.WHITE,
                _ => Color.BLUE
            };

            var newX = (int)(InvLerp(0, _noteCopy.Max(n => n.Time), noteCollection.Time) * Program.CanvasWidth);
            var newY = (int)(InvLerp(0, 16, Math.Clamp(frameStress, 0, 16)) * Program.CanvasHeight);

            _lineSegments.Add(new LinePoint(_lastPosition[0], Program.CanvasHeight - _lastPosition[1],
                newX, Program.CanvasHeight - newY, lineColor));

            _lastPosition[0] = newX;
            _lastPosition[1] = newY;

            _noteCopy.Remove(noteCollection);
            
            totalStress += frameStress;
        }
        
        Raylib.DrawText($"TME: {timeSinceStart:00:00.0}\n" +
                        $"FRM: {frameStress:00}* | AVG: {totalStress / notesHit * 1.53f:0.000}*", 10, 10, 20, Color.WHITE);

        foreach (var lineSegment in _lineSegments)
        {
            lineSegment.Update();
        }
    }

    private static float InvLerp(float a, float b, float t)
    {
        return (t - a) / (b - a);
    }
}