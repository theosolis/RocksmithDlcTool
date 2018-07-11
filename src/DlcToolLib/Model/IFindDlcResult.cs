using System.Collections.Generic;

namespace DlcToolLib.Model
{
	public interface IFindDlcResult<T> : ICarriesErrors
	{
		List<T> DlcList { get; set; }
	}
}