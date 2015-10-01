using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class ROSEImport {

    private static string dataPath = "";

    public class MapListData
    {
        public STB stb;
        public STL stl;
    }
    private static MapListData mapListData = null;

    public static void MaybeUpdate()
    {
        string curDataPath = ROSEImportWindow.GetDataPath();
        if (curDataPath != dataPath)
        {
            dataPath = curDataPath;
            Update();
        }
    }

    private static void Update()
    {
        var md = new MapListData();
        md.stb = new STB(Utils.CombinePath(dataPath, "3DDATA/STB/LIST_ZONE.STB"));
        md.stl = new STL(Utils.CombinePath(dataPath, "3DDATA/STB/LIST_ZONE_S.STL"));
        mapListData = md;
    }

    public static string GetCurrentPath()
    {
        return dataPath;
    }

    public static MapListData GetMapListData()
    {
        return mapListData;
    }

    public static void ImportMap(int mapIdx)
    {
        Debug.Log("Importing Map #" + mapIdx);

        AssetHelper.StartAssetEditing();
        try
        {
            var importer = new ZscImporter("3DDATA\\JUNON\\LIST_CNST_JPT.ZSC");

            for (var i = 1; i < 10; ++i)
            {
                importer.ImportObject(i);
            }
        } finally
        {
            AssetHelper.StopAssetEditing();
        }
    }

    public class ZscImporter
    {
        private string planetName = "";
        private string dbName = "";
        private ZSC zsc = null;

        public ZscImporter(string path)
        {
            planetName = Path.GetDirectoryName(path);
            if (!planetName.StartsWith("3DDATA", System.StringComparison.InvariantCultureIgnoreCase))
            {
                throw new System.Exception("Unexpected ZSC name...");
            }
            planetName = planetName.Substring(7);
            var fileName = Path.GetFileNameWithoutExtension(path);
            if (!fileName.StartsWith("LIST_", System.StringComparison.InvariantCultureIgnoreCase))
            {
                throw new System.Exception("Unexpected ZSC name...");
            }
            dbName = fileName.Substring(5);
            
            zsc = new ZSC(Utils.CombinePath(dataPath, path));
        }

        public RoseMapObjectData ImportObject(int objectIdx)
        {
            var objPath = Utils.CombinePath("Assets/MapObjects", planetName, dbName, "Obj_" + objectIdx + ".asset");
            //if (!File.Exists(objPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(objPath));

                var zscObj = zsc.Objects[objectIdx];
                var mdl = ScriptableObject.CreateInstance<RoseMapObjectData>();

                for (int j = 0; j < zscObj.Models.Count; ++j)
                {
                    var part = zscObj.Models[j];
                    var subObj = new RoseMapObjectData.SubObject();

                    subObj.mesh = ImportMesh(part.ModelID);
                    subObj.material = ImportMaterial(part.TextureID);
                    subObj.animation = null;
                    subObj.parent = part.Parent;
                    subObj.position = part.Position / 100;
                    subObj.rotation = part.Rotation;
                    subObj.scale = part.Scale;

                    /*
                    if (part.CollisionLevel == ZSC.CollisionLevelType.None)
                    {
                        subObj.colMode = 0;
                    }
                    else
                    {
                        subObj.colMode = 1;
                    }

                    if (part.AnimationFilePath != "")
                    {
                        var animPath = _basePath + "Anim_" + i.ToString() + "_" + j.ToString() + ".asset";
                        var clip = ImportNodeAnimation(animPath, part.AnimationFilePath);
                        subObj.animation = clip;
                    }
                    */

                    mdl.subObjects.Add(subObj);
                }

                AssetDatabase.CreateAsset(mdl, objPath);
                EditorUtility.SetDirty(mdl);
            }
            return AssetDatabase.LoadAssetAtPath<RoseMapObjectData>(objPath);
        }

        public Mesh ImportMesh(int meshIdx)
        {
            return ImportMesh(zsc.Models[meshIdx]);
        }

        public Material ImportMaterial(int materialIdx)
        {
            var zscMat = zsc.Textures[materialIdx];
            
            var matPath = Utils.CombinePath("Assets/MapObjects", planetName, dbName, "/Materials/Mat_" + materialIdx + ".asset");
            if (!File.Exists(matPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(matPath));

                var mat = new Material(Shader.Find("Custom/ObjectShader"));
                AssetDatabase.CreateAsset(mat, matPath);

                ImportTexture(zscMat.Path, () =>
                {
                    Texture2D mainTex = ImportTexture(zscMat.Path);
                    mat.SetTexture("_MainTex", mainTex);
                });
            }
            return AssetDatabase.LoadAssetAtPath<Material>(matPath);
        }

        private static string GenerateAssetPath(string rosePath, string unityExt)
        {
            var dirPath = Path.GetDirectoryName(rosePath);
            if (!dirPath.StartsWith("3DDATA", System.StringComparison.InvariantCultureIgnoreCase))
            {
                throw new System.Exception("dirPath does not begin with 3DDATA :: " + dirPath);
            }
            var pathName = dirPath.Substring(7);
            var meshName = Path.GetFileNameWithoutExtension(rosePath);

            return Utils.CombinePath("Assets/GameData", pathName, meshName + unityExt);
        }

        public static Mesh ImportMesh(string path)
        {
            var fullPath = Utils.CombinePath(dataPath, path);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Could not find referenced mesh: " + fullPath);
                return null;
            }

            var meshPath = GenerateAssetPath(path, ".mesh.asset");
            if (!File.Exists(meshPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(meshPath));

                var zms = new ZMS(fullPath);
                AssetDatabase.CreateAsset(zms.getMesh(), meshPath);
            }
            return AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
        }

        public static Texture2D ImportTexture(string path, AssetHelper.ImportDone doneFn = null)
        {
            var fullPath = Utils.CombinePath(dataPath, path);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Could not find referenced texture: " + fullPath);
                return null;
            }

            var texPath = GenerateAssetPath(path, Path.GetExtension(path));
            if (!File.Exists(texPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(texPath));
                File.Copy(fullPath, texPath);
                AssetHelper.ImportTexture(texPath, doneFn);
            }
            return AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
        }
    }

    public class AssetHelper
    {
        public delegate void ImportDone();

        public static void StartAssetEditing()
        {
            AssetDatabase.StartAssetEditing();
        }

        public static void StopAssetEditing()
        {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.StartAssetEditing();
            foreach (var lateImport in lateImportList)
            {
                lateImport();
            }
            lateImportList.Clear();
            AssetDatabase.SaveAssets();
            AssetDatabase.StopAssetEditing();
        }

        public static void ImportTexture(string path, ImportDone doneFn = null)
        {
            AssetDatabase.ImportAsset(path);
            if (doneFn != null)
                lateImportList.Add(doneFn);
        }

        public static List<ImportDone> lateImportList = new List<ImportDone>();
    }

}
