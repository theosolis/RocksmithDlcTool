using System.Collections.Generic;
using DlcToolLib.Entities;

namespace DlcToolLib.Model
{
	public class ExistingDlcList
	{
		public List<ExistingDlcItem> ExistingDlc { get; set; } = new List<ExistingDlcItem>();
	}
}