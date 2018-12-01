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

namespace Revise.Files.AIP.Conditions {
    /// <summary>
    /// Represents a condition to check the magic status of the source or target character.
    /// </summary>
    public class CheckMagicStatusCondition : IArtificialIntelligenceCondition {
        #region Properties

        /// <summary>
        /// Gets the condition type.
        /// </summary>
        public ArtificialIntelligenceCondition Type {
            get {
                return ArtificialIntelligenceCondition.CheckMagicStatus;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to check the attacking target or source character.
        /// </summary>
        public bool CheckTarget {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the magic status type to check.
        /// </summary>
        public MagicStatus Status {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating if the condition should pass if the targets magic status value is set or unset.
        /// </summary>
        public bool IsTrue {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Reads the condition data from the underlying stream.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void Read(BinaryReader reader) {
            CheckTarget = reader.ReadBoolean();
            Status = (MagicStatus)reader.ReadByte();
            IsTrue = reader.ReadBoolean();
        }

        /// <summary>
        /// Writes the condition data to the underlying stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(BinaryWriter writer) {
            writer.Write(CheckTarget);
            writer.Write((byte)Status);
            writer.Write(IsTrue);
        }
    }
}