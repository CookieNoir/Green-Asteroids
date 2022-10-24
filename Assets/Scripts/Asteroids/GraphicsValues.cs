using UnityEngine;
using Asteroids.Levels;
using Asteroids.Math;

namespace Asteroids
{
    public class GraphicsValues : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _skyRenderer;

        public void SetValues(Vector3 sunDirection, Texture2D shadingTexture, Material skyMaterial,
            WaveNoise noise, Vector4 windProperties)
        {
            Shader.SetGlobalVector("_SunDirection", sunDirection.normalized);
            Shader.SetGlobalTexture("_Shading", shadingTexture);
            _skyRenderer.material = skyMaterial;
            Shader.SetGlobalVector("_NoiseX", noise.XValues);
            Shader.SetGlobalVector("_NoiseY", noise.YValues);
            Shader.SetGlobalVector("_NoiseZ", noise.ZValues);
            Shader.SetGlobalVector("_WindProperties", windProperties);
        }

        public void SetValues(LevelGraphics levelGraphics)
        {
            SetValues(levelGraphics.SunDirection, levelGraphics.ShadingTexture, levelGraphics.SkyMaterial,
                levelGraphics.Noise, levelGraphics.WindProperties);
        }
    }
}