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
using System.Linq;
using System.Text;

namespace Revise.Files.IDX {
    /// <summary>
    /// Provides the ability to create, open and save IDX files for storing file entries.
    /// </summary>
    public class VirtualFileIndexFile : FileLoader {
        #region Properties

        /// <summary>
        /// Gets or sets the base version.
        /// </summary>
        public int BaseVersion {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current version.
        /// </summary>
        public int CurrentVersion {
            get;
            set;
        }

        /// <summary>
        /// Gets the file systems.
        /// </summary>
        public List<VirtualFileIndexSystem> FileSystems {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualFileIndexFile"/> class.
        /// </summary>
        public VirtualFileIndexFile() {
            FileSystems = new List<VirtualFileIndexSystem>();

            Reset();
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            BaseVersion = reader.ReadInt32();
            CurrentVersion = reader.ReadInt32();

            int fileSystemCount = reader.ReadInt32();

            for (int i = 0; i < fileSystemCount; i++) {
                VirtualFileIndexSystem fileSystem = new VirtualFileIndexSystem();
                fileSystem.FileName = reader.ReadShortString();

                int offset = reader.ReadInt32();
                long nextFileSystem = stream.Position;

                stream.Seek(offset, SeekOrigin.Begin);

                int fileCount = reader.ReadInt32();
                int deleteCount = reader.ReadInt32();
                int startOffset = reader.ReadInt32();

                for (int j = 0; j < fileCount; j++) {
                    VirtualFileIndexEntry file = new VirtualFileIndexEntry();
                    file.FilePath = reader.ReadShortString();
                    file.Offset = reader.ReadInt32();
                    file.Size = reader.ReadInt32();
                    file.BlockSize = reader.ReadInt32();
                    file.IsDeleted = reader.ReadBoolean();
                    file.IsCompressed = reader.ReadBoolean();
                    file.IsEncrypted = reader.ReadBoolean();
                    file.Version = reader.ReadInt32();
                    file.Checksum = reader.ReadInt32();

                    fileSystem.Files.Add(file);
                }

                FileSystems.Add(fileSystem);

                if (i < fileSystemCount - 1) {
                    stream.Seek(nextFileSystem, SeekOrigin.Begin);
                }
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.Write(BaseVersion);
            writer.Write(CurrentVersion);
            writer.Write(FileSystems.Count);

            long[] fileSystemOffsets = new long[FileSystems.Count];
            
            for (int i = 0; i < FileSystems.Count; i++) {
                VirtualFileIndexSystem fileSystem = FileSystems[i];
                writer.WriteShortString(fileSystem.FileName);

                fileSystemOffsets[i] = stream.Position;
                writer.Write(0);
            }

            for (int i = 0; i < FileSystems.Count; i++) {
                VirtualFileIndexSystem fileSystem = FileSystems[i];

                long entryOffset = stream.Position;
                stream.Seek(fileSystemOffsets[i], SeekOrigin.Begin);

                writer.Write((int)entryOffset);

                stream.Seek(entryOffset, SeekOrigin.Begin);

                var deletedFiles = from f in fileSystem.Files
                                   where f.IsDeleted
                                   select f;

                var startOffset = from f in fileSystem.Files
                                  orderby f.Offset
                                  select f.Offset;

                writer.Write(fileSystem.Files.Count);
                writer.Write(deletedFiles.Count());
                writer.Write(startOffset.First());

                fileSystem.Files.ForEach(file => {
                    writer.WriteShortString(file.FilePath);
                    writer.Write(file.Offset);
                    writer.Write(file.Size);
                    writer.Write(file.BlockSize);
                    writer.Write(file.IsDeleted);
                    writer.Write(file.IsCompressed);
                    writer.Write(file.IsEncrypted);
                    writer.Write(file.Version);
                    writer.Write(file.Checksum);
                });
            }
        }

        /// <summary>
        /// Clears all file systems.
        /// </summary>
        public void Clear() {
            FileSystems.Clear();
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