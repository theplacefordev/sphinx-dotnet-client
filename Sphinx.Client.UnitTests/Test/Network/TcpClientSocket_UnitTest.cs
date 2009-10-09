using Sphinx.Client.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Sphinx.Client.UnitTests.Test.Network
{
    
    
    /// <summary>
    ///This is a test class for TcpClientSocketUnitTest and is intended
    ///to contain all TcpClientSocketUnitTest Unit Tests
    ///</summary>
    [TestClass]
    public class TcpClientSocketUnitTest
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

        /// <summary>
        ///A test for TcpClientSocket Constructor
        ///</summary>
        [TestMethod]
        public void TcpClientSocketConstructorTestEmpty()
        {
            using (new TcpClientSocket())
            {
            }
        }

        /// <summary>
        ///A test for TcpClientSocket Constructor
        ///</summary>
        [TestMethod]
        public void TcpClientSocketConstructorTest()
        {
            string host = string.Empty; 
            int port = 0; 
            new TcpClientSocket(host, port);
        }

        /// <summary>
        ///A test for Port
        ///</summary>
        [TestMethod]
        public void PortTest()
        {
            TcpClientSocket target = new TcpClientSocket(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Port = expected;
            actual = target.Port;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Host
        ///</summary>
        [TestMethod]
        public void HostTest()
        {
            TcpClientSocket target = new TcpClientSocket(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Host = expected;
            actual = target.Host;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DataStream
        ///</summary>
        [TestMethod]
        public void DataStreamTest()
        {
            TcpClientSocket target = new TcpClientSocket(); // TODO: Initialize to an appropriate value
            Stream actual;
            actual = target.DataStream;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ConnectionTimeout
        ///</summary>
        [TestMethod]
        public void ConnectionTimeoutTest()
        {
            TcpClientSocket target = new TcpClientSocket(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.ConnectionTimeout = expected;
            actual = target.ConnectionTimeout;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Connected
        ///</summary>
        [TestMethod]
        public void ConnectedTest()
        {
            TcpClientSocket target = new TcpClientSocket(); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Connected;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Open
        ///</summary>
        [TestMethod]
        public void OpenTest()
        {
            TcpClientSocket target = new TcpClientSocket(); // TODO: Initialize to an appropriate value
            target.Open();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod]
        [DeploymentItem("Sphinx.Client.dll")]
        public void DisposeTest1()
        {
            TcpClientSocket_Accessor target = new TcpClientSocket_Accessor(); // TODO: Initialize to an appropriate value
            bool disposing = false; // TODO: Initialize to an appropriate value
            target.Dispose(disposing);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod]
        public void DisposeTest()
        {
            TcpClientSocket target = new TcpClientSocket(); // TODO: Initialize to an appropriate value
            target.Dispose();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Close
        ///</summary>
        [TestMethod]
        public void CloseTest()
        {
            TcpClientSocket target = new TcpClientSocket(); // TODO: Initialize to an appropriate value
            target.Close();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

    }
}
