using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class ROSEImport
{
    public class MaterialImport : Importer.ImportItem<Material>
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
}
