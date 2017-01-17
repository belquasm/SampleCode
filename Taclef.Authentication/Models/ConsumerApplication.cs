using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taclef.Authentication.Models
{
	public class ConsumerApplication : UuidEntity
	{
		[StringLength(50)]
		[Required(AllowEmptyStrings = false)]
		[Index(IsUnique = true)]
		public virtual string Name { get; set; }

		[StringLength(100)]
		public virtual string DisplayName { get; set; }

		public virtual string CustomSettings { get; set; }

		public virtual string SecretKey { get; set; }

		[StringLength(256)]
		public virtual string WebsiteUrl { get; set; }

		[Required(AllowEmptyStrings = false)]
		[StringLength(256)]
		public virtual string SuccessUrl { get; set; }

		[Required(AllowEmptyStrings = false)]
		[StringLength(256)]
		public virtual string FailedUrl { get; set; }

	}
}