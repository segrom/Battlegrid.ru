using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Battlegrid.ru.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Battlegrid.ru.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            HomeIndexViewModel allPosts = new HomeIndexViewModel();
            using (var db = new BlogModels()) {
                allPosts.AllPosts = db.Posts.OrderByDescending(p => p.CreationTime).Take(4).ToArray();
                allPosts.AllPosts.ForEach(p=>p.Views++);
            }
            return View(allPosts);
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}