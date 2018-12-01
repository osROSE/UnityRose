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
using Revise.Files.PTL.Interfaces;
using UnityEngine;

namespace Revise.Files.PTL {
    /// <summary>
    /// Represents a particle sequence.
    /// </summary>
    public class ParticleSequence {
        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the lifetime range.
        /// </summary>
        public MinMax<float> Lifetime {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the emit rate range.
        /// </summary>
        public MinMax<float> EmitRate {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the loop count.
        /// </summary>
        public int LoopCount {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the spawn direction range.
        /// </summary>
        public MinMax<Vector3> SpawnDirection {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the emit radius range.
        /// </summary>
        public MinMax<Vector3> EmitRadius {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the gravity range.
        /// </summary>
        public MinMax<Vector3> Gravity {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the texture file name.
        /// </summary>
        public string TextureFileName {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the particle count.
        /// </summary>
        public int ParticleCount {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the alignment type.
        /// </summary>
        public AlignmentType Alignment {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the update coordinate type.
        /// </summary>
        public CoordinateType UpdateCoordinate {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width of the texture.
        /// </summary>
        public int TextureWidth {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height of the texture.
        /// </summary>
        public int TextureHeight {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the implementation type.
        /// </summary>
        public ImplementationType Implementation {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the destination blend mode.
        /// </summary>
        public Blend DestinationBlendMode {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the source blend mode.
        /// </summary>
        public Blend SourceBlendMode {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the blend operation.
        /// </summary>
        public BlendOperation BlendOperation {
            get;
            set;
        }

        /// <summary>
        /// Gets the events.
        /// </summary>
        public List<IParticleEvent> Events {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleSequence"/> class.
        /// </summary>
        public ParticleSequence() {
            Name = string.Empty;
            Events = new List<IParticleEvent>();
        }
    }
}
