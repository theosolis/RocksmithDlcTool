using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DlcToolLib
{
	public class StringCleaner
	{
		private readonly bool _cleanPunctuation;
		private readonly bool _cleanUnicode;
		private readonly bool _stripArtistName;
		private readonly bool _stripSongName;

		public StringCleaner(bool cleanPunctuation, bool cleanUnicode, bool stripArtistName, bool stripSongName)
		{
			_cleanPunctuation = cleanPunctuation;
			_cleanUnicode = cleanUnicode;
			_stripArtistName = stripArtistName;
			_stripSongName = stripSongName;
		}

		public string Clean(string s)
		{
			if (_cleanUnicode)
			{
				s = new string(s.Normalize(System.Text.NormalizationForm.FormD)
					.ToCharArray()
					.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
					.ToArray());
			}
			if (_cleanPunctuation)
			{
				s = s.Replace("’", "'").Replace("“", "\"").Replace("”", "\"").Replace("–", "-");
			}
			return s;
		}


		public string MakeArtistSortName(string s)
		{
			s = Clean(s).ToLower();

			if (s.StartsWith("the ", StringComparison.InvariantCultureIgnoreCase))
				s = s.Substring(4);

			if (!_stripArtistName)
				return s;

			s = Regex.Replace(s, " and ", "&", RegexOptions.IgnoreCase);
			return StripPunctuation(s);
		}

		public string StripPunctuation(string s)
		{
			return new string(s.Where(c => !char.IsPunctuation(c) && c != ' ').ToArray());
		}

		public string MakeSongSortName(string s)
		{
			var prepped = Clean(s).ToLower();

			if (!_stripSongName)
				return prepped;

			return StripPunctuation(prepped);
		}

	}
}
