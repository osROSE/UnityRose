﻿#region License

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

using UnityEngine;

namespace Revise.Files.ZMD {
    /// <summary>
    /// Represents a bone.
    /// </summary>
    public class Bone {
        #region Properties

        /// <summary>
        /// Gets or sets the bone parent.
        /// </summary>
        public int Parent {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the bone name.
        /// </summary>
        public string Name {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the translation position value.
        /// </summary>
        public Vector3 Translation {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rotation value.
        /// </summary>
        public Quaternion Rotation {
            get;
            set;
        }

        #endregion
    }
}