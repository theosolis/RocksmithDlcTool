using Fclp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DlcToolLib;
using DlcToolLib.Entities;


namespace DlcDatabase
{
	class Program
	{
		static void Main(string[] args)
		{
			var programArgs = ParseArguments(args);
			if (programArgs == null) return;

			var dlcFuncs = new DlcFunctions();
			dlcFuncs.LoadSourceToStore(programArgs.PathToDatabase, programArgs.DlcSourcePath, programArgs.SourceType, programArgs.ReplaceExistingItems);
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
			.WithDescription("Type of DLC source - Official, RiffRepeater, Existing, or Steam")
			.Required();

			p.Setup<string>('d', "database")
			.Callback(dbPath => programArgs.PathToDatabase = dbPath)
			.WithDescription("Database file for storing/loading DLC items")
			.Required();

			p.Setup<bool>('r', "reset")
				.Callback(val => programArgs.ReplaceExistingItems = val)
				.WithDescription("Reset the dlc inside the database ie delete existing when reloading (WARNING!)");

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
