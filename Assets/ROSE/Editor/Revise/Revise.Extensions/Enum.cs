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
using System.Linq;

/// <summary>
/// A collection of extensions for the <see cref="Enum"/> class.
/// </summary>
public static class EnumExtensions {
    /// <summary>
    /// Gets the attribute value from the enumeration value.
    /// </summary>
    /// <typeparam name="T">The enumeration type.</typeparam>
    /// <typeparam name="Expected">The return type expected.</typeparam>
    /// <param name="expression">The lambda expression.</param>
    /// <returns>The expected value.</returns>
    public static Expected GetAttributeValue<T, Expected>(this Enum enumeration, Func<T, Expected> expression)
        where T : Attribute {
        T attribute = enumeration.GetType().GetMember(enumeration.ToString())[0].GetCustomAttributes(typeof(T), false).Cast<T>().SingleOrDefault();

        if (attribute == null) {
            return default(Expected);
        }

        return expression(attribute);
    }
}