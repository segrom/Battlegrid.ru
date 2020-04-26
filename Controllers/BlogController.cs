using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            using (var db = new BlogModels())
            {
                allPosts.AllPosts = db.Posts.Include(p=>p.Comments).OrderByDescending(p => p.CreationTime).Take(100).ToArray();
                var allAuthorsId = allPosts.AllPosts.Select(p => p.Author);
                using (var idb = new IdentityDbContext())
                {
                    var allIdentityUsers = from user in idb.Users
                        where allAuthorsId.Contains(user.Id)
                        select user;
                    foreach (var post in allPosts.AllPosts)
                    {
                        post.Views++;
                    }

                    allPosts.AllAuthors = allIdentityUsers.ToArray();
                    db.SaveChanges();
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
                        Label = model.Label, Text = model.Text, ImageUrl = model.ImageUrl,
                        CreationTime = DateTime.Now, Author = User.Identity.GetUserId(),
                        Likes = 0, Views = 0
                    };
                    db.Posts.Add(new_post);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", "Blog");
            }

            ModelState.AddModelError("", "Что то не правильно");
            return View(model);
        }

        [Authorize]
        public ActionResult WriteComment(int postId)
        {
            Post post;
            using (var db = new BlogModels())
            {
                post = db.Posts.Include(p=>p.Comments).First(p => p.Id == postId);
            }

            var model = new WriteCommentModel();
            model.Post = post;
            using (var idb = new IdentityDbContext())
            {
                var authors = post.Comments.Select(p => p.Author).ToList();
                authors.Add(post.Author);
                List<IdentityUser> allAuth = new List<IdentityUser>();
                foreach (string author in authors)
                {

                    var autor = idb.Users.FirstOrDefault(u => u.Id == author);
                    if (autor != null) allAuth.Add(autor);
                }

                model.AllAuthors = allAuth.ToArray();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult WriteComment(WriteCommentModel model, int postId)
        {

            using (var db = new BlogModels())
            {
                Post post;
                post = db.Posts.Include(p=>p.Comments).Single(p => p.Id == postId);

                model.Post = post;
                using (var idb = new IdentityDbContext())
                {
                    var authors = post.Comments.Select(p => p.Author).ToList();
                    authors.Add(post.Author);
                    List<IdentityUser> allAuth = new List<IdentityUser>();
                    foreach (string author in authors)
                    {

                        var autor = idb.Users.FirstOrDefault(u => u.Id == author);
                        if (autor != null) allAuth.Add(autor);
                    }

                    model.AllAuthors = allAuth.ToArray();
                }

                if (ModelState.IsValid)
                {

                    Comment newC = new Comment()
                    {
                        Author = User.Identity.GetUserId(),
                        Body = model.Body,
                        Likes = 0,
                        Views = 0,
                        CreationTime = DateTime.Now,
                    };
                    db.Entry(post).Entity.Comments.Add(newC);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Blog");
                }

                ModelState.AddModelError("", "Заполните все поля!");
                return View(model);
            }
        }
    }
}