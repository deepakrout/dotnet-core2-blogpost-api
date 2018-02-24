using Microsoft.EntityFrameworkCore;
using Rou.BlogPost.Model.Models;

namespace Rou.BlogPost.Model.DB
{
    public class BlogPostDbContext : DbContext
    {
         public BlogPostDbContext(DbContextOptions<BlogPostDbContext> options)
            : base(options)
        { }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

    }
}