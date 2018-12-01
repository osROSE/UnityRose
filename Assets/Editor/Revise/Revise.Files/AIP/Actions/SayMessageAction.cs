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
using Revise.Files.AIP.Interfaces;
using Revise.IO;

namespace Revise.Files.AIP.Actions {
    /// <summary>
    /// Represents an action to say the specified message.
    /// </summary>
    public class SayMessageAction : IArtificialIntelligenceAction {
        #region Properties

        /// <summary>
        /// Gets the action type.
        /// </summary>
        public ArtificialIntelligenceAction Type {
            get {
                return ArtificialIntelligenceAction.SayMessage;
            }
        }

        /// <summary>
        /// Gets or sets the text message.
        /// </summary>
        public string Text {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the LTB string ID.
        /// </summary>
        public int StringID {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Text"/> is enabled, if true, <see cref="StringID"/> is not used.
        /// </summary>
        public bool UseText {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SayMessageAction"/> class.
        /// </summary>
        public SayMessageAction() {
            Text = string.Empty;
        }

        /// <summary>
        /// Reads the condition data from the underlying stream.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void Read(BinaryReader reader) {
            reader.BaseStream.Seek(-8, SeekOrigin.Current);

            int size = reader.ReadInt32() - 8;
            UseText = size != 4;

            reader.BaseStream.Seek(4, SeekOrigin.Current);

            if (reader.GetType() == typeof(AlignedBinaryReader)) {
                AlignedBinaryReader alignedReader = (AlignedBinaryReader)reader;
                alignedReader.Reset();

                if (UseText) {
                    alignedReader.DisableAlignment = true;
                }
            }

            if (UseText) {
                Text = reader.ReadString(size);
            } else {
                StringID = reader.ReadInt32();
            }
        }

        /// <summary>
        /// Writes the condition data to the underlying stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(BinaryWriter writer) {
            if (UseText) {
                writer.WriteString(Text);
            } else {
                writer.Write(StringID);
            }
        }
    }
}