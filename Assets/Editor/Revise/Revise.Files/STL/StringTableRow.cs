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

namespace Revise.Files.STL {
    /// <summary>
    /// Represents a string table row.
    /// </summary>
    public class StringTableRow {
        private string[] text;
        private string[] descriptions;
        private string[] startMessages;
        private string[] endMessages;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTableRow"/> class.
        /// </summary>
        /// <param name="languageCount">The number of languages.</param>
        public StringTableRow(int languageCount) {
            text = new string[languageCount];
            descriptions = new string[languageCount];
            startMessages = new string[languageCount];
            endMessages = new string[languageCount];

            for (int i = 0; i < languageCount; i++) {
                text[i] = string.Empty;
                descriptions[i] = string.Empty;
                startMessages[i] = string.Empty;
                endMessages[i] = string.Empty;
            }
        }

        /// <summary>
        /// Gets the text of the specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns>The text.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the specified language is invalid.</exception>
        public string GetText(StringTableLanguage language = StringTableLanguage.English) {
            if (!Enum.IsDefined(typeof(StringTableLanguage), language)) {
                throw new ArgumentException("language", "Language does not exist");
            }

            return text[(int)language];
        }

        /// <summary>
        /// Sets the specified languages text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="language">The language.</param>
        /// <exception cref="System.ArgumentException">Thrown when the specified language is invalid.</exception>
        public void SetText(string text, StringTableLanguage language = StringTableLanguage.English) {
            if (!Enum.IsDefined(typeof(StringTableLanguage), language)) {
                throw new ArgumentException("language", "Language does not exist");
            }

            this.text[(int)language] = text;
        }

        /// <summary>
        /// Gets the description of the specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns>The description.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the specified language is invalid.</exception>
        public string GetDescription(StringTableLanguage language = StringTableLanguage.English) {
            if (!Enum.IsDefined(typeof(StringTableLanguage), language)) {
                throw new ArgumentException("language", "Language does not exist");
            }

            return descriptions[(int)language];
        }

        /// <summary>
        /// Sets the specified languages description.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="language">The language.</param>
        /// <exception cref="System.ArgumentException">Thrown when the specified language is invalid.</exception>
        public void SetDescription(string description, StringTableLanguage language = StringTableLanguage.English) {
            if (!Enum.IsDefined(typeof(StringTableLanguage), language)) {
                throw new ArgumentException("language", "Language does not exist");
            }

            descriptions[(int)language] = description;
        }

        /// <summary>
        /// Gets the start message of the specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns>The start message.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the specified language is invalid.</exception>
        public string GetStartMessage(StringTableLanguage language = StringTableLanguage.English) {
            if (!Enum.IsDefined(typeof(StringTableLanguage), language)) {
                throw new ArgumentException("language", "Language does not exist");
            }

            return startMessages[(int)language];
        }

        /// <summary>
        /// Sets the specified languages start message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="language">The language.</param>
        /// <exception cref="System.ArgumentException">Thrown when the specified language is invalid.</exception>
        public void SetStartMessage(string message, StringTableLanguage language = StringTableLanguage.English) {
            if (!Enum.IsDefined(typeof(StringTableLanguage), language)) {
                throw new ArgumentException("language", "Language does not exist");
            }

            startMessages[(int)language] = message;
        }

        /// <summary>
        /// Gets the end message of the specified language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns>The end message.</returns>
        /// <exception cref="System.ArgumentException">Thrown when the specified language is invalid.</exception>
        public string GetEndMessage(StringTableLanguage language = StringTableLanguage.English) {
            if (!Enum.IsDefined(typeof(StringTableLanguage), language)) {
                throw new ArgumentException("language", "Language does not exist");
            }

            return endMessages[(int)language];
        }

        /// <summary>
        /// Sets the specified languages end message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="language">The language.</param>
        /// <exception cref="System.ArgumentException">Thrown when the specified language is invalid.</exception>
        public void SetEndMessage(string message, StringTableLanguage language = StringTableLanguage.English) {
            if (!Enum.IsDefined(typeof(StringTableLanguage), language)) {
                throw new ArgumentException("language", "Language does not exist");
            }

            endMessages[(int)language] = message;
        }
    }
}