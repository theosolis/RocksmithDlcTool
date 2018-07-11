using System.Collections.Generic;
using DlcToolLib.Entities;

namespace DlcToolLib.Model
{
	public class DlcOwnership
	{
		public List<OfficialDlcItem> MissingOfficialDlc { get; set; } = new List<OfficialDlcItem>();
		public List<SongPack> SongPacks { get; set; } = new List<SongPack>();
		public List<ExistingDlcItem> UnknownExisting { get; set; } = new List<ExistingDlcItem>();
	}
}