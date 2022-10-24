using UnityEngine;

namespace Asteroids.Math {
    public class NoiseHandler : MonoBehaviour
    {
        private WaveNoise _noise;

        public void SetNoise(WaveNoise noise)
        {
            _noise = noise;
        }

        public float GetValue(in Vector3 position)
        {
            if (_noise == null) return 0f;
            return _noise.GetValue(position);
        }
    }
}