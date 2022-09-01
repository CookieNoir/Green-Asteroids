using System.Collections;
using UnityEngine;

public class GridToggler : GameObjectToggler
{
    [SerializeField, Range(0.2f, 5f)] private float _toggleSpeed = 1f;
    [SerializeField, Range(0f, 1f)] private float _startGridMultiplier = 1f;
    private bool _state;
    private IEnumerator _toggleGridCoroutine;

    private void Awake()
    {
        _toggleGridCoroutine = _ToggleGridCoroutine();
        Shader.SetGlobalFloat("_GridMultiplier", _startGridMultiplier);
        _state = _startGridMultiplier > 0f;
    }

    protected override bool OnToggle()
    {
        _state = !_state;
        StopCoroutine(_toggleGridCoroutine);
        _toggleGridCoroutine = _ToggleGridCoroutine();
        StartCoroutine(_toggleGridCoroutine);
        return _state;
    }

    private IEnumerator _ToggleGridCoroutine()
    {
        float currentFactor = Shader.GetGlobalFloat("_GridMultiplier");
        float startValue, endValue;
        if (_state)
        {
            startValue = 0f;
            endValue = 1f;
        }
        else
        {
            startValue = 1f;
            endValue = 0f;
            currentFactor = 1f - currentFactor;
        }
        while (currentFactor < 1f)
        {
            currentFactor += _toggleSpeed * Time.deltaTime;
            Shader.SetGlobalFloat("_GridMultiplier", Mathf.Lerp(startValue, endValue, currentFactor));
            yield return null;
        }
        Shader.SetGlobalFloat("_GridMultiplier", endValue);
    }
}
