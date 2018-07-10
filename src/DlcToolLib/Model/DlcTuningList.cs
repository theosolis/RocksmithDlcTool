using System.Collections.Generic;

namespace DlcToolLib.Model
{
	public class DlcTuningList : ICarriesErrors
	{
		public List<DlcTuningItem> DlcTunings { get; set; } = new List<DlcTuningItem>();
		public List<string> Errors { get; set; } = new List<string>();
	}


}