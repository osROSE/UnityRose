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
using Revise.Files.PTL.Interfaces;

namespace Revise.Files.PTL.Events {
    /// <summary>
    /// Represents the default particle event.
    /// </summary>
    public class ParticleEvent : IParticleEvent {
        #region Properties

        /// <summary>
        /// Gets the event type.
        /// </summary>
        public virtual ParticleEventType Type {
            get {
                return ParticleEventType.None;
            }
        }

        /// <summary>
        /// Gets or sets the time range.
        /// </summary>
        public MinMax<float> TimeRange {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this event fades into the next event.
        /// </summary>
        public bool Fade {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Reads the event data from the underlying stream.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public virtual void Read(BinaryReader reader) {
            TimeRange = new MinMax<float>(reader.ReadSingle(), reader.ReadSingle());
            Fade = reader.ReadBoolean();
        }

        /// <summary>
        /// Writes the event data to the underlying stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public virtual void Write(BinaryWriter writer) {
            writer.Write(TimeRange.Minimum);
            writer.Write(TimeRange.Maximum);
            writer.Write(Fade);
        }
    }
}