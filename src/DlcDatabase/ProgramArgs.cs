using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DlcToolLib.Entities;

namespace DlcDatabase
{

	class ProgramArgs
	{
		public string DlcSourcePath { get; set; }

		public DlcSourceType SourceType { get; set; }

		public string PathToDatabase { get; set; }

		public bool ReplaceExistingItems { get; set; }
	}
}
