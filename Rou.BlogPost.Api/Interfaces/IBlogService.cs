using System.Collections.Generic;
using Rou.BlogPost.Model.Models;

namespace Rou.BlogPost.Api.Interfaces
{
    public interface IBlogService
    {
         IEnumerable<Blog> GetBlogs(int? blogId);
         Blog CreateBlog(Blog blog);
    }
}