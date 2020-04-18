using System;
using System.EnterpriseServices;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Battlegrid.ru.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Battlegrid.ru.Controllers
{
    public class BlogController : Controller
    {
        // GET
        public ActionResult Index()
        {
            AllPostsModel allPosts = new AllPostsModel();
            using (var db =new BlogModels())
            {
                allPosts.AllPosts = db.Posts.OrderByDescending(p => p.CreationTime).Take(100).ToArray();
                var allAuthorsId = allPosts.AllPosts.Select(p => p.Author);
                using (var idb = new IdentityDbContext())
                {
                    var allIdentityUsers = from user in idb.Users
                        where allAuthorsId.Contains(user.Id)
                        select user;
                    allPosts.AllPosts.ForEach(u => u.Views+=1);
                    allPosts.AllAuthors = allIdentityUsers.ToArray();
                }
            }
            return View(allPosts);
        }
        [Authorize]
        public ActionResult NewPost()
        {
            if (!User.IsInRole("admin")) return RedirectToAction("Index");
            var model = new NewPostModel();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult NewPost(NewPostModel model)
        {
            if (!User.IsInRole("admin")) return RedirectToAction("Index");
            if (ModelState.IsValid)
            {
                using (var db = new BlogModels())
                {
                    Post new_post = new Post()
                    {
                        Label = model.Label, Text =  model.Text, ImageUrl = model.ImageUrl,
                        CreationTime = DateTime.Now, Author = User.Identity.GetUserId(),
                        Likes = 0, Views = 0
                    };
                    db.Posts.Add(new_post);
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Blog");
            }
            ModelState.AddModelError("","Что то не правильно");
            return View(model);
        }
    }
}