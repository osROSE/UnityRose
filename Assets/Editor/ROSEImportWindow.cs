using UnityEngine;
using UnityEditor;
using System.Collections;

public class ROSEImportWindow : EditorWindow {

    private const string DataPathKey = "ROSE_DataPath";

    private string dataPath = "";

    [MenuItem("ROSE/Import Window")]
    static void Init() {
        var window = GetWindow<ROSEImportWindow>();
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        dataPath = EditorGUILayout.TextField("3DData Path", dataPath);
    }

    void OnFocus()
    {
        if (EditorPrefs.HasKey(DataPathKey))
        {
            dataPath = EditorPrefs.GetString(DataPathKey);
        }
    }

    void OnLostFocus()
    {
        EditorPrefs.SetString(DataPathKey, dataPath);
    }

    void OnDestroy()
    {
        EditorPrefs.SetString(DataPathKey, dataPath);
    }

    static string GetDataPath() {
        var window = GetWindow<ROSEImportWindow>();
        if (window.dataPath != "")
        {
            return window.dataPath;
        }
        else
        {
            return EditorPrefs.GetString(DataPathKey);
        }
    }
}
