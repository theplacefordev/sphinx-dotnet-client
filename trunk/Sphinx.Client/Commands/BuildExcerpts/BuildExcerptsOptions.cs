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
        OrderByWeight = 16,

		/// <summary>
		/// Added in Sphinx 1.10-beta. Whether to handle keywords in list as a query in <a href="http://sphinxsearch.com/docs/current.html#extended-syntax">extended syntax</a>, or as a bag of words (default behavior). For instance, in query mode ("one two" | "three four") will only highlight and include those occurrences "one two" or "three four" when the two words from each pair are adjacent to each other. In default mode, any single occurrence of "one", "two", "three", or "four" would be highlighted.
		/// </summary>
		QueryMode = 32,

		/// <summary>
		/// Added in version 1.10-beta. Ignores the snippet length limit until it includes all the keywords.
		/// </summary>
		ForceAllWords = 64,

		/// <summary>
		/// Added in version 1.10-beta. Whether to handle documents in list as data to extract snippets from (default behavior), or to treat it as file names, and load data from specified files on the server side. 
		/// </summary>
		LoadFiles = 128,

		/// <summary>
		/// Added in version 1.10-beta. Allows empty string to be returned as highlighting result when a snippet could not be generated (no keywords match, or no passages fit the limit). By default, the beginning of original text would be returned instead of an empty string.
		/// </summary>
		AllowEmpty = 256
    }
}
