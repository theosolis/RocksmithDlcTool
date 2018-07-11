using System.Collections.Generic;
using DlcToolLib.Entities;

namespace DlcToolLib.Model
{
	public class DlcTuningList : IFindDlcResult<DlcTuningItem>
	{
		public List<DlcTuningItem> DlcList { get; set; } = new List<DlcTuningItem>();
		public List<string> Errors { get; set; } = new List<string>();
	}


}