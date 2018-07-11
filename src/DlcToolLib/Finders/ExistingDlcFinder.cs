using System.Collections.Generic;
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
	public class ExistingDlcFinder
	{
		public ExistingDlcList FindAllDlc(string rs2014DlcFolder, string rs1DlcFolder)
		{
			var rv = new ExistingDlcList();
			rv.ExistingDlc.AddRange(FindRocksmith2014(rs2014DlcFolder));

			if(!string.IsNullOrWhiteSpace(rs1DlcFolder))
				rv.ExistingDlc.AddRange(FindRocksmith1(rs1DlcFolder));

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

		private bool ArrangementsAreInFile(PsarcBrowser browser, SongInfo x)
		{
			var firstArrangement = browser.GetArrangement(x.Identifier, x.Arrangements.First());
			return firstArrangement != null;
		}

		private ExistingDlcItem MapSongToExistingDlcItem(SongInfo song, string psArcFile)
		{
			var rv = new ExistingDlcItem
			{
				Artist = song.Artist,
				Song = song.Title,
				PathToFile = psArcFile,
				Identifier = song.Identifier,
				DlcSource = DlcSourceType.Rs2014
			};

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
				DlcSource = DlcSourceType.Rs1
			};

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