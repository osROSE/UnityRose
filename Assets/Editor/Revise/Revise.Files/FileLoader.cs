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

using System.IO;
using Revise.Files.Exceptions;

namespace Revise.Files {
    /// <summary>
    /// Provides a base for classes which load and save files.
    /// </summary>
    public abstract class FileLoader {
        #region Properties

        /// <summary>
        /// Gets the file path of the loaded file.
        /// </summary>
        public string FilePath {
            get;
            protected set;
        }

        #endregion

        /// <summary>
        /// Loads the file at the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <exception cref="System.IO.FileNotFoundException">Thrown when the specified file path does not exist.</exception>
        /// <exception cref="Revise.Exceptions.FileInUseException">Thrown when the specified file is in use by another process.</exception>
        public virtual void Load(string filePath) {
            FileInfo file = new FileInfo(filePath);

            if (!file.Exists) {
                throw new FileNotFoundException(filePath);
            }

            FileStream stream;

            try {
                stream = file.Open(FileMode.Open);
            } catch (IOException) {
                throw new FileInUseException(filePath);
            }

            Reset();
            FilePath = filePath;

            Load(stream);

            stream.Close();
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public abstract void Load(Stream stream);

        /// <summary>
        /// Saves the file at the previously loaded file path.
        /// </summary>
        /// <exception cref="Revise.Exceptions.FileNotLoadedException">Thrown when the load method has not been called before-hand.</exception>
        public virtual void Save() {
            if (FilePath == null) {
                throw new FileNotLoadedException("The file must be loaded before saving without specifying a file path");
            }

            Save(FilePath);
        }

        /// <summary>
        /// Saves the file at the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <exception cref="Revise.Exceptions.FileReadOnlyException">Thrown when the specified file is set to read-only.</exception>
        /// <exception cref="Revise.Exceptions.FileInUseException">Thrown when the specified file is in use by another process.</exception>
        public virtual void Save(string filePath) {
            FilePath = filePath;

            FileInfo file = new FileInfo(filePath);

            if (file.Exists && file.IsReadOnly) {
                throw new FileReadOnlyException(filePath);
            }

            FileStream stream;

            try {
                stream = file.Open(FileMode.Create);
            } catch (IOException) {
                throw new FileInUseException(filePath);
            }

            Save(stream);

            stream.Close();
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public abstract void Save(Stream stream);

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public virtual void Reset() {
            FilePath = null;
        }
    }
}