using DlcToolLib.Entities;

namespace DlcToolLib.Loading
{
	public interface ILoadCoordinatorFactory
	{
		ILoadCoordinator CreateLoadCoordinator(DlcSourceType sourceType);
	}
}