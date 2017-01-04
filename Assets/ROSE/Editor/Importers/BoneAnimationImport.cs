using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class ROSEImport
{
    public class BoneAnimationImport : Importer.ImportItem<AnimationClip>
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
}
