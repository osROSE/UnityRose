using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;

public class ROSEImportWindow : EditorWindow {

    private const string DataPathKey = "ROSE_DataPath";

    private bool wasEditing = false;
    private string dataPath = "";

    [MenuItem("ROSE/Import Window")]
    static void Init() {
        var window = GetWindow<ROSEImportWindow>();
        window.Show();
    }

    private Vector2 mapListScrollPosition;
    private bool mapListShowUnnamed = false;
    void OnGUI()
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        dataPath = EditorGUILayout.TextField("3DData Path", dataPath);

        GUILayout.Label("Importing", EditorStyles.boldLabel);
        GUILayout.Label("Current Path: " + ROSEImport.GetCurrentPath());


        GUILayout.Label("Maps", EditorStyles.boldLabel);

        mapListShowUnnamed = GUILayout.Toggle(mapListShowUnnamed, "Show Unnamed Maps");

        var mapData = ROSEImport.GetMapListData();
        if (mapData != null)
        {
            mapListScrollPosition = GUILayout.BeginScrollView(mapListScrollPosition, GUILayout.Height(400));
            for (var i = 0; i < mapData.stb.Cells.Count; ++i)
            {
                string mapName = mapData.stl.GetString(mapData.stb.Cells[i][27], STL.Language.English);
                if (mapName != null || mapListShowUnnamed)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("[" + i.ToString() + "] " + mapName);
                    if (GUILayout.Button("Import", GUILayout.Width(100)))
                    {
                        ROSEImport.ImportMap(i);
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();
        }


        if (EditorGUIUtility.editingTextField)
        {
            wasEditing = true;
        }
        else
        {
            if (wasEditing)
            {
                wasEditing = false;
                EditorPrefs.SetString(DataPathKey, dataPath);
                ROSEImport.MaybeUpdate();
            }
        }
    }

    void OnFocus()
    {
        if (EditorPrefs.HasKey(DataPathKey))
        {
            dataPath = EditorPrefs.GetString(DataPathKey);
        }
        ROSEImport.MaybeUpdate();
    }

    void OnLostFocus()
    {
        EditorPrefs.SetString(DataPathKey, dataPath);
        ROSEImport.MaybeUpdate();
    }

    void OnDestroy()
    {
        EditorPrefs.SetString(DataPathKey, dataPath);
        ROSEImport.MaybeUpdate();
    }

    public static string GetDataPath() {
        return EditorPrefs.GetString(DataPathKey);
    }

}
