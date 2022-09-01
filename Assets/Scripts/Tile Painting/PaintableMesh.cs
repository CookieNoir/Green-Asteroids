using UnityEngine;
[RequireComponent(typeof(MeshFilter))]
public class PaintableMesh : PaintableTile
{
    private MeshFilter _meshFilter;
    private Mesh _mesh;
    private Color[] _currentColors;

    protected override void OnPaintingInitiating(out Vector3 center, out Vector3[] positions)
    {
        _meshFilter = GetComponent<MeshFilter>();
        _mesh = _meshFilter.mesh;
        _currentColors = new Color[_mesh.vertices.Length];
        center = (_mesh.bounds.center).normalized;
        positions = _mesh.vertices;
    }

    protected override void SetValue(in int index, in float value)
    {
        _currentColors[index] = Color.white * value;
    }

    protected override float GetValue(in int index)
    {
        return _currentColors[index].r;
    }

    protected override void ApplyValues(AnimationCurve evaluationCurve)
    {
        _mesh.colors = _currentColors;
        _meshFilter.mesh = _mesh;
    }
}