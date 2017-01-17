using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Taclef.Authentication.Models;
using Taclef.Authentication.Models.D2LLTI;

namespace Taclef.Authentication.Areas.D2LAdmin.Controllers
{
    public abstract class ControllerBase : Authentication.Controllers.ControllerBase
    {
        //private static string[] _displayRoles = new[] { "BoardAdmin", "SchoolAdmin", "Teacher" };

	    public List<RoleInfo> GetRoles()
	    {

		    return
			    Db.ApplicationRoles.Where(r => r.IsStandardRole)
				    .Select(r => new RoleInfo {DisplayName = r.DisplayName, Id = r.Id, Name = r.Name})
				    .ToList();
	    }
        
    }
}