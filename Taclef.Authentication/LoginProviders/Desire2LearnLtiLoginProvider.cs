using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Taclef.Authentication.Models;

namespace Taclef.Authentication.LoginProviders
{
	public class Desire2LearnLtiLoginProvider : OAuthLoginProvider
	{
		public override void ProcessLoginRequest(NameValueCollection form, NameValueCollection query, AuthRequestModel model)
		{
			if (form == null) throw new ArgumentNullException("form");
			if (model == null) throw new ArgumentNullException("model");
			var data = new Desire2LearnLtiData(form);

			model.User = new UserInfo
			{
				FirstName = data.FirstName,
				LastName = data.LastName,
				Email = data.Email
			};
			model.ProviderCredentials = CreateCredentials(data);
			model.ProviderDerivedKey = CreateDerivedKey(data);
			//TODO: verify oauth signature
		}

		private static string CreateDerivedKey(Desire2LearnLtiData data)
		{
			if (data == null) throw new ArgumentNullException("data");
			if (string.IsNullOrWhiteSpace(data.ProfileUrl)) throw new ApplicationException("Missing or invalid parameters for key derivation");
			return CreateDerivedKey(data.ProfileUrl);
		}

		private static string CreateCredentials(Desire2LearnLtiData data)
		{
			if (data == null) throw new ArgumentNullException("data");
			if (string.IsNullOrWhiteSpace(data.UserName)) throw new ApplicationException("Missing or invalid parameters for credentials");
			return CreateCredentials(data.UserName);
		}

		public static string CreateDerivedKey(string profileUrl)
		{
			if (profileUrl == null) throw new ArgumentNullException("profileUrl");
			var key = new Desire2LearnDerivedKey { TcProfileUrl = profileUrl.Trim().ToLowerInvariant() };
			// TODO: determine if we need some other info to include in derived key to uniquely identify the source of the request
			return Serializer.SerializeToBase64(key);
		}

		public static string CreateCredentials(string username)
		{
			if (username == null) throw new ArgumentNullException("username");
			var credentials = new Desire2LearnCredentials { UserId = username.Trim().ToLowerInvariant() };
			// TODO: determine if we need some other info to include in credentials to uniquely identify the user
			return Serializer.SerializeToBase64(credentials);
		}

		public static Desire2LearnCredentials ReadCredentials(string credentials)
		{
			if (credentials == null) return null;
			return Serializer.DeserializeBase64<Desire2LearnCredentials>(credentials);
		}
	}

	
}