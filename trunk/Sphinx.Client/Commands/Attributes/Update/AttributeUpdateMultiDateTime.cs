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
using Sphinx.Client.Helpers;
using Sphinx.Client.IO;

#endregion

namespace Sphinx.Client.Commands.Attributes.Update
{
    /// <summary>
    /// Represents attribute multi-DateTime values and document IDs set to update.
    /// </summary>
    public class AttributeUpdateMultiDateTime : AttributeUpdateBase, IAttributeValuesPerDocument<List<DateTime>>
    {
        #region Fields
        private readonly Dictionary<long, List<DateTime>> _values = new Dictionary<long, List<DateTime>>();
        
        #endregion

        #region Constructors
        internal AttributeUpdateMultiDateTime()
        {
        }

        public AttributeUpdateMultiDateTime(string name, IDictionary<long, List<DateTime>> values): base(name)
        {
            ArgumentAssert.IsNotNull(values, "values");
            ArgumentAssert.IsNotEmpty(values.Count, "values.Count");
            CollectionUtil.UnionDictionaries(Values, values);
        }
        
        #endregion

        #region Properties
        public override AttributeType AttributeType
        {
            get { return AttributeType.MultiTimestamp; }
        }

        #region Implementation of IAttributeValuesPerDocument
        public IDictionary<long, List<DateTime>> Values
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
            IList<DateTime> values = Values[id];
            writer.Write(values.Count);
            foreach (DateTime val in values)
            {
                writer.Write(val);
            }
        }

        #endregion

    }
}
