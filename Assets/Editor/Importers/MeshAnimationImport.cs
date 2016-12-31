using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class ROSEImport
{
    public class MeshAnimationImport : Importer.ImportItem
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
}
