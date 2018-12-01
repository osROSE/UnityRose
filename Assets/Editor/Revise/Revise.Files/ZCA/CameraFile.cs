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
using Revise.Files.Exceptions;
using UnityEngine;

namespace Revise.Files.ZCA {
    /// <summary>
    /// Provides the ability to create, open and save ZCA files used for camera information.
    /// </summary>
    public class CameraFile : FileLoader {
        #region Constants

        private const string FILE_IDENTIFIER = "ZCA0001";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the type of the projection.
        /// </summary>
        public ProjectionType ProjectionType {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the model view matrix.
        /// </summary>
        public Matrix4x4 ModelView {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the projection matrix.
        /// </summary>
        public Matrix4x4 Projection {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the field of view.
        /// </summary>
        public float FieldOfView {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the aspect ratio.
        /// </summary>
        public float AspectRatio {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the near plane.
        /// </summary>
        public float NearPlane {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the far plane.
        /// </summary>
        public float FarPlane {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the eye position.
        /// </summary>
        public Vector3 Eye {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the center position.
        /// </summary>
        public Vector3 Center {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets up position.
        /// </summary>
        public Vector3 Up {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraFile"/> class.
        /// </summary>
        public CameraFile() {
            Reset();
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.GetEncoding("EUC-KR"));

            string identifier = reader.ReadString(7);

            if (string.Compare(identifier, FILE_IDENTIFIER, false) != 0) {
                throw new FileIdentifierMismatchException(FilePath, FILE_IDENTIFIER, identifier);
            }

            ProjectionType = (ProjectionType)reader.ReadInt32();

            ModelView = reader.ReadMatrix();
            Projection = reader.ReadMatrix();

            FieldOfView = reader.ReadSingle();
            AspectRatio = reader.ReadSingle();
            NearPlane = reader.ReadSingle();
            FarPlane = reader.ReadSingle();

            Eye = reader.ReadVector3();
            Center = reader.ReadVector3();
            Up = reader.ReadVector3();
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.GetEncoding("EUC-KR"));

            writer.WriteString(FILE_IDENTIFIER);
            writer.Write((int)ProjectionType);

            writer.Write(ModelView);
            writer.Write(Projection);

            writer.Write(FieldOfView);
            writer.Write(AspectRatio);
            writer.Write(NearPlane);
            writer.Write(FarPlane);

            writer.Write(Eye);
            writer.Write(Center);
            writer.Write(Up);
        }
    }
}
