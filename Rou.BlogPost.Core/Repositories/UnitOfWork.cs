using Microsoft.EntityFrameworkCore;
using Rou.BlogPost.Core.Interfaces;
using Rou.BlogPost.Model.DB;

namespace Rou.BlogPost.Core.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public BlogPostDbContext Context { get; }
 
        public UnitOfWork(BlogPostDbContext context)
        {
            Context = context;
        }
        public void Commit()
        {
            Context.SaveChanges();
        }
 
        public void Dispose()
        {
           Context.Dispose();
            
        }
    }
}