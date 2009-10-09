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
    public class AttributeUpdateBoolean : AttributeUpdateBase, IAttributeValuesPerDocument<bool>
    {
        #region Fields
        private readonly Dictionary<long, bool> _values = new Dictionary<long, bool>();
        
        #endregion

        #region Constructors
        internal AttributeUpdateBoolean()
        {
        }

        public AttributeUpdateBoolean(string name, IDictionary<long, bool> values): base(name)
        {
            ArgumentAssert.IsNotNull(values, "values");
            ArgumentAssert.IsNotEmpty(values.Count, "values.Count");
            CollectionUtil.UnionDictionaries(_values, values);
        }
        
        #endregion

        #region Properties
        #region Implementation of IAttributeValuesPerDocument
        public IDictionary<long, bool> Values
        {
            get { return _values; }
        }

        #endregion

        #region Implementation of AttributeUpdateBase
        public override AttributeType AttributeType
        {
            get { return AttributeType.Boolean; }
        }
        #endregion
        
        #endregion

        #region Methods
		#region Implementation of AttributeUpdateBase
        internal override IEnumerable<long> GetDocumentsIdSet()
        {
            return new List<long>(Values.Keys);
        }

        internal override void Serialize(BinaryWriterBase writer, long id)
        {
            writer.Write(Values[id]);
        }

        #endregion
 
	    #endregion    
    }
}
