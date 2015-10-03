using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

    public static void ClearData()
    {
        ClearUData();
        Directory.Delete("Assets/GameData", true);
        AssetDatabase.Refresh();
    }

    public static void ClearUData()
    {
        Directory.Delete("Assets/MapObjects", true);
        Directory.Delete("Assets/NpcParts", true);
        Directory.Delete("Assets/Npcs", true);
        AssetDatabase.Refresh();
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
            var importer = new ChrImporter();
            for (var i = 0; i < 10; ++i)
            {
                importer.ImportNpc(i);
            }

            //var importerX = new ZscImporter("3DDATA\\JUNON\\LIST_CNST_JPT.ZSC");

            for (var i = 1; i < 10; ++i)
            {
                //importerX.ImportObject(i);
            }
        } finally
        {
            AssetHelper.StopAssetEditing();
        }
    }


    private static string GenerateAssetPath(string rosePath, string unityExt)
    {
		rosePath = Utils.NormalizePath (rosePath);

        var dirPath = Path.GetDirectoryName(rosePath);
        if (!dirPath.StartsWith("3DDATA", System.StringComparison.InvariantCultureIgnoreCase))
        {
            throw new System.Exception("dirPath does not begin with 3DDATA :: " + dirPath);
        }
        var pathName = dirPath.Substring(7);
        var meshName = Path.GetFileNameWithoutExtension(rosePath);

        return Utils.CombinePath("Assets/GameData", pathName, meshName + unityExt);
    }

    public static RoseSkeletonData ImportSkeleton(string path)
    {
        var fullPath = Utils.CombinePath(dataPath, path);
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("Could not find referenced skeleton: " + fullPath);
            return null;
        }

        var skelPath = GenerateAssetPath(path, ".skel.asset");
        if (!File.Exists(skelPath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(skelPath));

            var zmd = new ZMD(fullPath);
            var skel = ScriptableObject.CreateInstance<RoseSkeletonData>();

            for (var i = 0; i < zmd.bones.Count; ++i)
            {
                var zmdBone = zmd.bones[i];
                var bone = new RoseSkeletonData.Bone();
                bone.name = zmdBone.Name;
                bone.parent = zmdBone.ParentID;
                bone.translation = zmdBone.Position;
                bone.rotation = zmdBone.Rotation;
                skel.bones.Add(bone);
            }

            for (var i = 0; i < zmd.dummies.Count; ++i)
            {
                var zmdBone = zmd.dummies[i];
                var bone = new RoseSkeletonData.Bone();
                bone.name = zmdBone.Name;
                bone.parent = zmdBone.ParentID;
                bone.translation = zmdBone.Position;
                bone.rotation = zmdBone.Rotation;
                skel.dummies.Add(bone);
            }

            AssetDatabase.CreateAsset(skel, skelPath);
            EditorUtility.SetDirty(skel);
            AssetDatabase.SaveAssets();
        }
        return AssetDatabase.LoadAssetAtPath<RoseSkeletonData>(skelPath);
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
            var mesh = zms.getMesh();
            AssetDatabase.CreateAsset(mesh, meshPath);
            return mesh;
        }
        return AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
    }

    public static AnimationClip ImportAnimation(string path, RoseSkeletonData skeleton)
    {
        var fullPath = Utils.CombinePath(dataPath, path);
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("Could not find referenced animation: " + fullPath);
            return null;
        }

        var animPath = GenerateAssetPath(path, ".anim.asset");
        //if (!File.Exists(animPath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(animPath));

            var zmo = new ZMO(fullPath);
            var anim = zmo.BuildSkeletonAnimationClip(skeleton);
            AssetDatabase.CreateAsset(anim, animPath);
            return anim;
        }
        return AssetDatabase.LoadAssetAtPath<AnimationClip>(animPath);
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
        else
        {
            AssetHelper.Delay(doneFn);
        }
        return AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
    }

    public class ChrImporter
    {
        private string targetPath = "";
        private CHR chr = null;
        private ZscImporter zsc = null;

        public ChrImporter()
        {
            targetPath = "Assets/Npcs";
            chr = new CHR(Utils.CombinePath(dataPath, "3DDATA/NPC/LIST_NPC.CHR"));
            zsc = new ZscImporter("3DDATA/NPC/PART_NPC.ZSC");
        }

        public RoseNpcData ImportNpc(int npcIdx)
        {
            if (!chr.Characters[npcIdx].IsEnabled)
            {
                return null;
            }

            var npcPath = Utils.CombinePath(targetPath, "Npc_" + npcIdx + ".asset");
            //if (!File.Exists(npcPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(npcPath));

                var chrObj = chr.Characters[npcIdx];
                var npc = ScriptableObject.CreateInstance<RoseNpcData>();

                var skelFile = chr.SkeletonFiles[chrObj.ID];
                npc.skeleton = ImportSkeleton(skelFile);

                for (var i = 0; i < chrObj.Objects.Count; ++i)
                {
                    var zscPart = chrObj.Objects[i];
                    var partData = zsc.ImportPart(zscPart.Object);
                    Debug.Log(zscPart.Object);
                    Debug.Log(partData);
                    npc.parts.Add(partData);
                }

                for (var i = 0; i < chrObj.Animations.Count; ++i)
                {
                    var zscMotion = chrObj.Animations[i];
                    if (zscMotion.Animation >= 0)
                    {
                        var anim = ImportAnimation(chr.MotionFiles[zscMotion.Animation], npc.skeleton);
						while (npc.animations.Count <= (int)zscMotion.Type) {
							npc.animations.Add(null);
						}
						npc.animations[(int)zscMotion.Type] = anim;
                    }
                }

                AssetDatabase.CreateAsset(npc, npcPath);
                EditorUtility.SetDirty(npc);
                AssetDatabase.SaveAssets();
            }
            return AssetDatabase.LoadAssetAtPath<RoseNpcData>(npcPath);
        }

    }

    public class ZscImporter
    {
        private string targetPath = "";
        private ZSC zsc = null;

        private static Regex mapZsc = new Regex("3DDATA/([A-Z]*)/LIST_([A-Z_]*).ZSC", RegexOptions.IgnoreCase);
        private static Regex npcZsc = new Regex("(.*)/PART_NPC.ZSC", RegexOptions.IgnoreCase);

        public ZscImporter(string path)
        {
            path = Utils.NormalizePath(path);

            var mapMatches = mapZsc.Match(path);
            var npcMatches = npcZsc.Match(path);
            if (npcMatches.Success)
            {
                targetPath = Utils.NormalizePath("Assets/NpcParts");
            } else  if (mapMatches.Success)
            {
                var baseName = mapMatches.Groups[1].Value;
                var dbName = mapMatches.Groups[2].Value;
                if (baseName == "AVATAR")
                {
                    targetPath = Utils.CombinePath("Assets/CharParts", dbName);
                }
                else
                {
                    targetPath = Utils.CombinePath("Assets/MapObjects", baseName, dbName);
                }
            }
            else
            {
                throw new System.Exception("Unexpected ZSC name...");
            }

            zsc = new ZSC(Utils.CombinePath(dataPath, path));
        }

        public RoseCharPartData ImportPart(int partIdx)
        {
            var partPath = Utils.CombinePath(targetPath, "Obj_" + partIdx + ".asset");
            if (!File.Exists(partPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(partPath));

                var zscObj = zsc.Objects[partIdx];
                var mdl = ScriptableObject.CreateInstance<RoseCharPartData>();

                for (int j = 0; j < zscObj.Models.Count; ++j)
                {
                    var part = zscObj.Models[j];
                    var subObj = new RoseCharPartData.Model();

                    subObj.mesh = ImportMesh(part.ModelID);
                    subObj.material = ImportMaterial(part.TextureID);
                    subObj.boneIndex = -1;

                    mdl.models.Add(subObj);
                }

                AssetDatabase.CreateAsset(mdl, partPath);
                EditorUtility.SetDirty(mdl);
                AssetDatabase.SaveAssets();
            }
            return AssetDatabase.LoadAssetAtPath<RoseCharPartData>(partPath);
        }

        public RoseMapObjectData ImportObject(int objectIdx)
        {
            var objPath = Utils.CombinePath(targetPath, "Obj_" + objectIdx + ".asset");
            if (!File.Exists(objPath))
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
                AssetDatabase.SaveAssets();
            }
            return AssetDatabase.LoadAssetAtPath<RoseMapObjectData>(objPath);
        }

        public Mesh ImportMesh(int meshIdx)
        {
            return ROSEImport.ImportMesh(zsc.Models[meshIdx]);
        }

        public Material ImportMaterial(int materialIdx)
        {
            var zscMat = zsc.Textures[materialIdx];
            
            var matPath = Utils.CombinePath(targetPath, "Materials", "Mat_" + materialIdx + ".asset");
            if (!File.Exists(matPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(matPath));

                var mat = new Material(Shader.Find("Standard"));
                AssetDatabase.CreateAsset(mat, matPath);

                ImportTexture(zscMat.Path, () =>
                {
                    Texture2D mainTex = ImportTexture(zscMat.Path);
                    mat.SetTexture("_MainTex", mainTex);
                    EditorUtility.SetDirty(mat);
                });
            }
            return AssetDatabase.LoadAssetAtPath<Material>(matPath);
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

        public static void Delay(ImportDone doneFn = null)
        {
            if (doneFn != null)
                lateImportList.Add(doneFn);
        }

        public static List<ImportDone> lateImportList = new List<ImportDone>();
    }

}
