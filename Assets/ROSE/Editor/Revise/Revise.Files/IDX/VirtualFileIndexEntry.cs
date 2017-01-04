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

namespace Revise.Files.IDX {
    /// <summary>
    /// Represents a file entry in a data index.
    /// </summary>
    public class VirtualFileIndexEntry {
        #region Properties

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        public string FilePath {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the file entry offset.
        /// </summary>
        public int Offset {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the file entry size.
        /// </summary>
        public int Size {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size of the block.
        /// </summary>
        public int BlockSize {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this file entry is deleted.
        /// </summary>
        public bool IsDeleted {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this file entry is compressed.
        /// </summary>
        public bool IsCompressed {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this file entry is encrypted.
        /// </summary>
        public bool IsEncrypted {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public int Version {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the checksum.
        /// </summary>
        public int Checksum {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualFileIndexEntry"/> class.
        /// </summary>
        public VirtualFileIndexEntry() {
            FilePath = string.Empty;
        }
    }
}