# Introduction #

Changelog for version 0.3.8 of Sphinx .NET client.


# Details #

  * Implementation is upgraded to support new Sphinx 1.10-beta features, related with Search command: added support for new attribute type 'string'. Currently supported only as query result attribute values (including MVA).

  * changed default TCP port value to 9312 (used by default by latest Sphinx versions)

  * added support for all new features introduced in Sphinx 1.10-beta related with BuildExcerpts function:
    * added new command parameter SnippetsCountLimit (limit\_passages option)
    * added new command parameter WordsCountLimit (limit\_words option)
    * added all new boolean options (ForceAllWords, LoadFiles, AllowEmpty) to BuildExcerptsOptions enum
    * added new command parameter StartPassageId (start\_passage\_id option)
    * added new command parameter HtmlStripMode (html\_strip\_mode option) represented as enum

  * implemented new command represents new FlushAttributes feature  http://sphinxsearch.com/docs/current.html#api-func-flushattributes