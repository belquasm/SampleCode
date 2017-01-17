using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Web;
using WebGrease.Css.Extensions;

namespace Taclef.Authentication.Models
{
	public class AuthResponseModel
	{
		public string Message { get; set; }
		public string Signature { get; set; }
		public string Error { get; set; }
		public string Url { get; set; }

		public IDictionary<string, string> LtiData { get; set; }
	}

	public class AuthRequestModel
	{
		public UserInfo User { get; set; }

		public string ProviderCredentials { get; set; }

		public string ProviderDerivedKey { get; set; }
	}

	public class AuthResponseMessage
	{
		public UserInfo User { get; set; }

		public List<RoleInfo> Roles { get; set; }

		public SchoolInfo School { get; set; }

		public BoardInfo Board { get; set; }

		public static AuthResponseMessage CreateFor(ApplicationUser user)
		{
			if (user == null) throw new ArgumentNullException("user");
			var model = new AuthResponseMessage
			{
				User = new UserInfo
				{
					Email = user.EmailAddress,
					FirstName = user.FirstName,
					LastName = user.LastName,
					Id = user.Id
				},
				Roles = user.Roles.Select(r => new RoleInfo{ DisplayName = r.DisplayName, Id = r.Id, Name = r.Name}).ToList()
			};

			if (user.School != null)
			{
				model.School = new SchoolInfo {Id = user.School.Id, Name = user.School.Name, ShortName = user.School.ShortName};
			}
			if (user.Board != null)
			{
				model.Board = new BoardInfo { Id = user.Board.Id, Name = user.Board.Name, ShortName = user.Board.ShortName };
			}
			return model;
		}
	}

	public class UserInfo
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string Id { get; set; }
	}

	public class SchoolInfo
	{
		public string ShortName { get; set; }

		public string Name { get; set; }

		public string Id { get; set; }
	}

	public class BoardInfo
	{
		public string ShortName { get; set; }

		public string Name { get; set; }

		public string Id { get; set; }
	}

	public class RoleInfo
	{
		public string Name { get; set; }

		public string DisplayName { get; set; }

		public string Id { get; set; }
	}
}