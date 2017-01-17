using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Taclef.Authentication.Models
{
	public class School : UuidEntity
	{
		public virtual int MinistryUniqueId { get; set; }

		public virtual string ShortName { get; set; }

		public virtual string Name { get; set; }

		[Required]
		public virtual SchoolBoard Board { get; set; }

		public virtual ICollection<ApplicationUser> Users { get; set; }
	}
}