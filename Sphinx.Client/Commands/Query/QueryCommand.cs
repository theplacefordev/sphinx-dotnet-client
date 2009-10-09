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

using Sphinx.Client.Commands.Search;
using Sphinx.Client.Connections;
using Sphinx.Client.Helpers;
using Sphinx.Client.IO;

#endregion

namespace Sphinx.Client.Commands.Query
{
    /// <summary>
    /// Represents command to execute SphinxQL query (SQL like syntax).
    /// </summary>
    public class QueryCommand : CommandWithResultBase<QueryCommandResult>
    {
        #region Constants
        private const short COMMAND_VERSION = 0x100;
        private const short RESPONSE_VERSION = SearchCommand.COMMAND_VERSION;

        #endregion

        #region Fields
        private static readonly CommandInfo _commandInfo = new CommandInfo(ServerCommand.Query, COMMAND_VERSION);

        private string _query;

        #endregion

        #region Constructors
        public QueryCommand(ConnectionBase connection): base(connection)
        {
        }

        public QueryCommand(ConnectionBase connection, string query): base(connection)
        {
            Query = query;
        }

        #endregion

        #region Properties
        #region Overrides of CommandWithResultBase
        protected override CommandInfo CommandInfo
        {
            get { return _commandInfo; }
        }

        #endregion

        /// <summary>
        /// SQL like search query.
        /// </summary>
        public string Query
        {
            get { return _query; }
            set
            {
                ArgumentAssert.IsNotEmpty(value, "Query");
                _query = value;
            }
        }
        #endregion

        #region Methods
        #region Overrides of CommandWithResultBase
        public override void Execute()
        {
            ArgumentAssert.IsNotEmpty(Query, "Query");

            base.Execute();
        }

        protected override void SerializeRequest(BinaryWriterBase writer)
        {
            writer.Write(RESPONSE_VERSION);
            writer.Write(Query);
        }

        protected override void DeserializeResponse(BinaryReaderBase reader)
        {
            Result.Deserialize(reader);
        }

        #endregion

        #endregion

    }
}
