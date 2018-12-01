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

using System;

namespace Revise.Files.STL.Attributes {
    /// <summary>
    /// Represents an attribute for identifying table types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class StringTableTypeIdentifierAttribute : Attribute {
        #region Properties

        /// <summary>
        /// Gets the value.
        /// </summary>
        public string Value {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTableTypeIdentifierAttribute"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public StringTableTypeIdentifierAttribute(string value) {
            Value = value;
        }
    }
}