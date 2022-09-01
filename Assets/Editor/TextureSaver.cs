using System;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace EditorOnly
{
    public static class TextureSaver
    {
        public static void SaveTextureAsPNG(Texture2D texture, string path)
        {
            byte[] bytes = ImageConversion.EncodeToPNG(texture);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = path + string.Format("{0:yyyy-MM-dd_HH-mm-ss-fff}.png", DateTime.Now);
            File.WriteAllBytes(fileName, bytes);
            Debug.Log(string.Format("Texture created at path: {0}", fileName));
            AssetDatabase.Refresh();
        }
    }
}