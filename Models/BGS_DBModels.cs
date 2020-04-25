using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using BGS.Models;


namespace Battlegrid.ru.Models
{
    public class BGS_DBContext : DbContext
    {
        public BGS_DBContext() : base("BGS_DB")
        {
        }

        public DbSet<User> Users { get; set; }
    }
}