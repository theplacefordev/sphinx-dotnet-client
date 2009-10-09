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
    /// Represents attribute multi-boolean values and document IDs set to update.
    /// </summary>
    public class AttributeUpdateMultiBoolean : AttributeUpdateBase, IAttributeValuesPerDocument<List<bool>>
    {
        #region Fields
        private readonly Dictionary<long, List<bool>> _values = new Dictionary<long, List<bool>>();
        
        #endregion

        #region Constructors
        internal AttributeUpdateMultiBoolean()
        {
        }

        public AttributeUpdateMultiBoolean(string name, IDictionary<long, List<bool>> values): base(name)
        {
            ArgumentAssert.IsNotNull(values, "values");
            ArgumentAssert.IsNotEmpty(values.Count, "values.Count");
            CollectionUtil.UnionDictionaries(_values, values);
        }
        
        #endregion

        #region Properties
        public override AttributeType AttributeType
        {
            get { return AttributeType.MultiBoolean; }
        }

        #region Implementation of IAttributeValuesPerDocument
        public IDictionary<long, List<bool>> Values
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
            List<bool> values = Values[id];
            writer.Write(values.Count);
            foreach (bool val in values)
            {
                writer.Write(val);
            }
        }

        #endregion

    }
}
