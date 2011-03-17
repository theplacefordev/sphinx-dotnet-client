using System.IO;
using Sphinx.Client.Network;

namespace Sphinx.Client.IO
{
    /// <summary>
    /// Interface for binary formatters factory
    /// </summary>
    public interface IBinaryFormatterFactory
    {
		BinaryReaderBase CreateReader(IStreamAdapter stream);
		BinaryWriterBase CreateWriter(IStreamAdapter stream);
    }
}