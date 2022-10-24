using UnityEngine;
using Asteroids.Grid;

namespace Asteroids.Entities
{
    public class EntitiesManager : MonoBehaviour
    {
        [SerializeField, Range(0.01f, 0.99f)] private float _startFactor = 0.5f;
        [SerializeField, Range(0.25f, 4f)] private float _speed = 1f;
        [SerializeField, Range(0.001f, 0.02f)] private float _rotationOffset = 0.01f; // Value, that is used to correct the rotation of entities
        private bool _isMoving;
        private float _currentFactor;
        private MovingEntity[] _movingEntities;

        public void SetEntities(CubicGrid<ChangeableObstacle> cubicGrid, MovingEntityProperties[] properties)
        {
            _movingEntities = new MovingEntity[properties.Length];
            for (int i = 0; i < _movingEntities.Length; ++i)
            {
                GameObject entityObject = Instantiate(properties[i].EntityPrefab, transform);
                int cellIndex = properties[i].StartSide * cubicGrid.CellsOnSide + properties[i].StartCellOnSide;
                GridCell<ChangeableObstacle> startCell = cubicGrid.GetCellByIndex(cellIndex);
                _movingEntities[i] = new MovingEntity(entityObject.transform, startCell, properties[i].StartDirection, properties[i].RotationDirection);
            }
            StopMoving();
        }

        public void StartMoving()
        {
            _SetEntitiesDestinations();
            _isMoving = true;
        }

        public void StopMoving()
        {
            _isMoving = false;
            ResetEntities();
            _currentFactor = _startFactor;
        }

        public void ResetEntities()
        {
            foreach (MovingEntity entity in _movingEntities)
            {
                entity.Reset();
            }
        }

        private void _SetEntitiesDestinations()
        {
            foreach (MovingEntity entity in _movingEntities)
            {
                entity.SetDestination();
            }
        }

        private void _MoveEntities()
        {
            foreach (MovingEntity entity in _movingEntities)
            {
                entity.SetPosition(_currentFactor, _rotationOffset);
            }
            _currentFactor += Time.deltaTime * _speed;
            if (_currentFactor >= 1f)
            {
                _currentFactor %= 1f;
                _SetEntitiesDestinations();
            }
        }

        private void Update()
        {
            if (_isMoving)
            {
                _MoveEntities();
            }
        }
    }
}