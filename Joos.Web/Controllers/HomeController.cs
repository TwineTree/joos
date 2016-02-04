﻿using System.Web.Mvc;
using Abp.Web.Mvc.Authorization;

namespace Joos.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : JoosControllerBase
    {
        public ActionResult Index()
        {
            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
	}
}