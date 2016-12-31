using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class ROSEImport
{
    public class EffectImport : Importer.ImportItem
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

}
