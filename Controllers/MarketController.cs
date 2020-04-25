using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Battlegrid.ru.Models;
using Battlegrid.ru.Utils;
using BGS.Models;

namespace Battlegrid.ru.Controllers
{
    public class MarketController : Controller
    {
        // GET
        public ActionResult Index()
        {
            var model = new AllLots();
            List<UnitModel> unitModels = new List<UnitModel>();
            List<WeaponModel> weaponModels = new List<WeaponModel>();
            List<ArmorModel> armorModels = new List<ArmorModel>();
            List<AccessoryModel> accessoryModels = new List<AccessoryModel>();
            using (var db = new BGS_DBContext())
            {
                var last100lots = db.LotModels.Include(u=>u.Seller).OrderByDescending(u => u.Id).Take(100).ToArray();
                foreach (LotModel lot in last100lots)
                {
                    switch (lot.Type)
                    {
                        case LotType.Unit: unitModels.Add(db.UnitModels.Include(u=>u.Specialization.Skills).SingleOrDefault(u=>u.LotId==lot.Id));break;
                        case LotType.Weapon: weaponModels.Add(db.WeaponModels
                            .Include(u=>u.AimModification)
                            .Include(u => u.BarrelModification)
                            .Include(u => u.MagazineModification)
                            .Include(u => u.ButtModification)
                            .SingleOrDefault(u=>u.LotId==lot.Id));break;
                        case LotType.Armor: armorModels.Add(db.ArmorModels.SingleOrDefault(u=>u.LotId==lot.Id));break;
                        case LotType.Accessory: accessoryModels.Add(db.AccessoryModels.SingleOrDefault(u=>u.LotId==lot.Id));break;
                    }
                }
                model.LotUnits = unitModels.ToArray();
                model.LotArmors = armorModels.ToArray();
                model.LotAccessoryes = accessoryModels.ToArray();
                model.LotWeapons = weaponModels.ToArray();
                model.Last100Lots = last100lots;
            }
            return View(model);
        }

        //GET
        [Authorize(Roles = "admin")]
        public ActionResult NewUnitLot()
        {
            var model = new NewUnitLotModel();
            return View(model);
        }
        //POST
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult NewUnitLot(NewUnitLotModel model) {
            if (ModelState.IsValid)
            {
                using (var db = new BGS_DBContext())
                {
                    for (int i = 0; i < model.LotCount; i++)
                    {
                        UnitModel newUnitModel = GameUtilCreater.UnitModelFromModel(model);
                        db.UnitModels.Add(newUnitModel);
                    }

                    db.SaveChanges();
                }
            }
            ModelState.AddModelError("", "Что то не правильно");
            return View(model);
        }
    }
}