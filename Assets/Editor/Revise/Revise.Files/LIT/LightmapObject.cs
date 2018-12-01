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

using System.Collections.Generic;

namespace Revise.Files.LIT {
    /// <summary>
    /// Represents a lightmap object.
    /// </summary>
    public class LightmapObject {
        #region Properties

        /// <summary>
        /// Gets or sets the ID of the object.
        /// </summary>
        public int ID {
            get;
            set;
        }

        /// <summary>
        /// Gets the light parts.
        /// </summary>
        public List<LightmapPart> Parts {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LightmapObject"/> class.
        /// </summary>
        public LightmapObject() {
            Parts = new List<LightmapPart>();
        }
    }
}