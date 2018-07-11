using System;

namespace DlcToolLib.Model
{
	public class DlcLoadPolicy
	{
		public bool ClearExistingBeforeLoad { get; set; }
		public bool AddNewItems { get; set; }
		public bool UpdateExisting { get; set; }
	}
}