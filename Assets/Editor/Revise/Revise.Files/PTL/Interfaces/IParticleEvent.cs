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

namespace Revise.Files.PTL.Interfaces {
    /// <summary>
    /// Provides an interface for particle events.
    /// </summary>
    public interface IParticleEvent {
        #region Properties

        /// <summary>
        /// Gets the event type.
        /// </summary>
        ParticleEventType Type {
            get;
        }

        /// <summary>
        /// Gets or sets the time range.
        /// </summary>
        MinMax<float> TimeRange {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this event fades into the next event.
        /// </summary>
        bool Fade {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Reads the event data from the underlying stream.
        /// </summary>
        /// <param name="reader">The reader.</param>
        void Read(BinaryReader reader);

        /// <summary>
        /// Writes the event data to the underlying stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        void Write(BinaryWriter writer);
    }
}