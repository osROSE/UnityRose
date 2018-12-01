#region License

/**
 * Copyright (C) 2012 Jack Wakefield
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

#endregion

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Revise.Files.ZON {
    /// <summary>
    /// Provides the ability to create, open and save ZON files used for zone information.
    /// </summary>
    public class ZoneFile : FileLoader {
        private const int BLOCK_COUNT = 5;
        
        #region Properties

        /// <summary>
        /// Gets or sets the zone type.
        /// </summary>
        public ZoneType Type {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width of the zone.
        /// </summary>
        public int Width {
            get {
                return width;
            }
            set {
                width = value;

                ZonePosition[,] positions = new ZonePosition[width, Positions.GetLength(1)];
                int copyWidth = width < Positions.GetLength(0) ? width : Positions.GetLength(0);

                for (int w = 0; w < copyWidth; w++) {
                    for (int h = 0; h < Positions.GetLength(1); h++) {
                        positions[w, h] = Positions[w, h];
                    }
                }

                Positions = positions;
            }
        }

        /// <summary>
        /// Gets or sets the height of the zone.
        /// </summary>
        public int Height {
            get {
                return height;
            }
            set {
                height = value;

                ZonePosition[,] positions = new ZonePosition[Positions.GetLength(0), height];
                int copyHeight = height < Positions.GetLength(1) ? height : Positions.GetLength(1);

                for (int w = 0; w < Positions.GetLength(0); w++) {
                    for (int h = 0; h < copyHeight; h++) {
                        positions[w, h] = Positions[w, h];
                    }
                }

                Positions = positions;
            }
        }

        /// <summary>
        /// Gets or sets the grid count.
        /// </summary>
        public int GridCount {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size of the grid.
        /// </summary>
        public float GridSize {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the start position.
        /// </summary>
        public IntVector2 StartPosition {
            get;
            set;
        }

        /// <summary>
        /// Gets the zone positions.
        /// </summary>
        public ZonePosition[,] Positions {
            get;
            private set;
        }

        /// <summary>
        /// Gets the spawn points.
        /// </summary>
        public List<SpawnPoint> SpawnPoints {
            get;
            private set;
        }

        /// <summary>
        /// Gets the tile texture paths.
        /// </summary>
        public List<string> Textures {
            get;
            private set;
        }

        /// <summary>
        /// Gets the tiles.
        /// </summary>
        public List<ZoneTile> Tiles {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the zone name.
        /// </summary>
        public string Name {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this zone is underground.
        /// </summary>
        public bool IsUnderground {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the background music file path.
        /// </summary>
        public string BackgroundMusicFilePath {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sky file path.
        /// </summary>
        public string SkyFilePath {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rate at which to calculate the economy values in minutes.
        /// </summary>
        public int EconomyCheckRate {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the population base.
        /// </summary>
        public int PopulationBase {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the population growth rate.
        /// </summary>
        public int PopulationGrowthRate {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the metal consumption value.
        /// </summary>
        public int MetalConsumption {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the stone consumption value.
        /// </summary>
        public int StoneConsumption {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the wood consumption value.
        /// </summary>
        public int WoodConsumption {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the leather consumption value.
        /// </summary>
        public int LeatherConsumption {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the cloth consumption value.
        /// </summary>
        public int ClothConsumption {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the alchemy consumption value.
        /// </summary>
        public int AlchemyConsumption {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the chemical consumption value.
        /// </summary>
        public int ChemicalConsumption {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the industrial consumption value.
        /// </summary>
        public int IndustrialConsumption {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the medicine consumption value.
        /// </summary>
        public int MedicineConsumption {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the food consumption value.
        /// </summary>
        public int FoodConsumption {
            get;
            set;
        }

        #endregion

        private int width;
        private int height;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneFile"/> class.
        /// </summary>
        public ZoneFile() {
            SpawnPoints = new List<SpawnPoint>();
            Textures = new List<string>();
            Tiles = new List<ZoneTile>();

            Reset();
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            int blockCount = reader.ReadInt32();

            for (int i = 0; i < blockCount; i++) {
                ZoneBlock type = (ZoneBlock)reader.ReadInt32();
                int offset = reader.ReadInt32();

                long nextBlock = stream.Position;
                stream.Seek(offset, SeekOrigin.Begin);

                switch (type) {
                    case ZoneBlock.Info:
                        Type = (ZoneType)reader.ReadInt32();
                        Width = reader.ReadInt32();
                        Height = reader.ReadInt32();
                        GridCount = reader.ReadInt32();
                        GridSize = reader.ReadSingle();
                        StartPosition = new IntVector2(reader.ReadInt32(), reader.ReadInt32());

                        for (int w = 0; w < Width; w++) {
                            for (int h = 0; h < Height; h++) {
                                Positions[w, h].IsUsed = reader.ReadBoolean();
                                Positions[w, h].Position = reader.ReadVector2();
                            }
                        }
                        break;
                    case ZoneBlock.SpawnPoints:
                        int spawnCount = reader.ReadInt32();

                        for (int j = 0; j < spawnCount; j++) {
                            SpawnPoint spawnPoint = new SpawnPoint();
                            spawnPoint.Position = reader.ReadVector3();
                            spawnPoint.Name = reader.ReadByteString();

                            SpawnPoints.Add(spawnPoint);
                        }
                        break;
                    case ZoneBlock.Textures:
                        int textureCount = reader.ReadInt32();

                        for (int j = 0; j < textureCount; j++) {
                            Textures.Add(reader.ReadByteString());
                        }
                        break;
                    case ZoneBlock.Tiles:
                        int tileCount = reader.ReadInt32();

                        for (int j = 0; j < tileCount; j++) {
                            ZoneTile tile = new ZoneTile();
                            tile.Layer1 = reader.ReadInt32();
                            tile.Layer2 = reader.ReadInt32();
                            tile.Offset1 = reader.ReadInt32();
                            tile.Offset2 = reader.ReadInt32();
                            tile.BlendingEnabled = reader.ReadInt32() != 0;
                            tile.Rotation = (TileRotation)reader.ReadInt32();
                            tile.TileType = reader.ReadInt32();

                            Tiles.Add(tile);
                        }
                        break;
                    case ZoneBlock.Economy:
                        Name = reader.ReadByteString();
                        IsUnderground = reader.ReadInt32() != 0;
                        BackgroundMusicFilePath = reader.ReadByteString();
                        SkyFilePath = reader.ReadByteString();
                        EconomyCheckRate = reader.ReadInt32();
                        PopulationBase = reader.ReadInt32();
                        PopulationGrowthRate = reader.ReadInt32();
                        MetalConsumption = reader.ReadInt32();
                        StoneConsumption = reader.ReadInt32();
                        WoodConsumption = reader.ReadInt32();
                        LeatherConsumption = reader.ReadInt32();
                        ClothConsumption = reader.ReadInt32();
                        AlchemyConsumption = reader.ReadInt32();
                        ChemicalConsumption = reader.ReadInt32();
                        IndustrialConsumption = reader.ReadInt32();
                        MedicineConsumption = reader.ReadInt32();
                        FoodConsumption = reader.ReadInt32();
                        break;
                }

                if (i < blockCount - 1) {
                    stream.Seek(nextBlock, SeekOrigin.Begin);
                }
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.Write(BLOCK_COUNT);
            long[] offsets = new long[BLOCK_COUNT];

            for (int i = 0; i < BLOCK_COUNT; i++) {
                writer.Write(i);
                writer.Write(0);
            }

            offsets[0] = stream.Position;
            writer.Write((int)Type);
            writer.Write(Width);
            writer.Write(Height);
            writer.Write(GridCount);
            writer.Write(GridSize);
            writer.Write(StartPosition.X);
            writer.Write(StartPosition.Y);

            for (int x = 0; x < Width; x++) {
                for (int y = 0; y < Height; y++) {
                    writer.Write(Positions[x, y].IsUsed);
                    writer.Write(Positions[x, y].Position);
                }
            }

            offsets[1] = stream.Position;
            writer.Write(SpawnPoints.Count);

            SpawnPoints.ForEach(spawnPoint => {
                writer.Write(spawnPoint.Position);
                writer.WriteByteString(spawnPoint.Name);
            });

            offsets[2] = stream.Position;
            writer.Write(Textures.Count);

            Textures.ForEach(texture => {
                writer.WriteByteString(texture);
            });

            offsets[3] = stream.Position;
            writer.Write(Tiles.Count);

            Tiles.ForEach(tile => {
                writer.Write(tile.Layer1);
                writer.Write(tile.Layer2);
                writer.Write(tile.Offset1);
                writer.Write(tile.Offset2);
                writer.Write(tile.BlendingEnabled ? 1 : 0);
                writer.Write((int)tile.Rotation);
                writer.Write(tile.TileType);
            });

            offsets[4] = stream.Position;
            writer.WriteByteString(Name);
            writer.Write(IsUnderground ? 1 : 0);
            writer.WriteByteString(BackgroundMusicFilePath);
            writer.WriteByteString(SkyFilePath);
            writer.Write(EconomyCheckRate);
            writer.Write(PopulationBase);
            writer.Write(PopulationGrowthRate);
            writer.Write(MetalConsumption);
            writer.Write(StoneConsumption);
            writer.Write(WoodConsumption);
            writer.Write(LeatherConsumption);
            writer.Write(ClothConsumption);
            writer.Write(AlchemyConsumption);
            writer.Write(ChemicalConsumption);
            writer.Write(IndustrialConsumption);
            writer.Write(MedicineConsumption);
            writer.Write(FoodConsumption);

            for (int i = 0; i < BLOCK_COUNT; i++) {
                stream.Seek(sizeof(int) + i * sizeof(int) * 2 + sizeof(int), SeekOrigin.Begin);
                writer.Write((int)offsets[i]);
            }
        }

        /// <summary>
        /// Clears all spawn points, textures and tiles.
        /// </summary>
        public void Clear() {
            SpawnPoints.Clear();
            Textures.Clear();
            Tiles.Clear();
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();

            Positions = new ZonePosition[0,0];

            Type = ZoneType.Grass;
            Width = 0;
            Height = 0;
            GridCount = 0;
            GridSize = 0.0f;
            StartPosition = new IntVector2();

            Name = string.Empty;
            IsUnderground = false;
            BackgroundMusicFilePath = string.Empty;
            SkyFilePath = string.Empty;
            EconomyCheckRate = 50;
            PopulationBase = 100;
            PopulationGrowthRate = 10;
            MetalConsumption = 50;
            StoneConsumption = 50;
            WoodConsumption = 50;
            LeatherConsumption = 50;
            ClothConsumption = 50;
            AlchemyConsumption = 50;
            ChemicalConsumption = 50;
            IndustrialConsumption = 50;
            MedicineConsumption = 50;
            FoodConsumption = 50;

            Clear();
        }
    }
}