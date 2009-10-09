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
using Sphinx.Client.Helpers;
using Sphinx.Client.IO;

#endregion

namespace Sphinx.Client.Commands.Attributes.Update
{
    /// <summary>
    /// Represents attribute ordinal values and document IDs set to update.
    /// </summary>
    public class AttributeUpdateOrdinal : AttributeUpdateBase, IAttributeValuesPerDocument<int>
    {
        #region Fields
        private readonly Dictionary<long, int> _values;
        
        #endregion

        #region Constructors
        internal AttributeUpdateOrdinal()
        {

        }

        public AttributeUpdateOrdinal(string name, Dictionary<long, int> values): base(name)
        {
            ArgumentAssert.IsNotNull(values, "values");
            ArgumentAssert.IsNotEmpty(values.Count, "values");
            _values = values;
        }
        
        #endregion

        #region Properties
        public override AttributeType AttributeType
        {
            get { return AttributeType.Ordinal; }
        }

        #region Implementation of IAttributeValuesPerDocument
        public IDictionary<long, int> Values
        {
            get { return _values; }
        }
        
        #endregion
        
        #endregion

        #region Methods
        internal override IEnumerable<long> GetDocumentsIdSet()
        {
            return new List<long>(Values.Keys);
        }

        internal override void Serialize(BinaryWriterBase writer, long id)
        {
            writer.Write(Values[id]);
        }

        #endregion


    }
}
