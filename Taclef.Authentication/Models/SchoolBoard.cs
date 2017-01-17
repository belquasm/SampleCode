using System.Collections.Generic;

namespace Taclef.Authentication.Models
{
	public class SchoolBoard : UuidEntity
	{

		public virtual string ShortName { get; set; }

		public virtual string Name { get; set; }
		
		public virtual ICollection<School> Schools { get; set; }

		public virtual ICollection<ApplicationUser> Users { get; set; }

	}
}