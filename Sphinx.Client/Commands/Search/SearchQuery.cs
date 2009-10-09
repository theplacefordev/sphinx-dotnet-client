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
using Sphinx.Client.Helpers;
using Sphinx.Client.IO;
using Sphinx.Client.Resources;

#endregion

namespace Sphinx.Client.Commands.Search
{
    public class SearchQuery
    {
        #region Contants
        private const int ID64_MARKER = 1;// id64 range marker

        private const int DEFAULT_OFFSET = 0;
        private const int DEFAULT_LIMIT = 20;
        private const int DEFAULT_MAX_MATCHES = 1000;
        private const int DEFAULT_CUTOFF = 0; // 0 = disabled
        private const long DEFAULT_MIN_ID = 0; // 0 = disabled
        private const long DEFAULT_MAX_ID = 0; // 0 = disabled
        private const MatchMode DEFAULT_MATCH_TYPE = MatchMode.All;
        private const MatchRankMode DEFAULT_RANK_MODE = MatchRankMode.ProximityBM25;
        private const ResultsSortMode DEFAULT_RESULTS_SORT_MODE = ResultsSortMode.Relevance;
        private const string DEFAULT_SORT_BY = "";
        private const ResultsGroupFunction DEFAULT_GROUP_FUNC = ResultsGroupFunction.Day;
        private const string DEFAULT_GROUP_BY = "";
        private const string DEFAULT_GROUP_SORT = "@group desc";
        private const string DEFAULT_GROUP_DISTINCT = "";
        private const int DEFAULT_MAX_QUERY_TIME_MS = 0;
        private const string DEFAULT_SELECT = "*";
        private const int DEFAULT_RETRY_COUNT = 0; // 0 = disabled
        private const int DEFAULT_RETRY_DELAY_MS = 0;

        private const int LIMIT_COMMENT_LENGTH = 128;
        #endregion

        #region Fields

        // query string
        private string _query;

        // search query result set offset
        private int _offset = DEFAULT_OFFSET;
        // limit results per query
        private int _limit = DEFAULT_LIMIT;
        // how much matches searchd will keep in RAM while searching
        private int _maxMatches = DEFAULT_MAX_MATCHES;
        // max found matches threshold
        private int _cutoff = DEFAULT_CUTOFF;

        // min. document ID to match
        private long _minId = DEFAULT_MIN_ID; 
        // max. document ID to match
        private long _maxId = DEFAULT_MAX_ID; 
        
        // query match type
        private MatchMode _matchMode = DEFAULT_MATCH_TYPE;
        // query results raking mode
        private MatchRankMode _rankMode = DEFAULT_RANK_MODE;
        // results sort mode
        private ResultsSortMode _sortMode = DEFAULT_RESULTS_SORT_MODE;
        // sort column
        private string _sortBy = DEFAULT_SORT_BY;
        // group function
        private ResultsGroupFunction _groupFunc = DEFAULT_GROUP_FUNC;
        // group by column name
        private string _groupBy = DEFAULT_GROUP_BY;
        // group-by sorting clause 
        private string _groupSort = DEFAULT_GROUP_SORT;
        // attribute name for per-group distinct values count calculations
        private string _groupDistinct = DEFAULT_GROUP_DISTINCT;
        
        // max. query time
        private int _maxQueryTime = DEFAULT_MAX_QUERY_TIME_MS;

        // select clause, listing specific attributes to fetch
        private string _select = DEFAULT_SELECT;

        // distributed retries count
        private int _retryCount = DEFAULT_RETRY_COUNT;
        // distributed retry delay, msec
        private int _retryDelay = DEFAULT_RETRY_DELAY_MS;

        // index names
        private readonly StringList _indexes = new IndexListWithWildcard();

        // index weights
        private readonly IndexWeightMap _indexWeights = new IndexWeightMap();

        // field weights
        private readonly FieldWeightMap _fieldWeights = new FieldWeightMap();

        // attribute overrides
        private readonly AttributeOverrideList _attributeOverrides = new AttributeOverrideList();
        
        // attribute filters
        private readonly AttributeFilterList _attributeFilters = new AttributeFilterList();

        // anchor point for and geodistance 
        private GeoAnchor _geoAnchor = new GeoAnchor();

