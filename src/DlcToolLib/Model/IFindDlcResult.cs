using System.Collections.Generic;
using DlcToolLib.Entities;

namespace DlcToolLib.Model
{
	public interface IFindDlcResult<T> : ICarriesErrors
		where T: IDlc
	{
		List<T> DlcList { get; set; }
	}
}