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
    /// Represents a condition to compare abilities.
    /// </summary>
    public class CompareAbilityCondition : IArtificialIntelligenceCondition {
        #region Properties

        /// <summary>
        /// Gets the condition type.
        /// </summary>
        public ArtificialIntelligenceCondition Type {
            get {
                return ArtificialIntelligenceCondition.CompareAbility;
            }
        }

        /// <summary>
        /// Gets or sets the health value.
        /// </summary>
        public AbilityType Ability {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to check if the target characters ability is more or less than the source characters.
        /// </summary>
        public bool More {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Reads the condition data from the underlying stream.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void Read(BinaryReader reader) {
            Ability = (AbilityType)reader.ReadByte();
            More = reader.ReadBoolean();
        }

        /// <summary>
        /// Writes the condition data to the underlying stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(BinaryWriter writer) {
            writer.Write((byte)Ability);
            writer.Write(More);
        }
    }
}