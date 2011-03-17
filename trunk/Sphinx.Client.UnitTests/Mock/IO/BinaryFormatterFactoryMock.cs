using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sphinx.Client.IO;
using Sphinx.Client.Network;

namespace Sphinx.Client.UnitTests.Mock.IO
{
    public class BinaryFormatterFactoryMock : IBinaryFormatterFactory
    {
        #region Implementation of IBinaryFormatterFactory

        public BinaryReaderBase CreateReader(IStreamAdapter stream)
        {
            return new BinaryReaderMock(stream);
        }

		public BinaryWriterBase CreateWriter(IStreamAdapter stream)
        {
            return new BinaryWriterMock(stream);
        }

        #endregion
    }
}
