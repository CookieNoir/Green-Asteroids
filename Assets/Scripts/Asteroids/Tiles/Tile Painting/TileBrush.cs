using UnityEngine;

namespace Asteroids.Tiles.Painting
{
    public class TileBrush : MonoBehaviour
    {
        [SerializeField] private float _positionsRadius;
        [SerializeField, Min(0f)] private float _originRadiusAddition;
        private AnimationCurve _falloffCurve;
        private float _originRadiusSquared;

        public void SetBrush(AnimationCurve falloffCurve)
        {
            float originRadius = _positionsRadius + _originRadiusAddition;
            _originRadiusSquared = originRadius * originRadius;
            _falloffCurve = falloffCurve;
        }

        public void TryToPaint(PaintableTile paintable)
        {
            Vector3 direction = paintable.Center - transform.position;
            if (direction.sqrMagnitude <= _originRadiusSquared)
            {
                paintable.TryToPaint(transform.position, _positionsRadius, _falloffCurve);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _positionsRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _positionsRadius + _originRadiusAddition);
        }
    }
}