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
using System.Collections.Generic;
using Sphinx.Client.Commands.Collections;
using Sphinx.Client.Connections;
using Sphinx.Client.Helpers;
using Sphinx.Client.IO;

#endregion

namespace Sphinx.Client.Commands.BuildExcerpts
{
    /// <summary>
    /// Represents document content excerpts (snippets) builder command. 
    /// Requests Sphinx server to generate excerpts (snippets) from given text content and returns the results with highlighted text. 
    /// </summary>
    public class BuildExcerptsCommand : CommandWithResultBase<BuildExcerptsCommandResult>
    {
        #region Constants
        internal const short COMMAND_VERSION = 0x102;
        private const int MODE = 0; // reserved for future, ignored by server

        private const string DEFAULT_BEFORE_MATCH = "<strong>";
        private const string DEFAULT_AFTER_MATCH = "</strong>";
        private const string DEFAULT_SNIPPETS_DELIMITER = " ... ";
        private const int DEFAULT_SNIPPET_SIZE_LIMIT = 256;
		private const int DEFAULT_SNIPPETS_COUNT_LIMIT = 0;
		private const int DEFAULT_WORDS_AROUND_KEYWORD = 5;
		private const int DEFAULT_WORDS_COUNT_LIMIT = 0;
    	private const int DEFAULT_START_PASSAGE_ID = 1;
    	private const HtmlStripMode DEFAULT_HTML_STRIP_MODE = HtmlStripMode.Index;
        
        #endregion

        #region Fields
        private static readonly CommandInfo _commandInfo = new CommandInfo(ServerCommand.Excerpt, COMMAND_VERSION);

        // options
        private string _beforeMatch = DEFAULT_BEFORE_MATCH;
        private string _afterMatch = DEFAULT_AFTER_MATCH;
        private string _snippetsDelimiter = DEFAULT_SNIPPETS_DELIMITER;
        private int _snippetSizeLimit = DEFAULT_SNIPPET_SIZE_LIMIT;
    	private int _snippetsCountLimit = DEFAULT_SNIPPETS_COUNT_LIMIT;
        private int _wordsAroundKeyword = DEFAULT_WORDS_AROUND_KEYWORD;
    	private int _wordsCountLimit = DEFAULT_WORDS_COUNT_LIMIT;
    	private int _startPassageId = DEFAULT_START_PASSAGE_ID;
    	private HtmlStripMode _htmlStripMode = DEFAULT_HTML_STRIP_MODE;
        private BuildExcerptsOptions _options = BuildExcerptsOptions.RemoveSpaces;

