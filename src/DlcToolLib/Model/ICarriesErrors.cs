using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DlcToolLib.Model
{
	public interface ICarriesErrors
	{
		List<string> Errors { get; set; }
	}
}
