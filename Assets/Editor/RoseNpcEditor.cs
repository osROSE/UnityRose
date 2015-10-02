using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(RoseNpc))]
public class RoseNpcEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUI.changed)
        {
            (target as RoseNpc).UpdateModels();
        }
    }

}
