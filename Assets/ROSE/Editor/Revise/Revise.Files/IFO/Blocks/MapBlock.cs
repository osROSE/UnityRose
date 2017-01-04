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
using Revise.Files.IFO.Interfaces;
using UnityEngine;

namespace Revise.Files.IFO.Blocks {
    /// <summary>
    /// Represents a map block.
    /// </summary>
    public class MapBlock : IMapBlock {
        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the warp ID.
        /// </summary>
        public short WarpID {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the event ID.
        /// </summary>
        public short EventID {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the object type.
        /// </summary>
        public int ObjectType {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the object ID.
        /// </summary>
        public int ObjectID {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the map position.
        /// </summary>
        public IntVector2 MapPosition {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Vector3 Position {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        public Quaternion Rotation {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        public Vector3 Scale {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Reads the block data from the underlying stream.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public virtual void Read(BinaryReader reader) {
            Name = reader.ReadByteString();
            WarpID = reader.ReadInt16();
            EventID = reader.ReadInt16();
            ObjectType = reader.ReadInt32();
            ObjectID = reader.ReadInt32();
            MapPosition = new IntVector2(reader.ReadInt32(), reader.ReadInt32());
            Rotation = reader.ReadQuaternion();
            Position = reader.ReadVector3();
            Scale = reader.ReadVector3();
        }

        /// <summary>
        /// Writes the block data to the underlying stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public virtual void Write(BinaryWriter writer) {
            writer.WriteByteString(Name);
            writer.Write(WarpID);
            writer.Write(EventID);
            writer.Write(ObjectType);
            writer.Write(ObjectID);
            writer.Write(MapPosition.X);
            writer.Write(MapPosition.Y);
            writer.Write(Rotation);
            writer.Write(Position);
            writer.Write(Scale);
        }
    }
}