using UnityEngine;
[CreateAssetMenu(fileName = "New Graphics Values", menuName = "ScriptableObjects/Level Graphics")]
public class LevelGraphics : ScriptableObject
{
    [SerializeField] private Vector3 _sunDirection;
    public Vector3 SunDirection { get => _sunDirection; private set => _sunDirection = value; }
    [SerializeField] private Texture2D _shadingTexture;
    public Texture2D ShadingTexture { get => _shadingTexture; private set => _shadingTexture = value; }
    [SerializeField] private Material _skyMaterial;
    public Material SkyMaterial { get => _skyMaterial; private set => _skyMaterial = value; }
    [SerializeField] private Vector4 _noiseXValues;
    public Vector4 NoiseXValues { get => _noiseXValues; private set => _noiseXValues = value; }
    [SerializeField] private Vector4 _noiseYValues;
    public Vector4 NoiseYValues { get => _noiseYValues; private set => _noiseYValues = value; }
    [SerializeField] private Vector4 _noiseZValues;
    public Vector4 NoiseZValues { get => _noiseZValues; private set => _noiseZValues = value; }
    [SerializeField] private Vector4 _windProperties;
    public Vector4 WindProperties { get => _windProperties; private set => _windProperties = value; }
}
