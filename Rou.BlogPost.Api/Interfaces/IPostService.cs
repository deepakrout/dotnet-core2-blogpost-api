using System.Collections.Generic;
using System.Linq;
using Rou.BlogPost.Model.Models;

namespace Rou.BlogPost.Api.Interfaces
{
    public interface IPostService
    {
         IQueryable<Post> GetPosts(int? postId);
         Post CreatePost(Post post);

         void UpdatePost(Post post);

         void DeletePost(Post post);
    }
}