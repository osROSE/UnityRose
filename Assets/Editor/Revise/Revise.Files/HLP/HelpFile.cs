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
using System.IO;
using System.Text;
using Revise.Files.Exceptions;

namespace Revise.Files.HLP {
    /// <summary>
    /// Provides the ability to create, open and save HLP files for help data.
    /// </summary>
    public class HelpFile : FileLoader {
        private const string FILE_VERSION = "1.0";

        #region Properties

        /// <summary>
        /// Gets the root node.
        /// </summary>
        public HelpNode RootNode {
            get;
            private set;
        }

        /// <summary>
        /// Gets the pages.
        /// </summary>
        public List<HelpPage> Pages {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HelpFile"/> class.
        /// </summary>
        public HelpFile() {
            Pages = new List<HelpPage>();

            Reset();
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <exception cref="Revise.Exceptions.InvalidVersionException">Thrown when the version of the file is invalid.</exception>
        /// <exception cref="Revise.Exceptions.InvalidHelpNodeCountException">Thrown when the root node count is more than one.</exception>
        public override void Load(Stream stream) {
            BinaryReader reader = new BinaryReader(stream, Encoding.Unicode);

            string version = reader.ReadString(reader.ReadByte(), Encoding.UTF8);

            if (string.Compare(version, FILE_VERSION, false) != 0) {
                throw new InvalidVersionException(version);
            }

            int nodeCount = reader.ReadInt32();

            if (nodeCount != 1) {
                throw new InvalidHelpNodeCountException();
            }

            LoadNode(RootNode, reader);

            int pageCount = reader.ReadInt32();

            for (int i = 0; i < pageCount; i++) {
                HelpPage page = new HelpPage();
                page.Title = reader.ReadString();
                page.Content = reader.ReadString();

                Pages.Add(page);
            }
        }

        /// <summary>
        /// Loads the node and subsequent child nodes from the reader.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="reader">The reader.</param>
        private void LoadNode(HelpNode node, BinaryReader reader) {
            node.Name = reader.ReadString();
            int nodeCount = reader.ReadInt32();

            for (int i = 0; i < nodeCount; i++) {
                HelpNode child = new HelpNode();
                LoadNode(child, reader);

                node.Children.Add(child);
            }
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.Unicode);

            writer.Write((byte)FILE_VERSION.Length);
            writer.WriteString(FILE_VERSION);

            writer.Write(1);
            SaveNode(RootNode, writer);

            writer.Write(Pages.Count);

            Pages.ForEach(page => {
                writer.Write(page.Title);
                writer.Write(page.Content);
            });
        }

        /// <summary>
        /// Saves the node and the children.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="writer">The writer.</param>
        private void SaveNode(HelpNode node, BinaryWriter writer) {
            writer.Write(node.Name);
            writer.Write(node.Children.Count);

            node.Children.ForEach(child => {
                SaveNode(child, writer);
            });
        }

        /// <summary>
        /// Removes all nodes and pages.
        /// </summary>
        public void Clear() {
            RootNode = new HelpNode();
            Pages.Clear();
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();
            Clear();
        }
    }
}