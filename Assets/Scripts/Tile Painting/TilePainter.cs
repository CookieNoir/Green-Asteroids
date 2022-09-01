using System.Collections.Generic;
using UnityEngine;

public class TilePainter : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1f)] private float _paintingStrengthPerSecond;
    [SerializeField] private Transform _tileMapOrigin;
    [SerializeField] private Transform _entitiesOrigin;
    [SerializeField] private AnimationCurve _evaluationCurve;
    [SerializeField] private AnimationCurve _falloffCurve;
    private List<PaintableTile> _paintables;
    private List<TileBrush> _brushes;
    private bool _isSet;
    private bool _isPainting;

    public void SetValues(int cellsCount)
    {
        if (_isSet) Drop();
        _paintables = new List<PaintableTile>(cellsCount);
        foreach (Transform child in _tileMapOrigin)
        {
            foreach (PaintableTile paintable in child.GetComponents<PaintableTile>())
            {
                paintable.InitiatePainting(_evaluationCurve);
                _paintables.Add(paintable);
            }
        }
        _brushes = new List<TileBrush>(_entitiesOrigin.childCount);
        foreach (Transform child in _entitiesOrigin)
        {
            TileBrush tileBrush = child.GetComponent<TileBrush>();
            tileBrush.SetBrush(_falloffCurve);
            if (tileBrush != null) _brushes.Add(tileBrush);
        }
        _isSet = true;
        _isPainting = false;
    }

    private void _Paint()
    {
        foreach (PaintableTile paintable in _paintables)
        {
            foreach (TileBrush brush in _brushes)
            {
                brush.TryToPaint(paintable);
            }
        }
        float strength = _paintingStrengthPerSecond * Time.fixedDeltaTime;
        foreach (PaintableTile paintable in _paintables)
        {
            paintable.Paint(strength);
        }
    }

    public void Drop()
    {
        if (_paintables != null) _paintables.Clear();
        if (_brushes != null) _brushes.Clear();
        _isSet = false;
    }

    public void StopPainting()
    {
        _isPainting = false;
        foreach (PaintableTile paintable in _paintables)
        {
            paintable.DropValues();
        }
    }

    public void StartPainting()
    {
        _isPainting = true;
    }

    private void FixedUpdate()
    {
        if (_isSet && _isPainting) _Paint();
    }
}
