using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MeshGenEditor : EditorWindow
{
    SphereGenerator sphereGenerator;
    GameObject targetObject;
    [MenuItem("Tools/Mesh Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(MeshGenEditor));
    }

    private void OnGUI()
    {
        sphereGenerator = EditorGUILayout.ObjectField("Generator", sphereGenerator, typeof(SphereGenerator), true) as SphereGenerator;
        targetObject = EditorGUILayout.ObjectField("Targer Object", targetObject, typeof(GameObject), true) as GameObject;
        if (GUILayout.Button("Generate And Export"))
        {
            sphereGenerator.GenerateMesh();
            ObjExporter.DoExport(new GameObject[] { targetObject });
        }
    }
}
