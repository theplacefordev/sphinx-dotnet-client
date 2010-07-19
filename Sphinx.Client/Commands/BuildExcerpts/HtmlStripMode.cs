using System;
using System.Collections.Generic;
using System.Text;

namespace Sphinx.Client.Commands.BuildExcerpts
{
	/// <summary>
	/// HTML stripping modes for <see cref="BuildExcerptsCommand"/> command.
	/// </summary>
	public enum HtmlStripMode
	{
		None,
		Strip,
		Index,
		Retain
	}
}
