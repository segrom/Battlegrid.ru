using BGS.Models;

namespace Battlegrid.ru.Models
{
    public class AllInventoryModel
    {
        public UnitModel[] UnitModels { get; set; }
        public WeaponModel[] WeaponModels { get; set; }
        public ArmorModel[] ArmorModels { get; set; }
        public StorageModel[] StorageModels { get; set; }
        public MeleeWeaponModel[] MeleeWeaponModels { get; set; }
        public AccessoryModel[] AccessoryModels { get; set; }
    }
}