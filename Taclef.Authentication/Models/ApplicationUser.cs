using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Taclef.Authentication.Models
{
	public class ApplicationUser : UuidEntity
	{
		[StringLength(256)]
		[DataType(DataType.EmailAddress)]
		public virtual string EmailAddress { get; set; }

		[StringLength(50)]
		public virtual string FirstName { get; set; }

		[StringLength(50)]
		public virtual string LastName { get; set; }

		public virtual ICollection<ApplicationRole> Roles { get; set; }

		[Range(1, 999999999)]
		public virtual int? EmployeeNumber { get; set; }

// ReSharper disable once InconsistentNaming
		public string MEN
		{
			get { return EmployeeNumber == null ? null : EmployeeNumber.Value.ToString("D9"); }
		}

		public virtual ICollection<UserLogin> Logins { get; set; }

		public virtual bool IsLocked { get; set; }

		public virtual bool IsDeleted { get; set; }

		public virtual School School { get; set; }

		public virtual SchoolBoard Board { get; set; }
	}
}