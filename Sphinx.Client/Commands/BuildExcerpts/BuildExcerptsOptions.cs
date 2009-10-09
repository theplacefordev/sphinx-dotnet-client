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

#endregion

namespace Sphinx.Client.Commands.BuildExcerpts
{
    [Flags]
    public enum BuildExcerptsOptions : int
    {
        None = 0,

        /// <summary>
        /// Remove unneeded spaces
        /// </summary>
        RemoveSpaces = 1,

        /// <summary>
        /// Whether to highlight exact query phrase matches only instead of individual keywords
        /// </summary>
        HighlightExactPhrase = 2,

        /// <summary>
        /// Whether to extract single best passage only
        /// </summary>
        ExtractOnlyBestPassage = 4,

        /// <summary>
        /// Extract passages by phrase boundaries setup in tokenizer
        /// </summary>
        UseBoundaries = 8,

        /// <summary>
        /// If flag is set, sort the extracted passages in order of relevance (decreasing weight), or in order of appearance in the document (increasing position) otherwise
        /// </summary>
        OrderByWeight = 16

    }
}
