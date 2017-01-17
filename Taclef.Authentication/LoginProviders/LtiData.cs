using System.Collections.Specialized;

namespace Taclef.Authentication.LoginProviders
{
	public class LtiData : OAuthData
	{
		public LtiData(NameValueCollection form) : base(form)
		{
		}

		public string ContextId { get { return GetValue("context_id"); } }
		public string UserId { get { return GetValue("user_id"); } }
		public string Email { get { return GetValue("lis_person_contact_email_primary"); } }
		public string FirstName { get { return GetValue("lis_person_name_given"); } }
		public string LastName { get { return GetValue("lis_person_name_family"); } }
		public string Role { get { return GetValue("roles"); } }
	}
}