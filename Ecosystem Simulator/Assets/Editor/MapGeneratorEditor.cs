using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WorldGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WorldGenerator generator = (WorldGenerator)target;

        if (DrawDefaultInspector())
        {
            if (generator.autoUpdate)
            {
                generator.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            generator.GenerateMap();
        }
    }
}