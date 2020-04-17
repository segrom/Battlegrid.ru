using System.Linq;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Battlegrid.ru.Models
{
    public class HomeIndexViewModel
    {
        public Post[] AllPosts { get; set; }
    }
}