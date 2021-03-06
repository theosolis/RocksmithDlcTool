﻿namespace DlcToolLib.Entities
{
	public class DlcTuningItem : BaseEntity, IDlc
	{
		public const string TableName = "dlctuningitem";

		public string UniqueKey { get; set; }
		public string Artist { get; set; }
		public string ArtistSort { get; set; }
		public string Song { get; set; }
		public string SongSort { get; set; }

		public string LeadTuning { get; set; }
		public string RhythmTuning { get; set; }
		public string BassTuning { get; set; }
	}
}