using UnityEngine;
using Random = System.Random;

public class TileGenerationMethod : MonoBehaviour
{
    public virtual void Generate(Transform parentTransform, Vector3 position, Random random)
    {
    }
}