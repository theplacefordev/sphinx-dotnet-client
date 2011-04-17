using System;
using Microsoft.Moles.Framework;
using Sphinx.Client.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sphinx.Client.Commands.Moles;
using Sphinx.Client.Connections.Moles;
using Sphinx.Client.Network;
using Sphinx.Client.Connections;
using Sphinx.Client.UnitTests.Mock.Connections;

namespace Sphinx.Client.UnitTests
{
	[TestClass]
	public class CommandBase_UnitTest
	{
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

		#region Constructor
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException), "ArgumentNullException must be thrown for null argument")]
		public void Constructor_NullConnection_ThrowsArgumentNullException()
		{
			new SCommandBase(null);
		}

		[TestMethod]
		public void Constructor_ValidConnection_DoesntThrowExceptions()
		{
			using (ConnectionBase connection = new ConnectionMock())
			{
				CreateCommandBase(connection);
			}
		}

		
		#endregion

		#region Properties
		[TestMethod]
		public void ConnectionTest()
		{
			using (ConnectionBase connection = new ConnectionMock())
			{
				SCommandBase target = CreateCommandBase(connection);
				Assert.AreEqual(target.Connection, connection);
			}
		}

		#endregion

		#region Methods
		[TestMethod]
		[HostType("Moles")]
		public void ExecuteTest()
		{
			bool performCommandCalled = false;
			CommandBase target = null;
			ConnectionBase connection = new MTcpConnection
            	{
					PerformCommandCommandBase = (command) => { performCommandCalled = (command == target); }
            	};
			target = CreateCommandBase(connection);

			target.Execute();

			Assert.IsTrue(performCommandCalled);
		}

		/// <summary>
		///A test for Serialize
		///</summary>
		[TestMethod]
		public void SerializeTest()
		{
			/*CommandBase target = new SCommandBase(); // TODO: Initialize to an appropriate value
			IStreamAdapter stream = null; // TODO: Initialize to an appropriate value
			target.Serialize(stream);
			Assert.Inconclusive("A method that does not return a value cannot be verified.");*/
		}

		/// <summary>
		///A test for Deserialize
		///</summary>
		[TestMethod]
		public void DeserializeTest()
		{
			/*CommandBase target = CreateCommandBase(); // TODO: Initialize to an appropriate value
			IStreamAdapter stream = null; // TODO: Initialize to an appropriate value
			target.Deserialize(stream);
			Assert.Inconclusive("A method that does not return a value cannot be verified.");*/
		}
		#endregion

		#region Helper methods
		protected SCommandBase CreateCommandBase(ConnectionBase connection)
		{
			SCommandBase target = new SCommandBase(connection);
			target.CallBase = true;
			return target;
		}

		protected CommandBase_Accessor GetCommandAccessor(CommandBase command)
		{
			PrivateObject po = new PrivateObject(command);
			CommandBase_Accessor accessor = new CommandBase_Accessor(po);
			return accessor;
		}

		#endregion
	}
}
