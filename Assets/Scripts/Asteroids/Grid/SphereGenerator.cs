using UnityEngine;

namespace Asteroids.Grid
{
    public class SphereGenerator : MonoBehaviour
    {
        [SerializeField] private int _gridSize;
        [SerializeField, Min(1)] private int _gridSizeMultiplier;
        [SerializeField] private MeshFilter _meshFilter;

        public void GenerateMesh()
        {
            int meshGridSize = _gridSize * _gridSizeMultiplier;

            Vector3[] vertices = CubicSphere.GetInstance().GetPoints(meshGridSize, true);
            int meshGridSize1 = meshGridSize + 1;
            int[] triangles = new int[meshGridSize * meshGridSize * 36];
            int t = 0;
            for (int i = 0; i < 6; ++i)
            {
                int offset_i = i * meshGridSize1 * meshGridSize1;
                for (int j = 0; j < meshGridSize; ++j)
                {
                    int offset_j = j * meshGridSize1;
                    for (int k = 0; k < meshGridSize; ++k)
                    {
                        int offset_k = offset_i + offset_j + k;
                        triangles[t] = offset_k;
                        triangles[t + 1] = offset_k + 1;
                        triangles[t + 2] = offset_k + meshGridSize1 + 1;

                        triangles[t + 3] = offset_k;
                        triangles[t + 4] = offset_k + meshGridSize1 + 1;
                        triangles[t + 5] = offset_k + meshGridSize1;

                        t += 6;
                    }
                }
            }

            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.normals = vertices;
            mesh.triangles = triangles;
            _meshFilter.mesh = mesh;
        }
    }
}