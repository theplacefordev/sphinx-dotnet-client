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

#endregion

namespace Sphinx.Client.Commands.Attributes.Filters.Range
{
    /// <summary>
    /// Represents integer values range to filter matches by attributes in search results.
    /// </summary>
    public abstract class AttributeFilterRangeBase<T> : AttributeFilterBase
    {
        #region Constructors
        public AttributeFilterRangeBase(string name, T minValue, T maxValue, bool exclude): base(name, exclude)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }
        
        #endregion

        #region Properties
        /// <summary>
        /// Attribute filter type. AttributeFilterType.RangeInt32 is default value.
        /// </summary>
        protected override AttributeFilterType FilterType
        {
            get { return AttributeFilterType.RangeInt32; }
        }

        /// <summary>
        /// Represents the smallest possible value of attribute to filter (inclusive).
        /// </summary>
        public T MinValue { get; set; }

        /// <summary>
        /// Represents the largest possible value of attribute to filter (inclusive).
        /// </summary>
        public T MaxValue { get; set; }

        #endregion

    }
}
