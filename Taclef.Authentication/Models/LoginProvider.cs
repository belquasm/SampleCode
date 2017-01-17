using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taclef.Authentication.Models
{
	public class LoginProvider : UuidEntity
	{
		[StringLength(50)]
		[Required(AllowEmptyStrings = false)]
		[Index(IsUnique = true)]
		public virtual string Name { get; set; }

		[StringLength(50)]
		[Required(AllowEmptyStrings = false)]
		public virtual string ProviderType { get; set; }

		[Required(AllowEmptyStrings = false)]
		public virtual string DerivedKey { get; set; }

		[StringLength(100)]
		public virtual string DisplayName { get; set; }

		[StringLength(256)]
		public virtual string WebsiteUrl { get; set; }

		public virtual ICollection<UserLogin> Logins { get; set; }
	}
}