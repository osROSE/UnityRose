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

namespace Revise.Files.LIT {
    /// <summary>
    /// Represents an lightmap part.
    /// </summary>
    public class LightmapPart {
        #region Properties

        /// <summary>
        /// Gets or sets the name of the part.
        /// </summary>
        public string Name {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ID of the part.
        /// </summary>
        public int ID {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the lightmap graphics file.
        /// </summary>
        public string FileName {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the lightmap index value.
        /// </summary>
        public int LightmapIndex {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the pixels per object value.
        /// </summary>
        public int PixelsPerObject {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the objects per width value.
        /// </summary>
        public int ObjectsPerWidth {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the object position.
        /// </summary>
        public int ObjectPosition {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LightmapPart"/> class.
        /// </summary>
        public LightmapPart() {
            Name = string.Empty;
            FileName = string.Empty;
        }
    }
}