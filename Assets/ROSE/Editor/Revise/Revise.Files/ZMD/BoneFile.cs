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
using Revise.Files.Exceptions;

namespace Revise.Files.ZMD {
    /// <summary>
    /// Provides the ability to create, open and save ZMD files used for bone information.
    /// </summary>
    public class BoneFile : FileLoader {
        #region Constants

        private const string FILE_IDENTIFIER_2 = "ZMD0002";
        private const string FILE_IDENTIFIER_3 = "ZMD0003";

        private const int MAXIMUM_BONE_COUNT = 1000;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the bones.
        /// </summary>
        public List<Bone> Bones {
            get;
            private set;
        }

        /// <summary>
        /// Gets the dummy bones.
        /// </summary>
        public List<Bone> DummyBones {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BoneFile"/> class.
        /// </summary>
        public BoneFile() {
            Bones = new List<Bone>();
            DummyBones = new List<Bone>();

            Reset();
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            string identifier = reader.ReadString(7);

            int version;

            if (string.Compare(identifier, FILE_IDENTIFIER_2, false) == 0) {
                version = 2;
            } else if (string.Compare(identifier, FILE_IDENTIFIER_3, false) == 0) {
                version = 3;
            } else {
                throw new FileIdentifierMismatchException(FilePath, string.Format("{0} / {1}", FILE_IDENTIFIER_2, FILE_IDENTIFIER_3), identifier);
            }

            int boneCount = reader.ReadInt32();

            if (boneCount >= MAXIMUM_BONE_COUNT) {
                throw new InvalidBoneCountException();
            }

            for (int i = 0; i < boneCount; i++) {
                Bone bone = new Bone();
                bone.Parent = reader.ReadInt32();
                bone.Name = reader.ReadNullTerminatedString();
                bone.Translation = reader.ReadVector3();
                bone.Rotation = reader.ReadQuaternion(true);

                Bones.Add(bone);
            }

            int dummyCount = reader.ReadInt32();

            if (boneCount >= MAXIMUM_BONE_COUNT) {
                throw new InvalidBoneCountException();
            }

            for (int i = 0; i < dummyCount; i++) {
                Bone dummy = new Bone();
                dummy.Name = reader.ReadNullTerminatedString();
                dummy.Parent = reader.ReadInt32();
                dummy.Translation = reader.ReadVector3();

                if (version == 3) {
                    dummy.Rotation = reader.ReadQuaternion(true);
                }

                DummyBones.Clear();
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.WriteString(FILE_IDENTIFIER_3);
            writer.Write(Bones.Count);

            Bones.ForEach(bone => {
                writer.Write(bone.Parent);
                writer.WriteString(bone.Name);
                writer.Write((byte)0);
                writer.Write(bone.Translation);
                writer.Write(bone.Rotation, true);
            });

            writer.Write(DummyBones.Count);

            DummyBones.ForEach(dummy => {
                writer.WriteString(dummy.Name);
                writer.Write(dummy.Parent);
                writer.Write((byte)0);
                writer.Write(dummy.Translation);
                writer.Write(dummy.Rotation, true);
            });
        }

        /// <summary>
        /// Clears all bones and dummy bones.
        /// </summary>
        public void Clear() {
            Bones.Clear();
            DummyBones.Clear();
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