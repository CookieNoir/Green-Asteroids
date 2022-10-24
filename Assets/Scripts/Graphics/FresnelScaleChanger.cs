using UnityEngine;

public class FresnelScaleChanger : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField, Min(0.01f)] private float _animationLength = 1f;
    [SerializeField, Range(-1f, 1f)] private float _startValue = 0f;
    private float _value;
    private bool _ascending;

    private void Start()
    {
        ResetValues();
    }

    private void _SetFresnelScale()
    {
        float value = _ascending ? _value : 1f - _value;
        float evaluated = _animationCurve.Evaluate(value);
        Shader.SetGlobalFloat("_fresnelScale", evaluated);
    }

    private void _RecalculateValue()
    {
        _value += Time.deltaTime / _animationLength;
        if (_value >= 1f)
        {
            _value %= 1f;
            _ascending = !_ascending;
        }
    }

    public void ResetValues()
    {
        _ascending = _startValue >= 0f;
        _value = _ascending ? _startValue : -_startValue;
        _SetFresnelScale();
    }

    private void OnValidate()
    {
        ResetValues();
    }

    void Update()
    {
        _RecalculateValue();
        _SetFresnelScale();
    }
}
