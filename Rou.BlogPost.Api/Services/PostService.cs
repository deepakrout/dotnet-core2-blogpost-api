using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Rou.BlogPost.Api.Interfaces;
using Rou.BlogPost.Core.Extensions;
using Rou.BlogPost.Core.Infrastructure;
using Rou.BlogPost.Core.Interfaces;
using Rou.BlogPost.Model.Models;

namespace Rou.BlogPost.Api.Services {
    public class PostService : IPostService {
        private IUnitOfWork _unitOfWork;
        private IPostRepository _postRepository;
        private readonly ILogger<PostService> _logger;

        public PostService (IUnitOfWork unitOfWork, 
                            IPostRepository postRespository,
                            ILogger<PostService> logger ) {
            _unitOfWork = unitOfWork;
            _postRepository = postRespository;
            _logger = logger;
        }

        public IQueryable<Post> GetPosts (int? postId) {
            var postTerm = PredicateBuilder.True<Post>();
            if (postId != 0){
               postTerm = postTerm.And(a=>a.PostId == postId);
            }
            return _postRepository.Get(postTerm);
        }

        public Post CreatePost (Post post) {
            _postRepository.Add (post);
            _unitOfWork.Commit ();
            var newPost = _postRepository.Get ().Where (a => a.PostId == post.PostId).FirstOrDefault ();
            return newPost;
        }

        public void UpdatePost(Post post)
        {
            if (post != null){
                _postRepository.Update(post);
                _unitOfWork.Commit();
            }
            else{
                _logger.LogError(LoggingEvents.UpdateItemNotFound,"Update Item is null");
                throw new ArgumentNullException(nameof(post), "Can not update null post");
            }
        }

        public void DeletePost(Post post)
        {
            if(post != null){
                _postRepository.Delete(post);
                _unitOfWork.Commit();
            }
            else{
                _logger.LogError(LoggingEvents.DeleteItem,"Cannot delete null post");
                throw new ArgumentNullException(nameof(post), "Can not delete null post");
            }
        }
    }
}