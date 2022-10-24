using System;
using UnityEngine;
using Random = System.Random;

namespace Asteroids.Tiles
{
    public class ForestGenerator : TileGenerationMethod
    {
        [Serializable]
        public struct PlantData
        {
            public GameObject[] Prefabs;
            [Range(0f, 1f)] public float Density;
            public Vector2 SizeRange;
        }

        [SerializeField, Range(1, 6)] private int _gridSize = 1;
        [SerializeField, Range(0f, 1f)] private float _maxRelativeOffset = 0.5f;
        [SerializeField, Range(0.1f, 10f)] private float _bushesOverTreesRatio = 2f; // Expected bushes over trees ratio
        [SerializeField] private PlantData _bushes;
        [SerializeField] private PlantData _trees;
        private float _gridStep;
        private float _gridOffset;
        private float _bushesSpawnThreshold;
        private float _treesSpawnThreshold;
        private float _treesBorder;
        private Transform _parent;
        private Random _random;

        private void Awake()
        {
            _gridStep = 1f / _gridSize;
            _gridOffset = _gridStep * 0.5f;
            _treesBorder = 1f / (_bushesOverTreesRatio + 1f);
            _bushesSpawnThreshold = _treesBorder + (1f - _treesBorder) * _bushes.Density;
            _treesSpawnThreshold = _treesBorder * _trees.Density;
        }

        public override void Generate(Transform parentTransform, Vector3 position, Random random)
        {
            _parent = parentTransform;
            _random = random;
            for (int i = 0; i < _gridSize; ++i)
            {
                for (int j = 0; j < _gridSize; ++j)
                {
                    float value = (float)_random.NextDouble();
                    if (value < _treesBorder)
                    {
                        if (value < _treesSpawnThreshold)
                        {
                            _SetPlant(_trees, _GetLocalPosition(i, j));
                        }
                    }
                    else
                    {
                        if (value < _bushesSpawnThreshold)
                        {
                            _SetPlant(_bushes, _GetLocalPosition(i, j));
                        }
                    }
                }
            }
        }

        private Vector3 _GetLocalPosition(in int x, in int z)
        {
            float offsetX = _gridStep * ((float)_random.NextDouble() - 0.5f) * _maxRelativeOffset;
            float offsetZ = _gridStep * ((float)_random.NextDouble() - 0.5f) * _maxRelativeOffset;
            return new Vector3(_gridOffset + _gridStep * x + offsetX, 0f, _gridOffset + _gridStep * z + offsetZ);
        }

        private void _SetPlant(in PlantData plantData, in Vector3 localPosition)
        {
            int index = _random.Next(0, plantData.Prefabs.Length);
            GameObject newObject = Instantiate(plantData.Prefabs[index], _parent);
            Transform newObjectTransform = newObject.transform;
            newObjectTransform.localPosition = localPosition;
            newObjectTransform.localRotation = Quaternion.Euler(0f, (float)_random.NextDouble() * 360f, 0f);
            newObjectTransform.localScale = Vector3.one * (plantData.SizeRange.x
                + (float)_random.NextDouble() * (plantData.SizeRange.y - plantData.SizeRange.x));
        }

        private void OnDrawGizmosSelected()
        {
            float step = 1f / _gridSize;
            Gizmos.color = Color.green;
            for (int i = 0; i <= _gridSize; ++i)
            {
                Vector3 offset = Vector3.right * (i * step);
                Gizmos.DrawLine(transform.position + offset, transform.position + offset + Vector3.forward);
            }
            for (int i = 0; i <= _gridSize; ++i)
            {
                Vector3 offset = Vector3.forward * (i * step);
                Gizmos.DrawLine(transform.position + offset, transform.position + offset + Vector3.right);
            }
        }
    }
}