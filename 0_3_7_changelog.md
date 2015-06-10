# Introduction #

Changelog for new version of Sphinx .NET client.

# Details #
  * fixed bug: "EndOfStreamException" occurred while reading server response from NetworkStream, with large amount of data
  * Removed unnecessary ConnectionTimeout constraint
  * "Port" property constraint constant mapped to UInt16.MaxValue