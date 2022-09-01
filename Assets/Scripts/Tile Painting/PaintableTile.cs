using UnityEngine;

public class PaintableTile : MonoBehaviour
{
    private Vector3 _center;
    public Vector3 Center { get => _center; }
    private Vector3[] _positions;
    private float[] _targetValues;
    private bool[] _paintable;
    private int _paintableCount;
    private AnimationCurve _evaluationCurve;

    public void InitiatePainting(AnimationCurve evaluationCurve)
    {
        OnPaintingInitiating(out _center, out _positions);
        _targetValues = new float[_positions.Length];
        _paintable = new bool[_positions.Length];
        _evaluationCurve = evaluationCurve;
        DropValues();
    }

    public void DropValues()
    {
        for (int i = 0; i < _targetValues.Length; ++i)
        {
            _targetValues[i] = SetTarget(0f);
            SetValue(i, 0f);
            _paintable[i] = false;
        }
        _paintableCount = 0;
        ApplyValues(_evaluationCurve);
    }

    protected virtual void SetValue(in int index, in float value) { }

    protected virtual float GetValue(in int index) { return 0f; }

    protected virtual void OnPaintingInitiating(out Vector3 center, out Vector3[] positions)
    {
        center = Vector3.zero;
        positions = null;
    }

    public void TryToPaint(Vector3 brushCenter, float brushRadius, AnimationCurve falloff)
    {
        for (int i = 0; i < _targetValues.Length; ++i)
        {
            float magnitude = (_positions[i] - brushCenter).magnitude;
            if (magnitude <= brushRadius)
            {
                float value = falloff.Evaluate(magnitude / brushRadius);
                if (value > _targetValues[i])
                {
                    _targetValues[i] = SetTarget(value);
                    if (!_paintable[i])
                    {
                        _paintable[i] = true;
                        _paintableCount++;
                    }
                }
            }
        }
    }

    protected virtual float SetTarget(in float value)
    {
        return value;
    }

    public void Paint(in float strength)
    {
        if (_paintableCount > 0)
        {
            for (int i = 0; i < _targetValues.Length; ++i)
            {
                if (_paintable[i])
                {
                    if (_targetValues[i] - GetValue(i) > strength)
                    {
                        float value = GetValue(i) + strength;
                        SetValue(i, value);
                    }
                    else
                    {
                        SetValue(i, _targetValues[i]);
                        _paintable[i] = false;
                        _paintableCount--;
                    }
                }
            }
            ApplyValues(_evaluationCurve);
        }
    }

    protected virtual void ApplyValues(AnimationCurve evaluationCurve) { }
}