        // params
		private readonly StringList _documents = new DocumentList();
        private readonly StringList _keywords = new StringList();
        private string _indexName;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildExcerptsCommand"/> class.
        /// </summary>
        /// <param name="connection">Connection to Sphinx are used to execute command.</param>
        public BuildExcerptsCommand(ConnectionBase connection): base(connection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildExcerptsCommand"/> class that initializes main command parameters with specified arguments values.
        /// </summary>
        /// <param name="connection">Connection to Sphinx are used to execute command.</param>
        /// <param name="documents">Documents text content list are copied to <see cref=".Documents"/> property.</param>
        /// <param name="keywords">Keywords list are copied to <see cref=".Keywords"/> property.</param>
        /// <param name="index">Index name are copied to <see cref=".Index"/> property.</param>
        public BuildExcerptsCommand(ConnectionBase connection, IEnumerable<string> documents, IEnumerable<string> keywords, string index): base(connection)
        {
            Documents.AddRange(documents);
            Keywords.AddRange(keywords);
            Index = index;
        }
        #endregion

        #region Properties

        #region Command parameters
        /// <summary>
        /// List of strings with documents content. List can't be empty.
        /// </summary>
        public StringList Documents
        {
            get { return _documents; }
        }

        /// <summary>
        /// Index name. Will use given index settings (such as charset, morphology, wordforms) to build excerpts. Can't be null.
        /// </summary>
        public string Index
        {
            get { return _indexName; }
            set
            {
                ArgumentAssert.IsNotEmpty(value, "Index");
                _indexName = value;
            }
        }

        /// <summary>
        /// List must contain keywords to highlight. Can contain wildcards. List can't be empty.
        /// </summary>
        public StringList Keywords
        {
            get { return _keywords; }
        }
        
        #endregion

        #region Command settings
        /// <summary>
		/// Template part used to highlight phrase before match. Starting with version 1.10-beta, a %PASSAGE_ID% macro can be used in this string. The macro is replaced with an incrementing passage number within a current snippet. Numbering starts at 1 by default but can be overridden with "start_passage_id" option. In a multi-document call, %PASSAGE_ID% would restart at every given document.
		/// Default value is "<strong>". Can't be null.
        /// </summary>
        public string BeforeMatch
        {
            get { return _beforeMatch; }
            set
            {
                ArgumentAssert.IsNotNull(value, "BeforeMatch");
                _beforeMatch = value;
            }
        }

        /// <summary>
		/// Template part used to highlight phrase after match. Starting with version 1.10-beta, a %PASSAGE_ID% macro can be used in this string.
        /// Default value is "</strong>". Can't be null.
        /// </summary>
        public string AfterMatch
        {
            get { return _afterMatch; }
            set
            {
                ArgumentAssert.IsNotNull(value, "AfterMatch");
                _afterMatch = value;
            }
        }

        /// <summary>
        /// Delimiter string to insert between snippet chunks (passages). Default value is ellipsis. Can't be null.
        /// </summary>
        public string SnippetsDelimiter
        {
            get { return _snippetsDelimiter; }
            set
            {
                ArgumentAssert.IsNotNull(value, "SnippetsDelimiter");
                _snippetsDelimiter = value;
            }
        }

        /// <summary>
        /// Maximum snippet size, in symbols (codepoints). Default value is 256. Can't be less than 1.
        /// </summary>
        public int SnippetSizeLimit
        {
            get { return _snippetSizeLimit; }
            set
            {
                ArgumentAssert.IsGreaterThan(value, 0, "SnippetSizeLimit");
                _snippetSizeLimit = value;
            }
        }

		/// <summary>
		/// Added in version 1.10-beta. Limits the maximum number of passages that can be included into the snippet. Default is 0 (no limit).
		/// </summary>
    	public int SnippetsCountLimit
    	{
			get { return _snippetsCountLimit; }
			set
			{
				ArgumentAssert.IsGreaterOrEqual(value, 0, "SnippetsCountLimit");
				_snippetsCountLimit = value;
			}
    	}

        /// <summary>
        /// How much words to pick around each matching keywords block. Default values is 5. Can't be less than zero.
        /// </summary>
        public int WordsAroundKeyword
        {
            get { return _wordsAroundKeyword; }
            set
            {
				ArgumentAssert.IsGreaterOrEqual(value, 0, "WordsAroundKeyword");
                _wordsAroundKeyword = value;
            }
        }

		/// <summary>
		/// Added in version 1.10-beta. Limits the maximum number of keywords that can be included into the snippet. Default is 0 (no limit).
		/// </summary>
		public int WordsCountLimit
		{
			get { return _wordsCountLimit; }
			set
			{
				ArgumentAssert.IsGreaterOrEqual(value, 0, "WordsCountLimit");
				_wordsCountLimit = value;
			}
		}

		/// <summary>
		/// Added in version 1.10-beta. Specifies the starting value of %PASSAGE_ID% macro (that gets detected and expanded in <see cref="BeforeMatch"/>, <see cref="AfterMatch"/> strings).
		/// Default is 1.
		/// </summary>
		public int StartPassageId
		{
			get { return _startPassageId; }
			set { _startPassageId = value; }
		}

		/// <summary>
		/// Added in version 1.10-beta. HTML stripping mode setting. Defaults to "index", which means that index settings will be used. The other values are "none" and "strip", that forcibly skip or apply stripping irregardless of index settings; and "retain", that retains HTML markup and protects it from highlighting. The "retain" mode can only be used when highlighting full documents and thus requires that no snippet size limits are set.
		/// </summary>
		public HtmlStripMode HtmlStripMode
		{
			get { return _htmlStripMode; }
			set { _htmlStripMode = value; }
		}

        /// <summary>
        /// Build excerpts flag options. Flag <see cref="BuildExcerptsOptions.RemoveSpaces"/> is set by default.
        /// </summary>
        public BuildExcerptsOptions Options
        {
            get { return _options; }
            set { _options = value; }
        }
        
        #endregion

        #region Overrides of CommandWithResultBase
        protected override CommandInfo CommandInfo
        {
            get { return _commandInfo; }
        }

    	#endregion

        #endregion

        #region Methods

        #region Overrides of CommandWithResultBase
        /// <summary>
        /// Execute command against specified <see cref=".Connection"/>
        /// </summary>
        public override void Execute()
        {
			ArgumentAssert.IsGreaterThan(Documents.Count, 0, "Documents.Count");
            ArgumentAssert.IsGreaterThan(Keywords.Count, 0, "Keywords.Count");
            ArgumentAssert.IsNotEmpty(Index, "Index");

            base.Execute();
        }

        /// <summary>
        /// Serialize command parameters using specified binary stream writer.
        /// </summary>
        /// <param name="writer">Binary stream writer object</param>
        protected override void SerializeRequest(BinaryWriterBase writer)
        {
            // build mode
            writer.Write(MODE);

            // parameters
            writer.Write((int)Options);
            writer.Write(Index);
            Keywords.Serialize(writer);

            // options
            writer.Write(BeforeMatch);
            writer.Write(AfterMatch);
            writer.Write(SnippetsDelimiter);
            writer.Write(SnippetSizeLimit);
            writer.Write(WordsAroundKeyword);
			// 1.10-beta
			writer.Write(SnippetsCountLimit);
        	writer.Write(WordsCountLimit);
			writer.Write(StartPassageId);
			writer.Write(Enum.GetName(typeof(HtmlStripMode), HtmlStripMode).ToLowerInvariant());

            // serialize documents list
			Documents.Serialize(writer);
        }

        /// <summary>
        /// Deserialize server response body using specified binary stream reader.
        /// </summary>
        /// <param name="reader">Binary stream reader object</param>
        protected override void DeserializeResponse(BinaryReaderBase reader)
        {
            Result.Deserialize(reader, Documents.Count);
        }

        #endregion

        #endregion
    }
}
