using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taclef.Authentication.Models
{
	public abstract class UuidEntity
	{
        
		[Key]
		[Required(AllowEmptyStrings = false)]
		[StringLength(128, MinimumLength = 1)]
		public virtual string Id { get; set; }

		[NotMapped]
		public Guid Uuid
		{
			get
			{
				Guid guid;
				return Guid.TryParse(Id, out guid) ? guid : Guid.Empty;
			}
			set { Id = value.ToString(); }
		}
	}
}