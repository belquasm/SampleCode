using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Taclef.Authentication.Areas.Admin.Models;
using ControllerBase = Taclef.Authentication.Controllers.ControllerBase;

namespace Taclef.Authentication.Areas.Admin.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
	        var model = new HomeIndexViewModel();
	        model.Consumers =
		        Db.ConsumerApplications.OrderBy(a => a.DisplayName).ToList().Select(AppInfo.FromEntity).ToList();
				

            return View(model);
        }

        public ActionResult Edit(string id)
        {
            throw new NotImplementedException();
        }
    }
}