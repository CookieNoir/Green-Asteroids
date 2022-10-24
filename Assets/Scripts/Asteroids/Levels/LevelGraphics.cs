using UnityEngine;
using Asteroids.Math;

namespace Asteroids.Levels
{
    [CreateAssetMenu(fileName = "New Graphics Values", menuName = "ScriptableObjects/Level Graphics")]
    public class LevelGraphics : ScriptableObject
    {
        [field: SerializeField] public Vector3 SunDirection { get; private set; }
        [field:SerializeField] public Texture2D ShadingTexture { get; private set; }
        [field:SerializeField] public Material SkyMaterial { get; private set; }
        [field: SerializeField] public WaveNoise Noise { get; private set; }
        [field: SerializeField] public Vector4 WindProperties { get; private set; }
    }
}