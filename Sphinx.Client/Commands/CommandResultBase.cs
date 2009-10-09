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

using System.Collections.Generic;

#endregion

namespace Sphinx.Client.Commands
{
    public abstract class CommandResultBase
    {
        #region Fields
        // reported warnings list
        private readonly List<string> _warnings = new List<string>();

        // command execution status
        private CommandStatus _status = CommandStatus.Unknown;
        
        #endregion

        #region Properties
        /// <summary>
        /// True if command execution was successfull
        /// </summary>
        public virtual bool Success
        {
            get { return _status == CommandStatus.Ok || _status == CommandStatus.Warning; }
        }

        /// <summary>
        /// Command execution status
        /// </summary>
        public virtual CommandStatus Status
        {
            get { return _status; }
            internal protected set { _status = value; }
        }

        /// <summary>
        /// Warning messages, if any
        /// </summary>
        public virtual IList<string> Warnings
        {
            get { return _warnings; }
        }
 
        #endregion

    }
}
