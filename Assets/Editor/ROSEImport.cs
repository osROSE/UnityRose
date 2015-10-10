using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class ROSEImport {

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

        public class TextureImport : ImportItem<Texture2D>
        {
            public string SourcePath = "";

            public TextureImport(Importer parent, string path)
            {
                SourcePath = Utils.NormalizePath(path);
            }

            protected override void DoImport(string targetPath)
            {
                File.Copy(GetDataPath(SourcePath), targetPath, true);
                AssetDatabase.ImportAsset(targetPath, ImportAssetOptions.ForceSynchronousImport);
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

        public class SkeletonImport : ImportItem
        {
            public string SourcePath = "";
            public ZMD SkeletonData = null;
            public Matrix4x4[] BindPose = null;
            public string[] BonePaths = null;

            public SkeletonImport(Importer parent, string path)
            {
                SourcePath = path;
            }

            protected override void DoLoad()
            {
                SkeletonData = new ZMD(GetDataPath(SourcePath));

                // Build BindPoses
                var boneTransforms = new Matrix4x4[SkeletonData.bones.Count];
                BindPose = new Matrix4x4[SkeletonData.bones.Count];
                BonePaths = new string[SkeletonData.bones.Count];
                for (var i = 0; i < SkeletonData.bones.Count; ++i)
                {
                    var bone = SkeletonData.bones[i];

                    Matrix4x4 myMat = Matrix4x4.TRS(
                        bone.Position,
                        bone.Rotation,
                        Vector3.one);
                    var boneName = "Bone_" + i;

                    if (i != 0)
                    {
                        myMat = boneTransforms[bone.ParentID] * myMat;
                        boneName = BonePaths[bone.ParentID] + "/" + boneName;
                    }

                    boneTransforms[i] = myMat;
                    BindPose[i] = myMat.inverse;
                    BonePaths[i] = boneName;
                }
            }

            protected override void DoImport(string targetPath)
            {
            }

            public bool HierarchyMatches(SkeletonImport rhs)
            {
                if (SkeletonData.bones.Count != rhs.SkeletonData.bones.Count)
                    return false;

                var lhsD = SkeletonData;
                var rhsD = rhs.SkeletonData;
                for (var i = 0; i < lhsD.bones.Count; ++i)
                {
                    if (lhsD.bones[i].ParentID != rhsD.bones[i].ParentID)
                        return false;
                    if (Vector3.Distance(lhsD.bones[i].Position, rhsD.bones[i].Position) > 0.01)
                        return false;
                    if (lhsD.bones[i].Rotation != rhsD.bones[i].Rotation)
                        return false;
                }

                return true;
            }

            public bool HierarchyMatches(SkeletonImport rhs, BoneAnimationImport anim)
            {
                if (SkeletonData.bones.Count < anim.NumBones)
                    return false;
                if (rhs.SkeletonData.bones.Count < anim.NumBones)
                    return false;

                var lhsD = SkeletonData;
                var rhsD = rhs.SkeletonData;
                for (var i = 0; i < anim.NumBones; ++i)
                {
                    if (lhsD.bones[i].ParentID != rhsD.bones[i].ParentID)
                        return false;
                }

                return true;
            }

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

        public class MeshImport : ImportItem<Mesh>
        {
            public string SourcePath = "";
            public SkeletonImport Skeleton = null;

            public MeshImport(Importer parent, string mesh, SkeletonImport skeleton)
            {
                SourcePath = Utils.NormalizePath(mesh);
                Skeleton = skeleton;
            }

            protected override void DoImport(string targetPath)
            {
                var zms = new ZMS(GetDataPath(SourcePath));
                var mesh = zms.getMesh();
                if (Skeleton != null)
                    mesh.bindposes = Skeleton.BindPose;
                AssetDatabase.CreateAsset(mesh, targetPath);
            }

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

        public class BoneAnimationImport : ImportItem<AnimationClip>
        {
            public string SourcePath = "";
            public ZMO AnimData = null;
            public int NumBones = 0;
            public SkeletonImport Skeleton = null;

            public BoneAnimationImport(Importer parent, string anim, SkeletonImport skeleton)
            {
                SourcePath = Utils.NormalizePath(anim);
                Skeleton = skeleton;
            }

            protected override void DoLoad()
            {
                AnimData = new ZMO(GetDataPath(SourcePath));

                for (var i = 0; i < AnimData.Channels.Length; ++i)
                {
                    if (NumBones < AnimData.Channels[i].ID + 1)
                        NumBones = AnimData.Channels[i].ID + 1;
                }
            }

            protected override void DoImport(string targetPath)
            {
                var zmo = new ZMO(GetDataPath(SourcePath));
                var anim = zmo.BuildSkeletonAnimationClip(Skeleton.BonePaths);
                AssetDatabase.CreateAsset(anim, targetPath);
            }
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

        public class MeshAnimationImport : ImportItem
        {
            public string SourcePath = "";
            public MeshImport Mesh = null;

            public MeshAnimationImport(Importer parent, string anim, MeshImport mesh)
            {
                SourcePath = Utils.NormalizePath(anim);
                Mesh = mesh;
            }

            protected override void DoImport(string targetPath)
            {
                Debug.Log("Import MeshAnimation " + targetPath);
            }
        }

        public class EffectImport : ImportItem
        {
            public string SourcePath = "";

            public EffectImport(Importer parent, string path)
            {
                SourcePath = Utils.NormalizePath(path);
            }

            protected override void DoImport(string targetPath)
            {
                Debug.Log("Import Effect " + targetPath);
            }
        }

        public class ParticleSystemImport : ImportItem
        {
            public class Sequence : ImportItem
            {
                public Revise.Files.PTL.ParticleSequence SourceSeq;
                public PsMaterial Material = null;

                public Sequence(Importer parent)
                {

                }

            }

            public class PsMaterial : ImportItem<Material>
            {
                public Revise.Files.PTL.ParticleSequence SourceSeq;
                public TextureImport Texture = null;

                public PsMaterial(Importer parent)
                {

                }

                protected override void DoImport(string targetPath)
                {
                    var mat = new Material(Shader.Find("Particles/Alpha Blended"));
                    mat.SetTexture("_MainTex", Texture.GetData());
                    AssetDatabase.CreateAsset(mat, targetPath);
                }

            }

            public string SourcePath = "";
            public List<Sequence> Sequences = null;

            public ParticleSystemImport(Importer parent, string path)
            {
                SourcePath = Utils.NormalizePath(path);

                var basePath = GenerateAssetPath(path, ".particle");
                var matBasePath = Utils.CombinePath(Path.GetDirectoryName(basePath), "Materials");
                var matBaseName = Path.GetFileNameWithoutExtension(basePath);

                var ptl = new Revise.Files.PTL.ParticleFile();
                ptl.Load(GetDataPath(SourcePath));

                Sequences = new List<Sequence>();
                for (var i = 0; i < ptl.Sequences.Count; ++i)
                {
                    var seq = ptl.Sequences[i];
                    var psSeq = new Sequence(parent);

                    psSeq.SourceSeq = seq;

                    var mat = new PsMaterial(parent);
                    mat._targetPath = Utils.CombinePath(matBasePath, matBaseName + "_" + i + ".asset");
                    mat.Texture = parent.MakeTexture(ptl.Sequences[i].TextureFileName);
                    parent.AddItem(mat);
                    psSeq.Material = mat;

                    Sequences.Add(psSeq);
                }
            }

            public enum MinMaxCurveState
            {
                k_Scalar,
                k_Curve,
                k_TwoCurves,
                k_TwoScalars
            }

            public class SerializedMinMaxCurve
            {
                protected SerializedProperty _scalar;
                protected SerializedProperty _minCurve;
                protected SerializedProperty _maxCurve;
                protected SerializedProperty _minMaxState;

                public SerializedMinMaxCurve(SerializedProperty prop)
                {
                    _scalar = prop.FindPropertyRelative("scalar");
                    _minCurve = prop.FindPropertyRelative("minCurve");
                    _maxCurve = prop.FindPropertyRelative("maxCurve");
                    _minMaxState = prop.FindPropertyRelative("minMaxState");
                }

                public float Scalar
                {
                    get { return _scalar.floatValue; }
                    set { _scalar.floatValue = value; }
                }

                public MinMaxCurveState MinMaxState
                {
                    get { return (MinMaxCurveState)_minMaxState.intValue; }
                    set { _minMaxState.intValue = (int)value; }
                }

                public AnimationCurve MinCurve
                {
                    get { return _minCurve.animationCurveValue; }
                    set { _minCurve.animationCurveValue = value; }
                }

                public AnimationCurve MaxCurve
                {
                    get { return _maxCurve.animationCurveValue; }
                    set { _maxCurve.animationCurveValue = value; }
                }

                public void SetValue(float val)
                {
                    Scalar = val;
                    MinMaxState = MinMaxCurveState.k_Scalar;
                }

                public void SetMinMax(float min, float max)
                {
                    if (min == max)
                    {
                        SetValue(min);
                        return;
                    }

                    Scalar = max;
                    var minCurve = new AnimationCurve();
                    minCurve.AddKey(0, min / max);
                    MinCurve = minCurve;
                    var maxCurve = new AnimationCurve();
                    maxCurve.AddKey(0, 1);
                    MaxCurve = maxCurve;
                    MinMaxState = MinMaxCurveState.k_TwoScalars;
                }

                public void SetValue(AnimationCurve curve, float scalar)
                {
                    Scalar = scalar;
                    MaxCurve = curve;
                    MinMaxState = MinMaxCurveState.k_Curve;
                }

                public void SetMinMax(AnimationCurve minCurve, AnimationCurve maxCurve, float scalar)
                {
                    if (Utils.CurvesMatch(minCurve, maxCurve) || minCurve == null || maxCurve == null)
                    {
                        if (maxCurve == null)
                            SetValue(minCurve, scalar);
                        else
                            SetValue(maxCurve, scalar);
                        return;
                    }
                    MinCurve = minCurve;
                    MaxCurve = maxCurve;
                    Scalar = scalar;
                    MinMaxState = MinMaxCurveState.k_TwoCurves;
                }

            }

            public class PtlCurveBuilder
            {
                private float _lifetime = 0;
                private float _scalar = 0;
                private Dictionary<float, float> _min = new Dictionary<float, float>();
                private Dictionary<float, float> _max = new Dictionary<float, float>();

                public PtlCurveBuilder(float lifetime)
                {
                    _lifetime = lifetime;
                }

                public bool IsEmpty
                {
                    get { return _min.Count == 0 && _max.Count == 0; }
                }

                public void AddKey(float minTime, float maxTime, float min, float max)
                {
                    _min[minTime] = min;
                    _max[minTime] = max;
                    if (maxTime != minTime)
                    {
                        //_min[maxTime] = min;
                        //_max[maxTime] = max;
                    }

                    if (_scalar < min * 1.1f)
                        _scalar = min * 1.1f;
                    if (_scalar < max * 1.1f)
                        _scalar = max * 1.1f;
                }

                public void ApplyToMinMax(SerializedMinMaxCurve minMax)
                {
                    var minCurve = new AnimationCurve();
                    foreach (var i in _min)
                        minCurve.AddKey(i.Key / _lifetime, i.Value / _scalar);

                    var maxCurve = new AnimationCurve();
                    foreach (var i in _max)
                        maxCurve.AddKey(i.Key / _lifetime, i.Value / _scalar);

                    minMax.SetMinMax(minCurve, maxCurve, _scalar);
                }

            }

            private GameObject BuildSequence(Sequence seq)
            {
                var go = new GameObject();
                go.name = "Particle System";

                var ps = go.AddComponent<ParticleSystem>();
                var psr = go.GetComponent<ParticleSystemRenderer>();
                var psData = new SerializedObject(ps);

                var lifetimeCurve = new SerializedMinMaxCurve(psData.FindProperty("InitialModule.startLifetime"));
                lifetimeCurve.SetMinMax(seq.SourceSeq.Lifetime.Minimum, seq.SourceSeq.Lifetime.Maximum);

                var speedCurve = new SerializedMinMaxCurve(psData.FindProperty("InitialModule.startSpeed"));
                speedCurve.SetValue(0);

                psData.FindProperty("InitialModule.maxNumParticles").intValue = seq.SourceSeq.ParticleCount;

                
                var sizeIck = false;
                var sizeCurves = new PtlCurveBuilder(seq.SourceSeq.Lifetime.Maximum);
                var velXCurves = new PtlCurveBuilder(seq.SourceSeq.Lifetime.Maximum);
                var velYCurves = new PtlCurveBuilder(seq.SourceSeq.Lifetime.Maximum);
                var velZCurves = new PtlCurveBuilder(seq.SourceSeq.Lifetime.Maximum);
                var rotCurves = new PtlCurveBuilder(seq.SourceSeq.Lifetime.Maximum);
                var uvCurves = new PtlCurveBuilder(seq.SourceSeq.Lifetime.Maximum);

                for (var i = 0; i < seq.SourceSeq.Events.Count; ++i)
                {
                    var evt = seq.SourceSeq.Events[i];

                    if (evt.Type == Revise.Files.PTL.ParticleEventType.Scale)
                    {
                        var scaleEvt = (Revise.Files.PTL.Events.ScaleEvent)evt;

                        if (scaleEvt.Scale.Minimum.x != scaleEvt.Scale.Minimum.y)
                            sizeIck = true;

                        var minVal = (scaleEvt.Scale.Minimum.x + scaleEvt.Scale.Minimum.y) / 2;
                        var maxVal = (scaleEvt.Scale.Maximum.x + scaleEvt.Scale.Maximum.y) / 2;

                        sizeCurves.AddKey(
                            scaleEvt.TimeRange.Minimum, scaleEvt.TimeRange.Maximum,
                            minVal, maxVal);
                    } else if (evt.Type == Revise.Files.PTL.ParticleEventType.Velocity)
                    {
                        var velEvt = (Revise.Files.PTL.Events.VelocityEvent)evt;

                        velXCurves.AddKey(
                            velEvt.TimeRange.Minimum, velEvt.TimeRange.Maximum,
                            velEvt.Velocity.Minimum.x, velEvt.Velocity.Maximum.x);
                        velYCurves.AddKey(
                            velEvt.TimeRange.Minimum, velEvt.TimeRange.Maximum,
                            velEvt.Velocity.Minimum.y, velEvt.Velocity.Maximum.y);
                        velZCurves.AddKey(
                            velEvt.TimeRange.Minimum, velEvt.TimeRange.Maximum,
                            velEvt.Velocity.Minimum.z, velEvt.Velocity.Maximum.z);
                    } else if (evt.Type == Revise.Files.PTL.ParticleEventType.VelocityX)
                    {
                        var velEvt = (Revise.Files.PTL.Events.VelocityXEvent)evt;
                        velXCurves.AddKey(
                            velEvt.TimeRange.Minimum, velEvt.TimeRange.Maximum,
                            velEvt.Velocity.Minimum, velEvt.Velocity.Maximum);
                    }
                    else if (evt.Type == Revise.Files.PTL.ParticleEventType.VelocityY)
                    {
                        var velEvt = (Revise.Files.PTL.Events.VelocityYEvent)evt;
                        velXCurves.AddKey(
                            velEvt.TimeRange.Minimum, velEvt.TimeRange.Maximum,
                            velEvt.Velocity.Minimum, velEvt.Velocity.Maximum);
                    }
                    else if (evt.Type == Revise.Files.PTL.ParticleEventType.VelocityZ)
                    {
                        var velEvt = (Revise.Files.PTL.Events.VelocityZEvent)evt;
                        velXCurves.AddKey(
                            velEvt.TimeRange.Minimum, velEvt.TimeRange.Maximum,
                            velEvt.Velocity.Minimum, velEvt.Velocity.Maximum);
                    } else if (evt.Type == Revise.Files.PTL.ParticleEventType.Rotation)
                    {
                        var rotEvt = (Revise.Files.PTL.Events.RotationEvent)evt;

                        Debug.LogWarning("Unimplemented Particle Rotation Event");
                        
                    } else if (evt.Type == Revise.Files.PTL.ParticleEventType.Texture)
                    {
                        var texEvt = (Revise.Files.PTL.Events.TextureEvent)evt;

                        uvCurves.AddKey(
                            texEvt.TimeRange.Minimum, texEvt.TimeRange.Maximum,
                            texEvt.TextureIndex.Minimum, texEvt.TextureIndex.Maximum);
                    }
                }

                if (!sizeCurves.IsEmpty)
                {
                    if (sizeIck)
                        Debug.LogWarning("Particle effect with non-square scaling... " + _targetPath);

                    psData.FindProperty("SizeModule.enabled").boolValue = true;
                    sizeCurves.ApplyToMinMax(new SerializedMinMaxCurve(psData.FindProperty("SizeModule.curve")));
                }
                else
                {
                    psData.FindProperty("SizeModule.enabled").boolValue = false;
                }

                if (!velXCurves.IsEmpty || !velYCurves.IsEmpty || !velZCurves.IsEmpty)
                {
                    psData.FindProperty("VelocityModule.enabled").boolValue = true;
                    velXCurves.ApplyToMinMax(new SerializedMinMaxCurve(psData.FindProperty("VelocityModule.x")));
                    velYCurves.ApplyToMinMax(new SerializedMinMaxCurve(psData.FindProperty("VelocityModule.y")));
                    velZCurves.ApplyToMinMax(new SerializedMinMaxCurve(psData.FindProperty("VelocityModule.z")));
                } else
                {
                    psData.FindProperty("VelocityModule.enabled").boolValue = false;
                }

                if (!uvCurves.IsEmpty)
                {
                    psData.FindProperty("UVModule.enabled").boolValue = true;
                    psData.FindProperty("UVModule.tilesX").intValue = seq.SourceSeq.TextureWidth;
                    psData.FindProperty("UVModule.tilesY").intValue = seq.SourceSeq.TextureHeight;
                    uvCurves.ApplyToMinMax(new SerializedMinMaxCurve(psData.FindProperty("UVModule.frameOverTime")));
                } else
                {
                    psData.FindProperty("UVModule.enabled").boolValue = false;
                }

                psData.ApplyModifiedPropertiesWithoutUndo();

                psr.material = seq.Material.GetData();

                return go;
            }

            protected override void DoImport(string targetPath)
            {
                Debug.Log("Import Particle System " + targetPath);

                GameObject rootGo = null;
                for (var i = 0; i < Sequences.Count; ++i)
                {
                    GameObject seqGo = BuildSequence(Sequences[i]);
                    if (rootGo == null)
                        rootGo = seqGo;
                    else
                        seqGo.transform.parent = rootGo.transform;
                }
            }
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

        public class MaterialImport : ImportItem<Material>
        {
            public ZSC.Texture SourceMat;
            public TextureImport Texture;

            public MaterialImport(Importer parent, ZSC zsc, int materialIdx)
            {
                SourceMat = zsc.Textures[materialIdx];
                Texture = parent.MakeTexture(SourceMat.Path);
            }

            protected override void DoImport(string targetPath)
            {
                var mat = new Material(Shader.Find("Standard"));
                mat.SetTexture("_MainTex", Texture.GetData());
                AssetDatabase.CreateAsset(mat, targetPath);
            }
        }

        public class MapZscImport : ImportItem
        {
            public class Model : ImportItem
            {
                public class Part : ImportItem
                {
                    public ZSC.Object.Model SourcePart;
                    public MeshImport Mesh;
                    public MaterialImport Material;

                    public Part(Importer parent)
                    {
                    }
                }

                public ZSC.Object SourceObject;
                public List<Part> Parts;

                public Model(Importer parent)
                {
                }

                protected override void DoImport(string targetPath)
                {
                    Debug.Log("Import Object " + targetPath);
                }
            }

            public string SourcePath;
            public List<Model> Models;

            private static Regex mapZsc = new Regex("3DDATA/([A-Z]*)/LIST_([A-Z_]*).ZSC", RegexOptions.IgnoreCase);
            private static Regex npcZsc = new Regex("(.*)/PART_NPC.ZSC", RegexOptions.IgnoreCase);

            public MapZscImport(Importer parent, string path)
            {
                var objBasePath = GenerateZscBasePath(path);

                SourcePath = Utils.NormalizePath(path);

                var zsc = new ZSC(GetDataPath(SourcePath));

                var materials = new MaterialImport[zsc.Textures.Count];
                for (var i = 0; i < zsc.Textures.Count; ++i)
                {
                    materials[i] = new MaterialImport(parent, zsc, i);
                    materials[i]._targetPath = Utils.CombinePath(objBasePath, "Materials/Mat_" + i + ".asset");
                    parent.AddItem(materials[i]);
                }

                var meshes = new MeshImport[zsc.Models.Count];
                for (var i = 0; i < zsc.Models.Count; ++i)
                {
                    meshes[i] = parent.MakeMesh(zsc.Models[i], null);
                }

                var effects = new EffectImport[zsc.Effects.Count];
                for (var i = 0; i < zsc.Effects.Count; ++i)
                {
                    effects[i] = new EffectImport(parent, zsc.Effects[i]);
                }

                Models = new List<Model>();
                for (var i = 0; i < zsc.Objects.Count; ++i)
                {
                    var zscObj = zsc.Objects[i];
                    var obj = new Model(parent);
                    obj.SourceObject = zscObj;
                    obj.Parts = new List<Model.Part>();
                    for (var j = 0; j < zscObj.Models.Count; ++j)
                    {
                        var zscPart = zscObj.Models[j];
                        var part = new Model.Part(parent);
                        part.SourcePart = zscPart;
                        part.Mesh = meshes[zscPart.ModelID];
                        part.Material = materials[zscPart.TextureID];
                        obj.Parts.Add(part);
                    }
                    obj._targetPath = Utils.CombinePath(objBasePath, "Obj_" + i + ".asset");
                    parent.AddItem(obj);
                    Models.Add(obj);
                }
            }
        }

        public class ChrImporter : ImportItem
        {
            public class CharacterModel : ImportItem<GameObject>
            {
                public class Part : ImportItem
                {
                    public int ChrPartIdx = 0;
                    public int ZscPartIdx = 0;
                    public ZSC.Object.Model SourcePart;
                    public int ParentIdx = -1;
                    public MeshImport Mesh;
                    public MaterialImport Material;

                    public Part(Importer parent, int chrPartIdx, int zscPartIdx)
                    {
                        ChrPartIdx = chrPartIdx;
                        ZscPartIdx = zscPartIdx;
                    }
                }

                public CHR.Character SourceObject;
                public SkeletonImport Skeleton;
                public List<Part> Parts;
                public List<BoneAnimationImport> Animations;

                public CharacterModel(Importer parent)
                {

                }

                protected override void DoImport(string targetPath)
                {
                    var charObj = new GameObject();
                    charObj.name = "Npc";

                    // Build Skeleton
                    var charBoneTransforms = new Transform[Skeleton.SkeletonData.bones.Count];
                    for (var i = 0; i < Skeleton.SkeletonData.bones.Count; ++i)
                    {
                        var bone = Skeleton.SkeletonData.bones[i];

                        var charBone = new GameObject();
                        charBone.name = "Bone_" + i;

                        if (i == 0)
                            charBone.transform.parent = charObj.transform;
                        else
                            charBone.transform.parent = charBoneTransforms[bone.ParentID];

                        charBone.transform.localPosition = bone.Position;
                        charBone.transform.localRotation = bone.Rotation;

                        charBoneTransforms[i] = charBone.transform;

                        if (i == 0)
                            charBone.AddComponent<BoneDebug>();
                    }
                    for (var i = 0; i < Skeleton.SkeletonData.dummies.Count; ++i)
                    {
                        var bone = Skeleton.SkeletonData.dummies[i];

                        var charBone = new GameObject();
                        charBone.name = "Dummy_" + i;

                        charBone.transform.parent = charBoneTransforms[bone.ParentID];
                        charBone.transform.localPosition = bone.Position;
                        charBone.transform.localRotation = bone.Rotation;
  
                    }

                    var charParts = new GameObject[Parts.Count];
                    for(var i = 0; i < Parts.Count; ++i)
                    {
                        var part = Parts[i];

                        var charPart = new GameObject();
                        charPart.name = "Part_" + part.ChrPartIdx + "_" + part.ZscPartIdx;

                        if (part.ParentIdx == -1)
                            charPart.transform.parent = charObj.transform;
                        else
                            charPart.transform.parent = charParts[part.ParentIdx].transform;

                        charPart.transform.localPosition = part.SourcePart.Position;
                        charPart.transform.localRotation = part.SourcePart.Rotation;
                        charPart.transform.localScale = part.SourcePart.Scale;

                        var smr = charPart.AddComponent<SkinnedMeshRenderer>();
                        smr.sharedMesh = part.Mesh.GetData();
                        smr.sharedMaterial = part.Material.GetData();
                        smr.bones = charBoneTransforms;

                        charParts[i] = charPart;    
                    }

                    var animator = charObj.AddComponent<Animation>();
                    for (var i = 0; i < Animations.Count; ++i)
                    {
                        var clip = Animations[i].GetData();
                        animator.AddClip(clip, clip.name);

                        if (i == 1)
                            animator.clip = clip;
                    }
                    animator.wrapMode = WrapMode.Loop;

                    PrefabUtility.CreatePrefab(targetPath, charObj);
                    GameObject.DestroyImmediate(charObj);
                }
            }

            public class Character : ImportItem
            {
                public CHR.Character SourceObject;
                public CharacterModel Model;

                public Character(Importer parent)
                {
                }

                protected override void DoImport(string targetPath)
                {
                    Debug.Log("Import Character " + targetPath);

                    var data = ScriptableObject.CreateInstance<RoseNpcData>();
                    data.NpcName = SourceObject.Name;
                    data.Model = Model.GetData();
                    AssetDatabase.CreateAsset(data, targetPath);
                }
            }

            public string SourceChrPath;
            public string SourceZscPath;
            public List<Character> Characters;

            public bool CharsMatch(CHR.Character x, CHR.Character y)
            {
                if (x.ID != y.ID)
                    return false;

                if (x.Objects.Count != y.Objects.Count)
                    return false;
                for (var i = 0; i < x.Objects.Count; ++i)
                {
                    if (x.Objects[i].Object != y.Objects[i].Object)
                        return false;
                }

                if (x.Animations.Count != y.Animations.Count)
                    return false;
                for (var i = 0; i < x.Animations.Count; ++i)
                {
                    if (x.Animations[i].Animation != y.Animations[i].Animation)
                        return false;
                    if (x.Animations[i].Type != y.Animations[i].Type)
                        return false;
                }

                if (x.Effects.Count != y.Effects.Count)
                    return false;
                for (var i = 0; i < x.Effects.Count; ++i)
                {
                    if (x.Effects[i].Bone != y.Effects[i].Bone)
                        return false;
                    if (x.Effects[i].Effect != y.Effects[i].Effect)
                        return false;
                }

                return true;
            }

            public ChrImporter(Importer parent, string chrPath, string zscPath)
            {
                SourceChrPath = Utils.NormalizePath(chrPath);
                SourceZscPath = Utils.NormalizePath(zscPath);

                var chr = new CHR(GetDataPath(SourceChrPath));
                var zsc = new ZSC(GetDataPath(SourceZscPath));

                var materials = new MaterialImport[zsc.Textures.Count];
                for (var i = 0; i < zsc.Textures.Count; ++i)
                {
                    materials[i] = new MaterialImport(parent, zsc, i);
                    materials[i]._targetPath = "Assets/NpcModels/Materials/Mat_" + i + ".asset";
                    parent.AddItem(materials[i]);
                }

                var effects = new EffectImport[zsc.Effects.Count];
                for (var i = 0; i < zsc.Effects.Count; ++i)
                {
                    effects[i] = new EffectImport(parent, zsc.Effects[i]);
                }

                var charRemap = new List<int>();
                for (var i = 0; i < chr.Characters.Count; ++i)
                {
                    var charX = chr.Characters[i];
                    var remapId = i;
                    if (charX.IsEnabled)
                    {
                        for (var j = 0; j < i; ++j)
                        {
                            var charY = chr.Characters[j];
                            if (CharsMatch(charY, charX))
                            {
                                remapId = j;
                                break;
                            }
                        }
                    }
                    charRemap.Add(remapId);
                }

                var uniqueChars = new List<int>();
                foreach(var charIdx in charRemap)
                {
                    if (!uniqueChars.Contains(charIdx))
                        uniqueChars.Add(charIdx);
                }

                var models = new Dictionary<int, CharacterModel>();
                for (var i = 0; i < uniqueChars.Count; ++i)
                {
                    var chrId = uniqueChars[i];
                    var chrChar = chr.Characters[chrId];
                    if (!chrChar.IsEnabled)
                        continue;

                    var obj = new CharacterModel(parent);
                    obj.SourceObject = chrChar;
                    obj.Skeleton = parent.MakeSkeleton(chr.SkeletonFiles[chrChar.ID]);
                    obj.Parts = new List<CharacterModel.Part>();
                    for (var j = 0; j < chrChar.Objects.Count; ++j)
                    {
                        var chrPart = chrChar.Objects[j];
                        var zscObj = zsc.Objects[chrPart.Object];
                        var parentBase = obj.Parts.Count;
                        for (var k = 0; k < zscObj.Models.Count; ++k)
                        {
                            var zscPart = zscObj.Models[k];
                            var part = new CharacterModel.Part(parent, j, k);
                            part.SourcePart = zscPart;
                            part.Mesh = parent.MakeMesh(zsc.Models[zscPart.ModelID], obj.Skeleton);
                            part.Material = materials[zscPart.TextureID];

                            if (k == 0)
                                part.ParentIdx = -1;
                            else
                                part.ParentIdx = parentBase + zscPart.Parent;

                            obj.Parts.Add(part);
                        }
                    }
                    obj.Animations = new List<BoneAnimationImport>();
                    for (var j = 0; j < chrChar.Animations.Count; ++j)
                    {
                        var animIdx = chrChar.Animations[j].Animation;
                        if (animIdx < 0)
                            continue;

                        var anim = parent.MakeBoneAnimation(chr.MotionFiles[animIdx], obj.Skeleton);
                        obj.Animations.Add(anim);
                    }
                    obj._targetPath = "Assets/NpcModels/NPCMdl_" + chrId + ".prefab";
                    parent.AddItem(obj);
                    models.Add(chrId, obj);
                }

                Characters = new List<Character>();
                for (var i = 0; i < chr.Characters.Count; ++i)
                {
                    var chrChar = chr.Characters[i];
                    if (!chrChar.IsEnabled)
                        continue;

                    var obj = new Character(parent);
                    obj.SourceObject = chrChar;
                    obj.Model = models[charRemap[i]];
                    obj._targetPath = "Assets/Npcs/NPC_" + i + ".asset";
                    parent.AddItem(obj);
                    Characters.Add(obj);
                }
            }

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

                    try {
                        bool forceRefresh = false;

                        if (item.GetType() == typeof(ChrImporter.Character))
                            forceRefresh = true;

                        item.Load();
                        item.Import(item._targetPath, forceRefresh);
                    } catch (System.Exception e)
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

    public static void ClearData()
    {
        ClearUData();
        Directory.Delete("Assets/GameData", true);
        AssetDatabase.Refresh();
    }

    public static void ClearUData()
    {
        Directory.Delete("Assets/MapObjects", true);
        Directory.Delete("Assets/NpcModels", true);
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

    private static bool TypeImplements(System.Type t, System.Type i) {
        foreach (var x in t.GetInterfaces()) {
            if (x == i)
                return true;
        }
        return false;
    }

    public static void ImportMap(int mapIdx)
    {
        Debug.Log("Importing Map #" + mapIdx);

        var importer = new Importer();
        /*
        var map = new Importer.ChrImporter(importer, "3DDATA/NPC/LIST_NPC.CHR", "3DDATA/NPC/PART_NPC.ZSC");
        for (var i = 0; i < 40; ++i)
            importer.Mark(map.Characters[i]);
        */
        var ptl = importer.MakeParticleSystem("3DDATA/EFFECT/PARTICLES/FIRE_BALL_HIT01.PTL");
        importer.Mark(ptl);
        importer.Import();

            /*
        return;

        AssetHelper.StartAssetEditing();
        try
        {
            //*
            var importer = new ChrImporter();
            for (var i = 0; i < 20; ++i)
            {
                var x = importer.ImportNpc(i);
                //PrefabUtility.InstantiatePrefab(x);
            }
            //*/

        /*
        var importerX = new ZscImporter("3DDATA\\JUNON\\LIST_CNST_JPT.ZSC");

        for (var i = 1; i < 10; ++i)
        {
            var obj = importerX.ImportObject(i);
            GameObject pf = (GameObject)GameObject.Instantiate(obj);
            Debug.Log(pf);
        }
        /
    } finally
    {
        AssetHelper.StopAssetEditing();
    }
    */
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
        */

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
        if (!File.Exists(animPath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(animPath));

            var zmo = new ZMO(fullPath);
            var anim = zmo.BuildSkeletonAnimationClip(null);
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
