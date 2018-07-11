using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DlcToolLib;
using DlcToolLib.Model;
using System.IO;
using Fclp;

namespace DlcTool
{
	class Program
	{
		static void Main(string[] args)
		{
			//var dlcFinder = new DlcTuningsFinder();
			//var dlcTuningsList = dlcFinder.GetRocksmithDlcListFromFile(@"C:\Users\goliver\Documents\Visual Studio 2017\Projects\DlcTool\Testing\DlcList.html");
			////			var url = "https://theriffrepeater.com/rocksmith-2014-setlist/rocksmith-2014-tunings/";
			var cmdLineArgs = ParseCommandLine(args);
			if (cmdLineArgs == null) return;

			var dlcOwnershipInput = GetDlcOwnershipInputs(cmdLineArgs);

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

			if (cmdLineArgs.CheckDlc)
			{
				CheckDlc(dlcOwnershipInput, cmdLineArgs.OutputDirPath, cmdLineArgs.ConvertEncoding);
				return;
			}
			CalculateOwnership(dlcOwnershipInput, cmdLineArgs.OutputDirPath, cmdLineArgs.ConvertEncoding);
		}

		static CmdLineArgs ParseCommandLine(string[] args)
		{
			var p = new FluentCommandLineParser();
			var cmdLineArgs = new CmdLineArgs();

			p.Setup<string>("dlcfolder")
				.Callback(dlcFolder => cmdLineArgs.DlcFolder = dlcFolder)
				.WithDescription("Path to dlc folder for Rocksmith")
				.Required();

			p.Setup<string>("rs1dlcfolder")
				.Callback(rs1DlcFolder => cmdLineArgs.rs1DlcFolder = rs1DlcFolder)
				.WithDescription("Path to RS1 dlc");

			p.Setup<string>("source")
				.Callback(val => cmdLineArgs.OfficialDlcSource = val)
				.WithDescription("Where to find the list of Rocksmith dlc")
				.Required();

			p.Setup<string>("outputdir")
				.Callback(val => cmdLineArgs.OutputDirPath = val)
				.WithDescription("Where to write the output files")
				.Required();

			p.Setup<string>("officialsourcexpath")
				.Callback(val => cmdLineArgs.OfficialSourceXPath = val)
				.WithDescription("XPath to find the <songs> node in the official source")
				.SetDefault("//div[@class='tabPanel downloads']/songs")
				.Required();

			p.Setup<bool>("convert")
				.Callback(val => cmdLineArgs.ConvertEncoding = val)
				.WithDescription("Try to convert unicode characters to ASCII where possible (eg ’ to ')");

			p.Setup<string>("dlcmap")
				.Callback(val => cmdLineArgs.RemapFilePathOfficialToDlc = val)
				.WithDescription("Path to the mapping file for helping map official dlc list to dlc items")
				.SetDefault("OfficialEntriesToDlc.xml");

			p.Setup<string>("tuningmap")
				.Callback(val => cmdLineArgs.OutputDirPath = val)
				.WithDescription("Path to the mapping file for helping map official dlc list to tunings list")
				.SetDefault("OfficialEntriesToTunings.xml");

			p.Setup<bool>("checkdlc")
				.Callback(val => cmdLineArgs.CheckDlc = val)
				.WithDescription("compare sources to find missing items");

			p.Setup<string>("tunings")
				.Callback(val => cmdLineArgs.TuningSource = val)
				.WithDescription("RiffRepeater tunings list");

			p.SetupHelp("h", "help", "?")
			.Callback(text => Console.WriteLine(text));

			var result = p.Parse(args);

			if (result.HasErrors == true)
			{
				Console.WriteLine(result.ErrorText);
				return null;
			}
			if (result.HelpCalled)
				return null;

			return cmdLineArgs;
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
