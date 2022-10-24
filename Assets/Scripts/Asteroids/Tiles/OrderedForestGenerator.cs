using System;
using UnityEngine;
using Asteroids.Grid;
using Asteroids.Math;
using Random = System.Random;

namespace Asteroids.Tiles
{
    public class OrderedForestGenerator : TileGenerationMethod
    {
        [Serializable]
        public struct GenerationMethodFloatPair
        {
            public TileGenerationMethod TileGenerationMethod;
            public float Value; // Upper border of range, recommended value for the last item in array is 1
        }

        [SerializeField] private GenerationMethodFloatPair[] _pairs;
        [SerializeField] private NoiseHandler _noiseHandler;

        public override void Generate(Transform parentTransform, Vector3 position, Random random)
        {
            float value = _noiseHandler.GetValue(CubicSphere.GetSphericalPosition(position));
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
}