namespace DlcToolLib.Model
{
	public class DlcTuningItem : IDlc
	{
		public string Song { get; set; }
		public string Artist { get; set; }

		public string LeadTuning { get; set; }
		public string RhythmTuning { get; set; }
		public string BassTuning { get; set; }
	}
}