using Fclp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DlcToolLib;
using DlcToolLib.Entities;
using DlcToolLib.Model;


namespace DlcDatabase
{
	class Program
	{
		static void Main(string[] args)
		{
			var programArgs = ParseArguments(args);
			if (programArgs == null) return;

			var dlcFuncs = new DlcFunctions();
			//dlcFuncs.AttemptLinking();
			//return;
			var loadPolicy = new DlcLoadPolicy
			{
				AddNewItems = programArgs.AddNewRows,
				ClearExistingBeforeLoad = programArgs.ReplaceExistingItems,
				UpdateExisting = programArgs.UpdateExisting
			};

			dlcFuncs.LoadSourceToStore(programArgs.PathToDatabase, programArgs.DlcSourcePath, programArgs.SourceType, loadPolicy);
		}

		static ProgramArgs ParseArguments(string[] args)
		{
			var p = new FluentCommandLineParser();
			var programArgs = new ProgramArgs();

			p.Setup<string>('s', "source")
			.Callback(sourcePath => programArgs.DlcSourcePath = sourcePath)
			.WithDescription("Source path for DLC (url or file path)")
			.Required();

			p.Setup<DlcSourceType>('t', "type")
			.Callback(sourceType => programArgs.SourceType = sourceType)
			.WithDescription("Type of DLC source - Official, DlcTuning, Existing, or Steam")
			.Required();

			p.Setup<string>('d', "database")
			.Callback(dbPath => programArgs.PathToDatabase = dbPath)
			.WithDescription("Database file for storing/loading DLC items")
			.Required();

			p.Setup<bool>('r', "reset")
				.Callback(val => programArgs.ReplaceExistingItems = val)
				.WithDescription("Reset the dlc inside the database ie delete existing when reloading (WARNING!)");

			p.Setup<bool>('n', "addnew")
				.Callback(val => programArgs.AddNewRows = val)
				.WithDescription("Add new rows not currently in the db (automatically true if --reset is used)")
				.SetDefault(true);

			p.Setup<bool>('u', "update")
				.Callback(val => programArgs.UpdateExisting = val)
				.WithDescription("Update existing rows (if unique key matches)")
				.SetDefault(false);

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

			return programArgs;
		}
	}
}
