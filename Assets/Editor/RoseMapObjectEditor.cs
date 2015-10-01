using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(RoseMapObject))]
public class RoseMapObjectEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUI.changed)
        {
            (target as RoseMapObject).UpdateModels();
        }
    }

}
