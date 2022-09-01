using System;
using UnityEngine;
using Random = System.Random;

public class OrderedForestGenerator : TileGenerationMethod
{
    [Serializable]
    public struct GenerationMethodFloatPair
    {
        public TileGenerationMethod TileGenerationMethod;
        public float Value; // Upper border of range, recommended value for the last item in array is 1
    }

    [SerializeField] private GenerationMethodFloatPair[] _pairs;

    private float _GetNoiseValue(in Vector3 position)
    {
        float sum = GlobalValues.NoiseXValues.z * Mathf.Cos(position.x * GlobalValues.NoiseXValues.x + GlobalValues.NoiseXValues.y)
        + GlobalValues.NoiseYValues.z * Mathf.Cos(position.y * GlobalValues.NoiseYValues.x + GlobalValues.NoiseYValues.y)
        + GlobalValues.NoiseZValues.z * Mathf.Cos(position.z * GlobalValues.NoiseZValues.x + GlobalValues.NoiseZValues.y);
        sum /= GlobalValues.NoiseXValues.z + GlobalValues.NoiseYValues.z + GlobalValues.NoiseZValues.z;
        sum = sum * 0.5f + 0.5f;
        return sum;
    }

    public override void Generate(Transform parentTransform, Vector3 position, Random random)
    {
        float value = _GetNoiseValue(CubicSphere.GetSphericalPosition(position));
        for (int i = 0; i < _pairs.Length; ++i)
        {
            if (value < _pairs[i].Value)
            {
                _pairs[i].TileGenerationMethod.Generate(parentTransform, position, random);
                break;
            }
        }
    }
}