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
    /// Represents a map NPC.
    /// </summary>
    public class MapNPC : MapBlock {
        #region Properties

        /// <summary>
        /// Gets or sets an unused AI value.
        /// </summary>
        public int AI {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the conversation file name.
        /// </summary>
        public string ConversationFile {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MapNPC"/> class.
        /// </summary>
        public MapNPC() {
            ConversationFile = string.Empty;
        }

        /// <summary>
        /// Reads the block data from the underlying stream.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public override void Read(BinaryReader reader) {
            base.Read(reader);

            AI = reader.ReadInt32();
            ConversationFile = reader.ReadByteString();
        }

        /// <summary>
        /// Writes the block data to the underlying stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public override void Write(BinaryWriter writer) {
            base.Write(writer);

            writer.Write(AI);
            writer.WriteByteString(ConversationFile);
        }
    }
}