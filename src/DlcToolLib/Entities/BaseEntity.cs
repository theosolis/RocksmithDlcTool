using System;

namespace DlcToolLib.Entities
{
	public class BaseEntity : IBaseEntity
	{
		public int Id { get; set; }
		public DateTime CreateDateTime { get; set; }
		public DateTime UpdatedDateTime { get; set; }
	}
}