using System.IO;

namespace Sphinx.Client.IO
{
    /// <summary>
    /// Interface for binary formatters factory
    /// </summary>
    public interface IBinaryFormatterFactory
    {
        BinaryReaderBase CreateReader(Stream stream);
        BinaryWriterBase CreateWriter(Stream stream);
    }
}