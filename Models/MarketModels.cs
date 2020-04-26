using System.ComponentModel.DataAnnotations;
using BGS.Models;

namespace Battlegrid.ru.Models
{
    public class AllLots
    {[Range(typeof(float), "0", "1",ErrorMessage = "float значение от 0 до 1")] 
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
        [DataType(DataType.Text)] [Range(typeof(float), "0", "1",ErrorMessage = "float значение от 0 до 1")] public float XP { get; set; }
        [DataType(DataType.Text)] [Range(typeof(float), "0", "1",ErrorMessage = "float значение от 0 до 1")] public float BaseAccuracy { get; set; }  = 1;
        [Range(typeof(float), "0", "1",ErrorMessage = "float значение от 0 до 1")] public float BaseStrong { get; set; }  = 1;
        [Range(typeof(float), "0", "1",ErrorMessage = "float значение от 0 до 1")] public float BaseActionPoints { get; set; }  = 1;
        [Range(typeof(float), "0", "1",ErrorMessage = "float значение от 0 до 1")] public float BaseHealth { get; set; } = 1;
        [Range(typeof(float), "0", "1", ErrorMessage = "float значение от 0 до 1")] public float BaseMeleeDamage { get; set; } = 1;

        [Range(typeof(float), "0", "1",ErrorMessage = "float значение от 0 до 1")] public float HeavyWeapon { get; set; } = 1;
        [Range(typeof(float), "0", "1", ErrorMessage = "float значение от 0 до 1")] public float LightWeapon { get; set; } = 1;
        [Range(typeof(float), "0", "1", ErrorMessage = "float значение от 0 до 1")] public float AccurateWeapon { get; set; } = 1;
        [Range(typeof(float), "0", "1", ErrorMessage = "float значение от 0 до 1")] public float Handgun { get; set; } = 1;
        [Range(typeof(float), "0", "1", ErrorMessage = "float значение от 0 до 1")] public float Explosive { get; set; } = 1;
        [Range(typeof(float), "0", "1",ErrorMessage = "float значение от 0 до 1")] public float HeavyArmor { get; set; } = 1;
        [Range(typeof(float), "0", "1",ErrorMessage = "float значение от 0 до 1")] public float LightArmor { get; set; } = 1;
        [Range(typeof(float), "0", "1",ErrorMessage = "float значение от 0 до 1")] public float GasInsensitivity { get; set; } = 1;
        [Range(typeof(float), "0", "1",ErrorMessage = "float значение от 0 до 1")] public float FireInsensitivity { get; set; } = 1;
        [Range(typeof(float), "0", "1", ErrorMessage = "float значение от 0 до 1")] public float PainInsensitivity { get; set; } = 1;

        [Range(typeof(float), "0", "100000000", ErrorMessage = "float значение [0,100000000]")] public float Price { get; set; }
        public int LotCount { get; set; }

    }

}