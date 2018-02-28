using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rou.BlogPost.Api.Interfaces;
using Rou.BlogPost.Core.Extensions;
using Rou.BlogPost.Core.Interfaces;
using Rou.BlogPost.Model.Models;

namespace Rou.BlogPost.Api.Services {
    public class BlogService : IBlogService {
        private IUnitOfWork _unitOfWork;
        private IBlogRepository _blogRepository;
        private IPostRepository _postRepository;

        public BlogService (IUnitOfWork unitOfWork,
            IBlogRepository blogRepository,
            IPostRepository postRepository) {
            _unitOfWork = unitOfWork;
            _blogRepository = blogRepository;
            _postRepository = postRepository;
        }

        public Blog CreateBlog (Blog blog) {
            _blogRepository.Add (blog);
            _unitOfWork.Commit ();
            return _blogRepository.Get (a => a.BlogId == blog.BlogId).FirstOrDefault ();
        }

        public void DeleteBlog (Blog blog) {
            if (blog != null) {
                _blogRepository.Delete (blog);
                _unitOfWork.Commit ();
            }
        }

        public IQueryable<Blog> GetBlogs (int? blogId = 0) {
            //  Expression<Func<Blog, bool>> blogTerm = t=> blogId != 0 ? t.BlogId == blogId : t.BlogId == t.BlogId;
            var blogTerm = PredicateBuilder.True<Blog> ();
            if (blogId != 0) {
                blogTerm = blogTerm.And (a => a.BlogId == blogId);
            }
            return _blogRepository.Get (blogTerm).IncludeChild (a => a.Posts);
        }

        public void UpdateBlog (Blog blog) {
            var existingBlog = _blogRepository.Get (b => b.BlogId == blog.BlogId).AsNoTracking().IncludeChild (p=>p.Posts).FirstOrDefault();

            if (blog != null) {
                _blogRepository.Update (blog);

                //Removing deleted posts
                foreach (var existingChild in existingBlog.Posts) {
                    if (!blog.Posts.Any (c => c.PostId == existingChild.PostId))
                        //_postRepository.Delete (existingChild);
                        _unitOfWork.Context.Entry(existingChild).State= EntityState.Deleted;
                }

                //Update or add Post.
                foreach (var post in blog.Posts) {
                    var existingPosts = _postRepository.Get (a => a.PostId == post.PostId).FirstOrDefault ();
                    if (existingPosts != null) {
                        _postRepository.Update (post);
                    } else {
                        _postRepository.Add (post);
                    }
                }
                _unitOfWork.Commit ();
            }
        }
    }
}