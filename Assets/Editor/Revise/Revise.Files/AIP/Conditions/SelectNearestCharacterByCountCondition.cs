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
    /// Represents a condition to select the nearest character.
    /// </summary>
    public class SelectNearestCharacterByCountCondition : IArtificialIntelligenceCondition {
        #region Properties

        /// <summary>
        /// Gets the condition type.
        /// </summary>
        public ArtificialIntelligenceCondition Type {
            get {
                return ArtificialIntelligenceCondition.SelectNearestCharacterByCount;
            }
        }

        /// <summary>
        /// Gets or sets the distance.
        /// </summary>
        public int Distance {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to find allied or enemy characters.
        /// </summary>
        public bool IsAllied {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum level difference.
        /// </summary>
        public short MinimumLevelDifference {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maxmimum level difference.
        /// </summary>
        public short MaxmimumLevelDifference {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the count for the maximum number of characters.
        /// </summary>
        public short CharacterCount {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the comparison operator to use when comparing the number of characters found to <see cref="CharacterCount"/>.
        /// </summary>
        public ComparisonOperator Operation {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Reads the condition data from the underlying stream.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void Read(BinaryReader reader) {
            Distance = reader.ReadInt32();
            IsAllied = reader.ReadBoolean();
            MinimumLevelDifference = reader.ReadInt16();
            MaxmimumLevelDifference = reader.ReadInt16();
            CharacterCount = reader.ReadInt16();
            Operation = (ComparisonOperator)reader.ReadByte();
        }

        /// <summary>
        /// Writes the condition data to the underlying stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(BinaryWriter writer) {
            writer.Write(Distance);
            writer.Write(IsAllied);
            writer.Write(MinimumLevelDifference);
            writer.Write(MaxmimumLevelDifference);
            writer.Write(CharacterCount);
            writer.Write((byte)Operation);
        }
    }
}