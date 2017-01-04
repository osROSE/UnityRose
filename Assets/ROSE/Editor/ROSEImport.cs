using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class ROSEImport {

    public class Importer
    {
        public class ImportItem
        {
            public bool _isLoaded = false;
            public bool _isImported = false;
            public int _importPriority = -1;
            public string _targetPath = "";

            public void Load()
            {
                if (_isLoaded)
                    return;

                DoLoad();

                // If DoLoad throws, this will never be called...
                _isLoaded = true;
            }

            public void Import(string targetPath, bool force = false)
            {
                if (_isImported)
                    return;

                if (!File.Exists(targetPath) || force)
                    DoImport(targetPath);

                // If DoImport throws, this will never be called
                _isImported = true;
            }

            protected virtual void DoLoad()
            {
            }

            protected virtual void DoImport(string targetPath)
            {
            }

        }

        public class ImportItem<ImportType> : ImportItem where ImportType : UnityEngine.Object
        {
            public ImportType GetData()
            {
                return AssetDatabase.LoadAssetAtPath<ImportType>(_targetPath);
            }
        }

        public TextureImport MakeTexture(string path)
        {
            var targetPath = GenerateAssetPath(path, Path.GetExtension(path));
            var tex = GetItem<TextureImport>(targetPath);
            if (tex != null)
                return tex;

            var newTex = new TextureImport(this, path);
            newTex._targetPath = targetPath;
            AddItem(newTex);
            return newTex;
        }


        public SkeletonImport MakeSkeleton(string path)
        {
            var targetPath = GenerateAssetPath(path, ".skeleton");

            var skel = GetItem<SkeletonImport>(targetPath);
            if (skel != null)
                return skel;

            var newSkel = new SkeletonImport(this, path);
            newSkel._targetPath = targetPath;
            AddItem(newSkel);
            return newSkel;
        }

        public MeshImport MakeMesh(string path, SkeletonImport skeleton)
        {
            var targetPath = GenerateAssetPath(path, ".mesh.asset");
            if (targetPath.Equals("Assets/GameData/NPC/ETC/archeryTarget/archeryTarget.mesh.asset", System.StringComparison.CurrentCultureIgnoreCase))
            {
                if (skeleton._targetPath.Equals("Assets/GameData/NPC/ETC/archeryTarget/archeryTargetE.skeleton", System.StringComparison.CurrentCultureIgnoreCase))
                    targetPath = "Assets/GameData/NPC/ETC/archeryTarget/archeryTargetE.mesh.asset";
                else if (skeleton._targetPath.Equals("Assets/GameData/NPC/ETC/archeryTarget/archeryTargetNW.skeleton", System.StringComparison.CurrentCultureIgnoreCase))
                    targetPath = "Assets/GameData/NPC/ETC/archeryTarget/archeryTargetNW.mesh.asset";
                else if (skeleton._targetPath.Equals("Assets/GameData/NPC/ETC/archeryTarget/archeryTargetSW.skeleton", System.StringComparison.CurrentCultureIgnoreCase))
                    targetPath = "Assets/GameData/NPC/ETC/archeryTarget/archeryTargetSW.mesh.asset";
            }

            var mesh = GetItem<MeshImport>(targetPath);
            if (mesh != null)
            {
                if (mesh.Skeleton == skeleton)
                    return mesh;

                // Check if the hierarchy matches
                mesh.Skeleton.Load();
                skeleton.Load();
                if (mesh.Skeleton.HierarchyMatches(skeleton))
                    return mesh;

                Debug.LogWarning("Mesh " + targetPath + " used with multiple skeletons with differing hierarchies " + mesh.Skeleton._targetPath + "," + skeleton._targetPath);
            }

            var newMesh = new MeshImport(this, path, skeleton);
            newMesh._targetPath = targetPath;
            AddItem(newMesh);
            return newMesh;
        }

        public BoneAnimationImport MakeBoneAnimation(string path, SkeletonImport skeleton)
        {
            var targetPath = GenerateAssetPath(path, ".anim.asset");
            if (skeleton._targetPath.Equals("Assets/GameData/NPC/ANIMAL/LARVA_MOUNT/larva_mount.skeleton", System.StringComparison.CurrentCultureIgnoreCase))
            {
                if (targetPath.Equals("Assets/GameData/MOTION/NPC/larva/larva_attack.anim.asset", System.StringComparison.CurrentCultureIgnoreCase))
                    targetPath = "Assets/GameData/MOTION/NPC/larva_mount/larva_attack.anim.asset";
                else if (targetPath.Equals("Assets/GameData/MOTION/NPC/larva/larva_hit.anim.asset", System.StringComparison.CurrentCultureIgnoreCase))
                    targetPath = "Assets/GameData/MOTION/NPC/larva_mount/larva_hit.anim.asset";
                else if (targetPath.Equals("Assets/GameData/MOTION/NPC/larva/larva_die.anim.asset", System.StringComparison.CurrentCultureIgnoreCase))
                    targetPath = "Assets/GameData/MOTION/NPC/larva_mount/larva_die.anim.asset";
            }

            var anim = GetItem<BoneAnimationImport>(targetPath);
            if (anim != null)
            {
                if (anim.Skeleton == skeleton)
                    return anim;

                // Check if the hierarchy matches
                anim.Skeleton.Load();
                skeleton.Load();
                if (anim.Skeleton.HierarchyMatches(skeleton))
                    return anim;

                // Check if the hierarchy is close enough to match the animation
                anim.Load();
                if (anim.Skeleton.HierarchyMatches(skeleton, anim))
                    return anim;

                Debug.LogWarning("Animation " + targetPath + " used with multiple skeletons with differing hierarchies " + anim.Skeleton._targetPath + "," + skeleton._targetPath);
            }

            var newAnim = new BoneAnimationImport(this, path, skeleton);
            newAnim._targetPath = targetPath;
            AddItem(newAnim);
            return newAnim;
        }


        public ParticleSystemImport MakeParticleSystem(string path)
        {
            var targetPath = GenerateAssetPath(path, ".ps.asset");

            var ps = GetItem<ParticleSystemImport>(targetPath);
            if (ps != null)
                return ps;

            var newPs = new ParticleSystemImport(this, path);
            newPs._targetPath = targetPath;
            AddItem(newPs);
            return newPs;
        }

        public List<ImportItem> _items = new List<ImportItem>();
        public Dictionary<string, ImportItem> _itemTargets = new Dictionary<string, ImportItem>();
        public void AddItem(ImportItem item)
        {
            if (item._targetPath == "")
            {
                Debug.LogWarning("Item of type " + item.GetType().Name + " added to import list with no TargetPath.");
                return;
            }

            var targetPath = item._targetPath.ToLower();
            if (_itemTargets.ContainsKey(targetPath))
            {
                Debug.LogWarning("Multiple items with targetPath of " + item._targetPath);
                return;
            }

            _items.Add(item);
            _itemTargets.Add(targetPath, item);
        }

        public ImportType GetItem<ImportType>(string targetPath) where ImportType : ImportItem
        {
            ImportItem foundItem;
            if (!_itemTargets.TryGetValue(targetPath.ToLower(), out foundItem))
                return null;

            if (foundItem.GetType() != typeof(ImportType))
                throw new System.Exception("Multiple items with the same targetPath but different types created.");

            return (ImportType)foundItem;
        }

        private void Mark(ImportItem item, List<ImportItem> seen, int depth)
        {
            // Ignore blank items
            if (item == null)
                return;

            // Make sure we don't accidentally have a circular dependance
            if (seen.Contains(item))
                throw new System.Exception("Circular asset dependance detected");

            // If we already have a higher priority for this import,
            //   we don't need to do anything.
            if (item._importPriority > depth)
                return;

            // Update the priority to the current depth
            item._importPriority = depth;

            // Add this item to the scene list till we're done this loop
            seen.Add(item);

            // Recurse all the fields and keep building...
            var props = item.GetType().GetFields();
            for (var i = 0; i < props.Length; ++i)
            {
                var field = props[i];
                if (field.FieldType.IsSubclassOf(typeof(ImportItem)))
                    Mark((ImportItem)field.GetValue(item), seen, depth + 1);
                if (TypeImplements(field.FieldType, typeof(IEnumerable)))
                {
                    var list = (IEnumerable)field.GetValue(item);
                    if (list != null)
                    {
                        foreach (var listItem in list)
                        {
                            var listItemType = listItem.GetType();
                            if (listItemType.IsSubclassOf(typeof(ImportItem)))
                                Mark((ImportItem)listItem, seen, depth + 1);
                        }
                    }
                }
            }

            // Remove me from our seen list and continue
            seen.Remove(item);
        }

        public void Mark(ImportItem item)
        {
            Mark(item, new List<ImportItem>(), 0);
        }

        public void Import()
        {
            Dictionary<string, bool> directoryNames = new Dictionary<string, bool>();
            List<List<ImportItem>> priorityList = new List<List<ImportItem>>();
            for (var i = 0; i < _items.Count; ++i)
            {
                var item = _items[i];

                // Skip items that aren't being imported
                if (item._importPriority < 0)
                    continue;

                if (item._targetPath == "")
                {
                    Debug.LogWarning("Skipping asset of type " + item.GetType().Name + " due to missing target path.");
                    continue;
                }

                // Extend the priority list as needed
                while (priorityList.Count <= item._importPriority)
                    priorityList.Add(new List<ImportItem>());

                // Add to prioritized list
                priorityList[item._importPriority].Add(item);

                // Add Directory to list
                directoryNames[Path.GetDirectoryName(item._targetPath)] = true;
            }

            // Create relevant directories
            foreach (var dirName in directoryNames)
            {
                if (!Directory.Exists(dirName.Key))
                {
                    Debug.Log("Creating directory for import: " + dirName.Key);
                    Directory.CreateDirectory(dirName.Key);
                }
            }

            // Process imports in order
            for (var j = priorityList.Count - 1; j >= 0; --j)
            {
                if (priorityList[j].Count == 0)
                {
                    Debug.Log("Skipping priority " + j + " as it has no items.");
                    continue;
                }

                Debug.Log("Processing imports for priority " + j);

                AssetDatabase.StartAssetEditing();

                var items = priorityList[j];
                for (var i = 0; i < items.Count; ++i)
                {
                    var item = items[i];

                    try
                    {
                        bool forceRefresh = false;

                        if (item.GetType() == typeof(ChrImporter.Character))
                            forceRefresh = true;

                        item.Load();
                        item.Import(item._targetPath, forceRefresh);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogWarning("Failed to import item");
                        Debug.LogWarning(item);
                        Debug.LogWarning(e);
                    }
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.StopAssetEditing();
            }
        }
    }



    public static string GetDataPath(string path)
    {
        return Path.Combine(dataPath, path);
    }

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

    private static void ClearFolder(string name)
    {
        if (Directory.Exists(name))
        {
            Directory.Delete(name, true);
        }

        Directory.CreateDirectory(name);
    }

    public static void ClearData()
    {
        ClearUData();
        ClearFolder("Assets/GameData");
        AssetDatabase.Refresh();
    }

    public static void ClearUData()
    {
        if (Directory.Exists(""))
            ClearFolder("Assets/MapObjects");
        ClearFolder("Assets/NpcParts");
        ClearFolder("Assets/NpcAnims");
        ClearFolder("Assets/NpcModels");
        ClearFolder("Assets/Npcs");
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

    private static bool TypeImplements(System.Type t, System.Type i) {
        foreach (var x in t.GetInterfaces()) {
            if (x == i)
                return true;
        }
        return false;
    }

    public static void ImportAllTiles()
    {
        Debug.Log("Importing All Tiles");

        var importer = new Importer();

        var tiles = new MapTileImport(importer);
        for (var i = 0; i < tiles.Textures.Count; ++i)
        {
            importer.Mark(tiles.Textures[i]);
        }

        importer.Import();
    }

    public static void ImportTest()
    {
        Debug.Log("Importing Stuff");

        var importer = new Importer();

        var cnst = new MapZscImport(importer, "3DDATA/JUNON/LIST_CNST_JPT.ZSC");
        for (var i = 0; i < System.Math.Min(10, cnst.Models.Count); ++i)
        {
            importer.Mark(cnst.Models[i]);
        }

        var deco = new MapZscImport(importer, "3DDATA/JUNON/LIST_DECO_JPT.ZSC");
        for (var i = 0; i < System.Math.Min(10, deco.Models.Count); ++i)
        {
            importer.Mark(deco.Models[i]);
        }

        importer.Import();
    }

    public static void ImportMap(int mapIdx)
    {
        Debug.Log("Importing Map #" + mapIdx);

        var importer = new Importer();

        var decoPath = mapListData.stb.Cells[mapIdx][12];
        var decoImp = new MapZscImport(importer, decoPath);
        for (var i = 0; i < decoImp.Models.Count; ++i)
            importer.Mark(decoImp.Models[i]);

        var cnstPath = mapListData.stb.Cells[mapIdx][13];
        var cnstImp = new MapZscImport(importer, cnstPath);
        for (var i = 0; i < cnstImp.Models.Count; ++i)
            importer.Mark(cnstImp.Models[i]);

        importer.Import();
    }

    private static string GenerateAssetPath(string rosePath, string unityExt)
    {
        rosePath = Utils.NormalizePath(rosePath);

        var dirPath = Path.GetDirectoryName(rosePath);
        if (!dirPath.StartsWith("3DDATA", System.StringComparison.InvariantCultureIgnoreCase))
        {
            throw new System.Exception("dirPath does not begin with 3DDATA :: " + dirPath);
        }
        var pathName = dirPath.Substring(7);
        var meshName = Path.GetFileNameWithoutExtension(rosePath);

        return Utils.CombinePath("Assets/GameData", pathName, meshName + unityExt);
    }

    /*
    public class RoseSkeletonInfo
    {
        public GameObject RootBone;
        public GameObject[] Bones;
        public GameObject[] Dummies;
        public Transform[] BoneTransforms;
        public Matrix4x4[] BindPose;
    }

    public static RoseSkeletonInfo ImportSkeleton(string path)
    {
        var fullPath = Utils.CombinePath(dataPath, path);
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("Could not find referenced skeleton: " + fullPath);
            return null;
        }

        var zmd = new ZMD(fullPath);
        var info = new RoseSkeletonInfo();

        info.Bones = new GameObject[zmd.bones.Count];
        for (var i = 0; i < zmd.bones.Count; ++i)
        {
            var bone = new GameObject();
            var zmdBone = zmd.bones[i];

            bone.name = "Bone_" + i;
            if (i != 0)
                bone.transform.parent = info.Bones[zmdBone.ParentID].transform;
            bone.transform.localPosition = zmdBone.Position;
            bone.transform.localRotation = zmdBone.Rotation;

            info.Bones[i] = bone;
        }

        /*
        info.Dummies = new GameObject[zmd.dummies.Count];
        for (var i = 0; i < zmd.dummies.Count; ++i)
        {
            var bone = new GameObject();
            var zmdBone = zmd.dummies[i];

            bone.name = "Dummy_" + i;
            bone.transform.parent = info.Bones[zmdBone.ParentID].transform;
            bone.transform.localPosition = zmdBone.Position;
            bone.transform.localRotation = zmdBone.Rotation;

            info.Dummies[i] = bone;
        }
        /

        info.BindPose = new Matrix4x4[zmd.bones.Count];
        for (var i = 0; i < zmd.bones.Count; ++i)
        {
            var bone = zmd.bones[i];

            Matrix4x4 myMat = Matrix4x4.TRS(
                bone.Position,
                bone.Rotation,
                Vector3.one);

            if (i == 0)
            {
                info.BindPose[i] = myMat;
            }
            else
            {
                info.BindPose[i] = info.BindPose[bone.ParentID] * myMat;
            }
        }
        info.BoneTransforms = new Transform[zmd.bones.Count];
        for (var i= 0; i < zmd.bones.Count; ++i)
        {
            info.BindPose[i] = info.BindPose[i].inverse;
            info.BoneTransforms[i] = info.Bones[i].transform;
        }

        info.RootBone = info.Bones[0];
        return info;
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

    public static AnimationClip ImportAnimation(string path, GameObject[] bones)
    {
        var fullPath = Utils.CombinePath(dataPath, path);
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("Could not find referenced animation: " + fullPath);
            return null;
        }

        var animPath = GenerateAssetPath(path, ".anim");
        //if (!File.Exists(animPath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(animPath));

            List<string> boneNames = new List<string>();
            for (var i = 0; i < bones.Length; ++i)
            {
                GameObject bone = bones[i];
                string boneName = bone.name;
                if (bone != bones[0])
                {
                    while (bone)
                    {
                        if (!bone.transform.parent) break;
                        bone = bone.transform.parent.gameObject;
                        boneName = bone.name + "/" + boneName;
                        if (bone == bones[0]) break;
                    }
                }
                boneNames.Add(boneName);
            }

            Debug.Log("Bone Names:");
            for (var i = 0; i < boneNames.Count; ++i)
            {
                Debug.Log(boneNames[i]);
            }

            var zmo = new ZMO(fullPath);
            var anim = zmo.BuildSkeletonAnimationClip(boneNames.ToArray());
            AssetDatabase.CreateAsset(anim, animPath);
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

        public Object ImportNpc(int npcIdx)
        {
            if (!chr.Characters[npcIdx].IsEnabled)
            {
                return null;
            }

            var npcPath = Utils.CombinePath(targetPath, "Npc_" + npcIdx + ".prefab");
            //if (!File.Exists(npcPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(npcPath));

                var chrObj = chr.Characters[npcIdx];
                var rootGo = new GameObject();
                rootGo.name = "Character_" + npcIdx;

                var skelFile = chr.SkeletonFiles[chrObj.ID];
                var skelInfo = ImportSkeleton(skelFile);

                skelInfo.RootBone.transform.parent = rootGo.transform;
                skelInfo.RootBone.AddComponent<BoneDebug>();

                for (var i = 0; i < chrObj.Objects.Count; ++i)
                {
                    var zscPart = chrObj.Objects[i];
                    zsc.ImportPart(zscPart.Object, rootGo, skelInfo);
                }

                Animation animC = rootGo.AddComponent<Animation>();
                animC.wrapMode = WrapMode.Loop;

                for (var i = 0; i < chrObj.Animations.Count; ++i)
                {
                    var zscMotion = chrObj.Animations[i];
                    if (zscMotion.Animation >= 0)
                    {
                        var anim = ImportAnimation(chr.MotionFiles[zscMotion.Animation], skelInfo.Bones);
                        animC.AddClip(anim, "Anim_" + i);
                        if (animC.clip == null)
                        {
                            animC.clip = anim;
                        }
                    }
                }

                PrefabUtility.CreatePrefab(npcPath, rootGo);
                GameObject.DestroyImmediate(rootGo);
            }
            return AssetDatabase.LoadAssetAtPath<Object>(npcPath);
        }

    }
    */

    private static Regex mapZsc = new Regex("3DDATA/([A-Z]*)/LIST_([A-Z_]*).ZSC", RegexOptions.IgnoreCase);
    private static Regex npcZsc = new Regex("(.*)/PART_NPC.ZSC", RegexOptions.IgnoreCase);

    public static string GenerateZscBasePath(string path)
    {
        path = Utils.NormalizePath(path);

        var mapMatches = mapZsc.Match(path);
        var npcMatches = npcZsc.Match(path);
        if (npcMatches.Success)
        {
            return Utils.NormalizePath("Assets/NpcParts");
        }
        else if (mapMatches.Success)
        {
            var baseName = mapMatches.Groups[1].Value;
            var dbName = mapMatches.Groups[2].Value;
            if (baseName == "AVATAR")
            {
                return Utils.CombinePath("Assets/CharParts", dbName);
            }
            else
            {
                return Utils.CombinePath("Assets/MapObjects", baseName, dbName);
            }
        }
        else
        {
            throw new System.Exception("Unexpected ZSC name...");
        }
    }

    /*
    public class ZscImporter
    {
        private string targetPath = "";
        private ZSC zsc = null;

        public ZscImporter(string path)
        {
            targetPath = GenerateZscBasePath(path);

            zsc = new ZSC(Utils.CombinePath(dataPath, path));
        }

        public void ImportPart(int partIdx, GameObject rootGo, RoseSkeletonInfo skelInfo)
        {
            var zscObj = zsc.Objects[partIdx];

            for (int j = 0; j < zscObj.Models.Count; ++j)
            {
                var part = zscObj.Models[j];

                var mesh = ImportMesh(part.ModelID);
                var material = ImportMaterial(part.TextureID);

                mesh.bindposes = skelInfo.BindPose;

                var go = new GameObject();
                go.name = "Part_" + partIdx + "_" + j;
                go.transform.localPosition = part.Position;
                go.transform.localRotation = part.Rotation;
                go.transform.localScale = part.Scale;
                go.transform.parent = rootGo.transform;

                var smr = go.AddComponent<SkinnedMeshRenderer>();
                smr.sharedMesh = mesh;
                smr.sharedMaterial = material;
                smr.bones = skelInfo.BoneTransforms;
            }
        }

        public Object ImportObject(int objectIdx)
        {
            var objPath = Utils.CombinePath(targetPath, "Obj_" + objectIdx + ".prefab");
            //if (!File.Exists(objPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(objPath));

                var zscObj = zsc.Objects[objectIdx];

                var mdls = new GameObject[zscObj.Models.Count];
                for (var i = 0; i < zscObj.Models.Count; ++i)
                {
                    var part = zscObj.Models[i];

                    var mesh = ImportMesh(part.ModelID);
                    var material = ImportMaterial(part.TextureID);

                    var go = new GameObject();
                    go.name = "Part_" + i;

                    var mf = go.AddComponent<MeshFilter>();
                    mf.mesh = mesh;

                    var mr = go.AddComponent<MeshRenderer>();
                    mr.material = material;

                    go.transform.localPosition = part.Position / 100;
                    go.transform.localRotation = part.Rotation;
                    go.transform.localScale = part.Scale;

                    mdls[i] = go;
                    if (i != 0)
                        go.transform.parent = mdls[part.Parent - 1].transform;
                }

                PrefabUtility.CreatePrefab(objPath, mdls[0]);
                GameObject.DestroyImmediate(mdls[0]);
            }
            return AssetDatabase.LoadAssetAtPath<Object>(objPath);
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
    */

    /*
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
    */
}
