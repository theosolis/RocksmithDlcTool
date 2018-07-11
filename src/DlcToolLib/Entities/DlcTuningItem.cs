namespace DlcToolLib.Entities
{
	public class DlcTuningItem : IDlc
	{
		public int Id { get; set; }
		public string Song { get; set; }
		public string Artist { get; set; }

		public string LeadTuning { get; set; }
		public string RhythmTuning { get; set; }
		public string BassTuning { get; set; }
	}
}