using UnityEngine;

namespace Asteroids.Tiles.Painting
{
    public class ResizeableObjectCollection : PaintableTile
    {
        private float[] _currentSizes;
        private Vector3[] _defaultSizes;

        protected override void OnPaintingInitiating(out Vector3 center, out Vector3[] positions)
        {
            int quantity = transform.childCount;
            _defaultSizes = new Vector3[quantity];
            positions = new Vector3[quantity];
            center = Vector3.zero;
            for (int i = 0; i < _defaultSizes.Length; ++i)
            {
                Transform child = transform.GetChild(i);
                _defaultSizes[i] = child.localScale;
                positions[i] = child.position;
                center += positions[i];
            }
            center = (center / quantity).normalized;
            _currentSizes = new float[quantity];
        }

        protected override float SetTarget(in float value)
        {
            return value > 0f ? 1f : 0f;
        }

        protected override void SetValue(in int index, in float value)
        {
            _currentSizes[index] = value;
        }

        protected override float GetValue(in int index)
        {
            return _currentSizes[index];
        }

        protected override void ApplyValues(AnimationCurve evaluationCurve)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).localScale = evaluationCurve.Evaluate(_currentSizes[i]) * _defaultSizes[i];
            }
        }
    }
}