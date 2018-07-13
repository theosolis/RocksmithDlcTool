using LiteDB;

namespace DlcToolLib.Entities
{
	public class LinksBetweenDlcSources : BaseEntity
	{
		[BsonRef(OfficialDlcItem.TableName)]
		public OfficialDlcItem OfficialDlcItem { get; set; }

		[BsonRef(ExistingDlcItem.TableName)]
		public ExistingDlcItem ExistingDlcItem { get; set; }

		[BsonRef(DlcTuningItem.TableName)]
		public DlcTuningItem DlcTuningItem { get; set; }
	}

	
}