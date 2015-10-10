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

namespace Revise.Files.LIT {
    /// <summary>
    /// Provides the ability to create, open and save LIT files for lightmap data.
    /// </summary>
    public class LightmapFile : FileLoader {
        #region Properties

        /// <summary>
        /// Gets the lights objects.
        /// </summary>
        public List<LightmapObject> Objects {
            get;
            private set;
        }

        /// <summary>
        /// Gets the lightmap file names.
        /// </summary>
        public List<string> Files {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LightmapFile"/> class.
        /// </summary>
        public LightmapFile() {
            Objects = new List<LightmapObject>();
            Files = new List<string>();

            Reset();
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            int objectCount = reader.ReadInt32();

            for (int i = 0; i < objectCount; i++) {
                int partCount = reader.ReadInt32();

                LightmapObject @object = new LightmapObject();
                @object.ID = reader.ReadInt32() - 1;

                for (int j = 0; j < partCount; j++) {
                    LightmapPart part = new LightmapPart();
                    part.Name = reader.ReadString();
                    part.ID = reader.ReadInt32() - 1;
                    part.FileName = reader.ReadString();
                    part.LightmapIndex = reader.ReadInt32();
                    part.PixelsPerObject = reader.ReadInt32();
                    part.ObjectsPerWidth = reader.ReadInt32();
                    part.ObjectPosition = reader.ReadInt32();

                    @object.Parts.Add(@part);
                }

                Objects.Add(@object);
            }

            int fileCount = reader.ReadInt32();

            for (int i = 0; i < fileCount; i++) {
                Files.Add(reader.ReadString());
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.Write(Objects.Count);

            Objects.ForEach(@object => {
                writer.Write(@object.Parts.Count);
                writer.Write(@object.ID + 1);

                @object.Parts.ForEach(part => {
                    writer.Write(part.Name);
                    writer.Write(part.ID + 1);
                    writer.Write(part.FileName);
                    writer.Write(part.LightmapIndex);
                    writer.Write(part.PixelsPerObject);
                    writer.Write(part.ObjectsPerWidth);
                    writer.Write(part.ObjectPosition);
                });
            });

            writer.Write(Files.Count);

            Files.ForEach(name => {
                writer.Write(name);
            });
        }

        /// <summary>
        /// Removes all light objects.
        /// </summary>
        public void Clear() {
            Objects.Clear();
            Files.Clear();
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();
            Clear();
        }
    }
}