        // developer comment, useful for debugging
        private string _comment;

    	#endregion

        #region Constructors
        public SearchQuery(string query)
        {
            Query = query;
        }

        public SearchQuery(string query, IEnumerable<string> indexes) : this(query)
        {
            Indexes.UnionWith(indexes);
        }
        
        public SearchQuery(string query, IEnumerable<string> indexes, string comment): this(query, indexes)
        {
            Comment = comment;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Query string
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

        /// <summary>
        /// Offset in search query result set 
        /// </summary>
        public int Offset
        {
            get { return _offset; }
            set
            {
                _offset = value;
            }
        }

        /// <summary>
        /// Limit results per query
        /// </summary>
        public int Limit
        {
            get { return _limit; }
            set
            {
                ArgumentAssert.IsNotEmpty(value, "Limit");
                _limit = value;
            }
        }

        /// <summary>
        /// Max matches per-query
        /// </summary>
        public int MaxMatches
        {
            get { return _maxMatches; }
            set
            {
                ArgumentAssert.IsNotEmpty(value, "MaxMatches");
                _maxMatches = value;
            }
        }

        /// <summary>
        ///  Max. found matches threshold. Forcibly stop search query once N matches had been found and processed.
        /// </summary>
        public int Cutoff
        {
            get { return _cutoff; }
            set
            {
                ArgumentAssert.IsNotEmpty(value, "Cutoff");
                _cutoff = value;
            }
        }

        /// <summary>
        /// Set or get matching mode.
        /// </summary>
        public MatchMode MatchMode
        {
            get { return _matchMode; }
            set
            {
                ArgumentAssert.IsDefinedInEnum(typeof (MatchMode), value, "MatchMode");
                _matchMode = value;
            }
        }

        /// <summary>
        /// Set or get ranking mode.
        /// </summary>
        public MatchRankMode RankingMode
        {
            get { return _rankMode; }
            set
            {
                ArgumentAssert.IsDefinedInEnum(typeof(MatchRankMode), value, "RankingMode");
                _rankMode = value;
            }
        }

        /// <summary>
        /// Min. document ID to match
        /// </summary>
        public long MinDocumentId
        {
            get { return _minId; }
            set
            {
                _minId = value;
            }
        }

        /// <summary>
        /// Max. document ID to match
        /// </summary>
        public long MaxDocumentId
        {
            get { return _maxId; }
            set
            {
                _maxId = value;
            }
        }

        /// <summary>
        /// Search results set sort. mode
        /// </summary>
        public ResultsSortMode SortMode
        {
            get { return _sortMode; }
            set
            {
                ArgumentAssert.IsDefinedInEnum(typeof(ResultsSortMode), value, "SortMode");
                _sortMode = value;
            }
        }

        /// <summary>
        /// Search results should be sorted in the order specified in this field clause expression (SQL like syntax).
        /// </summary>
        public string SortBy
        {
            get { return _sortBy; }
            set
            {
                ArgumentAssert.IsNotNull(value, "SortBy");
                _sortBy = value;
            }
        }

        /// <summary>
        /// Time limit for query execution per index (in milliseconds). Zero value removes any limits. 
        /// </summary>
        public int MaxQueryTime
        {
            get { return _maxQueryTime; }
            set
            {
                ArgumentAssert.IsNotEmpty(value, "MaxQueryTime");
                _maxQueryTime = value;
            }
        }

        /// <summary>
        /// List of index names will be used in search
        /// </summary>
        public StringList Indexes
        {
            get { return _indexes; }
        }

        /// <summary>
        /// Bind per-index weights by index name.
        /// </summary>
        public IndexWeightMap IndexWeights
        {
            get { return _indexWeights; }
        }

        /// <summary>
        /// Bind per-field weights by field name.
        /// </summary>
        public FieldWeightMap FieldWeights
        {
            get { return _fieldWeights; }
        }

        /// <summary>
        /// Sets the select clause, listing specific attributes to fetch, and expressions to compute and fetch. Clause syntax mimics SQL.
        /// </summary>
        public string Select
        {
            get { return _select; }
            set
            {
                _select = String.IsNullOrEmpty(value) ? DEFAULT_SELECT : value;
            }
        }

        /// <summary>
        /// Anchor point for and geosphere distance (geodistance) calculations, and enable them.
        /// </summary>
        public GeoAnchor GeoAnchor
        {
            get { return _geoAnchor; }
            set { _geoAnchor = value; }
        }

        /// <summary>
        /// Temporary (per-query) per-document attribute value overrides list.
        /// </summary>
        public AttributeOverrideList AttributeOverrides
        {
            get { return _attributeOverrides; }
        }

        /// <summary>
        /// Attribute filters list.
        /// </summary>
        public AttributeFilterList AttributeFilters
        {
            get { return _attributeFilters; }
        }

        /// <summary>
        /// Grouping attribute column
        /// </summary>
        public string GroupBy
        {
            get { return _groupBy; }
            set { _groupBy = value; }
        }

        /// <summary>
        /// Group sort clause
        /// </summary>
        public string GroupSort
        {
            get { return _groupSort; }
            set { _groupSort = value; }
        }

        /// <summary>
        /// Function applied to the attribute value in order to compute group-by key
        /// </summary>
        public ResultsGroupFunction GroupFunc
        {
            get { return _groupFunc; }
            set { _groupFunc = value; }
        }

        /// <summary>
        /// Attribute name for per-group distinct values count calculations
        /// </summary>
        public string GroupDistinct
        {
            get { return _groupDistinct; }
            set { _groupDistinct = value; }
        }

        /// <summary>
        /// Distributed retries count
        /// </summary>
        public int RetryCount
        {
            get { return _retryCount; }
            set
            {
                ArgumentAssert.IsNotEmpty(value, "RetryCount");
                _retryCount = value;
            }
        }

        /// <summary>
        /// Distributed retries delay
        /// </summary>
        public int RetryDelay
        {
            get { return _retryDelay; }
            set
            {
                ArgumentAssert.IsNotEmpty(value, "RetryDelay");
                _retryDelay = value;
            }
        }

        /// <summary>
        /// Query comment, useful for debugging and searching queries in Sphinx logs
        /// </summary>
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (!String.IsNullOrEmpty(value))
                    ArgumentAssert.IsLessOrEqual(value.Length, LIMIT_COMMENT_LENGTH, "Comment");
                _comment = value;
            }
        }


