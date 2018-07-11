using System;
using DlcToolLib.Entities;
using DlcToolLib.Finders;

namespace DlcToolLib.Loading
{
	public class LoadCoordinatorFactory: ILoadCoordinatorFactory
	{
		private readonly FinderFactory _finderFactory;

		public LoadCoordinatorFactory(FinderFactory finderFactory)
		{
			_finderFactory = finderFactory;
		}

		public ILoadCoordinator CreateLoadCoordinator(DlcSourceType sourceType)
		{
			switch (sourceType)
			{
				case DlcSourceType.Official:
					return CreateOfficialDlcCoordinator();

				case DlcSourceType.DlcTuning:
					return CreateDlcTuningCoordinator();

				case DlcSourceType.Purchased:
					return CreatePurchasedDlcCoordinator();
			}

			throw new ApplicationException("Unsupported source type " + sourceType);
		}

		private ILoadCoordinator CreateOfficialDlcCoordinator()
		{
			var myFinder = _finderFactory.GetDefaultOfficialDlcFinder();
			var myLoader = new OfficialDlcLoader();

			return new GenericLoadCoordinator<OfficialDlcItem>(myFinder, myLoader);
		}

		private ILoadCoordinator CreateDlcTuningCoordinator()
		{
			var myFinder = _finderFactory.GeDlcTuningsDlcFinder();
			var myLoader = new DlcTuningLoader();

			return new GenericLoadCoordinator<DlcTuningItem>(myFinder, myLoader);
		}

		private ILoadCoordinator CreatePurchasedDlcCoordinator()
		{
			var myFinder = _finderFactory.GetExistingDlcFinder();
			var myLoader = new PurchasedDlcLoader();

			return new GenericLoadCoordinator<ExistingDlcItem>(myFinder, myLoader);
		}
	}
}