using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor( typeof (GenerationMap))]
public class GeneratorMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GenerationMap map = (GenerationMap)target;

        if (DrawDefaultInspector())
        {
            if(map.autoUpdate)
                map.DrawMapInEditor();
        }

        if (GUILayout.Button("Generate"))
        {
            map.DrawMapInEditor(); 
        }
    }
}
