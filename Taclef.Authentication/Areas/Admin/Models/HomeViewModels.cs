using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using Taclef.Authentication.Models;

namespace Taclef.Authentication.Areas.Admin.Models
{
	public class HomeIndexViewModel
	{
		public List<AppInfo> Consumers { get; set; } 
	}

	public class AppInfo
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public string Id { get; set; }
		public string SecretyKey { get; set; }
		public string WebsiteUrl { get; set; }
		public string SuccessUrl { get; set; }
		public string FailUrl { get; set; }

		public static AppInfo FromEntity(ConsumerApplication app)
		{
			var info = new AppInfo
			{
				Name = app.Name,
				DisplayName = app.DisplayName,
				Id = app.Id,
				SecretyKey = MessageSigner.GetPublicKey(app.SecretKey),
				WebsiteUrl = app.WebsiteUrl,
				SuccessUrl = app.SuccessUrl,
				FailUrl = app.FailedUrl
			};
			return info;
		}

	}
}