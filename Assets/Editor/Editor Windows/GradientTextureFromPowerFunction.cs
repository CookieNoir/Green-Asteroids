using UnityEditor;
using UnityEngine;

namespace EditorOnly
{
    public class GradientTextureFromPowerFunction : EditorWindow
    {
        Color _leftColor;
        Color _rightColor;
        float _power = 1.0f;
        int _textureWidth;
        string _path = "Generated Textures";

        [MenuItem("Tools/Texture Generators/Gradient Texture From Power Function")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(GradientTextureFromPowerFunction));
        }

        void OnGUI()
        {
            _leftColor = EditorGUILayout.ColorField("Left Color", _leftColor);
            _rightColor = EditorGUILayout.ColorField("Right Color", _rightColor);
            _power = EditorGUILayout.Slider("Power", _power, 0f, 10f);
            _textureWidth = EditorGUILayout.IntSlider("Texture Width", _textureWidth, 2, 1024);
            _path = EditorGUILayout.TextField("Texture Relative Path", _path);
            if (GUILayout.Button("Create Gradient Texture"))
            {
                _CreateTexture(_leftColor, _rightColor, _power, _textureWidth, Application.dataPath + "/" + _path + "/");
            }
        }

        private void _CreateTexture(Color leftColor, Color rightColor, float power, int textureWidth, string path)
        {
            Color[] colors = new Color[_textureWidth];

            float step = 1f / (_textureWidth - 1);
            colors[0] = leftColor;
            colors[_textureWidth - 1] = rightColor;
            for (int i = 1; i < _textureWidth - 1; ++i)
            {
                colors[i] = Color.Lerp(leftColor, rightColor, Mathf.Pow(i * step, power));
            }

            Texture2D texture = new Texture2D(textureWidth, 4, TextureFormat.RGBA32, false);
            texture.SetPixels(0, 0, _textureWidth, 1, colors);
            texture.SetPixels(0, 1, _textureWidth, 1, colors);
            texture.SetPixels(0, 2, _textureWidth, 1, colors);
            texture.SetPixels(0, 3, _textureWidth, 1, colors);
            TextureSaver.SaveTextureAsPNG(texture, path);
        }
    }

}