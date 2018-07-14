using LiteDB;

namespace DlcToolLib.Entities
{
	public class SteamDlcSongPackLink
	{
		[BsonRef(SteamDlcItem.TableName)]
		public SteamDlcItem SongPack { get; set; }

		[BsonRef(SteamDlcItem.TableName)]
		public SteamDlcItem Song { get; set; }
	}
}