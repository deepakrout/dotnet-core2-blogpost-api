using System.Collections.Generic;
using Rou.BlogPost.Model.Models;

namespace Rou.BlogPost.Api.Interfaces
{
    public interface IPostService
    {
         IEnumerable<Post> GetPosts(int? postId);
         Post CreatePost(Post post);
    }
}