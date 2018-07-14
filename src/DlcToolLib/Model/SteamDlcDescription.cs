using DlcToolLib.Entities;

namespace DlcToolLib.Model
{
	public class SteamDlcDescription
	{
		public bool ParsedSuccessfully { get; set; }

		public string DlcName { get; set; } = string.Empty;
		public string SongPack { get; set; } = string.Empty;
		public string Artist { get; set; } = string.Empty;
		public string SongName { get; set; } = string.Empty;

		public DlcGameVersionType GameVersion { get; set; }
		public SteamDlcItemType ItemType { get; set; }
	}
}