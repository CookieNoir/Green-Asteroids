using UnityEngine;
using Asteroids.Grid;

namespace Asteroids.Tiles
{
    [RequireComponent(typeof(MeshFilter))]
    public class SphericalTileMesh : SphericalTile
    {
        [SerializeField] private MeshFilter _meshFilter;
        private Mesh _newMesh;
        protected override void DeformOther(in Vector3 point1, in Vector3 point2, in Vector3 point3, in Vector3 point4, float height)
        {
            Vector3[] vertices = _meshFilter.mesh.vertices;

            for (int i = 0; i < vertices.Length; ++i)
            {
                float vertexHeight = 1f + vertices[i].y * height;
                Vector3 p12 = Vector3.Lerp(point1, point2, vertices[i].x);
                Vector3 p43 = Vector3.Lerp(point4, point3, vertices[i].x);
                Vector3 point = Vector3.Lerp(p43, p12, vertices[i].z);
                vertices[i] = CubicSphere.GetSphericalPosition(point) * vertexHeight;
            }
            _newMesh = _meshFilter.mesh;
            _newMesh.vertices = vertices;
            _newMesh.RecalculateNormals();
            _newMesh.RecalculateBounds();
            _meshFilter.mesh = _newMesh;
        }

        private void OnDestroy()
        {
            Destroy(_newMesh);
        }
    }
}
