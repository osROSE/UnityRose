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

using System;

namespace Revise.Files.AIP.Attributes {
    /// <summary>
    /// Represents an attribute for associating classes with condition or action types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ArtificialIntelligenceTypeAttribute : Attribute {
        #region Properties

        /// <summary>
        /// Gets the class associated with the condition or action type.
        /// </summary>
        public Type Type {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtificialIntelligenceTypeAttribute"/> class.
        /// </summary>
        /// <param name="type">The class associated with the condition or action type.</param>
        public ArtificialIntelligenceTypeAttribute(Type type) {
            Type = type;
        }
    }
}