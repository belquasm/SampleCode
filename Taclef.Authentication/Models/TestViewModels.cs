using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Taclef.Authentication.Models
{
	public class D2LLoginTestViewModel
	{
		public List<BoardInfo> Boards { get; set; }
		public List<SchoolInfo> Schools { get; set; }
		public List<RoleInfo> Roles { get; set; }

		public List<string> SelectedRole { get; set; }
		public string SelectedSchool { get; set; }
		public string SelectedBoard { get; set; }
		public string Login { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
	}
}