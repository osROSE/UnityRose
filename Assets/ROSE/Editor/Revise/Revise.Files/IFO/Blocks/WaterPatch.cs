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

namespace Revise.Files.IFO.Blocks {
    /// <summary>
    /// Represents a water patch.
    /// </summary>
    public struct WaterPatch {
        /// <summary>
        /// Gets or sets a value indicating whether this patch has water.
        /// </summary>
        public bool HasWater;

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public float Height;

        /// <summary>
        /// Gets or sets the water type.
        /// </summary>
        public int Type;

        /// <summary>
        /// Gets or sets the ID of the water.
        /// </summary>
        public int ID;

        /// <summary>
        /// Gets or sets the reserved value.
        /// </summary>
        public int Reserved;
    }
}