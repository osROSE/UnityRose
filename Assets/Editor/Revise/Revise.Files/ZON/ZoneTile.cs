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

namespace Revise.Files.ZON {
    /// <summary>
    /// Represents a tile.
    /// </summary>
    public class ZoneTile {
        #region Properties

        /// <summary>
        /// Gets or sets the first texture layer.
        /// </summary>
        public int Layer1 {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the second texture layer.
        /// </summary>
        public int Layer2 {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the first texture offset.
        /// </summary>
        public int Offset1 {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the second texture offset.
        /// </summary>
        public int Offset2 {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether blending is enabled.
        /// </summary>
        public bool BlendingEnabled {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rotation applied to the second layer.
        /// </summary>
        public TileRotation Rotation {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the tile.
        /// </summary>
        public int TileType {
            get;
            set;
        }

        #endregion
    }
}