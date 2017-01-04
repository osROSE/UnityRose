using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class ROSEImport
{
    public class SkeletonImport : Importer.ImportItem
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

        public Transform[] BuildGameObject()
        {
            // Build Skeleton
            var boneTransforms = new Transform[SkeletonData.bones.Count];
            for (var i = 0; i < SkeletonData.bones.Count; ++i)
            {
                var bone = SkeletonData.bones[i];

                var charBone = new GameObject();
                charBone.name = "Bone_" + i;

                if (i != 0)
                    charBone.transform.parent = boneTransforms[bone.ParentID];

                charBone.transform.localPosition = bone.Position;
                charBone.transform.localRotation = bone.Rotation;

                boneTransforms[i] = charBone.transform;

                if (i == 0)
                    charBone.AddComponent<BoneDebug>();
            }
            for (var i = 0; i < SkeletonData.dummies.Count; ++i)
            {
                var bone = SkeletonData.dummies[i];

                var charBone = new GameObject();
                charBone.name = "Dummy_" + i;

                charBone.transform.parent = boneTransforms[bone.ParentID];
                charBone.transform.localPosition = bone.Position;
                charBone.transform.localRotation = bone.Rotation;

            }

            return boneTransforms;
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
}
