using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class ROSEImport
{
    public class DdsReadImport : Importer.ImportItem
    {
        public class MakeIHVWriteable : Importer.ImportItem
        {
            public class CopyAsset : Importer.ImportItem
            {
                private DdsReadImport Importer;

                public CopyAsset(Importer parent, DdsReadImport importer)
                {
                    _targetPath = "Assets/" + System.Guid.NewGuid().ToString();

                    Importer = importer;
                }

                protected override void DoLoad()
                {
                    File.Copy(GetDataPath(Importer.SourcePath), Importer.TempPath, true);
                    AssetDatabase.ImportAsset(Importer.TempPath, ImportAssetOptions.ForceSynchronousImport);
                }

                protected override void DoImport(string targetPath)
                {
                }
            }

            private DdsReadImport Importer;
            public CopyAsset CopyAction;

            public MakeIHVWriteable(Importer parent, DdsReadImport importer)
            {
                _targetPath = "Assets/" + System.Guid.NewGuid().ToString();

                Importer = importer;
                CopyAction = new CopyAsset(parent, importer);
                parent.AddItem(CopyAction);
            }

            protected override void DoLoad()
            {
                var texImp = AssetImporter.GetAtPath(Importer.TempPath) as IHVImageFormatImporter;
                texImp.isReadable = true;
                texImp.SaveAndReimport();
            }

            protected override void DoImport(string targetPath)
            {
            }
        }

        public string SourcePath = "";
        private string TempPath = "";
        public MakeIHVWriteable WriteableAction;
        public Color[] Data = null;
        public int Width = 0;
        public int Height = 0;

        public DdsReadImport(Importer parent, string path)
        {
            _targetPath = "Assets/Temp_" + path.Replace("/", "__").Replace("\\", "__");

            SourcePath = path;
            TempPath = _targetPath;
            WriteableAction = new MakeIHVWriteable(parent, this);
            parent.AddItem(WriteableAction);
        }

        protected override void DoLoad()
        {
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(TempPath);
            Data = tex.GetPixels();
            Width = tex.width;
            Height = tex.height;

            AssetDatabase.DeleteAsset(TempPath);
        }

        protected override void DoImport(string targetPath)
        {
        }

    }
}
