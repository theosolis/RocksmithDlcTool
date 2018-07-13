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

				case DlcSourceType.Steam:
					return CreateSteamDlcCoordinator();
			}

			throw new ApplicationException("Unsupported source type " + sourceType);
		}

		private ILoadCoordinator CreateOfficialDlcCoordinator()
		{
			var myFinder = _finderFactory.GetDefaultOfficialDlcFinder();
			var myLoader = new GenericDlcLoader<OfficialDlcItem>(OfficialDlcItem.TableName, UpdateOfficialDlcItemRow);

			return new GenericLoadCoordinator<OfficialDlcItem>(myFinder, myLoader);
		}

		private bool UpdateOfficialDlcItemRow(OfficialDlcItem target, OfficialDlcItem source)
		{
			SetCommonDlcFieldsForUpdate(target, source);
			target.Genre = source.Genre;
			target.SongPack = source.SongPack;

			return true;
		}

		private ILoadCoordinator CreateDlcTuningCoordinator()
		{
			var myFinder = _finderFactory.GeDlcTuningsDlcFinder();
			var myLoader = new GenericDlcLoader<DlcTuningItem>(DlcTuningItem.TableName, UpdateDlcTuningRow);

			return new GenericLoadCoordinator<DlcTuningItem>(myFinder, myLoader);
		}

		private bool UpdateDlcTuningRow(DlcTuningItem target, DlcTuningItem source)
		{
			SetCommonDlcFieldsForUpdate(target, source);
			target.BassTuning = source.BassTuning;
			target.LeadTuning = source.LeadTuning;
			target.RhythmTuning = source.RhythmTuning;

			return true;
		}

		private ILoadCoordinator CreatePurchasedDlcCoordinator()
		{
			var myFinder = _finderFactory.GetExistingDlcFinder();
			var myLoader = new GenericDlcLoader<ExistingDlcItem>(ExistingDlcItem.TableName, UpdateExistingDlcItemRow);

			return new GenericLoadCoordinator<ExistingDlcItem>(myFinder, myLoader);
		}

		private bool UpdateExistingDlcItemRow(ExistingDlcItem target, ExistingDlcItem source)
		{
			SetCommonDlcFieldsForUpdate(target, source);

			target.DlcSource = source.DlcSource;
			target.Identifier = source.Identifier;
			target.PathToFile = source.PathToFile;

			return true;
		}

		private ILoadCoordinator CreateSteamDlcCoordinator()
		{
			var myFinder = _finderFactory.GetSteamDlcFinder();
			var myLoader = new GenericDlcLoader<SteamDlcItem>(SteamDlcItem.TableName, UpdateSteamDlcItemRow);

			return new GenericLoadCoordinator<SteamDlcItem>(myFinder, myLoader);
		}

		private bool UpdateSteamDlcItemRow(SteamDlcItem target, SteamDlcItem source)
		{
			SetCommonDlcFieldsForUpdate(target, source);

			target.DlcPageUrl = source.DlcPageUrl;

			return true;
		}

		private void SetCommonDlcFieldsForUpdate(IDlc target, IDlc source)
		{
			target.Artist = source.Artist;
			target.ArtistSort = source.ArtistSort;
			target.Song = source.Song;
			target.SongSort = source.SongSort;
		}
	}
}