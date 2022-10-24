using UnityEngine;
using Asteroids.Grid;

namespace Asteroids.Entities
{
    [System.Serializable]
    public class MovingEntityProperties
    {
        [SerializeField] private GameObject _entityPrefab;
        public GameObject EntityPrefab { get => _entityPrefab; private set => _entityPrefab = value; }
        [SerializeField] private int _startSide;
        public int StartSide { get => _startSide; private set => _startSide = value; }
        [SerializeField] private int _startCellOnSide;
        public int StartCellOnSide { get => _startCellOnSide; private set => _startCellOnSide = value; }
        [SerializeField] private MovingDirections _startDirection;
        public MovingDirections StartDirection { get => _startDirection; private set => _startDirection = value; }
        [SerializeField] private MovingDirections _rotationDirection;
        public MovingDirections RotationDirection { get => _rotationDirection; private set => _rotationDirection = value; }
    }
}