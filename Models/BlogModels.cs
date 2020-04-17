using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Battlegrid.ru.Models
{
    public class BlogModels : DbContext
    {
        public BlogModels() : base("DefaultConnection") {
            Database.SetInitializer<BlogModels>(new BlogModelsCreater());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {

            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            modelBuilder.Entity<Post>().HasMany(c => c.Comments).WithOptional(a => a.Post).HasForeignKey(a => a.PostId);
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
        public class BlogModelsCreater : DropCreateDatabaseAlways<BlogModels> {
            protected override void Seed(BlogModels context) {

            }
        }
    public class Post
    {
        [Key] [Required] public int Id { get; set; }
        [Required] public string Author { get; set; }
        [Required] public string Label { get; set; }
        [Required] public string Text { get; set; }
        [Required] public int Likes { get; set; }
        [Required] public int Views { get; set; }
        [Required]
        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class Comment
    {
        [Key] [Required] public int Id { get; set; }
        [Required] public string Author { get; set; }
        [Required] public string Body { get; set; }
        [Required] public int Likes { get; set; }
        [Required] public int Views { get; set; }
        public virtual Post Post { get; set; }
        [Required] public int PostId { get; set; }

    }
}