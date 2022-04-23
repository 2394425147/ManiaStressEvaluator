﻿using ManiaStressEvaluator.Interpreter.Data.Beatmap;

namespace ManiaStressEvaluator.Interpreter.Data.Models.Muscles;

public class Finger : MuscleBase
{
    private bool _lastNoteWasHeld;

    public void React(Note note)
    {
        Relax(note);

        var multiplier = 1f;

        if (_lastNoteWasHeld)
        {
            _lastNoteWasHeld = false;
            multiplier *= 1 + Math.Max(0, 0.04f / (note.Time - LastHitTime));
        }

        if (LastHitTime > 0)
            Stress += multiplier * (0.06f / (note.Time - LastHitTime));
        else
            Stress += multiplier * 0.012f;

        LastHitTime = note.Time;

        if (!note.isHold || note.Length < 0.1f) return;

        _lastNoteWasHeld = true;
        LastHitTime = note.Time + note.Length;
    }

    private void Relax(Note note)
    {
        const float reliefMultiplier = 21f;
        Stress = MathF.Max(0,
            Stress - reliefMultiplier * MathF.Pow(note.Time - LastHitTime, 1.5f));
    }

    public override float GetAccumulativeStress()
    {
        return Stress;
    }
}