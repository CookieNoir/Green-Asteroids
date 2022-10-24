using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Asteroids.Grid;
using Asteroids.Entities;

namespace Asteroids
{
    public class ObstaclesManager : MonoBehaviour
    {
        public UnityEvent<int, int> OnObstaclesCountChanged;
        [SerializeField] private MovingDirections _defaultObstacleDirection;
        [SerializeField] private GameObject _obstaclePrefab;
        private CubicGrid<ChangeableObstacle> _sphericalGrid;
        private Dictionary<GridCell<ChangeableObstacle>, GameObject> _cellObstaclePairs;
        private int _maxObstaclesCount;
        private int _currentObstaclesCount;
        private bool _toggleable;

        public void AllowToggling()
        {
            _toggleable = true;
        }

        public void RestrictToggling()
        {
            _toggleable = false;
        }

        public void SetValues(CubicGrid<ChangeableObstacle> sphericalGrid, in int maxObstaclesCount)
        {
            if (_cellObstaclePairs != null) _cellObstaclePairs.Clear();
            _maxObstaclesCount = maxObstaclesCount;
            _sphericalGrid = sphericalGrid;
            _cellObstaclePairs = new Dictionary<GridCell<ChangeableObstacle>, GameObject>(_maxObstaclesCount);
            _currentObstaclesCount = 0;
            OnObstaclesCountChanged.Invoke(_currentObstaclesCount, _maxObstaclesCount);
        }

        public void TryToToggleObstacle()
        {
            if (_toggleable)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    _ToggleObstacle(hit.point);
                }
            }
        }

        private void _ToggleObstacle(in Vector3 point)
        {
            GridCell<ChangeableObstacle> cell = _sphericalGrid.GetNearestCell(point);
            if (cell.data.isChangeable)
            {
                if (!cell.data.isObstacle)
                {
                    if (_currentObstaclesCount < _maxObstaclesCount)
                    {
                        cell.data.isObstacle = true;
                        GameObject obstacle = Instantiate(_obstaclePrefab, cell.Position, cell.GetLookAtRotation((int)_defaultObstacleDirection));
                        _cellObstaclePairs.Add(cell, obstacle);
                        _currentObstaclesCount++;
                        OnObstaclesCountChanged?.Invoke(_currentObstaclesCount, _maxObstaclesCount);
                    }
                }
                else
                {
                    GameObject obstacle;
                    if (_cellObstaclePairs.TryGetValue(cell, out obstacle))
                    {
                        cell.data.isObstacle = false;
                        _cellObstaclePairs.Remove(cell);
                        Destroy(obstacle);
                        _currentObstaclesCount--;
                        OnObstaclesCountChanged?.Invoke(_currentObstaclesCount, _maxObstaclesCount);
                    }
                }
            }
        }
    }
}