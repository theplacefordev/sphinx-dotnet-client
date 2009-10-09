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
namespace Sphinx.Client.Commands.Search
{
    /// <summary>
    /// Specifies the search query status retured by <see cref="Search"/> command.
    /// </summary>
    public enum QueryStatus
    {
        Unknown = -1,
        Ok = 0,
        Error = 1,
        Warning = 3        
    }
}