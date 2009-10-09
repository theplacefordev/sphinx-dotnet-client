using System;
using System.IO;
using System.Text;
using Sphinx.Client.Commands.BuildExcerpts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sphinx.Client.Connections;
using System.Collections.Generic;
using Sphinx.Client.IO;
using Sphinx.Client.Commands;
using Sphinx.Client.Commands.Collections;
using Sphinx.Client.UnitTests.Mock.Connections;
using Sphinx.Client.UnitTests.Mock.IO;

namespace Sphinx.Client.UnitTests.Test.BuildExcerpts
{
    
    
    /// <summary>
    /// This is a test class for BuildExcerptsCommandUnitTest and is intended
    /// to contain all BuildExcerptsCommand Unit Tests
    ///</summary>
    [TestClass]
    public class BuildExcerptsCommandUnitTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        #region Tests

        /// <summary>
        ///A test for BuildExcerptsCommand Constructor
        ///</summary>
        [TestMethod]
        public void BuildExcerptsCommandDefaultConstructorTest()
        {
            // null value
            try
            {
                new BuildExcerptsCommand(null);
                Assert.Fail("ArgumentException exception must thrown for invalid argument value");
            }
            catch (ArgumentException)
            {
                // test passed
            }
        }

        /// <summary>
        ///A test for BuildExcerptsCommand Constructor
        ///</summary>
        [TestMethod]
        public void BuildExcerptsCommandConstructorTest()
        {
            using (ConnectionBase connection = new ConnectionMock())
            {
                string[] documents = new string[] { "doc 1", "doc 2", "doc 3" };
                string[] keywords = new string[] { "keyword1", "keyword2" };
                string index = "test";
                BuildExcerptsCommand target = new BuildExcerptsCommand(connection, documents, keywords, index);
                Assert.AreEqual(target.Documents.Count, documents.Length);
                Assert.AreEqual(target.Documents[0], documents[0]);
                Assert.AreEqual(target.Documents[1], documents[1]);
                Assert.AreEqual(target.Documents[2], documents[2]);
                Assert.AreEqual(target.Keywords.Count, keywords.Length);
                Assert.AreEqual(target.Keywords[0], keywords[0]);
                Assert.AreEqual(target.Keywords[1], keywords[1]);
                Assert.AreEqual(target.Index, index);
            }
        }

        /// <summary>
        ///A test for BeforeMatch
        ///</summary>
        [TestMethod]
        public void BeforeMatchTest()
        {
            using (ConnectionBase connection = new ConnectionMock())
            {
                BuildExcerptsCommand target = new BuildExcerptsCommand(connection);
                // valid value
                string expected = "<pre>";
                target.BeforeMatch = expected;
                string actual = target.BeforeMatch;
                Assert.AreEqual(expected, actual);
                // invalid value
                try
                {
                    target.BeforeMatch = null;
                    Assert.Fail("ArgumentException exception must thrown for invalid argument value");
                }
                catch (ArgumentException)
                {
                    // test passed
                }
            }
        }

        /// <summary>
        ///A test for AfterMatch
        ///</summary>
        [TestMethod]
        public void AfterMatchTest()
        {
            using (ConnectionBase connection = new ConnectionMock())
            {
                BuildExcerptsCommand target = new BuildExcerptsCommand(connection);
                // valid value
                string expected = "</pre>";
                target.AfterMatch = expected;
                string actual = target.AfterMatch;
                Assert.AreEqual(expected, actual);
                // invalid value
                try
                {
                    target.AfterMatch = null;
                    Assert.Fail("ArgumentException exception must thrown for invalid argument value");
                }
                catch (ArgumentException)
                {
                    // test passed
                }
            }
        }

		/// <summary>
        ///A test for WordsAroundKeyword
        ///</summary>
        [TestMethod]
        public void WordsAroundKeywordTest()
        {
            using (ConnectionBase connection = new ConnectionMock())
            {
                BuildExcerptsCommand target = new BuildExcerptsCommand(connection);
                // valid value
                int expected = 0;
                target.WordsAroundKeyword = expected;
                int actual = target.WordsAroundKeyword;
                Assert.AreEqual(expected, actual);
                // invalid value
                try
                {
                    target.WordsAroundKeyword = -1;
                    Assert.Fail("ArgumentException exception must thrown for invalid argument value");
                }
                catch (ArgumentException)
                {
                    // test passed
                }
            }
        }

