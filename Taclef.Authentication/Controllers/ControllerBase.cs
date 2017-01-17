using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using Taclef.Authentication.Models;

namespace Taclef.Authentication.Controllers
{
	public abstract class ControllerBase : Controller
	{
		private ApplicationDbContext _db;

		public ApplicationDbContext Db { get { return _db ?? (_db = ApplicationDbContext.Create()); } }


		public string SerializeToJson(object obj)
		{
			var serializer = JsonSerializer.Create();
			using (var sw = new StringWriter())
			{
				serializer.Serialize(sw, obj);
				return sw.ToString();
			}
		}
	}
}