using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace DlcToolLib
{
	public static class DlcHelper
	{
		public static bool IsValidURL(String urlStr)
		{
			try
			{
				Uri uri = new Uri(urlStr);
				return string.Compare(uri.Scheme, "http", StringComparison.InvariantCultureIgnoreCase) == 0 ||
						string.Compare(uri.Scheme, "https", StringComparison.InvariantCultureIgnoreCase) == 0;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static HtmlDocument GetHtmlDocument(string sourcePath)
		{
			if (IsValidURL(sourcePath))
			{
				// From Web
				var web = new HtmlWeb();
				web.OverrideEncoding = Encoding.UTF8;
				var doc = web.Load(sourcePath);
				return doc;
			}

			// From File
			var fileDoc = new HtmlDocument();
			fileDoc.Load(sourcePath, Encoding.UTF8);
			return fileDoc;
		}
	}
}
