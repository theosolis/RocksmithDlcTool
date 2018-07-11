using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DlcDatabase
{
	public enum DlcSourceType { Official, RiffRepeater, Steam, Existing }

	class ProgramArgs
	{
		public string DlcSourcePath { get; set; }

		public DlcSourceType SourceType { get; set; }

		public string PathToDatabase { get; set; }
	}
}
