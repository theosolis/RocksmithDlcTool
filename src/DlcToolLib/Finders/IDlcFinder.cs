using DlcToolLib.Entities;
using DlcToolLib.Model;

namespace DlcToolLib.Finders
{
	public interface IDlcFinder<T>
		where T : IDlc
	{
		IFindDlcResult<T> FindDlc(string sourcePath);
	}
}