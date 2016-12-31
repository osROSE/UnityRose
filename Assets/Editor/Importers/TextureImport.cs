using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class ROSEImport
{
    public class TextureImport : Importer.ImportItem<Texture2D>
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
}
