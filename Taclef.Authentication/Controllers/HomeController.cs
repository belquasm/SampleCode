using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Taclef.Authentication.Models;

namespace Taclef.Authentication.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View(new D2LLoginTestViewModel());
		}
	}
}
