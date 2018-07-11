using System;

namespace DlcToolLib.Entities
{
	public interface IBaseEntity
	{
		DateTime CreateDateTime { get; set; }
		DateTime UpdatedDateTime { get; set; }
	}
}