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

namespace Revise.Files.TSI {
    /// <summary>
    /// Represents a sprite.
    /// </summary>
    public class Sprite {
        #region Properties

        /// <summary>
        /// Gets or sets the texture.
        /// </summary>
        public short Texture {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the first X coordinate.
        /// </summary>
        public int X1 {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the first Y coordinate.
        /// </summary>
        public int Y1 {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the second X coordinate.
        /// </summary>
        public int X2 {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the second Y coordinate.
        /// </summary>
        public int Y2 {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        public int Colour {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sprite ID.
        /// </summary>
        public string ID {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Sprite"/> class.
        /// </summary>
        public Sprite() {
            ID = string.Empty;
        }
    }
}
