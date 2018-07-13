namespace DlcToolLib.Finders
{
	public class SteamDlcFinder
	{
		private const string SongPackDescriptionRegex = @"([\""“].*?[\""”])(\s*?by\s*?)?(.*?)(\,|on|and)";
		private const string SongPackSongFinderRegex = @"(Play\s)(.*)on any electric guitar or bass";
	}
}