using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using BGS.GameCore;
using BGS.Models;


namespace Battlegrid.ru.Models
{
    public class BGS_DBContext : DbContext
    {
        public BGS_DBContext() : base("BGS_DB")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<LotModel> LotModels { get; set; }
        public DbSet<SquadModel> SquadModels { get; set; }
        public DbSet<ArmorModel> ArmorModels { get; set; }
        public DbSet<WeaponModel> WeaponModels { get; set; }
        public DbSet<UnitSpecializationModel> UnitSpecializationModels { get; set; }
        public DbSet<UnitModel> UnitModels { get; set; }
        public DbSet<SkillModel> SkillModels { get; set; }
        public DbSet<StorageModel> StorageModels { get; set; }
        public DbSet<AccessoryModel> AccessoryModels { get; set; }
        public DbSet<MeleeWeaponModel> MeleeWeaponModels { get; set; }
        public DbSet<MagazineModificationModel> MagazineModificationModels { get; set; }
        public DbSet<AimModificationModel> AimModificationModels { get; set; }
        public DbSet<BarrelModificationModel> BarrelModificationModels { get; set; }
        public DbSet<ButtModificationModel> ButtModificationModels { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {

            modelBuilder.Entity<User>().HasMany(u => u.ArmorModels).WithOptional(u => u.Owner)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.MagazineModifications).WithOptional(u => u.Owner)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.AimModifications).WithOptional(u => u.Owner)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.BarrelModifications).WithOptional(u => u.Owner)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.ButtModifications).WithOptional(u => u.Owner)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.Units).WithOptional(u => u.Owner)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.StorageModels).WithOptional(u => u.Owner)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.AccessoryModels).WithOptional(u => u.Owner)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.WeaponModels).WithOptional(u => u.Owner)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<User>().HasMany(u => u.Squads).WithRequired(u => u.Owner)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<SquadModel>().HasMany(u => u.Units).WithOptional(u => u.Squad)
                .HasForeignKey(u => u.SquadId);

            modelBuilder.Entity<WeaponModel>().HasOptional(u => u.MagazineModification)
                .WithOptionalPrincipal();
            modelBuilder.Entity<WeaponModel>().HasOptional(u => u.AimModification).WithOptionalPrincipal();
            modelBuilder.Entity<WeaponModel>().HasOptional(u => u.BarrelModification).WithOptionalPrincipal();
            modelBuilder.Entity<WeaponModel>().HasOptional(u => u.ButtModification).WithOptionalPrincipal();


            modelBuilder.Entity<UnitModel>().HasRequired(u => u.Specialization).WithRequiredPrincipal(u => u.Unit);
            modelBuilder.Entity<UnitModel>().HasOptional(u => u.Arms).WithOptionalPrincipal();
            modelBuilder.Entity<UnitModel>().HasOptional(u => u.Body).WithOptionalPrincipal();
            modelBuilder.Entity<UnitModel>().HasOptional(u => u.Helmet).WithOptionalPrincipal();
            modelBuilder.Entity<UnitModel>().HasOptional(u => u.Legs).WithOptionalPrincipal();
            modelBuilder.Entity<UnitModel>().HasOptional(u => u.Backpack).WithOptionalPrincipal();
            modelBuilder.Entity<UnitModel>().HasOptional(u => u.Vest).WithOptionalPrincipal();
            modelBuilder.Entity<UnitModel>().HasOptional(u => u.HandAccessory).WithOptionalPrincipal();
            modelBuilder.Entity<UnitModel>().HasOptional(u => u.HeadAccessory).WithOptionalPrincipal();
            modelBuilder.Entity<UnitModel>().HasOptional(u => u.FirstWeapon).WithOptionalPrincipal();
            modelBuilder.Entity<UnitModel>().HasOptional(u => u.SecondWeapon).WithOptionalPrincipal();
            modelBuilder.Entity<UnitModel>().HasOptional(u => u.MeleeWeapon).WithOptionalPrincipal();

            modelBuilder.Entity<UnitSpecializationModel>().HasMany(u => u.Skills).WithRequired(u => u.Owner)
                .HasForeignKey(u => u.UnitSpecId);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasMany(u => u.LotModels).WithRequired(u => u.Seller).HasForeignKey(u => u.SellerId);

        }
    }
}