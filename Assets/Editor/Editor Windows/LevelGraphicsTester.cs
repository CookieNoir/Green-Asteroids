using UnityEngine;
using UnityEditor;

public class LevelGraphicsTester : EditorWindow
{
    public GlobalValues globalValues;
    public LevelGraphics levelGraphics;

    [MenuItem("Tools/Level Graphics Tester")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LevelGraphicsTester));
    }

    private void OnGUI()
    {
        globalValues = EditorGUILayout.ObjectField("Global Values", globalValues, typeof(GlobalValues),true) as GlobalValues;
        levelGraphics = EditorGUILayout.ObjectField("Level Graphics", levelGraphics, typeof(LevelGraphics), true) as LevelGraphics;
        if (globalValues && levelGraphics && GUILayout.Button("Apply Graphics"))
        {
            globalValues.SetValues(levelGraphics);
        }
    }
}