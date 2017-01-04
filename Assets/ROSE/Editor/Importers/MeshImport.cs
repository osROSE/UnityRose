using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class ROSEImport
{
    public class MeshImport : Importer.ImportItem<Mesh>
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

}
