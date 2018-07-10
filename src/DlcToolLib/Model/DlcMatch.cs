using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DlcToolLib.Model
{
	public enum MatchResultType
	{
		Matched,
		UnmatchedLeftDlc,
		UnmatchedRightDlc
	}

	public class DlcMatch<TLeft, TRight>
	{
		public string UniqueKey { get; set; }
		public MatchResultType MatchResult { get; set; }
		public TLeft LeftDlc { get; set; }
		public TRight RightDlc { get; set; }
	}
}
