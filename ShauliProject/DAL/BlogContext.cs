using ShauliProject.Models;
using System.Data.Entity;

namespace ShauliProject.DAL
{
    public class BlogContext : DbContext
    {
        public BlogContext()
            : base("ShauliContext")
        {
        }

        public DbSet<Fan> Fans { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<PostStat> PostStats { get; set; }

    }
}