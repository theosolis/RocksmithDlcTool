using System.Collections.Generic;
using DlcToolLib.Entities;

namespace DlcToolLib.Model
{
	public class OfficialDlcList : IFindDlcResult<OfficialDlcItem>
	{
		public List<OfficialDlcItem> DlcList { get; set; } = new List<OfficialDlcItem>();
		public List<string> Errors { get; set; } = new List<string>();
	}
}