using System.Collections.Generic;
using DlcToolLib.Entities;

namespace DlcToolLib.Model
{
	public class SteamDlcList : IFindDlcResult<SteamDlcItem>
	{
		public List<string> Errors { get; set; } = new List<string>();
		public List<SteamDlcItem> DlcList { get; set; } = new List<SteamDlcItem>();
	}
}