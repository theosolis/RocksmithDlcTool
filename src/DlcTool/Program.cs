using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CommandLine;
using CommandLine.Text;
using DlcToolLib;
using DlcToolLib.Model;
using System.IO;

namespace DlcTool
{
	class Program
	{
		static void Main(string[] args)
		{
			//var dlcFinder = new DlcTuningsFinder();
			//var dlcTuningsList = dlcFinder.GetRocksmithDlcListFromFile(@"C:\Users\goliver\Documents\Visual Studio 2017\Projects\DlcTool\Testing\DlcList.html");
			////			var url = "https://theriffrepeater.com/rocksmith-2014-setlist/rocksmith-2014-tunings/";

			var dlcOwnershipInput = new DlcOwnershipInput();
			CmdLineArgs cmdLineArgs = new CmdLineArgs();
			var parserResults = Parser.Default.ParseArguments<CmdLineArgs>(args)
				.WithParsed(options =>
					{
						dlcOwnershipInput = GetDlcOwnershipInputs(options);
						cmdLineArgs = options;
					}
				);
			if (parserResults is NotParsed<CmdLineArgs>)
			{
				return;
			}

			dlcOwnershipInput.RemapOfficialEntries = LoadRemapOfficialEntries(cmdLineArgs.CheckDlc ? cmdLineArgs.RemapFilePathOfficialToTunings : cmdLineArgs.RemapFilePathOfficialToDlc);

			if (!dlcOwnershipInput.RemapOfficialEntries.Validate())
			{
				Console.WriteLine("Errors occurred trying to validate the mapping file.  The errors were: ");
				foreach(var msg in dlcOwnershipInput.RemapOfficialEntries.Errors)
				{
					Console.WriteLine(msg);
				}
				return;
			}

			if (!IsValidInputs(dlcOwnershipInput))
			{
				Console.WriteLine(HelpText.RenderUsageText(parserResults));
				return;
			}

			if (cmdLineArgs.CheckDlc)
			{
				CheckDlc(dlcOwnershipInput, cmdLineArgs.OutputDirPath, cmdLineArgs.ConvertEncoding);
				return;
			}
			CalculateOwnership(dlcOwnershipInput, cmdLineArgs.OutputDirPath, cmdLineArgs.ConvertEncoding);
		}

		private static RemapOfficialEntries LoadRemapOfficialEntries(string path)
		{
			var fileContents = File.ReadAllText(path);
			using (var reader = new StringReader(fileContents))
			{
				var serializer = new XmlSerializer(typeof(RemapOfficialEntries));
				return (RemapOfficialEntries)serializer.Deserialize(reader);
			}
		}

		private static void CalculateOwnership(DlcOwnershipInput dlcOwnershipInput, string outputDirPath, bool convertEncoding)
		{
			var dlcFuncs = new DlcFunctions();

			var ownershipDetails = new DlcFunctions().CalculateOwnership(dlcOwnershipInput);
			WriteOutputFiles(ownershipDetails, outputDirPath, convertEncoding);
		}

		private static void WriteOutputFiles(DlcOwnership ownershipDetails, string outputDirPath, bool convertUnicodeCharacters)
		{
			var dlcListWriter = new DlcListWriter(new StringCleaner(false, convertUnicodeCharacters, false, false));
			dlcListWriter.WriteOfficialDlcToFile(ownershipDetails.MissingOfficialDlc, Path.Combine(outputDirPath, "missing.csv"));
			dlcListWriter.WriteSongPackInfoToFile(ownershipDetails.SongPacks, Path.Combine(outputDirPath, "songpacks.csv"));
			dlcListWriter.WriteDlcListToFile(ownershipDetails.UnknownExisting, Path.Combine(outputDirPath, "unmatchedExisting.csv"));
		}

		private static bool IsValidInputs(DlcOwnershipInput missingDlcInput)
		{
			if (string.IsNullOrEmpty(missingDlcInput.DlcFolder2014))
				return false;

			if (missingDlcInput.RemapOfficialEntries == null)
				return false;

			return !string.IsNullOrEmpty(missingDlcInput.OfficialDlcSource);
		}

		private static DlcOwnershipInput GetDlcOwnershipInputs(CmdLineArgs cmdLineArgs)
		{
			return new DlcOwnershipInput
			{
				DlcFolder2014 = cmdLineArgs.DlcFolder,
				DlcFolder2012 = cmdLineArgs.rs1DlcFolder,
				OfficialDlcSource = cmdLineArgs.OfficialDlcSource,
				OfficialDlcSongNodeSelector = cmdLineArgs.OfficialSourceXPath,
				TuningSource = cmdLineArgs.TuningSource
			};
		}

		private static void CheckDlc(DlcOwnershipInput dlcOwnershipInput, string outputDirPath, bool convertUnicodeCharacters)
		{
			var dlcCheckResults = new DlcFunctions().CheckDlcLists(dlcOwnershipInput);
			var dlcListWriter = new DlcListWriter(new StringCleaner(false, convertUnicodeCharacters, false, false));

			var officialUnmatched = dlcCheckResults.Where(x => x.MatchResult == MatchResultType.UnmatchedLeftDlc).Select(x=>x.LeftDlc).ToList();
			var tuningsUnmatched = dlcCheckResults.Where(x => x.MatchResult == MatchResultType.UnmatchedRightDlc).Select(x=>x.RightDlc).ToList();

			dlcListWriter.WriteOfficialDlcToFile(officialUnmatched, Path.Combine(outputDirPath, "official_not_matched_to_tunings.csv"));
			dlcListWriter.WriteDlcListToFile(tuningsUnmatched, Path.Combine(outputDirPath, "tunings_not_match_to_official.csv"));
		}
	}
}
