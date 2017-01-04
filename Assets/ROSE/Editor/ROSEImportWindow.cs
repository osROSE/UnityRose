using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;

public class ROSEImportWindow : EditorWindow {

    private const string DataPathKey = "ROSE_DataPath";

    private bool wasEditing = false;
    private string dataPath = "";

    [MenuItem("ROSE/Import Window")]
    static void Init() {
        var window = GetWindow<ROSEImportWindow>("ROSE Import");
        window.Show();
    }

    [MenuItem("ROSE/Recalculate Normals")]
    static void RecalcNormals()
    {
        var mesh = Selection.activeObject as Mesh;
        if (mesh)
        {
            mesh.RecalculateNormals();
        }
    }

    private Vector2 mapListScrollPosition;
    private bool mapListShowUnnamed = false;
    void OnGUI()
    {
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        dataPath = EditorGUILayout.TextField("3DData Path", dataPath);

        GUILayout.Label("Cleanup", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Clear Unity 3Ddata"))
            ROSEImport.ClearUData();
        if (GUILayout.Button("Clear All 3Ddata"))
            ROSEImport.ClearData();

        GUILayout.EndHorizontal();

        GUILayout.Label("Importing", EditorStyles.boldLabel);
        GUILayout.Label("Current Path: " + ROSEImport.GetCurrentPath());

        var roseDataPath = Path.Combine(ROSEImport.GetCurrentPath(), "3DDATA");
        if (!Directory.Exists(roseDataPath)) {
            GUILayout.Space(10);
            GUILayout.Label("Invalid Path", EditorStyles.centeredGreyMiniLabel);
            GUILayout.Label("(Could not find `" + roseDataPath + "`)", EditorStyles.centeredGreyMiniLabel);
        } else {
            if (true) {
                if (GUILayout.Button("Import All Tiles"))
                    ROSEImport.ImportAllTiles();
            }

            if (true)
            {
                if (GUILayout.Button("Import Test"))
                    ROSEImport.ImportTest();
            }

            if (true) {
                GUILayout.Label("Maps", EditorStyles.boldLabel);

                mapListShowUnnamed = GUILayout.Toggle(mapListShowUnnamed, "Show Unnamed Maps");

                var mapData = ROSEImport.GetMapListData();
                if (mapData != null)
                {
                    mapListScrollPosition = GUILayout.BeginScrollView(mapListScrollPosition, GUILayout.MaxHeight(400));
                    for (var i = 0; i < mapData.stb.Cells.Count; ++i)
                    {
                        string mapName = mapData.stl.GetString(mapData.stb.Cells[i][27], STL.Language.English);
                        if (mapName != null || mapListShowUnnamed)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("[" + i.ToString() + "] " + mapName);
                            if (GUILayout.Button("Import", GUILayout.Width(60)))
                            {
                                ROSEImport.ImportMap(i);
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndScrollView();
                }
            }
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
