using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taclef.Authentication.Models
{
	public class ApplicationRole : UuidEntity
	{
		[StringLength(50)]
		[Required(AllowEmptyStrings = false)]
		[Index(IsUnique = true)]
		public virtual string Name { get; set; }

		[StringLength(100)]
		public virtual string DisplayName { get; set; }

        [Required]
        public virtual bool IsStandardRole { get; set; }

		public virtual ICollection<ApplicationUser> Users { get; set; } 
	}
}