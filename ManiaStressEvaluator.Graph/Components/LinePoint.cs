using Raylib_cs;

namespace ManiaStressEvaluator.Graph.Components;

public class LinePoint
{
    private readonly int _x0;
    private readonly int _x1;
    private readonly int _y0;
    private readonly int _y1;
    private readonly Color _color;
    
    public LinePoint(int x0, int y0, int x1, int y1, Color lineColor)
    {
        _x0 = x0;
        _y0 = y0;
        _x1 = x1;
        _y1 = y1;
        _color = lineColor;
    }

    public void Update()
    {
        Raylib.DrawLine(_x0, _y0, _x1, _y1, _color);
    }
}