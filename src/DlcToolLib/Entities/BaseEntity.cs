using System;

namespace DlcToolLib.Entities
{
	public class BaseEntity : IBaseEntity
	{
		public DateTime CreateDateTime { get; set; }
		public DateTime UpdatedDateTime { get; set; }
	}
}