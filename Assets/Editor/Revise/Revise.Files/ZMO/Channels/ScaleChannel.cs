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

using System;
using System.IO;

namespace Revise.Files.ZMO.Channels {
    /// <summary>
    /// Represents a scale channel.
    /// </summary>
    public class ScaleChannel : MotionChannel {
        #region Properties

        /// <summary>
        /// Gets the channel type.
        /// </summary>
        public override ChannelType Type {
            get {
                return ChannelType.Scale;
            }
        }

        /// <summary>
        /// Sets the frame count.
        /// </summary>
        internal override int FrameCount {
            set {
                Array.Resize<float>(ref frames, value);
            }
        }

        /// <summary>
        /// Gets the frames.
        /// </summary>
        public float[] Frames {
            get {
                return frames;
            }
        }

        #endregion

        private float[] frames;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleChannel"/> class.
        /// </summary>
        public ScaleChannel() {
            frames = new float[0];
        }

        /// <summary>
        /// Reads a channel frame from the underlying stream.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="frame">The frame to read.</param>
        public override void ReadFrame(BinaryReader reader, int frame) {
            frames[frame] = reader.ReadSingle();
        }

        /// <summary>
        /// Writes the specified channel frame to the underlying stream.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="frame">The frame to write.</param>
        public override void WriteFrame(BinaryWriter writer, int frame) {
            writer.Write(frames[frame]);
        }
    }
}