using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class ROSEImport
{
    public class ChrImporter : Importer.ImportItem
    {
        public class CharacterAnimCtrlr : Importer.ImportItem<UnityEditor.Animations.AnimatorController>
        {
            public List<BoneAnimationImport> Animations;

            protected override void DoImport(string targetPath)
            {
                var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(targetPath);
                var stateMachine = controller.layers[0].stateMachine;

                var animNames = new string[]
                {
                        "Idle",
                        "Move",
                        "Attack",
                        "Hit",
                        "Die",
                        "Run",
                        "CAction1",
                        "SAction1",
                        "CAction2",
                        "SAction2",
                        "Special"
                };

                for (var i = 0; i < Animations.Count; ++i)
                {
                    var state = stateMachine.AddState(animNames[i]);
                    state.motion = Animations[i].GetData();

                    if (!stateMachine.defaultState)
                    {
                        stateMachine.defaultState = state;
                    }
                }
            }
        }

        public class CharacterModel : Importer.ImportItem<GameObject>
        {
            public class Part : Importer.ImportItem
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
            public CharacterAnimCtrlr AnimCtrlr;
            public List<Part> Parts;

            public CharacterModel(Importer parent)
            {

            }

            protected override void DoImport(string targetPath)
            {
                var charObj = new GameObject();
                charObj.name = "Npc";

                // Build Skeleton
                var charBoneTransforms = Skeleton.BuildGameObject();
                charBoneTransforms[0].parent = charObj.transform;

                var charParts = new GameObject[Parts.Count];
                for (var i = 0; i < Parts.Count; ++i)
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

                var animator = charObj.AddComponent<Animator>();
                animator.runtimeAnimatorController = AnimCtrlr.GetData();

                PrefabUtility.CreatePrefab(targetPath, charObj);
                GameObject.DestroyImmediate(charObj);
            }
        }

        public class Character : Importer.ImportItem
        {
            public CHR.Character SourceObject;
            public CharacterModel Model;

            public Character(Importer parent)
            {
            }

            protected override void DoImport(string targetPath)
            {
                Debug.Log("Import Character " + targetPath);

                /*
                var data = ScriptableObject.CreateInstance<RoseNpcData>();
                data.NpcName = SourceObject.Name;
                data.Model = Model.GetData();
                AssetDatabase.CreateAsset(data, targetPath);
                */
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
                parent.AddItem(effects[i]);
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
            foreach (var charIdx in charRemap)
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

                {
                    var animCtrlr = new CharacterAnimCtrlr();
                    animCtrlr.Animations = new List<BoneAnimationImport>();
                    for (var j = 0; j < chrChar.Animations.Count; ++j)
                    {
                        var animIdx = chrChar.Animations[j].Animation;
                        if (animIdx < 0)
                            continue;

                        var anim = parent.MakeBoneAnimation(chr.MotionFiles[animIdx], obj.Skeleton);
                        animCtrlr.Animations.Add(anim);
                    }
                    animCtrlr._targetPath = "Assets/NpcAnims/NPCAnim_" + chrId + ".controller";
                    parent.AddItem(animCtrlr);

                    obj.AnimCtrlr = animCtrlr;
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
}
