using System;
using UnityEngine;

namespace Asteroids.Grid
{
    public class SphericalGridProperties : MonoBehaviour
    {
        [SerializeField, Min(3)] private int _gridSize;
        public int GridSize
        {
            get => _gridSize;
            private set => _gridSize = value;
        }
        [SerializeField] private bool[] _mask;

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