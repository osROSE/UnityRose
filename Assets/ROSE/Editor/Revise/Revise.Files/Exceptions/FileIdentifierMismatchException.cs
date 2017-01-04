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
using System.IO;

namespace Revise.Files.Exceptions {
    /// <summary>
    /// The exception that is thrown when the file identifier read does not match the expected value.
    /// </summary>
    public class FileIdentifierMismatchException : Exception {
        /// <summary>
        /// The format of the exception message.
        /// </summary>
        private const string MESSAGE_FORMAT = "File '{0}' has incorrect file identifier; Expecting '{1}', encountered '{2}'";

        #region Properties

        /// <summary>
        /// Gets the file path of the file which threw the exception.
        /// </summary>
        public string FilePath {
            get;
            private set;
        }

        /// <summary>
        /// Gets the file identifier the file reader expected.
        /// </summary>
        public string IdentifierExpected {
            get;
            private set;
        }

        /// <summary>
        /// Gets the file identifier the file reader read.
        /// </summary>
        public string IdentifierRead {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FileIdentifierMismatchException"/> class.
        /// </summary>
        /// <param name="filePath">The file path of the file which threw the exception.</param>
        /// <param name="identifierExpected">The file identifier the file reader expected.</param>
        /// <param name="identifierRead">The file identifier the file reader read.</param>
        public FileIdentifierMismatchException(string filePath, string identifierExpected, string identifierRead)
            : base(string.Format(MESSAGE_FORMAT, filePath, identifierExpected, identifierRead)) {
            FilePath = Path.GetFullPath(filePath);
            IdentifierExpected = identifierExpected;
            IdentifierRead = identifierRead;
        }
    }
}