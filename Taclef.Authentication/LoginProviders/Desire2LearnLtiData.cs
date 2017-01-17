using System.Collections.Specialized;

namespace Taclef.Authentication.LoginProviders
{
	public class Desire2LearnLtiData : LtiData
	{
		public Desire2LearnLtiData(NameValueCollection form) : base(form)
		{
		}

		public string UserName { get { return GetValue("ext_d2l_username"); } }
		public string OrgDefinedId { get { return GetValue("ext_d2l_orgdefinedid"); } }
		public string D2LRole { get { return GetValue("ext_d2l_role"); } }
		public string ProfileUrl { get { return GetValue("ext_tc_profile_url"); } }
		public string D2LTokenDigest { get { return GetValue("ext_d2l_token_digest"); } }
	}
}