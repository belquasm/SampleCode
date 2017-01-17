using System;
using System.Collections.Generic;
using System.Linq;
using Taclef.Authentication.LoginProviders;

namespace Taclef.Authentication.Models.D2LLTI
{
	public static class D2LExtensions
	{
		public static ApplicationUser FindD2LUser(this LoginProvider provider, string username)
		{
			if (provider == null) throw new ArgumentNullException("provider");
			if (username == null) throw new ArgumentNullException("username");

			var credentials = Desire2LearnLtiLoginProvider.CreateCredentials(username);
			return provider.Logins.Where(u => u.Credentials == credentials).Select(u => u.User).FirstOrDefault();
		}

		public static ApplicationUser FindUser(this ApplicationDbContext context, string loginProviderName, string username)
		{
			if (context == null) throw new ArgumentNullException("context");
			if (loginProviderName == null) throw new ArgumentNullException("loginProviderName");
			if (username == null) throw new ArgumentNullException("username");

			var credentials = Desire2LearnLtiLoginProvider.CreateCredentials(username);

			var query =
				from provider in context.LoginProviders
				where provider.Name == loginProviderName
				from login in provider.Logins
				where login.Credentials == credentials
				select login.User;

			return query.FirstOrDefault();
		}

		public static ApplicationUser CreateD2LUser(this ApplicationDbContext context, string loginProviderName, string username, int? employeeNumber = null)
		{
			if (context == null) throw new ArgumentNullException("context");
			if (loginProviderName == null) throw new ArgumentNullException("loginProviderName");
			if (username == null) throw new ArgumentNullException("username");

			var provider = context.LoginProviders.FirstOrDefault(p => p.Name == loginProviderName);
			if (provider == null) throw new ArgumentException("Could not find login provider");
			if (provider.ProviderType != "D2L-LTI") throw new ArgumentException("Login provider is not of type D2L-LTI");

			var credentials = Desire2LearnLtiLoginProvider.CreateCredentials(username);
			if (provider.Logins.Any(u => u.Credentials == credentials)) throw new ArgumentException("A login with that username already exists for the provider");

			var user = context.CreateAdd<ApplicationUser>(u =>
			{
				u.EmployeeNumber = employeeNumber;
			});

			var login = context.CreateAdd<UserLogin>(u =>
			{
				u.Credentials = credentials;
				u.User = user;
				u.Provider = provider;
			});

			return user;
		}

		public static IEnumerable<LoginProvider> GetD2LProviders(this ApplicationDbContext context)
		{
			if (context == null) throw new ArgumentNullException("context");
			return context.LoginProviders.Where(p => p.ProviderType == "D2L-LTI");
		}

		
	}
}