        /// <summary>
        ///A test for SnippetSizeLimit
        ///</summary>
        [TestMethod]
        public void SnippetSizeLimitTest()
        {
            using (ConnectionBase connection = new ConnectionMock())
            {
                BuildExcerptsCommand target = new BuildExcerptsCommand(connection);
                // valid value
                int expected = 1;
                target.SnippetSizeLimit = expected;
                int actual = target.SnippetSizeLimit;
                Assert.AreEqual(expected, actual);
                // invalid value
                try
                {
                    target.SnippetSizeLimit = 0;
                    Assert.Fail("ArgumentException exception must thrown for invalid argument value");
                }
                catch (ArgumentException)
                {
                    // test passed
                }
            }
        }

        /// <summary>
        ///A test for SnippetsDelimiter
        ///</summary>
        [TestMethod]
        public void SnippetsDelimiterTest()
        {
            using (ConnectionBase connection = new ConnectionMock())
            {
                BuildExcerptsCommand target = new BuildExcerptsCommand(connection);
                // valid value
                string expected = String.Empty;
                target.SnippetsDelimiter = expected;
                string actual = target.SnippetsDelimiter;
                Assert.AreEqual(expected, actual);
                // invalid value
                try
                {
                    target.SnippetsDelimiter = null;
                    Assert.Fail("ArgumentException exception must thrown for invalid argument value");
                }
                catch (ArgumentException)
                {
                    // test passed
                }
            }
        }

        /// <summary>
        /// A test for Keywords
        ///</summary>
        [TestMethod]
        public void KeywordsTest()
        {
            using (ConnectionMock connection = new ConnectionMock("test"))
            {
                connection.SkipHandshake = true;
                connection.SkipSerializeCommand = true;
                connection.SkipDeserializeCommand = true;

                BuildExcerptsCommand target = new BuildExcerptsCommand(connection);
                target.Documents.Add("test doc content");
                target.Index = "test";

                // empty list
                try
                {
                    target.Execute();
                    Assert.Fail("ArgumentException exception must thrown for invalid argument value");
                }
                catch (ArgumentException)
                {
                    // test passed
                }
                // not empty
                target.Keywords.Add("test");
                target.Execute();
                Assert.AreEqual(1, target.Keywords.Count);
            }
        }

        /// <summary>
        /// A test for Index
        ///</summary>
        [TestMethod]
        public void IndexTest()
        {
            using (ConnectionBase connection = new ConnectionMock())
            {
                BuildExcerptsCommand target = new BuildExcerptsCommand(connection);
                // valid value
                string expected = "test";
                target.Index = expected;
                string actual = target.Index;
                Assert.AreEqual(expected, actual);
                // invalid value
                try
                {
                    target.Index = null;
                    Assert.Fail("ArgumentException exception must thrown for invalid argument value");
                }
                catch (ArgumentException)
                {
                    // test passed
                }
            }
        }

        /// <summary>
        ///A test for Documents
        ///</summary>
        [TestMethod]
        public void DocumentsTest()
        {
            using (ConnectionMock connection = new ConnectionMock("test"))
            {
                connection.SkipHandshake = true;
                connection.SkipSerializeCommand = true;
                connection.SkipDeserializeCommand = true;

                BuildExcerptsCommand target = new BuildExcerptsCommand(connection);
                target.Keywords.Add("keyword");
                target.Index = "test";

                // empty list
                try
                {
                    target.Execute();
                    Assert.Fail("ArgumentException exception must thrown for invalid argument value");
                }
                catch (ArgumentException)
                {
                    // test passed
                }
                // not empty
                target.Documents.Add("test doc");
                target.Execute();
                Assert.AreEqual(1, target.Documents.Count);
            }
        }

        /// <summary>
        ///A test for CommandInfo
        ///</summary>
        [TestMethod]
        [DeploymentItem("Sphinx.Client.dll")]
        public void CommandInfoTest()
        {
            using (ConnectionMock connection = new ConnectionMock("test"))
            {
                BuildExcerptsCommand target = new BuildExcerptsCommand(connection);
                BuildExcerptsCommand_Accessor accessor = GetCommandAccessor(target);
                Assert.IsNotNull(accessor.CommandInfo);
                Assert.AreEqual(accessor.CommandInfo.Id, (short)ServerCommand.Excerpt);
                Assert.AreEqual(accessor.CommandInfo.Version, BuildExcerptsCommand_Accessor.COMMAND_VERSION);
            }
        }

