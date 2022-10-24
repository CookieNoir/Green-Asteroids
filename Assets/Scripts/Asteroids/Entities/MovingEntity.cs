using UnityEngine;
using Asteroids.Grid;
using Asteroids.Math;

namespace Asteroids.Entities
{
    public class MovingEntity
    {
        private Transform _entityTransform;
        private GridCell<ChangeableObstacle> _startCell;
        private GridCell<ChangeableObstacle> _currentCell;
        private GridCell<ChangeableObstacle> _nextCell;
        private int _startMovingDirection;
        private int _currentDirection;
        private int _rotationDirection;
        private BezierCurve _movingCurve;
        private bool _movingBack;
        private Vector3 _movingBackPosition;
        private Quaternion _movingBackRotation;

        public MovingEntity(Transform entityTransform, GridCell<ChangeableObstacle> startCell, MovingDirections startMovingDirection, MovingDirections rotationDirection)
        {
            _entityTransform = entityTransform;
            _startCell = startCell;
            _startMovingDirection = (int)startMovingDirection;
            _rotationDirection = (int)rotationDirection;
        }

        public void Reset()
        {
            _currentCell = _startCell;
            _currentDirection = _startMovingDirection;
            _nextCell = null;
            _movingCurve = null;
            _entityTransform.position = _currentCell.Position;
            _entityTransform.rotation = SphericalHelper.SphericalLookAt(_currentCell.Position, _currentCell.Next(_currentDirection).Position);
        }

        public void SetDestination()
        {
            if (_nextCell != null)
            {
                _currentDirection = CubicGrid<ChangeableObstacle>.ModifyDirection(_currentDirection, _currentCell, _nextCell);
                _currentCell = _nextCell;
            }
            int prevIndex = (_currentDirection + 2) % 4;
            GridCell<ChangeableObstacle> prevCell = _currentCell.Next(prevIndex);
            _nextCell = _currentCell.Next(_currentDirection);
            if ((_currentCell.data.isChangeable && _currentCell.data.isObstacle) || (!_nextCell.data.isChangeable && _nextCell.data.isObstacle))
            {
                _currentDirection = (_currentDirection + _rotationDirection) % 4;
                _nextCell = _currentCell.Next(_currentDirection);
                if (_rotationDirection != 2)
                {
                    if (!_nextCell.data.isChangeable && _nextCell.data.isObstacle)
                    {
                        _currentDirection = (_currentDirection + 2) % 4;
                        _nextCell = _currentCell.Next(_currentDirection);
                        if (!_nextCell.data.isChangeable && _nextCell.data.isObstacle)
                        {
                            _currentDirection = prevIndex;
                            _nextCell = _currentCell.Next(_currentDirection);
                        }
                    }
                }
            }
            _movingBack = prevCell == _nextCell;
            if (_movingBack)
            {
                _movingBackPosition = ((_nextCell.Position + _currentCell.Position) * 0.5f).normalized;
                _movingBackRotation = SphericalHelper.SphericalLookAt(_movingBackPosition, _currentCell.Next(_currentDirection).Position);
            }
            else
            {
                Vector3 cur = _currentCell.Position;
                Vector3 prev = ((prevCell.Position + cur) / 2f).normalized;
                Vector3 next = ((_nextCell.Position + cur) / 2f).normalized;
                _movingCurve = new QuadraticBezierCurve(prev, cur, next);
            }
        }

        public void SetPosition(in float factor, in float rotationOffset)
        {
            if (_movingBack)
            {
                _entityTransform.position = _movingBackPosition;
                _entityTransform.rotation = _movingBackRotation * Quaternion.Euler(0f, 180f - 180f * factor, 0f);
            }
            else
            {
                Vector3 position = _movingCurve.Evaluate(factor).normalized;
                Vector3 nextPosition = _movingCurve.Evaluate(factor + rotationOffset).normalized;
                _entityTransform.position = position;
                _entityTransform.rotation = SphericalHelper.SphericalLookAt(position, nextPosition);
            }
        }
    }
}