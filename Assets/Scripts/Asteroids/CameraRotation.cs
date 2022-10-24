using System.Collections;
using UnityEngine;
using Asteroids.Grid;

namespace Asteroids
{
    public class CameraRotation : MonoBehaviour
    {
        [SerializeField] private Transform _cameraOrigin;
        private bool _canRotate = true;
        private bool _isRotating = false;
        private IEnumerator _rotationQuaternion;
        private Quaternion _rorationOffset;

        private void Awake()
        {
            _rotationQuaternion = _RotateAlongLocalAxis(0, Vector3.zero);
            _rorationOffset = _cameraOrigin.rotation;
        }

        public void RotateForward()
        {
            if (_canRotate) _TryToRotate(90, Vector3.right);
        }

        public void RotateRight()
        {
            if (_canRotate) _TryToRotate(-90, Vector3.up);
        }

        public void RotateBack()
        {
            if (_canRotate) _TryToRotate(-90, Vector3.right);
        }

        public void RotateLeft()
        {
            if (_canRotate) _TryToRotate(90, Vector3.up);
        }

        public void Rotate(MovingDirections direction)
        {
            switch (direction)
            {
                case MovingDirections.Forward: { RotateForward(); break; }
                case MovingDirections.Right: { RotateRight(); break; }
                case MovingDirections.Back: { RotateBack(); break; }
                case MovingDirections.Left: { RotateLeft(); break; }
            }
        }

        public void SetCamera(Vector3 angle, bool canRotate)
        {
            StopCoroutine(_rotationQuaternion);
            _cameraOrigin.rotation = Quaternion.Euler(angle);
            _canRotate = canRotate;
        }

        private void _TryToRotate(float angle, Vector3 localAxis)
        {
            if (!_isRotating)
            {
                StopCoroutine(_rotationQuaternion);
                _rotationQuaternion = _RotateAlongLocalAxis(angle, localAxis);
                StartCoroutine(_rotationQuaternion);
                _isRotating = true;
            }
        }

        private IEnumerator _RotateAlongLocalAxis(float angle, Vector3 localAxis)
        {
            float f = 0f;
            while (f < 1f)
            {
                _cameraOrigin.rotation = _rorationOffset * Quaternion.Euler(Mathf.SmoothStep(0f, angle, f) * localAxis);
                yield return new WaitForEndOfFrame();
                f += Time.deltaTime;
            }
            _cameraOrigin.rotation = _rorationOffset * Quaternion.Euler(angle * localAxis);
            _rorationOffset = _cameraOrigin.rotation;
            _isRotating = false;
        }
    }
}