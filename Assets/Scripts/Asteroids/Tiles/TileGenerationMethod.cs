using UnityEngine;
using Random = System.Random;

namespace Asteroids.Tiles
{
    public class TileGenerationMethod : MonoBehaviour
    {
        public virtual void Generate(Transform parentTransform, Vector3 position, Random random)
        {
        }
    }
}