using UnityEngine;

public class BezierCurve
{
    protected Vector3 pointStart;
    protected Vector3 pointEnd;
    protected const float DirectionMultiplier = 0.01f;

    public BezierCurve(Vector3 start, Vector3 end)
    {
        pointStart = start;
        pointEnd = end;
    }

    public Vector3 Evaluate(in float t)
    {
        if (t < 0f) return PointBeforeStart();
        else if (t > 1f) return PointAfterEnd();
        else return EvaluateInRange(t);
    }

    private Vector3 PointBeforeStart()
    {
        return pointStart + BeforeStartDirection() * DirectionMultiplier;
    }

    protected virtual Vector3 BeforeStartDirection()
    {
        return(pointStart - pointEnd).normalized;
    }

    private Vector3 PointAfterEnd()
    {
        return pointEnd + AfterEndDirection() * DirectionMultiplier;
    }

    protected virtual Vector3 AfterEndDirection()
    {
        return (pointEnd - pointStart).normalized;
    }

    protected virtual Vector3 EvaluateInRange(in float t)
    {
        return (1 - t) * pointStart + t * pointEnd;
    }
}