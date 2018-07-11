using System;
using System.Collections.Generic;
using System.Linq;
using DlcToolLib.Entities;
using DlcToolLib.Model;
using LiteDB;

namespace DlcToolLib.Loading
{
	public class GenericDlcLoader<T> : IDlcLoader<T>
		where T: IDlc, IBaseEntity
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
			var rv = new List<string>();

			var uniqueListToLoad = new List<T>();
			foreach (var item in dlcList.DlcList.GroupBy(x => x.UniqueKey))
			{
				if (item.Count() > 1)
					rv.Add("Warning - duplicate key found " + item.Key);
				uniqueListToLoad.Add(item.First());
			}

			rv.AddRange(LoadDlcListAfterFiltering(dbDlc, uniqueListToLoad, loadPolicy));

			return rv;
		}

		private List<string> LoadDlcListAfterFiltering(LiteCollection<T> dbDlc, List<T> toLoad, DlcLoadPolicy loadPolicy)
		{
			var timeStamp = DateTime.Now;

			//this might get trickier once we have mapping tables in place
			if (loadPolicy.ClearExistingBeforeLoad)
			{
				dbDlc.Delete(x => true);
				
				foreach (var item in toLoad.OfType<BaseEntity>())
				{
					item.CreateDateTime = timeStamp;
					item.UpdatedDateTime = timeStamp;
				}

				dbDlc.InsertBulk(toLoad);
			}
			else
			{
				var inDb = dbDlc.FindAll();
				var existingByUniqueKey = inDb.ToDictionary(x => x.UniqueKey);

				var dlcThatMatchesExisting = toLoad.Where(x => existingByUniqueKey.ContainsKey(x.UniqueKey));
				var newRows = toLoad.Where(x => !existingByUniqueKey.ContainsKey(x.UniqueKey)).ToList();

				if (loadPolicy.AddNewItems)
				{
					foreach (var item in newRows.OfType<BaseEntity>())
					{
						item.CreateDateTime = timeStamp;
						item.UpdatedDateTime = timeStamp;
					}

					dbDlc.InsertBulk(newRows);
				}

				if (loadPolicy.UpdateExisting)
				{
					foreach (var dlcMatch in dlcThatMatchesExisting)
					{
						var existingItem = existingByUniqueKey[dlcMatch.UniqueKey];
						if (_funcUpdateRow(existingItem, dlcMatch))
						{
							var baseEntity = existingItem as BaseEntity;
							if (baseEntity != null)
								baseEntity.UpdatedDateTime = timeStamp;

							dbDlc.Update(existingItem);
						}
							
					}
				}
			}
			return new List<string>();
		}
	}
}