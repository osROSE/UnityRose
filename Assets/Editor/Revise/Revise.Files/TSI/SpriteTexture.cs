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
    /// Represents a sprite texture.
    /// </summary>
    public class SpriteTexture {
        #region Properties

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string FileName {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the colour key.
        /// </summary>
        public int ColourKey {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteTexture"/> class.
        /// </summary>
        public SpriteTexture() {
            FileName = string.Empty;
        }
    }
}