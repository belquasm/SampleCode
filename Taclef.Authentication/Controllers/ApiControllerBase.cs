using System.Web.Http;
using Taclef.Authentication.Models;

namespace Taclef.Authentication.Controllers
{
	public abstract class ApiControllerBase : ApiController
	{
		private ApplicationDbContext _db;

		public ApplicationDbContext Db { get { return _db ?? (_db = ApplicationDbContext.Create()); } }


	}
}