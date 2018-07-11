using DlcToolLib.Finders;

namespace DlcToolLib.Loading
{
	public static class LoadingOracle
	{
		public static ILoadCoordinatorFactory GetDefaultLoadCoordinatorFactory()
		{
			return new LoadCoordinatorFactory(new FinderFactory());
		}
	}
}