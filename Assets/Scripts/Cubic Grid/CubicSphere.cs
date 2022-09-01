using UnityEngine;

public class CubicSphere: SubdividedCube
{
    protected static new CubicSphere instance;

    protected CubicSphere() { }

    public static new SubdividedCube GetInstance()
    {
        if (instance == null) instance = new CubicSphere();
        return instance;
    }

    public override Vector3 GetPosition(in int gridSize, in int x, in int y, in int z)
    {
        Vector3 v = new Vector3(x, y, z) * 2f / gridSize - Vector3.one;
        return GetSphericalPosition(v);
    }

    public static Vector3 GetSphericalPosition(in Vector3 vertex)
    {
        float x2 = vertex.x * vertex.x;
        float y2 = vertex.y * vertex.y;
        float z2 = vertex.z * vertex.z;
        return new Vector3(
            vertex.x * Mathf.Sqrt(1f - y2 * 0.5f - z2 * 0.5f + y2 * z2 * 0.333333f),
            vertex.y * Mathf.Sqrt(1f - x2 * 0.5f - z2 * 0.5f + x2 * z2 * 0.333333f),
            vertex.z * Mathf.Sqrt(1f - x2 * 0.5f - y2 * 0.5f + x2 * y2 * 0.333333f));
    }
}