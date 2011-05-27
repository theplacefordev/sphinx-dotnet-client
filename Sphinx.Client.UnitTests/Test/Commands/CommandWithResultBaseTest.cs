using System.Collections;
using System.IO;
using Sphinx.Client.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sphinx.Client.Commands.Moles;
using Sphinx.Client.Connections;
using Sphinx.Client.Connections.Moles;
using Sphinx.Client.Network;
using Sphinx.Client.IO;
using Sphinx.Client.Network.Moles;
using Sphinx.Client.UnitTests.Mock.Commands;
using Sphinx.Client.UnitTests.Mock.Connections;
using Sphinx.Client.UnitTests.Mock.IO;
using Sphinx.Client.UnitTests.Mock.Network;

namespace Sphinx.Client.UnitTests.Test.Commands
{
    
	[TestClass]
	public class CommandWithResultBase_UnitTest
	{
    	public TestContext TestContext { get; set; }

    	#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion

		[TestMethod]
		public void ConstructorTest()
		{
			using (ConnectionBase connection = new ConnectionMock())
			{
				CreateCommandWithResultBase(connection);
			}
		}

		[TestMethod]
		public void ResultTest()
		{
			using (ConnectionBase connection = new ConnectionMock())
			{
				var target = CreateCommandWithResultBase(connection);
				var accessor = GetCommandAccessor(target);
				var expected = new CommandResultBaseMock();
				accessor.Result = expected;
				Assert.AreEqual(expected, target.Result);
			}
		}

		[TestMethod]
		[HostType("Moles")]
		public void ExecuteTest()
		{
			bool performCommandIsCalled = false;
			MTcpConnection connection = new MTcpConnection
        	{
        		PerformCommandCommandBase = (command) => { performCommandIsCalled = (command != null); }
        	};
			var target = CreateCommandWithResultBase(connection);
				
			target.Execute();

			Assert.IsNotNull(target.Result);
			Assert.IsTrue(performCommandIsCalled);
		}

		[TestMethod]
		[HostType("Moles")]
		public void SerializeTest()
		{
			using (ConnectionMock connection = new ConnectionMock())
			{
				ArrayList list = new ArrayList();
				connection.SetFormatterFactory(new ArrayListFormatterFactoryMock(list));

				string commandInfoCalled = "commandInfo Serialize is called";
				MCommandInfo commandInfo = new MCommandInfo
				{
					SerializeIBinaryWriter = (writer) => list.Add(commandInfoCalled)
				};
				
				var target = CreateCommandWithResultBase(connection);
				target.CommandInfoGet = () => commandInfo;
				string abstractSerializeRequestCalled = "SerializeRequest is called";
				target.SerializeRequestIBinaryWriter = (writer) => list.Add(abstractSerializeRequestCalled);
				var accessor = GetCommandAccessor(target);
				string writeBytesCalled = "WriteBytes is called";
				var streamAdapter = new SStreamAdapter(new MemoryStream())
				{
					WriteBytesByteArrayInt32 = (array, len) => list.Add(writeBytesCalled)
				};

				accessor.Serialize(streamAdapter);

				Assert.AreEqual(list[0], 0);
				Assert.AreEqual(list[1], commandInfoCalled);
				Assert.AreEqual(list[2], abstractSerializeRequestCalled);
				Assert.AreEqual(list[3], writeBytesCalled);

			}			
		}

		#region Helper methods
		private CommandWithResultBase_Accessor<TResult> GetCommandAccessor<TResult>(CommandWithResultBase<TResult> command)
			where TResult : CommandResultBase, new()
		{
			PrivateObject po = new PrivateObject(command);
			CommandWithResultBase_Accessor<TResult> accessor = new CommandWithResultBase_Accessor<TResult>(po);
			return accessor;
		}

		private SCommandWithResultBase<CommandResultBaseMock> CreateCommandWithResultBase(ConnectionBase connection)
		{
			var command = new SCommandWithResultBase<CommandResultBaseMock>(connection);
			command.CallBase = true;
			return command;
		}
		#endregion
	}
}
