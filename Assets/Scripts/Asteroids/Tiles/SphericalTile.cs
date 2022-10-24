using UnityEngine;
using Asteroids.Grid;
using Random = System.Random;

namespace Asteroids.Tiles
{
    public class SphericalTile : MonoBehaviour
    {
        [SerializeField] private string _generatorName; // Leave it empty if don't want to use any generators

        public void Deform(in Vector3 point1, in Vector3 point2, in Vector3 point3, in Vector3 point4, float height, Random random)
        {
            TileGenerator.GetInstance()?.Generate(_generatorName, transform, (point1 + point3) * 0.5f, random);
            foreach (Transform child in transform)
            {
                Vector3 p12 = Vector3.Lerp(point1, point2, child.position.x);
                Vector3 p43 = Vector3.Lerp(point4, point3, child.position.x);
                Vector3 point = Vector3.Lerp(p43, p12, child.position.z);
                child.position = CubicSphere.GetSphericalPosition(point);
                child.rotation = Quaternion.FromToRotation(Vector3.up, child.position) * child.rotation;
            }
            DeformOther(point1, point2, point3, point4, height);
        }

        protected virtual void DeformOther(in Vector3 point1, in Vector3 point2, in Vector3 point3, in Vector3 point4, float height) { }
    }
}