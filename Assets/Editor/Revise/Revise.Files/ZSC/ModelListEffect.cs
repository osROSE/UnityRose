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

using UnityEngine;

namespace Revise.Files.ZSC {
    /// <summary>
    /// Represents a model list effect.
    /// </summary>
    public class ModelListEffect {
        #region Properties

        /// <summary>
        /// Gets or sets the effect type.
        /// </summary>
        public EffectType EffectType {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the effect.
        /// </summary>
        public short Effect {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Vector3 Position {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rotation.
        /// </summary>
        public Quaternion Rotation {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        public Vector3 Scale {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        public short Parent {
            get;
            set;
        }

        #endregion
    }
}