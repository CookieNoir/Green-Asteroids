using UnityEngine;
using UnityEditor;
using Asteroids;
using Asteroids.Levels;

public class LevelGraphicsTester : EditorWindow
{
    public GraphicsValues globalValues;
    public LevelGraphics levelGraphics;

    [MenuItem("Tools/Level Graphics Tester")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LevelGraphicsTester));
    }

    private void OnGUI()
    {
        globalValues = EditorGUILayout.ObjectField("Global Values", globalValues, typeof(GraphicsValues),true) as GraphicsValues;
        levelGraphics = EditorGUILayout.ObjectField("Level Graphics", levelGraphics, typeof(LevelGraphics), true) as LevelGraphics;
        if (globalValues && levelGraphics && GUILayout.Button("Apply Graphics"))
        {
            globalValues.SetValues(levelGraphics);
        }
    }
}