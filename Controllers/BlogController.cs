using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Battlegrid.ru.Models;
using Microsoft.Ajax.Utilities;
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
                allPosts.AllPosts = db.Posts.OrderBy(p => p.CreationTime).Take(100).ToArray();
                var allAuthorsId = allPosts.AllPosts.Select(p => p.Author);
                using (var idb = new IdentityDbContext())
                {
                    var allIdentityUsers = from user in idb.Users
                        where allAuthorsId.Contains(user.Id)
                        select user;
                    allPosts.AllPosts.ForEach(u => u.Views++);
                    allPosts.AllAuthors = allIdentityUsers.ToArray();
                }
            }
            return View(allPosts);
        }
    }
}