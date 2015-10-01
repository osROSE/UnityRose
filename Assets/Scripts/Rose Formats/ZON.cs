// <copyright file="ZON.cs" company="Wadii Bellamine">
// Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Wadii Bellamine, Xadet, Brett19</author>
// <date>2/25/2015 8:37 AM </date>

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityRose.File;

#if UNITY_EDITOR
namespace UnityRose.Formats
{
    /// <summary>
    /// ZON class.
    /// </summary>
    public class ZON
    {
        /// <summary>
        /// Block type.
        /// </summary>
        public enum BlockType
        {
            /// <summary>
            /// Misc. information.
            /// </summary>
            Block0,

            /// <summary>
            /// Spawn points.
            /// </summary>
            SpawnPoints,

            /// <summary>
            /// Textures.
            /// </summary>
            Textures,

            /// <summary>
            /// Tiles.
            /// </summary>
            Tiles,

            /// <summary>
            /// Economy information.
            /// </summary>
            Economy
        }

        /// <summary>
        /// Tile rotation type.
        /// </summary>
        public enum RotationType
        {
            /// <summary>
            /// Normal.
            /// </summary>
            Normal = 1,

            /// <summary>
            /// Left to right.
            /// </summary>
            LeftRight = 2,

            /// <summary>
            /// Top to bottom.
            /// </summary>
            TopBottom = 3,

            /// <summary>
            /// Left to right and top to bottom.
            /// </summary>
            LeftRightTopBottom = 4,

            /// <summary>
            /// 90 degrees clockwise.
            /// </summary>
            Rotate90Clockwise = 5,

            /// <summary>
            /// 90 degrees counter clockwise.
            /// </summary>
            Rotate90CounterClockwise = 6
        }

        #region Sub Classes

        /// <summary>
        /// Block class.
        /// </summary>
        public class Block
        {
            #region Member Declarations

            /// <summary>
            /// Gets or sets the type.
            /// </summary>
            /// <value>The type.</value>
            public BlockType Type { get; set; }

            /// <summary>
            /// Gets or sets the offset.
            /// </summary>
            /// <value>The offset.</value>
            public int Offset { get; set; }

            #endregion
        }

        /// <summary>
        /// Block0 class.
        /// </summary>
        public class Block0
        {
            /// <summary>
            /// Zone part structure.
            /// </summary>
            public struct ZonePart
            {
                #region Member Declarations

                /// <summary>
                /// Gets or sets the use map.
                /// </summary>
                /// <value>The use map.</value>
                public byte UseMap { get; set; }

                /// <summary>
                /// Gets or sets the position.
                /// </summary>
                /// <value>The position.</value>
                public Vector2 Position { get; set; }

                #endregion
            };

            #region Member Declarations

            /// <summary>
            /// Gets or sets the type of the zone.
            /// </summary>
            /// <value>The type of the zone.</value>
            public int ZoneType { get; set; }

            /// <summary>
            /// Gets or sets the width of the zone.
            /// </summary>
            /// <value>The width of the zone.</value>
            public int ZoneWidth { get; set; }

            /// <summary>
            /// Gets or sets the height of the zone.
            /// </summary>
            /// <value>The height of the zone.</value>
            public int ZoneHeight { get; set; }

            /// <summary>
            /// Gets or sets the grid count.
            /// </summary>
            /// <value>The grid count.</value>
            public int GridCount { get; set; }

            /// <summary>
            /// Gets or sets the size of the grid.
            /// </summary>
            /// <value>The size of the grid.</value>
            public float GridSize { get; set; }

            /// <summary>
            /// Gets or sets the X count.
            /// </summary>
            /// <value>The X count.</value>
            public int XCount { get; set; }

            /// <summary>
            /// Gets or sets the Y count.
            /// </summary>
            /// <value>The Y count.</value>
            public int YCount { get; set; }

            /// <summary>
            /// Gets or sets the zone parts.
            /// </summary>
            /// <value>The zone parts.</value>
            public ZonePart[,] ZoneParts { get; set; }

            #endregion
        }

        /// <summary>
        /// SpawnPoint class.
        /// </summary>
        public class SpawnPoint
        {
            #region Member Declarations

