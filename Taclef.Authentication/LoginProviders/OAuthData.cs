using System;
using System.Collections.Specialized;

namespace Taclef.Authentication.LoginProviders
{
	public class OAuthData
	{
		private readonly NameValueCollection _form;

		public OAuthData(NameValueCollection form)
		{
			if (form == null) throw new ArgumentNullException("form");
			_form = form;
		}

		protected string GetValue(string key)
		{
			return _form[key];
		}
	}
}