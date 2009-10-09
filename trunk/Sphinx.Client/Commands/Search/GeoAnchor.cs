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

using Sphinx.Client.Helpers;
using Sphinx.Client.IO;

#endregion

namespace Sphinx.Client.Commands.Search
{
    /// <summary>
    /// Geographical anchor point.
    /// Required to use geo distance in filters and sorting.
    /// Distance will be computed to this point.
    /// </summary>
    public class GeoAnchor
    {
        #region Fields

        private string _latitudeAttr;
        private string _longitudeAttr;
        private float _latitudeVal;
        private float _longitudeVal;
        
        #endregion

        #region Constructors
        public GeoAnchor()
        {
        }

        /// <summary>
        /// Explicit constructor
        /// </summary>
        /// <param name="latitudeAttr">Latitude attribute name</param>
        /// <param name="latitudeVal">Latitude value (in radiants)</param>
        /// <param name="longitudeAttr">Longitude attribute name</param>
        /// <param name="longitudeVal">Longitude value (in radiants)</param>
        public GeoAnchor(string latitudeAttr, float latitudeVal, string longitudeAttr, float longitudeVal)
        {
            LatitudeAttributeName = latitudeAttr;
            LongitudeAttributeName = longitudeAttr;
            LatitudeValue = latitudeVal;
            LongitudeValue = longitudeVal;
            IsNotEmpty = true;
        }
        
        #endregion

        #region Properties
        /// <summary>
        /// Latitude attribute name
        /// </summary>
        public string LatitudeAttributeName
        {
            get { return _latitudeAttr; }
            set
            {
                ArgumentAssert.IsNotEmpty(value, "LatitudeAttributeName");
                _latitudeAttr = value;
                IsNotEmpty = true;
            }
        }

        /// <summary>
        /// Longitude attribute name
        /// </summary>
        public string LongitudeAttributeName
        {
            get { return _longitudeAttr; }
            set
            {
                ArgumentAssert.IsNotEmpty(value, "LongitudeAttributeName");
                _longitudeAttr = value;
                IsNotEmpty = true;
            }
        }

        /// <summary>
        /// Latitude value (in radiants)
        /// </summary>
        public float LatitudeValue
        {
            get { return _latitudeVal; }
            set
            {
                _latitudeVal = value;
                IsNotEmpty = true;
            }
        }

        /// <summary>
        /// Longitude value (in radiants)
        /// </summary>
        public float LongitudeValue
        {
            get { return _longitudeVal; }
            set
            {
                _longitudeVal = value;
                IsNotEmpty = false;
            }
        }

        protected bool IsNotEmpty { get; set; }

        #endregion

        #region Methods
        internal void Serialize(BinaryWriterBase writer)
        {
            writer.Write(IsNotEmpty);
            if (IsNotEmpty)
            {
                writer.Write(LatitudeAttributeName);
                writer.Write(LongitudeAttributeName);
                writer.Write(LatitudeValue);
                writer.Write(LongitudeValue);
            }
        }

        #endregion
    }
}
