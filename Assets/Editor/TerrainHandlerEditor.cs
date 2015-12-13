using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(TerrainHandler))]
public class TerrainHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainHandler ter = (TerrainHandler)target;

        if (GUILayout.Button("Reset Terrain"))
        {
            ter.ResetTerrain();
        }

        if (GUILayout.Button("No Snow"))
        {
            ter.NoSnow();
        }

        if (GUILayout.Button("Mild Snow"))
        {
            ter.MildSnow();
        }

        if (GUILayout.Button("Medium Snow"))
        {
            ter.MediumSnow();
        }

        if (GUILayout.Button("Heavy Snow"))
        {
            ter.HeavySnow();
        }

        if (GUILayout.Button("Mild Snow Storm"))
        {
            ter.MildSnowStorm();
        }

        if (GUILayout.Button("Medium Snow Storm"))
        {
            ter.MediumSnowStorm();
        }

        if (GUILayout.Button("Heavy Snow Storm"))
        {
            ter.HeavySnowStorm();
        }

        if (GUILayout.Button("Mild Storm"))
        {
            ter.MildStorm();
        }

        if (GUILayout.Button("Medium Storm"))
        {
            ter.MediumStorm();
        }

        if (GUILayout.Button("Heavy Storm"))
        {
            ter.HeavyStorm();
        }
    }
}
