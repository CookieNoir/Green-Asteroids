using UnityEngine;
using UnityEditor;
using Asteroids.Levels;

[CustomEditor(typeof(LevelProperties))]
public class LevelPropertiesEditor : Editor
{
    SerializedProperty _gridSize;
    SerializedProperty _randomSeed;
    SerializedProperty _mask;
    SerializedProperty _levelGraphics;
    SerializedProperty _movingEntitiesProperties;
    SerializedProperty _maxObstaclesCount;
    SerializedProperty _cameraStartAngle;
    SerializedProperty _canMoveCamera;
    private static string[] _names = new string[] { "Forward (0)", "Right (1)", "Back (2)", "Left (3)", "Top (4)", "Down (5)" };

    private void OnEnable()
    {
        _gridSize = serializedObject.FindProperty("_gridSize");
        _randomSeed = serializedObject.FindProperty("_randomSeed");
        _mask = serializedObject.FindProperty("_mask");
        _levelGraphics = serializedObject.FindProperty("_levelGraphics");
        _movingEntitiesProperties = serializedObject.FindProperty("_movingEntitiesProperties");
        _maxObstaclesCount = serializedObject.FindProperty("_maxObstaclesCount");
        _cameraStartAngle = serializedObject.FindProperty("_cameraStartAngle");
        _canMoveCamera = serializedObject.FindProperty("_canMoveCamera");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_gridSize);
        EditorGUILayout.PropertyField(_randomSeed);
        int gridSize = _gridSize.intValue;
        int offset = gridSize * gridSize;
        _mask.arraySize = 6 * offset;
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Clear all"))
                {
                    _SetValues(false);
                }
                if (GUILayout.Button("Fill all"))
                {
                    _SetValues(true);
                }
                EditorGUILayout.EndHorizontal();
            }
            for (int i = 0; i < 6; ++i)
            {
                EditorGUILayout.LabelField(_names[i]);
                for (int j = 0; j < gridSize; ++j)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int k = 0; k < gridSize; ++k)
                    {
                        SerializedProperty flag = _mask.GetArrayElementAtIndex(i * offset + j * gridSize + k);
                        EditorGUILayout.PropertyField(flag, GUIContent.none, GUILayout.MaxWidth(15));
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(_movingEntitiesProperties);
        EditorGUILayout.PropertyField(_maxObstaclesCount);
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Camera Properties");
        EditorGUILayout.PropertyField(_cameraStartAngle);
        EditorGUILayout.PropertyField(_canMoveCamera);
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(_levelGraphics);
        serializedObject.ApplyModifiedProperties();
    }

    private void _SetValues(bool value)
    {
        for (int i = 0; i < _mask.arraySize; ++i)
        {
            _mask.GetArrayElementAtIndex(i).boolValue = value;
        }
    }
}