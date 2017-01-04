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

using System.IO;

namespace Revise.Files.IFO.Blocks {
    /// <summary>
    /// Represents a map water patch.
    /// </summary>
    public class MapWaterPatches : MapBlock {
        #region Properties

        /// <summary>
        /// Gets the width.
        /// </summary>
        public int Width {
            get {
                return Patches.GetLength(1);
            }
            set {
                WaterPatch[,] patches = new WaterPatch[Patches.GetLength(0), value];
                int copyWidth = value < Patches.GetLength(1) ? value : Patches.GetLength(1);

                for (int h = 0; h < Patches.GetLength(0); h++) {
                    for (int w = 0; w < copyWidth; w++) {
                        patches[h, w] = Patches[h, w];
                    }
                }

                Patches = patches;
            }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        public int Height {
            get {
                return Patches.GetLength(0);
            }
            set {
                WaterPatch[,] patches = new WaterPatch[value, Patches.GetLength(1)];
                int copyHeight = value < Patches.GetLength(0) ? value : Patches.GetLength(0);

                for (int h = 0; h < copyHeight; h++) {
                    for (int w = 0; w < Patches.GetLength(1); w++) {
                        patches[h, w] = Patches[h, w];
                    }
                }

                Patches = patches;
            }
        }

        /// <summary>
        /// Gets or sets the water patches.
        /// </summary>
        public WaterPatch[,] Patches {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MapWaterPatches"/> class.
        /// </summary>
        public MapWaterPatches() {
            Patches = new WaterPatch[0, 0];
        }
        
        /// <summary>
        /// Reads the block data from the underlying stream.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public override void Read(BinaryReader reader) {
            base.Read(reader);

            int width = reader.ReadInt32();
            int height = reader.ReadInt32();

            Patches = new WaterPatch[height, width];

            for (int h = 0; h < height; h++) {
                for (int w = 0; w < width; w++) {
                    WaterPatch patch;
                    patch.HasWater = reader.ReadBoolean();
                    patch.Height = reader.ReadSingle();
                    patch.Type = reader.ReadInt32();
                    patch.ID = reader.ReadInt32();
                    patch.Reserved = reader.ReadInt32();

                    Patches[h, w] = patch;
                }
            }
        }

        /// <summary>
        /// Writes the block data to the underlying stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void Write(BinaryWriter writer) {
            base.Write(writer);

            writer.Write(Width);
            writer.Write(Height);

            for (int h = 0; h < Height; h++) {
                for (int w = 0; w < Width; w++) {
                    WaterPatch patch = Patches[h, w];
                    writer.Write(patch.HasWater);
                    writer.Write(patch.Height);
                    writer.Write(patch.Type);
                    writer.Write(patch.ID);
                    writer.Write(patch.Reserved);
                }
            }
        }
    }
}