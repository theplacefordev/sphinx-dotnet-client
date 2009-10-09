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
using Sphinx.Client.IO;

#endregion

namespace Sphinx.Client.Commands.Query
{
    /// <summary>
    /// Represents <see cref="QueryCommand"/> search command execution results.
    /// </summary>
    public class QueryCommandResult : CommandResultBase
    {
        #region Fields
        private SearchQueryResult _queryResult;

        #endregion

        #region Properties
        /// <summary>
        /// Search query result.
        /// </summary>
        public SearchQueryResult QueryResult
        {
            get
            {
                return _queryResult;
            }
        }

        #endregion

        #region Methods
        internal void Deserialize(BinaryReaderBase reader)
        {
            _queryResult = new SearchQueryResult();
            _queryResult.Deserialize(reader);
        }

        #endregion

    }
}
