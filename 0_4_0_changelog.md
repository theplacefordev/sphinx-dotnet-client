# Introduction #

Changelog for version 0.4.0 of Sphinx .NET client.

# Details #

  * Sources compatible with .NET 2.0 and Mono 2.6 compiler
  * Networking code has been refactoried to avoid timeout's during long query execution
  * Some bugs fixed
  * Removed obsolete commands (no longer supported by Sphinx)
  * Added Encoding property to ConnectionBase. Default encoding is utf8.
  * All MVA based attributes now exposes more generic IList interface, instead of List