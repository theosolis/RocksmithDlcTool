using System.Collections.Generic;
using DlcToolLib.Entities;

namespace DlcToolLib.Model
{
	public class ExistingDlcList : IFindDlcResult<ExistingDlcItem>
	{
		public List<ExistingDlcItem> DlcList { get; set; } = new List<ExistingDlcItem>();
		public List<string> Errors { get; set; } = new List<string>();
	}
}