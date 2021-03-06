﻿using System.Collections.Generic;
using System.IO;
using DlcToolLib.Entities;
using DlcToolLib.Model;
using RocksmithToTabLib;
using System.Linq;
using System;
using PSARC = RocksmithToolkitLib.PSARC.PSARC;
using RijndaelEncryptor = RocksmithToolkitLib.DLCPackage.RijndaelEncryptor;
using Newtonsoft.Json;
using RocksmithToolkitLib.DLCPackage.Manifest;

namespace DlcToolLib.Finders
{
	public class ExistingDlcFinder : IDlcFinder<ExistingDlcItem>
	{
		private IDlcSortCalculator _dlcSortCalculator;

		public ExistingDlcFinder(IDlcSortCalculator dlcSortCalculator)
		{
			_dlcSortCalculator = dlcSortCalculator;
		}

		public IFindDlcResult<ExistingDlcItem> FindDlc(string sourcePath)
		{
			var rv = new ExistingDlcList();

			rv.DlcList.AddRange(FindRocksmith2014(sourcePath));
			rv.DlcList.AddRange(FindRocksmith1(sourcePath));

			return rv;
		}

		public ExistingDlcList FindAllDlc(string rs2014DlcFolder, string rs1DlcFolder)
		{
			var rv = new ExistingDlcList();
			rv.DlcList.AddRange(FindRocksmith2014(rs2014DlcFolder));

			if(!string.IsNullOrWhiteSpace(rs1DlcFolder))
				rv.DlcList.AddRange(FindRocksmith1(rs1DlcFolder));

			return rv;
		}

		private List<ExistingDlcItem> FindRocksmith2014(string directoryToSearch)
		{
			var psArcFiles = Directory.GetFiles(directoryToSearch, "*p.psarc");
			var rv = new List<ExistingDlcItem>();

			foreach (var psArcFile in psArcFiles)
			{
				if (IsFileToIgnore(psArcFile))
					continue;

				var existingItems = GetExistingItemsFromPsArcFile(psArcFile);
				rv.AddRange(existingItems);
			}
			return rv;
		}

		
		private bool IsFileToIgnore(string filePath)
		{
			return Path.GetFileNameWithoutExtension(filePath).StartsWith("rs1compatibility");
		}

		private IEnumerable<ExistingDlcItem> GetExistingItemsFromPsArcFile(string psArcFile)
		{
			var browser = new PsarcBrowser(psArcFile);

			var songList = new List<SongInfo>(browser.GetSongList());
			foreach (var song in songList)
			{
				yield return MapSongToExistingDlcItem(song, psArcFile);
			}
		}

		private ExistingDlcItem MapSongToExistingDlcItem(SongInfo song, string psArcFile)
		{
			var rv = new ExistingDlcItem
			{
				Artist = song.Artist,
				Song = song.Title,
				PathToFile = psArcFile,
				Identifier = song.Identifier,
				DlcSource = DlcGameVersionType.Rs2014
			};

			var sortDetails = _dlcSortCalculator.CreateSortDetails(rv.Artist, rv.Song);
			rv.ArtistSort = sortDetails.ArtistSort;
			rv.SongSort = sortDetails.SongSort;
			rv.UniqueKey = sortDetails.UniqueKey;

			return rv;
		}

		private ExistingDlcItem MapSongToExistingDlcItem(Attributes song, string datFile)
		{
			var rv = new ExistingDlcItem
			{
				Artist = song.ArtistName,
				Song = song.SongName,
				PathToFile = datFile,
				Identifier = song.SongKey,
				DlcSource = DlcGameVersionType.Rs1
			};

			var sortDetails = _dlcSortCalculator.CreateSortDetails(rv.Artist, rv.Song);
			rv.ArtistSort = sortDetails.ArtistSort;
			rv.SongSort = sortDetails.SongSort;
			rv.UniqueKey = sortDetails.UniqueKey;

			return rv;
		}

		private List<ExistingDlcItem> FindRocksmith1(string directoryToSearch)
		{
			var datFiles = Directory.GetFiles(directoryToSearch, "*.dat");
			var rv = new List<ExistingDlcItem>();

			foreach (var datFile in datFiles)
			{
				var existingItems = GetExistingItemsFromDatFile(datFile);
				rv.AddRange(existingItems);
			}
			return rv;
		}

		private IEnumerable<ExistingDlcItem> GetExistingItemsFromDatFile(string datFile)
		{
			var rv = new List<ExistingDlcItem>();

			using (var archive = new PSARC())
			{
				using (var inputFileStream = File.OpenRead(datFile))
				{
					using (var inputStream = new MemoryStream())
					{
						RijndaelEncryptor.DecryptFile(inputFileStream, inputStream, RijndaelEncryptor.DLCKey);
						archive.Read(inputStream, false);

						var innerPsArcEntry = archive.TOC.SingleOrDefault(x => !x.Name.StartsWith("DLC_", StringComparison.CurrentCultureIgnoreCase) && x.Name.EndsWith("psarc", StringComparison.CurrentCultureIgnoreCase));
						if (innerPsArcEntry == null)
							return rv;

						archive.InflateEntry(innerPsArcEntry.Name);
						using (var innerPsArc = new PSARC())
						{
							innerPsArc.Read(innerPsArcEntry.Data, false);
							var manifest = innerPsArc.TOC.SingleOrDefault(x => x.Name.StartsWith("Manifests/songs.manifest.json", StringComparison.CurrentCultureIgnoreCase));
							if (manifest == null)
								return rv;

							using (var reader = new StreamReader(manifest.Data, System.Text.Encoding.UTF8))
							{
								var fromStream = reader.ReadToEnd();
								var manifestObj = JsonConvert.DeserializeObject<Manifest>(fromStream);
								var songAttributes = manifestObj.Entries.Values.First().Values.First();

								rv.Add(MapSongToExistingDlcItem(songAttributes, datFile));
							}
						}
					}
				}
			}
			return rv;
		}
	}
}