using System.Collections.Generic;

namespace DlcToolLib.Model
{
	public class OfficialDlcList : ICarriesErrors
	{
		public List<OfficialDlcItem> DlcItems { get; set; } = new List<OfficialDlcItem>();
		public List<string> Errors { get; set; } = new List<string>();
	}
}