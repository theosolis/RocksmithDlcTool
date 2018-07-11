using System;
using System.Collections.Generic;
using System.Linq;
using DlcToolLib.Entities;
using DlcToolLib.Model;
using LiteDB;
using RocksmithToolkitLib.Sng2014HSL;

namespace DlcToolLib.Loading
{
	public class GenericDlcLoader<T> : IDlcLoader<T>
		where T: IDlc
	{
		private readonly string _tableName;
		private readonly Func<T, T, bool> _funcUpdateRow;

		public GenericDlcLoader(string tableName, Func<T, T, bool> funcUpdateRow)
		{
			_tableName = tableName;
			_funcUpdateRow = funcUpdateRow;
		}

		public List<string> LoadDlcList(LiteDatabase db, IFindDlcResult<T> dlcList, DlcLoadPolicy loadPolicy)
		{
			var dbDlc = db.GetCollection<T>(_tableName);

			//this might get trickier once we have mapping tables in place
			if (loadPolicy.ClearExistingBeforeLoad)
			{
				dbDlc.Delete(x => true);
				dbDlc.InsertBulk(dlcList.DlcList);
			}
			else
			{
				var inDb = dbDlc.FindAll();
				var existingByUniqueKey = inDb.ToDictionary(x => x.UniqueKey);

				var dlcThatMatchesExisting = dlcList.DlcList.Where(x => existingByUniqueKey.ContainsKey(x.UniqueKey));
				var newRows = dlcList.DlcList.Where(x => !existingByUniqueKey.ContainsKey(x.UniqueKey));

				if (loadPolicy.AddNewItems)
				{
					dbDlc.InsertBulk(newRows);
				}

				if (loadPolicy.UpdateExisting)
				{
					foreach (var dlcMatch in dlcThatMatchesExisting)
					{
						var existingItem = existingByUniqueKey[dlcMatch.UniqueKey];
						if (_funcUpdateRow(existingItem, dlcMatch))
							dbDlc.Update(existingItem);
					}
				}
			}
			return new List<string>();
		}
	}
}