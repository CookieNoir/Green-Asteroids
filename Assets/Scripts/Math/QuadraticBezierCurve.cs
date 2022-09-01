using UnityEngine;

public class QuadraticBezierCurve : BezierCurve
{
    protected Vector3 point1;

    public QuadraticBezierCurve(Vector3 start, Vector3 point1, Vector3 end) : base(start, end)
    {
        this.point1 = point1;
    }

    protected override Vector3 BeforeStartDirection()
    {
        return (pointStart - point1).normalized;
    }

    protected override Vector3 AfterEndDirection()
    {
        return (pointEnd - point1).normalized;
    }

    protected override Vector3 EvaluateInRange(in float t)
    {
        float mt = 1f - t;
        return mt * mt * pointStart + 2f * mt * t * point1 + t * t * pointEnd;
    }
}