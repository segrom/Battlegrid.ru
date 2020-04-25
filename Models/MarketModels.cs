using BGS.Models;

namespace Battlegrid.ru.Models
{
    public class AllLots
    {
        public LotModel[] Last100Lots { get; set; }
        public UnitModel[] LotUnits { get; set; }
        public WeaponModel[] LotWeapons { get; set; }
        public AccessoryModel[] LotAccessoryes { get; set; }
        public MagazineModificationModel[] MagazineModificationModels { get; set; }
        public BarrelModificationModel[] BarrelModificationModels { get; set; }
        public ButtModificationModel[] ButtModificationModels { get; set; }
        public AimModificationModel[] AimModificationModels { get; set; }
        public ArmorModel[] LotArmors { get; set; }
    }

    public class NewUnitLotModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public bool RadnomNSA { get; set; }
        public float XP { get; set; } 
        public float BaseAccuracy { get; set; } 
        public float BaseStrong { get; set; } 
        public float BaseActionPoints { get; set; } 
        public float BaseHealth { get; set; } 
        public float BaseMeleeDamage { get; set; } 

        public float HeavyWeapon { get; set; }
        public float LightWeapon { get; set; }
        public float AccurateWeapon { get; set; }
        public float Handgun { get; set; }
        public float Explosive { get; set; }
        public float HeavyArmor { get; set; }
        public float LightArmor { get; set; }
        public float GasInsensitivity { get; set; }
        public float FireInsensitivity { get; set; }
        public float PainInsensitivity { get; set; }


        public int LotCount { get; set; }

    }

}