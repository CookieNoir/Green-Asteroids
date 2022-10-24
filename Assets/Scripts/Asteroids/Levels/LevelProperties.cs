using System;
using UnityEngine;
using Asteroids.Entities;

namespace Asteroids.Levels
{
    [CreateAssetMenu(fileName = "New Level", menuName = "ScriptableObjects/Level Properties")]
    public class LevelProperties : ScriptableObject
    {
        [SerializeField, Min(3)] private int _gridSize;
        public int GridSize { get => _gridSize; private set => _gridSize = value; }
        [SerializeField] private int _randomSeed;
        public int RandomSeed { get => _randomSeed; private set => _randomSeed = value; }
        [SerializeField] private bool[] _mask;
        [SerializeField] private MovingEntityProperties[] _movingEntitiesProperties;
        public MovingEntityProperties[] MovingEntitiesProperties { get => _movingEntitiesProperties; private set => _movingEntitiesProperties = value; }
        [SerializeField, Min(1)] private int _maxObstaclesCount;
        public int MaxObstaclesCount { get => _maxObstaclesCount; private set => _maxObstaclesCount = value; }
        [SerializeField] private LevelGraphics _levelGraphics;
        public LevelGraphics LevelGraphics { get => _levelGraphics; private set => _levelGraphics = value; }
        [SerializeField] private Vector3 _cameraStartAngle;
        public Vector3 CameraStartAngle { get => _cameraStartAngle; private set => _cameraStartAngle = value; }
        [SerializeField] private bool _canMoveCamera;
        public bool CanMoveCamera { get => _canMoveCamera; private set => _canMoveCamera = value; }


        public bool[] GetMask()
        {
            bool[] maskCopy = new bool[_mask.Length];
            Array.Copy(_mask, maskCopy, _mask.Length);
            return maskCopy;
        }

        private void OnValidate()
        {
            int expectedSize = _gridSize * _gridSize * 6;
            if (_mask.Length != expectedSize) _mask = new bool[expectedSize];
        }
    }
}