using System.Collections.Generic;
using System.Linq;
using Rou.BlogPost.Model.Models;

namespace Rou.BlogPost.Api.Interfaces
{
    public interface IBlogService
    {
         IQueryable<Blog> GetBlogs(int? blogId);
         Blog CreateBlog(Blog blog);

         void UpdateBlog(Blog blog);

         void DeleteBlog(Blog blog);
    }
}