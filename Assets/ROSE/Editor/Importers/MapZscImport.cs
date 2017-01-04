using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class ROSEImport
{
    public class MapZscImport : Importer.ImportItem
    {
        public class Model : Importer.ImportItem
        {
            public class Part : Importer.ImportItem
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
                if (Parts.Count == 0) return;

                var mdls = new List<GameObject>();

                for (var i = 0; i < Parts.Count; ++i)
                {
                    var part = Parts[i];
                    var sourcePart = part.SourcePart;

                    var go = new GameObject();
                    go.name = "Part_" + i;
                    mdls.Add(go);

                    var mf = go.AddComponent<MeshFilter>();
                    mf.mesh = part.Mesh.GetData();

                    var mr = go.AddComponent<MeshRenderer>();
                    mr.material = part.Material.GetData();

                    go.transform.localPosition = sourcePart.Position / 100;
                    go.transform.localRotation = sourcePart.Rotation;
                    go.transform.localScale = sourcePart.Scale;

                    if (i != 0)
                        go.transform.parent = mdls[sourcePart.Parent - 1].transform;
                }

                PrefabUtility.CreatePrefab(targetPath, mdls[0]);
                GameObject.DestroyImmediate(mdls[0]);
            }
        }

        public string SourcePath;
        public List<Model> Models;

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
                obj._targetPath = Utils.CombinePath(objBasePath, "Obj_" + i + ".prefab");
                parent.AddItem(obj);
                Models.Add(obj);
            }
        }
    }
}
