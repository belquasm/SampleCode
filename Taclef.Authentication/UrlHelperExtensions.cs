using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace Taclef.Authentication
{
	public static class UrlHelperExtensions
	{
		public static string Api(this UrlHelper url, string controller)
		{
			return url.RouteUrl("DefaultApi", new {httproute = "", controller});
		}

		public static string Api(this UrlHelper url, string controller, object id)
		{
			return url.RouteUrl("DefaultApi", new { httproute = "", controller, id });
		}

	}
}