        #endregion

        #region Methods
        internal void Serialize(BinaryWriterBase writer)
        {
            ArgumentAssert.IsTrue(SortMode == ResultsSortMode.Relevance || (!String.IsNullOrEmpty(SortBy)), String.Format(Messages.Exception_ArgumentResultsSortModeNotValid, Enum.GetName(typeof(ResultsSortMode), SortMode)));
            ArgumentAssert.IsTrue(MinDocumentId <= MaxDocumentId, Messages.Exception_ArgumentMinIdGreaterThanMaxId);

            // offset, limits and match mode
            writer.Write(Offset);
            writer.Write(Limit);
            writer.Write((int)MatchMode);

            writer.Write((int)RankingMode);

            // sorting
            writer.Write((int)SortMode);
            writer.Write(SortBy);
            writer.Write(Query);

            // NOTE: don't use deprecated per-field weights list, use FieldWeights instead
            writer.Write(0);

            Indexes.Serialize(writer);

            // documents id range
            writer.Write(ID64_MARKER);
            writer.Write(MinDocumentId);
            writer.Write(MaxDocumentId);

            // filters
            AttributeFilters.Serialize(writer);

            // grouping
            writer.Write((int)GroupFunc);
            writer.Write(GroupBy);
            writer.Write(MaxMatches);
            writer.Write(GroupSort);

            // cutoff 
            writer.Write(Cutoff);

            // retry
            writer.Write(RetryCount);
            writer.Write(RetryDelay);

            // group distinct
            writer.Write(GroupDistinct);

            // geo anchor
            GeoAnchor.Serialize(writer);

            // index weights
            IndexWeights.Serialize(writer);

            // max query time
            writer.Write(MaxQueryTime);

            // per-field weights
            FieldWeights.Serialize(writer);

            // comment
            writer.Write(Comment);

            // attribute overrides
            AttributeOverrides.Serialize(writer);

            // select clause
            writer.Write(Select);
        }

        #endregion 
    }
}