        /// <summary>
        ///A test for SerializeRequest
        ///</summary>
        [TestMethod]
        [DeploymentItem("Sphinx.Client.dll")]
        public void SerializeRequestTest()
        {
            using (ConnectionMock connection = new ConnectionMock("test"))
            {
                connection.SkipHandshake = true;
                connection.SkipDeserializeCommand = true;
                connection.Open();

                BuildExcerptsCommand target = new BuildExcerptsCommand(connection);
                target.Keywords.Add("test 1");
                target.Keywords.Add("test 2");
                target.Documents.Add("test doc content 1");
                target.Documents.Add("another test doc content 2");
                target.Index = "test";

                target.Execute();

                // read stream buffer using appropriate binary reader and test
                connection.Open();
                connection.BaseStream.Seek(0, SeekOrigin.Begin);
                BinaryFormatterFactoryMock factory = new BinaryFormatterFactoryMock();
                BinaryReaderBase reader = factory.CreateReader(connection.BaseStream);

                short commandId = reader.ReadInt16();
                Assert.AreEqual((short)ServerCommand.Excerpt, commandId);

                short version = reader.ReadInt16();
                Assert.AreEqual(BuildExcerptsCommand_Accessor.COMMAND_VERSION, version);

                int size = reader.ReadInt32();
                Assert.AreEqual(248, size);

                int mode = reader.ReadInt32();
                Assert.AreEqual(BuildExcerptsCommand_Accessor.MODE, mode);

                int options = reader.ReadInt32();
                Assert.AreEqual((int)target.Options, options);

                string index = reader.ReadString();
                Assert.AreEqual(target.Index, index);

                string keywords = reader.ReadString();
                string expectedKeywords = String.Join(",", target.Keywords.ToArray());
                Assert.AreEqual(expectedKeywords, keywords);

                string beforeMatch = reader.ReadString();
                Assert.AreEqual(target.BeforeMatch, beforeMatch);

                string afterMatch = reader.ReadString();
                Assert.AreEqual(target.AfterMatch, afterMatch);

                string snipDelim = reader.ReadString();
                Assert.AreEqual(target.SnippetsDelimiter, snipDelim);

                int sizeLimit = reader.ReadInt32();
                Assert.AreEqual(target.SnippetSizeLimit, sizeLimit);

                int wordsAroundKeyword = reader.ReadInt32();
                Assert.AreEqual(target.WordsAroundKeyword, wordsAroundKeyword);

                string documents = reader.ReadString();
                string expectedDocs = String.Join(",", target.Documents.ToArray());
                Assert.AreEqual(expectedDocs, documents);
                
            }
        }

        /// <summary>
        ///A test for Execute
        ///</summary>
        [TestMethod]
        public void ExecuteTest()
        {
            using (ConnectionMock connection = new ConnectionMock("test"))
            {
                connection.SkipHandshake = true;
                connection.SkipDeserializeCommand = true;
                connection.SkipSerializeCommand = true;

                BuildExcerptsCommand target = new BuildExcerptsCommand(connection);
                target.Documents.Add("test doc content");
                target.Index = "test";
                target.Keywords.Add("test");
                target.Execute();
            }
        }

        /// <summary>
        ///A test for DeserializeResponse
        ///</summary>
        [TestMethod]
        [DeploymentItem("Sphinx.Client.dll")]
        public void DeserializeResponseTest()
        {
            using (ConnectionMock connection = new ConnectionMock("test"))
            {
                connection.SkipHandshake = true;
                connection.SkipSerializeCommand = true;
                connection.Open();

                // preserialize fake server response to stream buffer using appropriate binary writer
                BinaryFormatterFactoryMock factory = new BinaryFormatterFactoryMock();
                BinaryWriterBase writer = factory.CreateWriter(connection.BaseStream);

                writer.Write((short)CommandStatus.Ok);
                writer.Write(BuildExcerptsCommand_Accessor.COMMAND_VERSION);

                writer.Write(21);
                string expectedExcerpt = "test";
                writer.Write(expectedExcerpt);
                connection.BaseStream.Seek(0, SeekOrigin.Begin);

                BuildExcerptsCommand target = new BuildExcerptsCommand(connection);
                target.Keywords.Add("test");
                target.Documents.Add("test");
                target.Index = "test";

                target.Execute();

                Assert.IsTrue(target.Result.Success);
                Assert.AreEqual(CommandStatus.Ok, target.Result.Status);
                Assert.AreEqual(1, target.Result.Excerpts.Count);
                Assert.AreEqual(expectedExcerpt, target.Result.Excerpts[0]);
            }
        }


	    #endregion
    
        #region Helper methods
        /// <summary>
        /// Returns <see cref="BuildExcerptsCommand_Accessor"/> object accessor for given <see cref="BuildExcerptsCommand"/> object.
        /// </summary>
        /// <param name="command"><see cref="BuildExcerptsCommand"/> object</param>
        /// <returns><see cref="BuildExcerptsCommand"/> object</returns>
        protected BuildExcerptsCommand_Accessor GetCommandAccessor(BuildExcerptsCommand command)
        {
            PrivateObject po = new PrivateObject(command);
            BuildExcerptsCommand_Accessor accessor = new BuildExcerptsCommand_Accessor(po);
            return accessor;
        }
        #endregion
    }
}
