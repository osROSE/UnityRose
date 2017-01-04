using UnityEngine;
using UnityEditor;
using UnityRose.Formats;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class ROSEImport
{
    public class MapTileImport : Importer.ImportItem
    {
        public class TileTexImport : Importer.ImportItem<Texture2D>
        {
            public DdsReadImport DdsRead = null;

            public TileTexImport(Importer parent, string path)
            {
                DdsRead = new DdsReadImport(parent, path);
                parent.AddItem(DdsRead);
            }

            protected override void DoImport(string targetPath)
            {
                var texColors = new Color[DdsRead.Data.Length];
                for (var i = 0; i < DdsRead.Data.Length; ++i)
                {
                    texColors[i] = DdsRead.Data[i];
                    texColors[i].a = 0.0f;
                }

                var newTex = new Texture2D(DdsRead.Width, DdsRead.Height);
                newTex.SetPixels(texColors);

                File.WriteAllBytes(targetPath, newTex.EncodeToPNG());
                AssetDatabase.ImportAsset(targetPath, ImportAssetOptions.ForceSynchronousImport);
            }
        }

        public List<TileTexImport> Textures;

        public MapTileImport(Importer parent)
        {
            var tileList = new Dictionary<string, string>();

            var dataRootAbs = Path.GetFullPath(GetCurrentPath());
            var planetRoot = GetDataPath("3DDATA/TERRAIN/TILES/");
            var planetDirs = Directory.GetDirectories(planetRoot);
            for (var i = 0; i < planetDirs.Length; ++i)
            {
                var setDirs = Directory.GetDirectories(planetDirs[i]);

                for (var j = 0; j < setDirs.Length; ++j)
                {
                    var files = Directory.GetFiles(setDirs[j]);

                    for (var k = 0; k < files.Length; ++k)
                    {
                        var path = files[k].Substring(dataRootAbs.Length + 1);

                        if (Path.GetExtension(path).ToLower() != ".dds")
                            continue;

                        var dirNames = Path.GetDirectoryName(path).Split('/', '\\');
                        var setName = dirNames[dirNames.Length - 1];

                        var fileName = Path.GetFileNameWithoutExtension(path);
                        var tileName = setName + "_" + fileName;

                        if (fileName[0] == 'T')
                        {
                            var tileId = fileName.Substring(0, 4);
                            tileName = setName + "_" + tileId;
                        }


                        if (!tileList.ContainsKey(tileName))
                        {
                            tileList.Add(tileName, path);
                        }
                    }
                }
            }

            Textures = new List<TileTexImport>();
            foreach (var tile in tileList)
            {
                var tileImp = new TileTexImport(parent, tile.Value);
                tileImp._targetPath = "Assets/MapTiles/" + tile.Key + ".png";
                parent.AddItem(tileImp);
                Textures.Add(tileImp);
            }
        }
    }
}
