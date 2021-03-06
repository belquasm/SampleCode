﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Taclef.Authentication.Controllers;
using Taclef.Authentication.Models;

namespace Taclef.Authentication
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}
        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    var httpContext = ((WebApiApplication)sender).Context;

        //    var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
        //    var currentController = " ";
        //    var currentAction = " ";

        //    if (currentRouteData != null)
        //    {
        //        if (currentRouteData.Values["controller"] != null && !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
        //        {
        //            currentController = currentRouteData.Values["controller"].ToString();
        //        }

        //        if (currentRouteData.Values["action"] != null && !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
        //        {
        //            currentAction = currentRouteData.Values["action"].ToString();
        //        }
        //    }

        //    var ex = Server.GetLastError();

        //    ApplicationLogger.LogError(ex);

        //    var controller = new ErrorController();
        //    var routeData = new RouteData();
        //    var action = "Index";

        //    //if (ex is HttpException)
        //    //{
        //    //    var httpEx = ex as HttpException;

        //    //    switch (httpEx.GetHttpCode())
        //    //    {
        //    //        case 404:
        //    //            action = "NotFound";
        //    //            break;

        //    //        // others if any

        //    //        default:
        //    //            action = "Index";
        //    //            break;
        //    //    }
        //    //}

        //    httpContext.ClearError();
        //    httpContext.Response.Clear();
        //    httpContext.Response.StatusCode = ex is HttpException ? ((HttpException)ex).GetHttpCode() : 500;
        //    httpContext.Response.TrySkipIisCustomErrors = true;
        //    routeData.Values["controller"] = "Error";
        //    routeData.Values["action"] = action;

        //    controller.ViewData.Model = new HandleErrorInfo(ex, currentController, currentAction);
        //    ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        //}
	}
}
