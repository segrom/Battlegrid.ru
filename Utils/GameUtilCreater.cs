using System;
using System.Collections.Generic;
using Battlegrid.ru.Models;
using BGS.Models;
using Newtonsoft.Json.Serialization;

namespace Battlegrid.ru.Utils
{
    public static class GameUtilCreater
    {
        public static User CreateNewUser(string email, string password,ApplicationUser appUser)
        {
            User newUser = new User(){
                Name = appUser.UserName, Password = password, GameLevel = 0, LastActivity = DateTime.Now,
                Rating = 5, Id = appUser.Id
            };
            newUser.AimModifications = new List<AimModificationModel>();
            return newUser;
        }

        public static UnitModel UnitModelFromModel(NewUnitLotModel model)
        {
            UnitModel newUnit = new UnitModel()
            {
                XP = model.XP,
                BaseAccuracy = model.BaseAccuracy,
                BaseActionPoints = model.BaseActionPoints,
                BaseMeleeDamage = model.BaseMeleeDamage,
                BaseHealth = model.BaseHealth,
                BaseStrong = model.BaseStrong
            };
            if (model.RadnomNSA)
            {

            }
            else
            {
                newUnit.Name = model.Name;
                newUnit.Surname = model.Surname;
                newUnit.Age = model.Age;
            }
            UnitSpecializationModel specializationModel = new UnitSpecializationModel()
            {
                LightArmor = model.LightArmor,
                GasInsensitivity = model.GasInsensitivity,
                FireInsensitivity = model.FireInsensitivity,
                PainInsensitivity = model.PainInsensitivity,
                HeavyWeapon = model.HeavyWeapon,
                LightWeapon = model.LightWeapon,
                AccurateWeapon = model.AccurateWeapon,
                Handgun = model.Handgun,
                Explosive = model.Explosive,
                HeavyArmor = model.HeavyArmor,
                Skills = new List<SkillModel>()
            };
            newUnit.Specialization=new UnitSpecializationModel();
            return newUnit;
        }

        public static WeaponModel WeaponModelFromModel(NewWeaponLotModel model)
        {
            return new WeaponModel()
            {
                Item = model.Item,
                Type = model.Type,
                AimModification = null,
                BarrelModification = null,
                MagazineModification = null,
                ButtModification = null
            };
        }
    }
}