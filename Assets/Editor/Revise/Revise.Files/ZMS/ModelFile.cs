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
using UnityEngine;

namespace Revise.Files.ZMS {
    /// <summary>
    /// Provides the ability to create, open and save ZMS files used for models.
    /// </summary>
    public class ModelFile : FileLoader {
        #region Constants

        private const string FILE_IDENTIFIER_7 = "ZMS0007";
        private const string FILE_IDENTIFIER_8 = "ZMS0008";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether positions are enabled in the vertex.
        /// </summary>
        public bool PositionsEnabled {
            get {
                return CheckFlag(VertexFormat.Position);
            }
            set {
                SetFlag(VertexFormat.Position, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether normals are enabled in the vertex.
        /// </summary>
        public bool NormalsEnabled {
            get {
                return CheckFlag(VertexFormat.Normal);
            }
            set {
                SetFlag(VertexFormat.Normal, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether colours are enabled in the vertex.
        /// </summary>
        public bool ColoursEnabled {
            get {
                return CheckFlag(VertexFormat.Colour);
            }
            set {
                SetFlag(VertexFormat.Colour, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether bones are enabled in the vertex.
        /// </summary>
        public bool BonesEnabled {
            get {
                return CheckFlag(VertexFormat.BlendWeight) && CheckFlag(VertexFormat.BlendIndex);
            }
            set {
                SetFlag(VertexFormat.BlendWeight, value);
                SetFlag(VertexFormat.BlendIndex, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether tangents are enabled in the vertex.
        /// </summary>
        public bool TangentsEnabled {
            get {
                return CheckFlag(VertexFormat.Tangent);
            }
            set {
                SetFlag(VertexFormat.Tangent, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the first texture coordinates are enabled in the vertex.
        /// </summary>
        public bool TextureCoordinates1Enabled {
            get {
                return CheckFlag(VertexFormat.TextureCoordinate1);
            }
            set {
                SetFlag(VertexFormat.TextureCoordinate1, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the second texture coordinates are enabled in the vertex.
        /// </summary>
        public bool TextureCoordinates2Enabled {
            get {
                return CheckFlag(VertexFormat.TextureCoordinate2);
            }
            set {
                SetFlag(VertexFormat.TextureCoordinate2, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the third texture coordinates are enabled in the vertex.
        /// </summary>
        public bool TextureCoordinates3Enabled {
            get {
                return CheckFlag(VertexFormat.TextureCoordinate3);
            }
            set {
                SetFlag(VertexFormat.TextureCoordinate3, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the fourth texture coordinates are enabled in the vertex.
        /// </summary>
        public bool TextureCoordinates4Enabled {
            get {
                return CheckFlag(VertexFormat.TextureCoordinate4);
            }
            set {
                SetFlag(VertexFormat.TextureCoordinate4, value);
            }
        }

        /// <summary>
        /// Gets or sets the bounding box values.
        /// </summary>
        public Bounds BoundingBox {
            get;
            set;
        }

        /// <summary>
        /// Gets the bone table.
        /// </summary>
        public List<short> BoneTable {
            get;
            private set;
        }

        /// <summary>
        /// Gets the vertices.
        /// </summary>
        public List<ModelVertex> Vertices {
            get;
            private set;
        }

        /// <summary>
        /// Gets the indices.
        /// </summary>
        public List<ShortVector3> Indices {
            get;
            private set;
        }

        /// <summary>
        /// Gets the material IDs.
        /// </summary>
        public List<short> Materials {
            get;
            private set;
        }

        /// <summary>
        /// Gets the strips.
        /// </summary>
        public List<short> Strips {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the pool type.
        /// </summary>
        public PoolType Pool {
            get;
            set;
        }

        #endregion

        private VertexFormat format;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelFile"/> class.
        /// </summary>
        public ModelFile() {
            BoneTable = new List<short>();
            Vertices = new List<ModelVertex>();
            Indices = new List<ShortVector3>();
            Materials = new List<short>();
            Strips = new List<short>();

            Reset();
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            string identifier = reader.ReadNullTerminatedString();

            int version;

            if (string.Compare(identifier, FILE_IDENTIFIER_7, false) == 0) {
                version = 7;
            } else if (string.Compare(identifier, FILE_IDENTIFIER_8, false) == 0) {
                version = 8;
            } else {
                throw new FileIdentifierMismatchException(FilePath, string.Format("{0} / {1}", FILE_IDENTIFIER_7, FILE_IDENTIFIER_8), identifier);
            }

            format = (VertexFormat)reader.ReadInt32();
            BoundingBox = new Bounds(reader.ReadVector3(), reader.ReadVector3());

            short boneCount = reader.ReadInt16();

            for (int i = 0; i < boneCount; i++) {
                BoneTable.Add(reader.ReadInt16());
            }

            short vertexCount = reader.ReadInt16();

            for (int i = 0; i < vertexCount; i++) {
                ModelVertex vertex = new ModelVertex();
                vertex.Position = reader.ReadVector3();

                Vertices.Add(vertex);
            }

            if (NormalsEnabled) {
                for (int i = 0; i < vertexCount; i++) {
                    ModelVertex vertex = Vertices[i];
                    vertex.Normal = reader.ReadVector3();
                }
            }

            if (ColoursEnabled) {
                for (int i = 0; i < vertexCount; i++) {
                    float a = reader.ReadSingle();
                    ModelVertex vertex = Vertices[i];
                    vertex.Colour = new Color(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), a);
                }
            }

            if (BonesEnabled) {
                for (int i = 0; i < vertexCount; i++) {
                    ModelVertex vertex = Vertices[i];
                    vertex.BoneWeights = reader.ReadVector4();
                    vertex.BoneIndices = reader.ReadShortVector4();
                }
            }

            if (TangentsEnabled) {
                for (int i = 0; i < vertexCount; i++) {
                    ModelVertex vertex = Vertices[i];
                    vertex.Tangent = reader.ReadVector3();
                }
            }

            if (TextureCoordinates1Enabled) {
                for (int i = 0; i < vertexCount; i++) {
                    ModelVertex vertex = Vertices[i];
                    vertex.TextureCoordinates[0] = reader.ReadVector2();
                }
            }

            if (TextureCoordinates2Enabled) {
                for (int i = 0; i < vertexCount; i++) {
                    ModelVertex vertex = Vertices[i];
                    vertex.TextureCoordinates[1] = reader.ReadVector2();
                }
            }

            if (TextureCoordinates3Enabled) {
                for (int i = 0; i < vertexCount; i++) {
                    ModelVertex vertex = Vertices[i];
                    vertex.TextureCoordinates[2] = reader.ReadVector2();
                }
            }

            if (TextureCoordinates4Enabled) {
                for (int i = 0; i < vertexCount; i++) {
                    ModelVertex vertex = Vertices[i];
                    vertex.TextureCoordinates[3] = reader.ReadVector2();
                }
            }

            short indexCount = reader.ReadInt16();

            for (int i = 0; i < indexCount; i++) {
                Indices.Add(reader.ReadShortVector3());
            }

            short materialCount = reader.ReadInt16();

            for (int i = 0; i < materialCount; i++) {
                Materials.Add(reader.ReadInt16());
            }

            short stripCount = reader.ReadInt16();

            for (int i = 0; i < stripCount; i++) {
                Strips.Add(reader.ReadInt16());
            }

            if (version >= 8) {
                Pool = (PoolType)reader.ReadInt16();
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.WriteString(FILE_IDENTIFIER_8);
            writer.Write((byte)0);

            writer.Write((int)format);

            writer.Write(BoundingBox.min);
            writer.Write(BoundingBox.max);

            writer.Write((short)BoneTable.Count);

            BoneTable.ForEach(bone => {
                writer.Write(bone);
            });

            writer.Write((short)Vertices.Count);

            Vertices.ForEach(vertex => {
                writer.Write(vertex.Position);
            });

            if (NormalsEnabled) {
                Vertices.ForEach(vertex => {
                    writer.Write(vertex.Normal);
                });
            }

            if (ColoursEnabled) {
                Vertices.ForEach(vertex => {
                    writer.Write(vertex.Colour.a);
                    writer.Write(vertex.Colour.r);
                    writer.Write(vertex.Colour.g);
                    writer.Write(vertex.Colour.b);
                });
            }

            if (BonesEnabled) {
                Vertices.ForEach(vertex => {
                    writer.Write(vertex.BoneWeights);
                    writer.Write(vertex.BoneIndices);
                });
            }

            if (TangentsEnabled) {
                Vertices.ForEach(vertex => {
                    writer.Write(vertex.Tangent);
                });
            }

            if (TextureCoordinates1Enabled) {
                Vertices.ForEach(vertex => {
                    writer.Write(vertex.TextureCoordinates[0]);
                });
            }

            if (TextureCoordinates2Enabled) {
                Vertices.ForEach(vertex => {
                    writer.Write(vertex.TextureCoordinates[1]);
                });
            }

            if (TextureCoordinates3Enabled) {
                Vertices.ForEach(vertex => {
                    writer.Write(vertex.TextureCoordinates[2]);
                });
            }

            if (TextureCoordinates4Enabled) {
                Vertices.ForEach(vertex => {
                    writer.Write(vertex.TextureCoordinates[3]);
                });
            }

            writer.Write((short)Indices.Count);

            Indices.ForEach(index => {
                writer.Write(index);
            });

            writer.Write((short)Materials.Count);

            Materials.ForEach(material => {
                writer.Write(material);
            });

            writer.Write((short)Strips.Count);

            Strips.ForEach(strip => {
                writer.Write(strip);
            });

            writer.Write((short)Pool);
        }

        /// <summary>
        /// Checks the flag against the vertex format.
        /// </summary>
        /// <param name="flag">The vertex format flag.</param>
        /// <returns>A value indicating if the flag is set.</returns>
        private bool CheckFlag(VertexFormat flag) {
            return (format & flag) == flag;
        }

        /// <summary>
        /// Enables the flag on the vertex format.
        /// </summary>
        /// <param name="flag">The vertex format flag.</param>
        /// <param name="enabled">if set to <c>true</c> the flag is set.</param>
        private void SetFlag(VertexFormat flag, bool enabled) {
            bool currentValue = CheckFlag(flag);

            if (enabled && !currentValue) {
                format = format | flag;
            } else if (!enabled && currentValue) {
                format = format ^ flag;
            }
        }

        /// <summary>
        /// Clears all vertices, indices, bone tables, materials and strips.
        /// </summary>
        public void Clear() {
            BoneTable.Clear();
            Vertices.Clear();
            Indices.Clear();
            Materials.Clear();
            Strips.Clear();
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();

            Pool = PoolType.Static;
            format = VertexFormat.Position | VertexFormat.TextureCoordinate1;

            Clear();
        }
    }
}