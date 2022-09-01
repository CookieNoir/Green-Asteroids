using UnityEngine;

public class GlobalValues : MonoBehaviour
{
    [SerializeField] private MeshRenderer _skyRenderer;

    // Used in ordered generators
    public static Vector4 NoiseXValues { get; private set; }
    public static Vector4 NoiseYValues { get; private set; }
    public static Vector4 NoiseZValues { get; private set; }

    public void SetValues(Vector3 sunDirection, Texture2D shadingTexture, Material skyMaterial,
        Vector4 noiseX, Vector4 noiseY, Vector4 noiseZ, Vector4 windProperties)
    {
        Shader.SetGlobalVector("_SunDirection", sunDirection.normalized);
        Shader.SetGlobalTexture("_Shading", shadingTexture);
        _skyRenderer.material = skyMaterial;
        Shader.SetGlobalVector("_NoiseX", noiseX);
        Shader.SetGlobalVector("_NoiseY", noiseY);
        Shader.SetGlobalVector("_NoiseZ", noiseZ);
        NoiseXValues = noiseX;
        NoiseYValues = noiseY;
        NoiseZValues = noiseZ;
        Shader.SetGlobalVector("_WindProperties", windProperties);
    }

    public void SetValues(LevelGraphics levelGraphics)
    {
        SetValues(levelGraphics.SunDirection, levelGraphics.ShadingTexture, levelGraphics.SkyMaterial,
            levelGraphics.NoiseXValues, levelGraphics.NoiseYValues, levelGraphics.NoiseZValues,
            levelGraphics.WindProperties);
    }
}
