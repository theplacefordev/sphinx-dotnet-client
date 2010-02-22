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
using System.Net.Sockets;
using Sphinx.Client.Helpers;

#endregion

namespace Sphinx.Client.Network
{
    public class TcpClientSocket : IClientSocket, IDisposable
    {
        #region Constructors
        public TcpClientSocket()
        {
            Socket = new TcpClient();
        }

        public TcpClientSocket( string host, int port ): this()
        {
            Host = host;
            Port = port;
        }
        
        #endregion

        #region Properties
        public bool Connected
        {
            get { return Socket.Connected; }
        }

        public int ConnectionTimeout
        {
            get { return Socket.ReceiveTimeout; }
            set { Socket.ReceiveTimeout = Socket.SendTimeout = value; }
        }

        public Stream DataStream
        {
            get { return Socket.GetStream(); }
        }

        protected TcpClient Socket { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        #endregion

        #region Methods
		public void Open()
        {
            ArgumentAssert.IsNotEmpty(Host, "Host");
            ArgumentAssert.IsInRange(Port, 1, UInt16.MaxValue, "Port");

            if (Socket.Connected) 
                Socket.Close();
            Socket.Connect(Host, Port);
        }

        public void Close()
        {
            // close underlying network datastream
            if (DataStream != null)
                DataStream.Close();
            Socket.Close();
        }

        /// <summary>
        /// Disposes this object and closes underlying socket.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                Close();
        }

	    #endregion    
    }
}