            /// <summary>
            /// Gets or sets the position.
            /// </summary>
            /// <value>The position.</value>
            public Vector3 Position { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; set; }

            #endregion
        }

        /// <summary>
        /// Texture class.
        /// </summary>
        public class Texture
        {
            #region Member Declarations

            /// <summary>
            /// Gets or sets the tile texture.
            /// </summary>
            /// <value>The tile texture.</value>
            public Texture2D Tex { get; set; }

            /// <summary>
            /// Gets or sets the path.
            /// </summary>
            /// <value>The path.</value>
            public string TexPath { get; set; }

            #endregion
        }

        /// <summary>
        /// Tile class.
        /// </summary>
        public class Tile
        {
            #region Member Declarations

            /// <summary>
            /// Gets or sets the base ID.
            /// </summary>
            /// <value>The base ID.</value>
            public int BaseID1 { get; set; }

            /// <summary>
            /// Gets or sets the base ID.
            /// </summary>
            /// <value>The base ID.</value>
            public int BaseID2 { get; set; }

            /// <summary>
            /// Gets or sets the offset.
            /// </summary>
            /// <value>The offset.</value>
            public int Offset1 { get; set; }

            /// <summary>
            /// Gets or sets the offset.
            /// </summary>
            /// <value>The offset.</value>
            public int Offset2 { get; set; }

            /// <summary>
            /// Gets the ID.
            /// </summary>
            /// <value>The ID.</value>
            public int ID1
            {
                get { return BaseID1 + Offset1; }
            }

            /// <summary>
            /// Gets the ID.
            /// </summary>
            /// <value>The ID.</value>
            public int ID2
            {
                get { return BaseID2 + Offset2; }
            }

            /// <summary>
            /// Gets or sets the is blending.
            /// </summary>
            /// <value>The is blending.</value>
            public bool IsBlending { get; set; }

            /// <summary>
            /// Gets or sets the rotation.
            /// </summary>
            /// <value>The rotation.</value>
            public RotationType Rotation { get; set; }

            /// <summary>
            /// Gets or sets the type of the tile.
            /// </summary>
            /// <value>The type of the tile.</value>
            public int TileType { get; set; }

            #endregion
        }

        /// <summary>
        /// Economy class
        /// </summary>
        public class Economy
        {
            #region Member Declarations

            /// <summary>
            /// Gets or sets the name of the area.
            /// </summary>
            /// <value>The name of the area.</value>
            public string AreaName { get; set; }

            /// <summary>
            /// Gets or sets the is underground.
            /// </summary>
            /// <value>The is underground.</value>
            public int IsUnderground { get; set; }

            /// <summary>
            /// Gets or sets the button BGM.
            /// </summary>
            /// <value>The button BGM.</value>
            public string ButtonBGM { get; set; }

            /// <summary>
            /// Gets or sets the button back.
            /// </summary>
            /// <value>The button back.</value>
            public string ButtonBack { get; set; }

            /// <summary>
            /// Gets or sets the check count.
            /// </summary>
            /// <value>The check count.</value>
            public int CheckCount { get; set; }

            /// <summary>
            /// Gets or sets the standard population.
            /// </summary>
            /// <value>The standard population.</value>
            public int StandardPopulation { get; set; }

            /// <summary>
            /// Gets or sets the standard growth rate.
            /// </summary>
            /// <value>The standard growth rate.</value>
            public int StandardGrowthRate { get; set; }

            /// <summary>
            /// Gets or sets the metal consumption.
            /// </summary>
            /// <value>The metal consumption.</value>
            public int MetalConsumption { get; set; }

            /// <summary>
            /// Gets or sets the stone consumption.
            /// </summary>
            /// <value>The stone consumption.</value>
            public int StoneConsumption { get; set; }

            /// <summary>
            /// Gets or sets the wood consumption.
            /// </summary>
            /// <value>The wood consumption.</value>
            public int WoodConsumption { get; set; }

            /// <summary>
            /// Gets or sets the leather consumption.
            /// </summary>
            /// <value>The leather consumption.</value>
            public int LeatherConsumption { get; set; }

            /// <summary>
            /// Gets or sets the cloth consumption.
            /// </summary>
            /// <value>The cloth consumption.</value>
            public int ClothConsumption { get; set; }

