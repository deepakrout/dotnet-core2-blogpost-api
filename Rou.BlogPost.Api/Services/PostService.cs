using System.Collections.Generic;
using System.Linq;
using Rou.BlogPost.Api.Interfaces;
using Rou.BlogPost.Core.Interfaces;
using Rou.BlogPost.Model.Models;

namespace Rou.BlogPost.Api.Services {
    public class PostService : IPostService {
        private IUnitOfWork _unitOfWork;
        private IPostRepository _postRepository;

        public PostService (IUnitOfWork unitOfWork, IPostRepository postRespository) {
            _unitOfWork = unitOfWork;
            _postRepository = postRespository;
        }

        public IEnumerable<Post> GetPosts (int? postId) {
            return _postRepository.Get ();
        }

        public Post CreatePost (Post post) {
            _postRepository.Add (post);
            _unitOfWork.Commit ();
            var newPost = _postRepository.Get ().Where (a => a.PostId == post.PostId).FirstOrDefault ();
            return newPost;
        }
    }
}