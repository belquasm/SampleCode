using System;
using System.Linq;
using System.Web.Mvc;
using Taclef.Authentication.LoginProviders;
using Taclef.Authentication.Models;

namespace Taclef.Authentication.Controllers
{
	public class D2LController : ControllerBase
	{
		public ApplicationUser GetOrCreateUser(AuthRequestModel authRequest)
		{
			var provider =
				Db.LoginProviders.FirstOrDefault(p => p.DerivedKey == authRequest.ProviderDerivedKey && p.ProviderType == "D2L-LTI");
			if (provider == null) throw new ApplicationException("Could not find a Login Provider corresponding to the request: " + authRequest.ProviderDerivedKey);

			var login =
				provider.Logins.FirstOrDefault(p => p.Credentials == authRequest.ProviderCredentials);

			ApplicationUser user;

			if (login == null)
			{
				// determine if a user already exists for this login
				user = Db.ApplicationUsers.FirstOrDefault(u => u.EmailAddress == authRequest.User.Email);

				if (user == null)
				{
					// create a new user
					user = Db.CreateAdd<ApplicationUser>();
				}

				// create new login for user
				login = Db.CreateAdd<UserLogin>(u =>
				{
					u.User = user;
					u.Provider = provider;
					u.Credentials = authRequest.ProviderCredentials;
				});
			}
			else
			{
				user = login.User;
			}

			if (user.IsDeleted) throw new ApplicationException("The user associated with these credentials has been deleted");
			if (user.IsLocked) throw new ApplicationException("The user associated with these credentials has been locked out");

			// update user details
			user.FirstName = authRequest.User.FirstName;
			user.LastName = authRequest.User.LastName;
			user.EmailAddress = authRequest.User.Email;

			Db.SaveChanges();
			return user;
		}

		[HttpPost]
		public ActionResult Launch(string id, FormCollection form)
		{
			var model = new AuthResponseModel();
			try
			{
				if (string.IsNullOrWhiteSpace(id)) return View("Error");
				var app = Db.ConsumerApplications.FirstOrDefault(a => a.Name == id);
				if (app == null) return View("Error");
				model.Url = app.FailedUrl;

				var loginProvider = new Desire2LearnLtiLoginProvider();
				var authRequest = new AuthRequestModel();
				loginProvider.ProcessLoginRequest(form, Request.QueryString, authRequest);
				var user = GetOrCreateUser(authRequest);
				var response = AuthResponseMessage.CreateFor(user);
				model.Message = SerializeToJson(response);
				model.Signature = MessageSigner.SignMessage(model.Message, app.SecretKey);
				model.Url = app.SuccessUrl;
			}
			catch (Exception exception)
			{
				model.Error = exception.ToString();
				model.Signature = null;
				model.Message = exception.Message;
				model.LtiData = form.AllKeys.ToDictionary(k => k, k => form[k]);
			}

			return View("AuthResponse", model);
		}
	}
}