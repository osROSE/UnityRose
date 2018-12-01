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
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Revise.Files.CON {
    /// <summary>
    /// Provides the ability to create, open and save CON files for conversation data.
    /// </summary>
    public class ConversationFile : FileLoader {
        private const int FUNCTION_COUNT = 16;

        #region Properties

        /// <summary>
        /// Gets the list of functions which are executed when opening the conversation.
        /// </summary>
        public ConversationFunction[] Functions {
            get;
            private set;
        }

        /// <summary>
        /// Gets the messages.
        /// </summary>
        public List<ConversationMessage> Messages {
            get;
            private set;
        }

        /// <summary>
        /// Gets the menus.
        /// </summary>
        public List<ConversationMenu> Menus {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the compiled script data.
        /// </summary>
        public byte[] Script {
            get;
            set;
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversationFile"/> class.
        /// </summary>
        public ConversationFile() {
            Functions = new ConversationFunction[FUNCTION_COUNT];
            Messages = new List<ConversationMessage>();
            Menus = new List<ConversationMenu>();

            Reset();
        }

        /// <summary>
        /// Loads the file from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public override void Load(Stream stream) {
            Encoding encoding = Encoding.GetEncoding("EUC-KR");
            BinaryReader reader = new BinaryReader(stream, encoding);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            short functionMask = reader.ReadInt16();

            for (int i = 0; i < Functions.Length; i++) {
                ConversationFunction function = Functions[i];
                function.Name = reader.ReadString(32).TrimEnd('\0');
                function.IsEnabled = (functionMask & (1 << i)) != 0;
            }

            stream.Seek(2, SeekOrigin.Current);

            int conversationOffset = reader.ReadInt32();
            int scriptOffset = reader.ReadInt32();

            int messageCount = reader.ReadInt32();
            int messageOffset = conversationOffset + reader.ReadInt32();

            int menuCount = reader.ReadInt32();
            int menuOffset = conversationOffset + reader.ReadInt32();

            for (int i = 0; i < messageCount; i++) {
                stream.Seek(messageOffset + sizeof(int) * i, SeekOrigin.Begin);
                int offset = reader.ReadInt32();

                stream.Seek(messageOffset + offset, SeekOrigin.Begin);

                ConversationMessage message = new ConversationMessage();
                message.ID = reader.ReadInt32();
                message.Type = (ConversationMessageType)reader.ReadInt32();
                message.TargetWindow = reader.ReadInt32();
                message.Condition = reader.ReadString(32).TrimEnd('\0');
                message.Action = reader.ReadString(32).TrimEnd('\0');
                message.StringID = reader.ReadInt32();

                Messages.Add(message);
            }

            for (int i = 0; i < menuCount; i++) {
                ConversationMenu menu = new ConversationMenu();

                stream.Seek(menuOffset + sizeof(int) * i, SeekOrigin.Begin);
                int menuDataOffset = menuOffset + reader.ReadInt32();

                stream.Seek(menuDataOffset, SeekOrigin.Begin);
                int menuSize = reader.ReadInt32();
                int menuMessageCount = reader.ReadInt32();

                for (int j = 0; j < menuMessageCount; j++) {
                    stream.Seek(menuDataOffset + sizeof(int) * 2 + sizeof(int) * j, SeekOrigin.Begin);
                    int menuItemOffset = Obfuscate(reader.ReadInt32(), menuMessageCount, menuSize);
                    
                    stream.Seek(menuDataOffset + menuItemOffset, SeekOrigin.Begin);

                    ConversationMessage message = new ConversationMessage();
                    message.ID = Obfuscate(reader.ReadInt32(), menuMessageCount, menuSize);
                    message.Type = (ConversationMessageType)Obfuscate(reader.ReadInt32(), menuMessageCount, menuSize);
                    message.TargetWindow = Obfuscate(reader.ReadInt32(), menuMessageCount, menuSize);

                    byte[] condition = reader.ReadBytes(32);
                    Obfuscate(condition, menuMessageCount, menuSize);
                    message.Condition = encoding.GetString(condition).TrimEnd('\0');

                    byte[] action = reader.ReadBytes(32);
                    Obfuscate(action, menuMessageCount, menuSize);
                    message.Action = encoding.GetString(action).TrimEnd('\0');

                    message.StringID = Obfuscate(reader.ReadInt32(), menuMessageCount, menuSize);
                    
                    menu.Messages.Add(message);
                }

                Menus.Add(menu);
            }

            stream.Seek(scriptOffset, SeekOrigin.Begin);

            int scriptSize = reader.ReadInt32();
            byte[] script = reader.ReadBytes(scriptSize);

            Obfuscate(script, scriptSize, (int)fileSize);

            Script = script;
        }

        /// <summary>
        /// Saves the file to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        public override void Save(Stream stream) {
            Encoding encoding = Encoding.GetEncoding("EUC-KR");
            BinaryWriter writer = new BinaryWriter(stream, encoding);

            short functionMask = 0;

            for (int i = 0; i < Functions.Length; i++) {
                ConversationFunction function = Functions[i];

                if (function.IsEnabled) {
                    functionMask |= (short)(1 << i);
                }
            }

            writer.Write(functionMask);

            for (int i = 0; i < Functions.Length; i++) {
                ConversationFunction function = Functions[i];
                writer.WriteString(function.Name, 32);
            }

            writer.Write((ushort)0xCCCC);

            long offsets = stream.Position;

            writer.Write(0);
            writer.Write(0);

            long conversationOffset = stream.Position;

            writer.Write(0);
            writer.Write(0);

            writer.Write(0);
            writer.Write(0);

            long messageOffset = stream.Position;

            for (int i = 0; i < Messages.Count; i++) {
                writer.Write(0);
            }

            for (int i = 0; i < Messages.Count; i++) {
                long offset = stream.Position - messageOffset;

                ConversationMessage message = Messages[i];
                writer.Write(message.ID);
                writer.Write((int)message.Type);
                writer.Write(message.TargetWindow);
                writer.WriteString(message.Condition, 32);
                writer.WriteString(message.Action, 32);
                writer.Write(message.StringID);

                long nextMessageOffset = stream.Position;

                stream.Seek(messageOffset + sizeof(int) * i, SeekOrigin.Begin);
                writer.Write((int)offset);

                stream.Seek(nextMessageOffset, SeekOrigin.Begin);
            }

            long menuOffset = stream.Position;

            for (int i = 0; i < Menus.Count; i++) {
                writer.Write(0);
            }

            for (int i = 0; i < Menus.Count; i++) {
                long offset = stream.Position;

                ConversationMenu menu = Menus[i];
                int menuSize = sizeof(int) * 2 + menu.Messages.Count * (sizeof(int) * 5 + 32 + 32);
                writer.Write(menuSize);
                writer.Write(menu.Messages.Count);

                for (int j = 0; j < menu.Messages.Count; j++) {
                    writer.Write(0);
                }

                for (int j = 0; j < menu.Messages.Count; j++) {
                    long menuItemOffset = stream.Position - offset;

                    ConversationMessage message = menu.Messages[j];
                    writer.Write(Obfuscate(message.ID, menu.Messages.Count, menuSize));
                    writer.Write(Obfuscate((int)message.Type, menu.Messages.Count, menuSize));
                    writer.Write(Obfuscate(message.TargetWindow, menu.Messages.Count, menuSize));

                    byte[] condition = encoding.GetBytes(message.Condition);
                    Array.Resize(ref condition, 32);
                    Obfuscate(condition, menu.Messages.Count, menuSize);

                    byte[] action = encoding.GetBytes(message.Action);
                    Array.Resize(ref action, 32);
                    Obfuscate(action, menu.Messages.Count, menuSize);

                    writer.Write(condition);
                    writer.Write(action);
                    writer.Write(Obfuscate(message.StringID, menu.Messages.Count, menuSize));

                    long nextMessageOffset = stream.Position;

                    stream.Seek(offset + sizeof(int) * 2 + sizeof(int) * j, SeekOrigin.Begin);
                    writer.Write(Obfuscate((int)menuItemOffset, menu.Messages.Count, menuSize));
                    
                    stream.Seek(nextMessageOffset, SeekOrigin.Begin);
                }

                long nextMenuOffset = stream.Position;

                stream.Seek(menuOffset + sizeof(int) * i, SeekOrigin.Begin);
                writer.Write((int)(offset - menuOffset));

                stream.Seek(nextMenuOffset, SeekOrigin.Begin);
            }

            long scriptOffset = stream.Position;

            byte[] script = new byte[Script.Length];
            Array.Copy(Script, script, Script.Length);

            Obfuscate(script, script.Length, (int)(scriptOffset + sizeof(int) + script.Length));

            writer.Write(script.Length);
            writer.Write(script);

            stream.Seek(offsets, SeekOrigin.Begin);

            writer.Write((int)conversationOffset);
            writer.Write((int)scriptOffset);

            writer.Write(Messages.Count);
            writer.Write((int)(messageOffset - conversationOffset));

            writer.Write(Menus.Count);
            writer.Write((int)(menuOffset - conversationOffset));
        }

        /// <summary>
        /// Removes functions, messages and menus.
        /// </summary>
        public void Clear() {
            for (int i = 0; i < Functions.Length; i++) {
                Functions[i] = new ConversationFunction();
            }

            Messages.Clear();
            Menus.Clear();
        }

        /// <summary>
        /// Resets properties to their default values.
        /// </summary>
        public override void Reset() {
            base.Reset();

            Script = new byte[0];

            Clear();
        }

        /// <summary>
        /// Obfuscates the specified value using the keys provided.
        /// </summary>
        /// <param name="value">The value to obfuscate.</param>
        /// <param name="keyA">The first key.</param>
        /// <param name="keyB">The second key.</param>
        /// <returns>The obfuscated value.</returns>
        private static int Obfuscate(int value, int keyA, int keyB) {
            byte key = (keyA & 1) != 0 ? (byte)keyA : (byte)keyB;

            return value ^ ((key << 24) | (key << 16) | (key << 8) | key);
        }

        /// <summary>
        /// Obfuscates the specified array using the keys provided.
        /// </summary>
        /// <param name="values">The values to obfuscate.</param>
        /// <param name="keyA">The first key.</param>
        /// <param name="keyB">The second key.</param>
        private static void Obfuscate(byte[] values, int keyA, int keyB) {
            byte key = (keyA & 1) != 0 ? (byte)keyA : (byte)keyB;

            for (int i = 0; i < values.Length; i++) {
                values[i] ^= key;
            }
        }
    }
}