using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Sphinx.Client.Helpers;
using Sphinx.Client.Resources;

namespace Sphinx.Client.Network
{
	public class TcpStreamAdapter : StreamAdapter
	{
		#region Constructor
		public TcpStreamAdapter(Stream stream): base(stream)
		{
		}
		
		#endregion

		#region Properties
		private ManualResetEvent ResetEvent { get; set; }
		
		#endregion

		#region Methods
		/// <summary>
		/// Read requested bytes from response stream. The implementation will block until all requested bytes readed from source stream, or <see cref="TimeoutException"/> will be thrown.
		/// </summary>
		/// <param name="buffer">An array of type Byte that is the location in memory to store data read from the NetworkStream.</param>
		/// <param name="length">Number of bytes to be read from the source stream.</param>
		/// <exception cref="TimeoutException">Thrown when the time allotted for data read operation has expired.</exception>
		public override int ReadBytes(byte[] buffer, int length)
		{
			ArgumentAssert.IsNotNull(buffer, "buffer");
			ArgumentAssert.IsGreaterThan(length, 0, "length");

			NetworkReadState state = new NetworkReadState { DataStream = Stream, BytesLeft = length };
			ResetEvent = new ManualResetEvent(false);
			while (state.BytesLeft > 0)
			{
				ResetEvent.Reset();
				Stream.BeginRead(buffer, length - state.BytesLeft, state.BytesLeft, ReadDataCallback, state);
				if (!ResetEvent.WaitOne(OperationTimeout, true))
				{
					throw new TimeoutException(Messages.Exception_ReadTimeoutExpired);
				}
			}
			return length;
		}

		private void ReadDataCallback(IAsyncResult asyncResult)
		{
			NetworkReadState state = ((NetworkReadState)asyncResult.AsyncState);
			if (!state.DataStream.CanRead)
				throw new IOException(String.Format(Messages.Exception_CouldNotReadFromStream, state.BytesLeft, 0));
			int actualBytes = state.DataStream.EndRead(asyncResult);
			if (actualBytes == 0)
				throw new IOException(String.Format(Messages.Exception_CouldNotReadFromStream, state.BytesLeft, actualBytes));
			state.BytesLeft -= actualBytes;
			ResetEvent.Set();
		}

		private class NetworkReadState
		{
			public Stream DataStream;
			public int BytesLeft;
		}
 
		#endregion	
	}
}
