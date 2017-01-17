using System.Collections.Specialized;
using System.Web;
using Taclef.Authentication.Models;

namespace Taclef.Authentication.LoginProviders
{
	public abstract class LoginProviderBase
	{
		public abstract void ProcessLoginRequest(NameValueCollection form, NameValueCollection query, AuthRequestModel model);

		public virtual bool TryProcessLoginRequest(NameValueCollection form, NameValueCollection query, AuthRequestModel model)
		{
			try
			{
				ProcessLoginRequest(form, query, model);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}