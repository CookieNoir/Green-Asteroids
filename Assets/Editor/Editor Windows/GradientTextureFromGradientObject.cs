using UnityEditor;
using UnityEngine;

namespace EditorOnly
{
    public class GradientTextureFromGradientObject : EditorWindow
    {
        Gradient _gradient = new Gradient();
        int _textureWidth;
        string _path = "Generated Textures";

        [MenuItem("Tools/Texture Generators/Gradient Texture From Gradient Object")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(GradientTextureFromGradientObject));
        }

        void OnGUI()
        {
            _gradient = EditorGUILayout.GradientField("Gradient", _gradient);
            _textureWidth = EditorGUILayout.IntSlider("Texture Width", _textureWidth, 2, 1024);
            _path = EditorGUILayout.TextField("Texture Relative Path", _path);
            if (GUILayout.Button("Create Gradient Texture"))
            {
                _CreateTexture(_gradient, _textureWidth, Application.dataPath + "/" + _path + "/");
            }
        }

        private void _CreateTexture(Gradient gradient, int textureWidth, string path)
        {
            Color[] colors = new Color[_textureWidth];

            float step = 1f / (_textureWidth - 1);
            colors[0] = gradient.Evaluate(0f);
            colors[_textureWidth - 1] = gradient.Evaluate(1f);
            for (int i = 1; i < _textureWidth - 1; ++i)
            {
                colors[i] = gradient.Evaluate(i * step);
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