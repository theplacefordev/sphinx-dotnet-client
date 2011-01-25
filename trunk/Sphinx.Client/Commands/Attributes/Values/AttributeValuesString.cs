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
using Sphinx.Client.IO;

#endregion

namespace Sphinx.Client.Commands.Attributes.Values
{
    /// <summary>
    /// Represents string attribute values in matched document.
    /// </summary>
    public class AttributeValuesString : AttributeValueBase, IAttributeValues<string>
    {
        #region Fields
        private readonly List<string> _values;
        
        #endregion

        #region Constructors
        internal AttributeValuesString()
        {
			_values = new List<string>();
        }

		public AttributeValuesString(string name, List<string> values): base(name)
        {
            _values = values;
        }
        
        #endregion

        #region Overrides of AbstractAttribute
        public override AttributeType AttributeType
        {
            get { return AttributeType.MultiString; }
        }

        #region Implementation of IAttributeValuesPerDocument
        public IList<string> Values
        {
            get { return _values; }
        }
        
        #endregion

        #endregion

        #region Methods
        public override object GetValue()
        {
            return Values;
        }

        internal override void Deserialize(BinaryReaderBase reader, AttributeInfo attributeInfo)
        {
            base.Deserialize(reader, attributeInfo);
            int count = reader.ReadInt32();
            for (int i=0; i < count; i++)
            {
                _values.Add(reader.ReadString());
            }
        }

        #endregion

    }
}
