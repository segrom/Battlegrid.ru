using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Battlegrid.ru.Models;
using Battlegrid.ru.Utils;
using BGS.GameCore;
using BGS.Models;
using Microsoft.AspNet.Identity;

namespace Battlegrid.ru.Controllers
{
    public class MarketController : Controller
    {
        // GET
        [Authorize]
        public ActionResult Index(string error,string buy)
        {
            if (error != null) ViewBag.error = error;
            if (buy != null) ViewBag.buy = buy;
            var model = new AllLots();
            model.ModificationTypes = new Dictionary<int, ModificationType>();
            List<UnitModel> unitModels = new List<UnitModel>();
            List<WeaponModel> weaponModels = new List<WeaponModel>();
            List<ArmorModel> armorModels = new List<ArmorModel>();
            List<AccessoryModel> accessoryModels = new List<AccessoryModel>();
            List<AimModificationModel> aimModels = new List<AimModificationModel>();
            List<MagazineModificationModel> magazineModels = new List<MagazineModificationModel>();
            List<BarrelModificationModel> barrelModels = new List<BarrelModificationModel>();
            List<ButtModificationModel> buttModels = new List<ButtModificationModel>();
            using (var db = new BGS_DBContext())
            {
                var last100lots = db.LotModels.Where(l=>l.Status==LotStatus.Available).Include(u=>u.Seller).OrderByDescending(u => u.Id).Take(100).ToArray();
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
                        case LotType.Modification:
                            AimModificationModel aim = db.AimModificationModels.SingleOrDefault(u => u.LotId == lot.Id);
                            MagazineModificationModel magazine = db.MagazineModificationModels.SingleOrDefault(u => u.LotId == lot.Id);
                            BarrelModificationModel barrel = db.BarrelModificationModels.SingleOrDefault(u => u.LotId == lot.Id);
                            ButtModificationModel butt = db.ButtModificationModels.SingleOrDefault(u => u.LotId == lot.Id);
                            if(aim!=null) {aimModels.Add(aim);model.ModificationTypes.Add(lot.Id,ModificationType.Aim);}
                            if(magazine!=null) {magazineModels.Add(magazine);model.ModificationTypes.Add(lot.Id,ModificationType.Magazine);}
                            if(butt!=null) {buttModels.Add(butt);model.ModificationTypes.Add(lot.Id,ModificationType.Butt);}
                            if(barrel!=null) {barrelModels.Add(barrel);model.ModificationTypes.Add(lot.Id,ModificationType.Barrel);}
                            break;
                    }
                }
                model.LotUnits = unitModels.ToArray();
                model.LotArmors = armorModels.ToArray();
                model.LotAccessoryes = accessoryModels.ToArray();
                model.LotWeapons = weaponModels.ToArray();
                model.AimModificationModels = aimModels.ToArray();
                model.MagazineModificationModels = magazineModels.ToArray();
                model.ButtModificationModels = buttModels.ToArray();
                model.BarrelModificationModels = barrelModels.ToArray();
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
        //GET
        [Authorize]
        public ActionResult GetUserBalance()
        {
            using (var db = new BGS_DBContext())
            {
                var id = User.Identity.GetUserId();
                float balance = db.Users.Single(u => u.Id == id).AccountBalance;

                return new ContentResult()
                {
                    Content = $"<a href=\"#\" class=\"btn btn-link m-1 bg-light text-success\">{balance.ToString("C", CultureInfo.CurrentUICulture)}$4<a/>",
                    ContentType = "text/html"
                };
            }
        }
        //POST
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult NewUnitLot(NewUnitLotModel model) {
            if (ModelState.IsValid)
            {
                using (var db = new BGS_DBContext())
                {
                    string userIdentityId = User.Identity.GetUserId();
                    var seller = db.Users.Single(u => u.Id == userIdentityId);
                    for (int i = 0; i < model.LotCount; i++)
                    {
                        UnitModel newUnitModel = GameUtilCreater.UnitModelFromModel(model);
                        db.UnitModels.Add(newUnitModel);
                        db.SaveChanges();
                        var lot = new LotModel()
                        {
                            Seller = seller,
                            ItemId = newUnitModel.Id,
                            Price = model.Price,
                            SellerId = seller.GameId,
                            Status = LotStatus.Available,
                            Type = LotType.Unit
                        };
                        db.LotModels.Add(lot);
                        db.SaveChanges();
                        newUnitModel.LotId = lot.Id;
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", "Market");
                }
            }
            ModelState.AddModelError("", "Что то не правильно");
            return View(model);
        }
        [Authorize]
        public ActionResult BuyLot(int lotId)
        {
            using (var db = new BGS_DBContext())
            {
                var id = User.Identity.GetUserId();
                var lot = db.LotModels.Single(p => p.Id == lotId);
                User user = db.Users.Single(u => u.Id == id);
                if (user.AccountBalance - lot.Price > 0)
                {
                    switch (lot.Type)
                    {
                        default:throw new Exception("No this lot");
                        case LotType.Unit: 
                            UnitModel unitModel = db.UnitModels.Single(u => u.LotId == lotId);
                            unitModel.LotId = null;
                            unitModel.Owner = user;
                            break;
                        case LotType.Armor: 
                            ArmorModel armorModel = db.ArmorModels.Single(u => u.LotId == lotId);
                            armorModel.LotId = null;
                            armorModel.Owner = user;
                            break;
                        case LotType.Accessory: 
                            AccessoryModel accessoryModel = db.AccessoryModels.Single(u => u.LotId == lotId);
                            accessoryModel.LotId = null;
                            accessoryModel.Owner = user;
                            break;
                        case LotType.Weapon: 
                            WeaponModel weaponModel = db.WeaponModels.Single(u => u.LotId == lotId);
                            weaponModel.LotId = null;
                            weaponModel.Owner = user;
                            break;
                        case LotType.Storage:
                            StorageModel storageModel = db.StorageModels.Single(u => u.LotId == lotId);
                            storageModel.LotId = null;
                            storageModel.Owner = user;
                            break;
                        case LotType.Modification:
                            AimModificationModel aim = db.AimModificationModels.SingleOrDefault(a => a.LotId == lot.Id);
                            MagazineModificationModel magazine = db.MagazineModificationModels.SingleOrDefault(a => a.LotId == lot.Id);
                            BarrelModificationModel barrel = db.BarrelModificationModels.SingleOrDefault(a => a.LotId == lot.Id);
                            ButtModificationModel butt = db.ButtModificationModels.SingleOrDefault(a => a.LotId == lot.Id);
                            if(aim!=null){ aim.LotId = null; aim.Owner = user; }
                            if(magazine!=null){ magazine.LotId = null; magazine.Owner = user; }
                            if(barrel!=null){ barrel.LotId = null; barrel.Owner = user; }
                            if(butt!=null){ butt.LotId = null; butt.Owner = user; }
                            break;
                    }
                    lot.Status = LotStatus.Closed;
                    lot.BuyerId = user.GameId;
                    user.AccountBalance -= lot.Price;
                    lot.Seller.AccountBalance += lot.Price;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Market",
                        new { buy = $"Лот #{lot.Id} успешно куплен за {lot.Price}$4" });
                }
                else
                {
                    return RedirectToAction("Index", "Market",
                        new {error = "У вас не хватает средств для этой покупки"});
                }
            }

        }
        //GET
        [Authorize(Roles = "admin")]
        public ActionResult NewWeaponLot() {
            var model = new NewWeaponLotModel();
            return View(model);
        }
        //POST
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult NewWeaponLot(NewWeaponLotModel model) {
            if (ModelState.IsValid) {
                using (var db = new BGS_DBContext()) {
                    string userIdentityId = User.Identity.GetUserId();
                    var seller = db.Users.Single(u => u.Id == userIdentityId);
                    for (int i = 0; i < model.LotCount; i++) {
                        WeaponModel newWeaponModelModel = GameUtilCreater.WeaponModelFromModel(model);
                        db.WeaponModels.Add(newWeaponModelModel);
                        db.SaveChanges();
                        var lot = new LotModel() {
                            Seller = seller,
                            ItemId = newWeaponModelModel.Id,
                            Price = model.Price,
                            SellerId = seller.GameId,
                            Status = LotStatus.Available,
                            Type = LotType.Weapon
                        };
                        db.LotModels.Add(lot);
                        db.SaveChanges();
                        newWeaponModelModel.LotId = lot.Id;
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", "Market");
                }
            }
            ModelState.AddModelError("", "Что то не правильно");
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public ActionResult NewArmorLot()
        {
            var model = new NewArmorLotModel();
            return View(model);
        }
        //POST
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult NewArmorLot(NewArmorLotModel model) {
            if (ModelState.IsValid) {
                using (var db = new BGS_DBContext()) {
                    string userIdentityId = User.Identity.GetUserId();
                    var seller = db.Users.Single(u => u.Id == userIdentityId);
                    for (int i = 0; i < model.LotCount; i++) {
                        ArmorModel newArmorModelModel = GameUtilCreater.ArmorModelFromModel(model);
                        db.ArmorModels.Add(newArmorModelModel);
                        db.SaveChanges();
                        var lot = new LotModel() {
                            Seller = seller,
                            ItemId = newArmorModelModel.Id,
                            Price = model.Price,
                            SellerId = seller.GameId,
                            Status = LotStatus.Available,
                            Type = LotType.Armor
                        };
                        db.LotModels.Add(lot);
                        db.SaveChanges();
                        newArmorModelModel.LotId = lot.Id;
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", "Market");
                }
            }
            ModelState.AddModelError("", "Что то не правильно");
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public ActionResult NewAccessoryLot() {
            var model = new NewAccessoryLotModel();
            return View(model);
        }
        //POST
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult NewAccessoryLot(NewAccessoryLotModel model) {
            if (ModelState.IsValid) {
                using (var db = new BGS_DBContext()) {
                    string userIdentityId = User.Identity.GetUserId();
                    var seller = db.Users.Single(u => u.Id == userIdentityId);
                    for (int i = 0; i < model.LotCount; i++) {
                        AccessoryModel newAccessoryModelModel = GameUtilCreater.AccessoryModelFromModel(model);
                        db.AccessoryModels.Add(newAccessoryModelModel);
                        db.SaveChanges();
                        var lot = new LotModel() {
                            Seller = seller,
                            ItemId = newAccessoryModelModel.Id,
                            Price = model.Price,
                            SellerId = seller.GameId,
                            Status = LotStatus.Available,
                            Type = LotType.Accessory
                        };
                        db.LotModels.Add(lot);
                        db.SaveChanges();
                        newAccessoryModelModel.LotId = lot.Id;
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", "Market");
                }
            }
            ModelState.AddModelError("", "Что то не правильно");
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public ActionResult NewStorageLot() {
            var model = new NewStorageLotModel();
            return View(model);
        }
        //POST
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult NewStorageLot(NewStorageLotModel model) {
            if (ModelState.IsValid) {
                using (var db = new BGS_DBContext()) {
                    string userIdentityId = User.Identity.GetUserId();
                    var seller = db.Users.Single(u => u.Id == userIdentityId);
                    for (int i = 0; i < model.LotCount; i++) {
                        StorageModel newStorageModel = GameUtilCreater.StorageModelFromModel(model);
                        db.StorageModels.Add(newStorageModel);
                        db.SaveChanges();
                        var lot = new LotModel() {
                            Seller = seller,
                            ItemId = newStorageModel.Id,
                            Price = model.Price,
                            SellerId = seller.GameId,
                            Status = LotStatus.Available,
                            Type = LotType.Storage
                        };
                        db.LotModels.Add(lot);
                        db.SaveChanges();
                        newStorageModel.LotId = lot.Id;
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", "Market");
                }
            }
            ModelState.AddModelError("", "Что то не правильно");
            return View(model);
        }
        [Authorize(Roles = "admin")]
        public ActionResult NewModificationLot() {
            var model = new NewModificationLotModel();
            return View(model);
        }
        //POST
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult NewModificationLot(NewModificationLotModel model) {
            if (ModelState.IsValid) {
                using (var db = new BGS_DBContext()) {
                    string userIdentityId = User.Identity.GetUserId();
                    var seller = db.Users.Single(u => u.Id == userIdentityId);
                    for (int i = 0; i < model.LotCount; i++) {
                        switch (model.Type)
                        {
                            case ModificationType.Aim:
                                AimModificationModel newAimModModel = new AimModificationModel(){Item = model.Item,WeaponType = model.WeaponType};
                                db.AimModificationModels.Add(newAimModModel);
                                db.SaveChanges();
                                var aimLot = new LotModel() {
                                Seller = seller,
                                ItemId = newAimModModel.Id,
                                Price = model.Price,
                                SellerId = seller.GameId,
                                Status = LotStatus.Available,
                                Type = LotType.Modification
                                };
                                db.LotModels.Add(aimLot);
                                db.SaveChanges();
                                newAimModModel.LotId = aimLot.Id;
                                break;
                            case ModificationType.Magazine:
                                MagazineModificationModel newMagazineModModel = new MagazineModificationModel() { Item = model.Item, WeaponType = model.WeaponType };
                                db.MagazineModificationModels.Add(newMagazineModModel);
                                db.SaveChanges();
                                var maganineLot = new LotModel() {
                                    Seller = seller,
                                    ItemId = newMagazineModModel.Id,
                                    Price = model.Price,
                                    SellerId = seller.GameId,
                                    Status = LotStatus.Available,
                                    Type = LotType.Modification
                                };
                                db.LotModels.Add(maganineLot);
                                db.SaveChanges();
                                newMagazineModModel.LotId = maganineLot.Id;
                                break;
                            case ModificationType.Barrel: 
                                BarrelModificationModel newBarrelModModel = new BarrelModificationModel() { Item = model.Item, WeaponType = model.WeaponType };
                                db.BarrelModificationModels.Add(newBarrelModModel);
                                db.SaveChanges();
                                var barrelLot = new LotModel() {
                                    Seller = seller,
                                    ItemId = newBarrelModModel.Id,
                                    Price = model.Price,
                                    SellerId = seller.GameId,
                                    Status = LotStatus.Available,
                                    Type = LotType.Modification
                                };
                                db.LotModels.Add(barrelLot);
                                db.SaveChanges();
                                newBarrelModModel.LotId = barrelLot.Id;
                                break;
                            case ModificationType.Butt:
                                ButtModificationModel newButtModModel = new ButtModificationModel() { Item = model.Item, WeaponType = model.WeaponType };
                                db.ButtModificationModels.Add(newButtModModel);
                                db.SaveChanges();
                                var buttLot = new LotModel() {
                                    Seller = seller,
                                    ItemId = newButtModModel.Id,
                                    Price = model.Price,
                                    SellerId = seller.GameId,
                                    Status = LotStatus.Available,
                                    Type = LotType.Modification
                                };
                                db.LotModels.Add(buttLot);
                                db.SaveChanges();
                                newButtModModel.LotId = buttLot.Id;
                                break;
                        }
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", "Market");
                }
            }
            ModelState.AddModelError("", "Что то не правильно");
            return View(model);
        }
    }
}