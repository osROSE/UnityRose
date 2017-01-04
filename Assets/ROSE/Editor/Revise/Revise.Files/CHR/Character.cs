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

namespace Revise.Files.CHR {
    /// <summary>
    /// Represents a character.
    /// </summary>
    public class Character {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this character is enabled.
        /// </summary>
        public bool IsEnabled {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        public short ID {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name {
            get;
            set;
        }

        /// <summary>
        /// Gets the character objects.
        /// </summary>
        public List<CharacterObject> Objects {
            get;
            private set;
        }

        /// <summary>
        /// Gets the character animations.
        /// </summary>
        public List<CharacterAnimation> Animations {
            get;
            private set;
        }

        /// <summary>
        /// Gets the character effects.
        /// </summary>
        public List<CharacterEffect> Effects {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Character"/> class.
        /// </summary>
        public Character() {
            Name = string.Empty;
            IsEnabled = true;

            Objects = new List<CharacterObject>();
            Animations = new List<CharacterAnimation>();
            Effects = new List<CharacterEffect>();
        }
    }
}