            /// <summary>
            /// Gets or sets the alchemy consumption.
            /// </summary>
            /// <value>The alchemy consumption.</value>
            public int AlchemyConsumption { get; set; }

            /// <summary>
            /// Gets or sets the chemical consumption.
            /// </summary>
            /// <value>The chemical consumption.</value>
            public int ChemicalConsumption { get; set; }

            /// <summary>
            /// Gets or sets the industrial consumption.
            /// </summary>
            /// <value>The industrial consumption.</value>
            public int IndustrialConsumption { get; set; }

            /// <summary>
            /// Gets or sets the medicine consumption.
            /// </summary>
            /// <value>The medicine consumption.</value>
            public int MedicineConsumption { get; set; }

            /// <summary>
            /// Gets or sets the food consumption.
            /// </summary>
            /// <value>The food consumption.</value>
            public int FoodConsumption { get; set; }

            #endregion
        }

        #endregion

        #region Member Declarations

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; set; }

        public string RootPath { get; set; }

        #endregion

        #region Data Declarations

        /// <summary>
        /// Gets or sets the blocks.
        /// </summary>
        /// <value>The blocks.</value>
        public List<Block> Blocks { get; set; }

        /// <summary>
        /// Gets or sets the zone info.
        /// </summary>
        /// <value>The zone info.</value>
        public Block0 ZoneInfo { get; set; }

        /// <summary>
        /// Gets or sets the spawn points.
        /// </summary>
        /// <value>The spawn points.</value>
        public List<SpawnPoint> SpawnPoints { get; set; }

        /// <summary>
        /// Gets or sets the textures.
        /// </summary>
        /// <value>The textures.</value>
        public List<Texture> Textures { get; set; }

        /// <summary>
        /// Gets or sets the tiles.
        /// </summary>
        /// <value>The tiles.</value>
        public List<Tile> Tiles { get; set; }

        /// <summary>
        /// Gets or sets the economy info.
        /// </summary>
        /// <value>The economy info.</value>
        public Economy EconomyInfo { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ZON"/> class.
        /// </summary>
        public ZON()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZON"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public ZON(string filePath)
        {
            string rootPath = "";
            char[] sep = { '/' };
            string[] tokens = filePath.Split(sep);
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].ToLower() == "3ddata")
                    break;

