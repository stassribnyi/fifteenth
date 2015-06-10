using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lesson_ASP.NET_MVC.Models;

namespace Lesson_ASP.NET_MVC.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(MainModel model)
        {
            ViewBag.Result = model.concat;
            return View();
        }
    }
}