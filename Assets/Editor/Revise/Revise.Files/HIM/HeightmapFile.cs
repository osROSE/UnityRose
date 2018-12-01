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
using System.Text;

namespace Revise.Files.HIM {
    /// <summary>
    /// Provides the ability to create, open and save HIM files for height data.
    /// </summary>
    public class HeightmapFile : FileLoader {
        #region Constants

        private const int DEFAULT_WIDTH = 65;
        private const int DEFAULT_HEIGHT = 65;

        private const int PATCH_GRID_COUNT = 4;
        private const float PATCH_SIZE = 250.0f;

        private const int PATCH_WIDTH = 16;
        private const int PATCH_HEIGHT = 16;
        private const int QUAD_PATCH_COUNT = 85;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the width dimension of the heightmap.
        /// </summary>
        public int Width {
            get {
                return heights.GetLength(1);
            }
        }

        /// <summary>
        /// Gets the height dimension of the heightmap.
        /// </summary>
        public int Height {
            get {
                return heights.GetLength(0);
            }
        }

        /// <summary>
        /// Gets the patches.
        /// </summary>
        public HeightmapPatch[,] Patches {
            get {
                return patches;
            }
        }

        /// <summary>
        /// Gets the quad patches.
        /// </summary>
        public HeightmapPatch[] QuadPatches {
            get {
                return quadPatches;
            }
        }

        #endregion

        private float[,] heights;
        private HeightmapPatch[,] patches;
        private HeightmapPatch[] quadPatches;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeightmapFile"/> class.
        /// </summary>
        public HeightmapFile() {
            Reset();
        }

        /// <summary>
        /// Gets the height value of the specified coordinates.
        /// </summary>
        public float this[int x, int y] {
            get {
                return heights[x, y];
            }
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            int width = reader.ReadInt32();
            int height = reader.ReadInt32();
            heights = new float[height, width];

            int patchGridCount = reader.ReadInt32();
            float patchSize = reader.ReadSingle();

            for (int h = 0; h < height; h++) {
                for (int w = 0; w < width; w++) {
                    heights[h, w] = reader.ReadSingle();
                }
            }

            string name = reader.ReadString();
            int patchCount = reader.ReadInt32();

            for (int h = 0; h < 16; h++) {
                for (int w = 0; w < 16; w++) {
                    patches[h, w].Maximum = reader.ReadSingle();
                    patches[h, w].Minimum = reader.ReadSingle();
                }
            }

            int quadPatchCount = reader.ReadInt32();

            for (int i = 0; i < quadPatchCount; i++) {
                quadPatches[i].Maximum = reader.ReadSingle();
                quadPatches[i].Minimum = reader.ReadSingle();
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.Write(Width);
            writer.Write(Height);

            writer.Write(PATCH_GRID_COUNT);
            writer.Write(PATCH_SIZE);

            for (int h = 0; h < Height; h++) {
                for (int w = 0; w < Width; w++) {
                    writer.Write(heights[h, w]);
                }
            }

            GeneratePatches();

            writer.Write("quad");
            writer.Write(patches.GetLength(0) * patches.GetLength(1));

            for (int h = 0; h < patches.GetLength(0); h++) {
                for (int w = 0; w < patches.GetLength(1); w++) {
                    writer.Write(patches[h, w].Maximum);
                    writer.Write(patches[h, w].Minimum);
                }
            }

            writer.Write(quadPatches.Length);

            for (int i = 0; i < quadPatches.Length; i++) {
                writer.Write(quadPatches[i].Maximum);
                writer.Write(quadPatches[i].Minimum);
            }
        }

        /// <summary>
        /// Generates the patches and quad patches from the current height data.
        /// </summary>
        public void GeneratePatches() {
            for (int h = 0; h < patches.GetLength(0); h++) {
                for (int w = 0; w < patches.GetLength(1); w++) {
                    float maximum = float.MinValue;
                    float minimum = float.MaxValue;

                    for (int vh = 0; vh < 5; vh++) {
                        for (int vw = 0; vw < 5; vw++) {
                            float height = heights[65 - (h * 4 + vh + 1), w * 4 + vw];

                            if (height > maximum) {
                                maximum = height;
                            }

                            if (height < minimum) {
                                minimum = height;
                            }
                        }
                    }

                    patches[h, w].Maximum = maximum;
                    patches[h, w].Minimum = minimum;
                }
            }

            GenerateQuadPatches(0, 0, 16, 0, 0);
        }

        /// <summary>
        /// Generates the quad patches.
        /// </summary>
        /// <param name="index">The quad patch index.</param>
        /// <param name="level">The recursion level.</param>
        /// <param name="quadSize">The size of the quad patch.</param>
        /// <param name="x">The x position of the heightmap.</param>
        /// <param name="y">The y position of the heightmap.</param>
        private void GenerateQuadPatches(int index, int level, int quadSize, int x, int y) {
            int nextSize = quadSize / 2;
            int nextLevel = level + 1;

            float minimum = 10000.0f;
            float maximum = -10.0f;

            for (int h = y; h < y + quadSize; h++) {
                for (int w = x; w < x + quadSize; w++) {
                    HeightmapPatch patch = patches[h, w];

                    if (patch.Maximum > maximum) {
                        maximum = patch.Maximum;
                    }

                    if (patch.Minimum < minimum) {
                        minimum = patch.Minimum;
                    }
                }
            }

            quadPatches[index].Minimum = minimum;
            quadPatches[index].Maximum = maximum;

            if (nextLevel < 4) {
                GenerateQuadPatches(index * 4 + 1, nextLevel, nextSize, x, y);
                GenerateQuadPatches(index * 4 + 2, nextLevel, nextSize, x + nextSize, y);
                GenerateQuadPatches(index * 4 + 3, nextLevel, nextSize, x + nextSize, y + nextSize);
                GenerateQuadPatches(index * 4 + 4, nextLevel, nextSize, x, y + nextSize);
            }
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();

            heights = new float[DEFAULT_HEIGHT, DEFAULT_WIDTH];
            patches = new HeightmapPatch[PATCH_HEIGHT, PATCH_WIDTH];
            quadPatches = new HeightmapPatch[QUAD_PATCH_COUNT];
        }
    }
}