using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Battlegrid.ru.Models;
using BGS.Models;
using Microsoft.AspNet.Identity;

namespace Battlegrid.ru.Controllers
{
    public class InventoryController : Controller
    {
        // GET
        [Authorize]
        public ActionResult Index()
        {
            var model = new AllInventoryModel();
            string userId = User.Identity.GetUserId();
            using (var db = new BGS_DBContext())
            {
                User user = db.Users
                    .Include(u=>u.Units.Select(a=>a.FirstWeapon))
                    .Include(u=>u.Units.Select(a=>a.Specialization.Skills))
                    .Include(u => u.WeaponModels.Select(a=>a.AimModification))
                    .Include(u => u.WeaponModels.Select(a=>a.BarrelModification))
                    .Include(u => u.WeaponModels.Select(a=>a.ButtModification))
                    .Include(u => u.WeaponModels.Select(a=>a.MagazineModification))
                    .Include(u => u.ArmorModels)
                    .Include(u => u.AccessoryModels)
                    .Include(u => u.StorageModels)
                    .Single(u => u.Id == userId);
                model.ArmorModels = user.ArmorModels.Where(u=>u.LotId==null).ToArray();
                model.UnitModels = user.Units.Where(u => u.LotId == null).ToArray();
                model.WeaponModels = user.WeaponModels.Where(u => u.LotId == null).ToArray();
                model.AccessoryModels = user.AccessoryModels.Where(u => u.LotId == null).ToArray();
                model.StorageModels = user.StorageModels.Where(u => u.LotId == null).ToArray();
            }
            return View(model);
        }
    }
}