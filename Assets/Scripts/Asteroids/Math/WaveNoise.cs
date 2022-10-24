using UnityEngine;

namespace Asteroids.Math
{
    [System.Serializable]
    public class WaveNoise
    {
        [field: SerializeField] public Vector4 XValues { get; private set; }
        [field: SerializeField] public Vector4 YValues { get; private set; }
        [field: SerializeField] public Vector4 ZValues { get; private set; }

        public float GetValue(in Vector3 position)
        {
            float sum = XValues.z * Mathf.Cos(position.x * XValues.x + XValues.y)
                + YValues.z * Mathf.Cos(position.y * YValues.x + YValues.y)
                + ZValues.z * Mathf.Cos(position.z * ZValues.x + ZValues.y);
            sum /= XValues.z + YValues.z + ZValues.z;
            sum = sum * 0.5f + 0.5f;
            return sum;
        }
    }
}