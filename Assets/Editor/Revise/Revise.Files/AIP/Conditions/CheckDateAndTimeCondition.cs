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
    /// Represents a condition to check the current date and time.
    /// </summary>
    public class CheckDateAndTimeCondition : IArtificialIntelligenceCondition {
        #region Properties

        /// <summary>
        /// Gets the condition type.
        /// </summary>
        public ArtificialIntelligenceCondition Type {
            get {
                return ArtificialIntelligenceCondition.CheckDateAndTime;
            }
        }

        /// <summary>
        /// Gets or sets the day of the month between 1 and 31.
        /// </summary>
        public byte Date {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum hour value.
        /// </summary>
        public byte MinimumHour {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum minute value.
        /// </summary>
        public byte MinimumMinute {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum hour value.
        /// </summar>y
        public byte MaximumHour {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum minute value.
        /// </summary>
        public byte MaximumMinute {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Reads the condition data from the underlying stream.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void Read(BinaryReader reader) {
            Date = reader.ReadByte();
            MinimumHour = reader.ReadByte();
            MinimumMinute = reader.ReadByte();
            MaximumHour = reader.ReadByte();
            MaximumMinute = reader.ReadByte();
        }

        /// <summary>
        /// Writes the condition data to the underlying stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void Write(BinaryWriter writer) {
            writer.Write(Date);
            writer.Write(MinimumHour);
            writer.Write(MinimumMinute);
            writer.Write(MaximumHour);
            writer.Write(MaximumMinute);
        }
    }
}