                rootPath += tokens[i] + "/";
            }

            RootPath = rootPath;
            RootPath = "Assets/";
            Load(filePath);
        }

        /// <summary>
        /// Loads the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Load(string filePath)
        {
            FileHandler fh = new FileHandler(FilePath = filePath, FileHandler.FileOpenMode.Reading, Encoding.UTF8);

            int blockCount = fh.Read<int>();

            Blocks = new List<Block>(blockCount);

            for (int i = 0; i < blockCount; i++)
            {
                Blocks.Add(new Block()
                {
                    Type = (BlockType)fh.Read<int>(),
                    Offset = fh.Read<int>()
                });
            }

            for (int i = 0; i < blockCount; i++)
            {
                fh.Seek(Blocks[i].Offset, SeekOrigin.Begin);

                switch (Blocks[i].Type)
                {
                    case BlockType.Block0:
                        {
                            ZoneInfo = new Block0()
                            {
                                ZoneType = fh.Read<int>(),
                                ZoneWidth = fh.Read<int>(),
                                ZoneHeight = fh.Read<int>(),
                                GridCount = fh.Read<int>(),
                                GridSize = fh.Read<float>(),

                                XCount = fh.Read<int>(),
                                YCount = fh.Read<int>()
                            };

                            ZoneInfo.ZoneParts = new Block0.ZonePart[ZoneInfo.ZoneWidth, ZoneInfo.ZoneHeight];

                            for (int j = 0; j < ZoneInfo.ZoneWidth; j++)
                            {
                                for (int k = 0; k < ZoneInfo.ZoneHeight; k++)
                                {
                                    ZoneInfo.ZoneParts[j, k] = new Block0.ZonePart()
                                    {
                                        UseMap = fh.Read<byte>(),
                                        Position = fh.Read<Vector2>()
                                    };
                                }
                            }
                        }
                        break;
                    case BlockType.SpawnPoints:
                        {
                            int spawnPointCount = fh.Read<int>();
                            SpawnPoints = new List<SpawnPoint>(spawnPointCount);

                            for (int j = 0; j < spawnPointCount; j++)
                            {
                                SpawnPoints.Add(new SpawnPoint()
                                {
                                    Position = new Vector3()
                                    {
                                        x = (fh.Read<float>() + 520000.00f) / 100.0f,
                                        z = fh.Read<float>() / 100.0f,
                                        y = (fh.Read<float>() + 520000.00f) / 100.0f
                                    },

                                    Name = fh.Read<BString>()
                                });
                            }
                        }
                        break;
                    case BlockType.Textures:
                        {
                            int textureCount = fh.Read<int>();
                            Textures = new List<Texture>(textureCount);

                            for (int j = 0; j < textureCount; j++)
                            {
                                Texture tex = new Texture();
                                string path = RootPath + fh.Read<BString>();
								tex.Tex = Utils.loadTex(ref path); //.ToLower().Replace("\\", "/").Replace(".dds",".png");
                                // = Resources.LoadAssetAtPath<Texture2D>(tex.TexPath);
								tex.TexPath = path;
							
								Textures.Add(tex);

                            }
                        }
                        break;
                    case BlockType.Tiles:
                        {
                            int tileCount = fh.Read<int>();
                            Tiles = new List<Tile>(tileCount);

                            for (int j = 0; j < tileCount; j++)
                            {
                                Tiles.Add(new Tile()
                                {
                                    BaseID1 = fh.Read<int>(),
                                    BaseID2 = fh.Read<int>(),
                                    Offset1 = fh.Read<int>(),
                                    Offset2 = fh.Read<int>(),
                                    IsBlending = fh.Read<int>() > 0,
                                    Rotation = (RotationType)fh.Read<int>(),
                                    TileType = fh.Read<int>()
                                });
                            }
                        }
                        break;
                    case BlockType.Economy:
                        {
                            try
                            {
                                EconomyInfo = new Economy()
                                {
                                    AreaName = fh.Read<BString>(),
                                    IsUnderground = fh.Read<int>(),
                                    ButtonBGM = fh.Read<BString>(),
                                    ButtonBack = fh.Read<BString>(),
                                    CheckCount = fh.Read<int>(),
                                    StandardPopulation = fh.Read<int>(),
                                    StandardGrowthRate = fh.Read<int>(),
                                    MetalConsumption = fh.Read<int>(),
                                    StoneConsumption = fh.Read<int>(),
                                    WoodConsumption = fh.Read<int>(),
                                    LeatherConsumption = fh.Read<int>(),
                                    ClothConsumption = fh.Read<int>(),
                                    AlchemyConsumption = fh.Read<int>(),
                                    ChemicalConsumption = fh.Read<int>(),
                                    IndustrialConsumption = fh.Read<int>(),
                                    MedicineConsumption = fh.Read<int>(),
                                    FoodConsumption = fh.Read<int>()
                                };
                            }
                            catch
                            {
                                Debug.LogError("-- Error reading the Economy Info block");


                                EconomyInfo = new Economy()
                                {
                                    AreaName = string.Empty,
                                    IsUnderground = 0,
                                    ButtonBGM = string.Empty,
                                    ButtonBack = string.Empty,
                                    CheckCount = 0,
                                    StandardPopulation = 0,
                                    StandardGrowthRate = 0,
                                    MetalConsumption = 0,
                                    StoneConsumption = 0,
                                    WoodConsumption = 0,
                                    LeatherConsumption = 0,
                                    ClothConsumption = 0,
                                    AlchemyConsumption = 0,
                                    ChemicalConsumption = 0,
                                    IndustrialConsumption = 0,
                                    MedicineConsumption = 0,
                                    FoodConsumption = 0
                                };
                            }
                        }
                        break;
                }
            }

            fh.Close();
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        public void Save()
        {
            Save(FilePath);
        }

        /// <summary>
        /// Saves the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Save(string filePath)
        {
            FileHandler fh = new FileHandler(FilePath = filePath, FileHandler.FileOpenMode.Writing, Encoding.UTF8);

            fh.Write<int>(Blocks.Count);

            for (int i = 0; i < Blocks.Count; i++)
            {
                fh.Write<int>(0);
                fh.Write<int>(0);
            }

            for (int i = 0; i < Blocks.Count; i++)
            {
                Blocks[i].Offset = fh.Tell();

                switch (Blocks[i].Type)
                {
                    case BlockType.Block0:
                        {
                            fh.Write<int>(ZoneInfo.ZoneType);
                            fh.Write<int>(ZoneInfo.ZoneWidth);
                            fh.Write<int>(ZoneInfo.ZoneHeight);
                            fh.Write<int>(ZoneInfo.GridCount);
                            fh.Write<float>(ZoneInfo.GridSize);

                            fh.Write<int>(ZoneInfo.XCount);
                            fh.Write<int>(ZoneInfo.YCount);

                            for (int x = 0; x < ZoneInfo.ZoneWidth; x++)
                            {
                                for (int y = 0; y < ZoneInfo.ZoneHeight; y++)
                                {
                                    fh.Write<byte>(ZoneInfo.ZoneParts[x, y].UseMap);
                                    fh.Write<Vector2>(ZoneInfo.ZoneParts[x, y].Position);
                                }
                            }
                        }
                        break;
                    case BlockType.SpawnPoints:
                        {
                            fh.Write<int>(SpawnPoints.Count);

                            for (int j = 0; j < SpawnPoints.Count; j++)
                            {
                                fh.Write<float>(-520000.0f + (SpawnPoints[j].Position.x * 100.0f));
                                fh.Write<float>(SpawnPoints[j].Position.z * 100.0f);
                                fh.Write<float>(-520000.0f + (SpawnPoints[j].Position.y * 100.0f));

                                fh.Write<BString>(SpawnPoints[j].Name);
                            }
                        }
                        break;
                    case BlockType.Textures:
                        {
                            fh.Write<int>(Textures.Count);

                            for (int j = 0; j < Textures.Count; j++)
                                fh.Write<BString>(Textures[j].TexPath);
                        }
                        break;
                    case BlockType.Tiles:
                        {
                            fh.Write<int>(Tiles.Count);

                            for (int j = 0; j < Tiles.Count; j++)
                            {
                                fh.Write<int>(Tiles[j].BaseID1);
                                fh.Write<int>(Tiles[j].BaseID2);
                                fh.Write<int>(Tiles[j].Offset1);
                                fh.Write<int>(Tiles[j].Offset2);
                                fh.Write<int>(Tiles[j].IsBlending ? 1 : 0);
                                fh.Write<int>((int)Tiles[j].Rotation);
                                fh.Write<int>(Tiles[j].TileType);
                            }
                        }
                        break;
                    case BlockType.Economy:
                        {
                            fh.Write<BString>(EconomyInfo.AreaName);
                            fh.Write<int>(EconomyInfo.IsUnderground);
                            fh.Write<BString>(EconomyInfo.ButtonBGM);
                            fh.Write<BString>(EconomyInfo.ButtonBack);
                            fh.Write<int>(EconomyInfo.CheckCount);
                            fh.Write<int>(EconomyInfo.StandardPopulation);
                            fh.Write<int>(EconomyInfo.StandardGrowthRate);
                            fh.Write<int>(EconomyInfo.MetalConsumption);
                            fh.Write<int>(EconomyInfo.StoneConsumption);
                            fh.Write<int>(EconomyInfo.WoodConsumption);
                            fh.Write<int>(EconomyInfo.LeatherConsumption);
                            fh.Write<int>(EconomyInfo.ClothConsumption);
                            fh.Write<int>(EconomyInfo.AlchemyConsumption);
                            fh.Write<int>(EconomyInfo.ChemicalConsumption);
                            fh.Write<int>(EconomyInfo.IndustrialConsumption);
                            fh.Write<int>(EconomyInfo.MedicineConsumption);
                            fh.Write<int>(EconomyInfo.FoodConsumption);
                        }
                        break;
                }
            }

            fh.Seek(4, SeekOrigin.Begin);

            for (int i = 0; i < Blocks.Count; i++)
            {
                fh.Write<int>((int)Blocks[i].Type);
                fh.Write<int>(Blocks[i].Offset);
            }

            fh.Close();
        }
    }
}

#endif