using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(RoseNpcData))]
public class RoseNpcDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var tgt = target as RoseNpcData;

        tgt.Model = (GameObject)EditorGUILayout.ObjectField("Model", tgt.Model, typeof(GameObject), false);
        tgt.NpcName = EditorGUILayout.TextField("Name", tgt.NpcName);
    }

}
