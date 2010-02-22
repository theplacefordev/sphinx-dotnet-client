#region Copyright
// 
// Copyright (c) 2009, Rustam Babadjanov <theplacefordev [at] gmail [dot] com>
// 
// This program is free software; you can redistribute it and/or modify it
// under the terms of the GNU Lesser General Public License version 2.1 as published
// by the Free Software Foundation.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
#endregion
#region Usings

using System;
using System.IO;
using System.Threading;
using Sphinx.Client.Common;
using Sphinx.Client.Connections;
using Sphinx.Client.IO;
using Sphinx.Client.Resources;

#endregion

namespace Sphinx.Client.Commands
{
    /// <summary>
    /// Represents an Sphinx searchd command to execute on server with strongly typed results object, based on <see cref="CommandResultBase"/> class. Provides a base class for specific classes that represent real Sphinx commands, returning command execution results data.
    /// An abstract class, it cannot be instantiated.
    /// </summary>
    public abstract class CommandWithResultBase<TResult> : CommandBase, ICommandResult<TResult>
                    where TResult : CommandResultBase, new()
    {
        #region Fields
        private TResult _result;
		private ManualResetEvent _resetEvent;

        #endregion

        #region Constructors
        protected CommandWithResultBase(ConnectionBase connection) : base(connection)
        {
        }
        
        #endregion

        #region Properties
        #region Implemented
        /// <summary>
        /// Command execution result object. Holds all information returned by server, including command execution state and Sphinx server warnings.
        /// </summary>
        public virtual TResult Result
        {
            get { return _result; }
            protected set { _result = value; }
        }
        #endregion

        #endregion

        #region Methods
        #region Implemented
        /// <summary>
        /// Executes Sphinx command against a connection object.
        /// </summary>
        /// <returns><see cref="CommandResultBase<TResult>"/> based result object.</returns>
        /// <exception cref="ServerErrorException"/>
        /// <exception cref="SphinxException"/>
        /// <exception cref="IOException"/>
        public override void Execute()
        {
            Result = new TResult();
            Connection.PerformCommand(this);
        }

        /// <summary>
        /// Serializes an command and graph of internal objects to the provided stream.
        /// </summary>
        /// <param name="stream">The stream where the command puts the serialized data.</param>
        /// <exception cref="IOException"/>
        internal protected override void Serialize(Stream stream)
        {
            BinaryWriterBase writer = Connection.FormatterFactory.CreateWriter(stream);
            // send command id and version information
            CommandInfo.Serialize(writer);

            // serialize request body to temp. buffer to get command body length
            MemoryStream buffer = new MemoryStream();
            BinaryWriterBase bufferWriter = Connection.FormatterFactory.CreateWriter(buffer);
            SerializeRequest(bufferWriter);
            // send body length first
            writer.Write((int)buffer.Length);
            // send buffer contents
            buffer.WriteTo(stream);
        }

        /// <summary>
        /// Deserializes the Sphinx server response data on the provided stream and reconstitutes the graph of objects.
        /// </summary>
        /// <param name="stream">The stream that contains the data to deserialize.</param>
        /// <exception cref="ServerErrorException"/>
        /// <exception cref="SphinxException"/>
        /// <exception cref="IOException"/>
        internal protected override void Deserialize(Stream stream)
        {
            BinaryReaderBase reader = Connection.FormatterFactory.CreateReader(stream);
            // read general command response header values
            Result.Status = (CommandStatus)reader.ReadInt16();
            short serverCommandVersion = reader.ReadInt16();

            // read response body length
            int length = reader.ReadInt32();
            if (length <= 0)
                throw new SphinxException(String.Format(Messages.Exception_InvalidServerResponseLength, length));

            // read server response body
        	MemoryStream bodyStream = ReadResponseBody(stream, length);
            BinaryReaderBase bodyReader = Connection.FormatterFactory.CreateReader(bodyStream);

            // check response status
            switch (Result.Status)
            {
                case CommandStatus.Ok:
                    break;
                case CommandStatus.Warning:
                    // cut warning message from response stream
                    _result.Warnings.Add(bodyReader.ReadString());
                    break;
                case CommandStatus.Error:
                    string errorMessage = bodyReader.ReadString();
                    throw new ServerErrorException(String.Format(Messages.Exception_ServerError, errorMessage));
                case CommandStatus.Retry:
                    string tempErrorMessage = bodyReader.ReadString();
                    throw new ServerErrorException(String.Format(Messages.Exception_TemproraryServerError, tempErrorMessage));
                default:
                    throw new SphinxException(String.Format(Messages.Exception_UnknowStatusCode, (int)Result.Status));
            }

            // check server command version
            short clientCommandVersion = CommandInfo.Version;
            if (serverCommandVersion < clientCommandVersion)
            {
                // NOTE: little hack - changing server response status and adding own warning message, because old Sphinx server version is detected and some protocol features might not work as expected
                if (Result.Status == CommandStatus.Ok)
                    Result.Status = CommandStatus.Warning;
                Result.Warnings.Add(String.Format(Messages.Warning_CommandVersion, serverCommandVersion >> 8, serverCommandVersion & 0xff, clientCommandVersion >> 8, clientCommandVersion & 0xff));
            }

            // parse command result 
            DeserializeResponse(bodyReader);
        }

		/// <summary>
		/// Read requested bytes server from response stream. The implementation will block until all requested bytes readed from source stream, or <see cref="TimeoutException"/> will be thrown.
		/// </summary>
		/// <param name="source">Source network stream.</param>
		/// <param name="length">Number of bytes to be read from the source stream.</param>
		/// <returns>Stream object, that contains server response data bytes.</returns>
		/// <exception cref="TimeoutException">Thrown when the time allotted for data read operation has expired.</exception>
		protected virtual MemoryStream ReadResponseBody(Stream source, int length)
		{
			byte[] buffer = new byte[length];
			NetworkReadState state = new NetworkReadState { DataStream = source, BytesLeft = length };
			_resetEvent =  new ManualResetEvent(false);
			while (state.BytesLeft > 0)
			{
				_resetEvent.Reset();
				source.BeginRead(buffer, length - state.BytesLeft, state.BytesLeft, ReadBodyCallback, state);
				if (!_resetEvent.WaitOne(Connection.ConnectionTimeout, true))
				{
					source.Close();
					throw new TimeoutException(Messages.Exception_NetworkConnectionIsUnavailable);
				}
			}

			return new MemoryStream(buffer);
		}

		private void ReadBodyCallback(IAsyncResult asyncResult)
		{
			NetworkReadState state = ((NetworkReadState)asyncResult.AsyncState);
			if (!state.DataStream.CanRead)
				throw new EndOfStreamException(String.Format(Messages.Exception_CouldNotReadFromStream, state.BytesLeft, 0));
			int actualBytes = state.DataStream.EndRead(asyncResult);
			if (actualBytes == 0)
				throw new EndOfStreamException(String.Format(Messages.Exception_CouldNotReadFromStream, state.BytesLeft, actualBytes));
			state.BytesLeft -= actualBytes;
			_resetEvent.Set();
		}

		private class NetworkReadState
		{
			public Stream DataStream;
			public int BytesLeft;
		}

        #endregion

        #region Abstract
        /// <summary>
        /// Serialize command request body to stream. 
        /// An abstract method, must be implemented in derived class.
        /// </summary>
        /// <param name="writer">Data stream object</param>
        protected abstract void SerializeRequest(BinaryWriterBase writer);

        /// <summary>
        /// Deserialize server response body.
        /// An abstract method, must be implemented in derived class.
        /// </summary>
        /// <param name="reader">Data stream object</param>
        protected abstract void DeserializeResponse(BinaryReaderBase reader);
        
        #endregion

        #endregion
    }
}
