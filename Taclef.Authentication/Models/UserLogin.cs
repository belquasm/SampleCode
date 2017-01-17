using System.ComponentModel.DataAnnotations;

namespace Taclef.Authentication.Models
{
	public class UserLogin : UuidEntity
	{
		[Required]
		public virtual ApplicationUser User { get; set; }

		[Required]
		public virtual LoginProvider Provider { get; set; }

		[Required(AllowEmptyStrings = false)]
		public virtual string Credentials { get; set; }
	}
}