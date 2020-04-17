using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Battlegrid.ru.Models
{
    public class BGS_DBContext : DbContext
    {
        public BGS_DBContext() : base("BGS_DB")
        {
        }

        public DbSet<User> Users { get; set; }
    }
    public class User {
        [Key]
        [Required] public int UserId { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Password { get; set; }
        [Required] public int GameLevel { get; set; }
        public string Avatar { get; set; }
        [Required] public DateTime LastActivity { get; set; }
        [Required] public double Rating { get; set; }
        [Required] public string Id { get; set; }
        public string Description { get; set; }
    }
}