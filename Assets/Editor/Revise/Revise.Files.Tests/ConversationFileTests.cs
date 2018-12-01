#region License

/**
 * Copyright (C) 2011 Jack Wakefield
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
using NUnit.Framework;
using Revise.Files.CON;

namespace Revise.Files.Tests {
    /// <summary>
    /// Provides testing for the <see cref="ConversationFile"/> class.
    /// </summary>
    [TestFixture]
    public class ConversationFileTests {
        private const string TEST_FILE = "Tests/Revise/Files/EM01-001.CON";

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [Test]
        public void TestLoadMethod() {
            Stream stream = File.OpenRead(TEST_FILE);

            stream.Seek(0, SeekOrigin.End);
            long fileSize = stream.Position;
            stream.Seek(0, SeekOrigin.Begin);

            ConversationFile conversationFile = new ConversationFile();
            conversationFile.Load(stream);

            long streamPosition = stream.Position;
            stream.Close();

            Assert.AreEqual(fileSize, streamPosition, "Not all of the file was read");
        }

        /// <summary>
        /// Tests the save method.
        /// </summary>
        [Test]
        public void TestSaveMethod() {
            ConversationFile conversationFile = new ConversationFile();
            conversationFile.Load(TEST_FILE);

            MemoryStream savedStream = new MemoryStream();
            conversationFile.Save(savedStream);
            savedStream.Seek(0, SeekOrigin.Begin);

            ConversationFile savedConversationFile = new ConversationFile();
            savedConversationFile.Load(savedStream);
            savedStream.Close();

            for (int i = 0; i < conversationFile.Functions.Length; i++) {
                Assert.AreEqual(conversationFile.Functions[i].Name, savedConversationFile.Functions[i].Name, "Function names do not match");
                Assert.AreEqual(conversationFile.Functions[i].IsEnabled, savedConversationFile.Functions[i].IsEnabled, "Function is enabled values do not match");
            }

            Assert.AreEqual(conversationFile.Messages.Count, savedConversationFile.Messages.Count, "Message counts do not match");

            for (int i = 0; i < conversationFile.Messages.Count; i++) {
                Assert.AreEqual(conversationFile.Messages[i].ID, savedConversationFile.Messages[i].ID, "Message ID values do not match");
                Assert.AreEqual(conversationFile.Messages[i].Type, savedConversationFile.Messages[i].Type, "Message type values do not match");
                Assert.AreEqual(conversationFile.Messages[i].TargetWindow, savedConversationFile.Messages[i].TargetWindow, "Message target window values do not match");
                Assert.AreEqual(conversationFile.Messages[i].Condition, savedConversationFile.Messages[i].Condition, "Message condition values do not match");
                Assert.AreEqual(conversationFile.Messages[i].Action, savedConversationFile.Messages[i].Action, "Message action values do not match");
                Assert.AreEqual(conversationFile.Messages[i].StringID, savedConversationFile.Messages[i].StringID, "Message string ID values do not match");
            }

            Assert.AreEqual(conversationFile.Menus.Count, savedConversationFile.Menus.Count, "Menu counts do not match");

            for (int i = 0; i < conversationFile.Menus.Count; i++) {
                Assert.AreEqual(conversationFile.Menus[i].Messages.Count, savedConversationFile.Menus[i].Messages.Count, "Menu message counts do not match");

                for (int j = 0; j < conversationFile.Menus[i].Messages.Count; j++) {
                    Assert.AreEqual(conversationFile.Menus[i].Messages[j].ID, savedConversationFile.Menus[i].Messages[j].ID, "Menu message ID values do not match");
                    Assert.AreEqual(conversationFile.Menus[i].Messages[j].Type, savedConversationFile.Menus[i].Messages[j].Type, "Menu message type values do not match");
                    Assert.AreEqual(conversationFile.Menus[i].Messages[j].TargetWindow, savedConversationFile.Menus[i].Messages[j].TargetWindow, "Menu message target window values do not match");
                    Assert.AreEqual(conversationFile.Menus[i].Messages[j].Condition, savedConversationFile.Menus[i].Messages[j].Condition, "Menu message condition values do not match");
                    Assert.AreEqual(conversationFile.Menus[i].Messages[j].Action, savedConversationFile.Menus[i].Messages[j].Action, "Menu message action values do not match");
                    Assert.AreEqual(conversationFile.Menus[i].Messages[j].StringID, savedConversationFile.Menus[i].Messages[j].StringID, "Menu message string ID values do not match");
                }
            }

            Assert.AreEqual(conversationFile.Script, savedConversationFile.Script, "Script values do not match");
        }
    }
}