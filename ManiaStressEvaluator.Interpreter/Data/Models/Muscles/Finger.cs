using ManiaStressEvaluator.Interpreter.Data.Beatmap;

namespace ManiaStressEvaluator.Interpreter.Data.Models.Muscles;

public class Finger : MuscleBase
{
    private float _heldReleaseTime;
    private bool _lastNoteWasHeld;
    private readonly Wrist _parentWrist;

    public Finger(Wrist wrist)
    {
        _parentWrist = wrist;
    }

    public void React(Note note)
    {
        Relax(note);
        
        if (LastHitTime > 0)
        {
            var multiplier = 1f;
            var otherOnWrist = _parentWrist.Fingers.First(f => f != this);
            
            if (otherOnWrist._lastNoteWasHeld) multiplier += 0.05f;
            if (note.Time - otherOnWrist.LastHitTime is > 0 and < 0.1f) multiplier += 0.1f;

            Stress += multiplier * (0.15f / (note.Time - LastHitTime));
        }

        LastHitTime = note.Time;

        if (note.Time - _heldReleaseTime < 0.15f && _lastNoteWasHeld)
        {
            Stress += 0.15f / (note.Time - _heldReleaseTime);
            _lastNoteWasHeld = false;
        }

        if (!note.isHold || note.Length < 0.15f) return;
        
        _lastNoteWasHeld = true;
        _heldReleaseTime = note.Time + note.Length;
    }

    private void Relax(Note note)
    {
        const float reliefMultiplier = 23f;
        const float reliefCap = 40f;
        Stress = MathF.Max(0, Stress - reliefMultiplier * MathF.Min(reliefCap, MathF.Pow(note.Time - LastHitTime, 1.5f)));
    }
}