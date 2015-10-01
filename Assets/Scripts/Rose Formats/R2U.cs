// <copyright file="R2U.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine</author>
// <date>2/25/2015 8:37 AM </date>

#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.IO;

namespace UnityRose.Formats
{
    public class R2U
    {
        // The following Get[Asset] functions will perform the following sequence:
        // 1. Determine if the unity asset is available, load it from Asset DB, then return a pointer to the asset
        // 2. If not present, create one from the corresponding rose files then place it in the same directory tree but GameData instead of 3DDATA
        // 3. Reload the asset from the Asset DB and return a pointer to it

        public static AnimationClip GetClip(string zmoPath, ZMD skeleton, string name)
        {
            DirectoryInfo zmoDir = new DirectoryInfo(zmoPath);
            string unityPath = zmoDir.FullName.Replace(zmoDir.Name, name) + ".anim";

            AnimationClip clip = (AnimationClip)Utils.LoadAsset(unityPath, ".anim");
            
            if (clip == null)
            {
                clip = new ZMO(zmoPath).buildAnimationClip(skeleton);
                clip.name = name;
                clip.legacy = true;
                clip = (AnimationClip)Utils.SaveReloadAsset(clip, unityPath, ".anim");
            }

            return clip;
        }

        public static Mesh GetMesh(string zmsPath)
        {
            Mesh mesh = (Mesh)Utils.LoadAsset(zmsPath);
            if (mesh == null)
            {
                mesh = new ZMS(zmsPath).getMesh();
                mesh = (Mesh)Utils.SaveReloadAsset(mesh, zmsPath);
            }

            return mesh;
        }

    }
}

#endif
