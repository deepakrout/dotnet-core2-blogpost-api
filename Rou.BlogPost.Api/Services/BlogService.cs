using System.Collections.Generic;
using System.Linq;
using Rou.BlogPost.Api.Interfaces;
using Rou.BlogPost.Core.Interfaces;
using Rou.BlogPost.Core.Extensions;
using Rou.BlogPost.Model.Models;
using System;
using System.Linq.Expressions;

namespace Rou.BlogPost.Api.Services
{
    public class BlogService : IBlogService
    {
        private IUnitOfWork _unitOfWork;
        private IBlogRepository _blogRepository;

        public BlogService(IUnitOfWork unitOfWork, IBlogRepository blogRepository)
        {
            _unitOfWork = unitOfWork;
            _blogRepository = blogRepository;
        }

        public Blog CreateBlog(Blog blog)
        {
            _blogRepository.Add(blog);
            _unitOfWork.Commit();
            return _blogRepository.Get(a=>a.BlogId == blog.BlogId).FirstOrDefault();
        }

        public void DeleteBlog(Blog blog)
        {
            if (blog != null){
                _blogRepository.Delete(blog);
                _unitOfWork.Commit();
            }
        }

        public IQueryable<Blog> GetBlogs(int? blogId =0 )
        {
          //  Expression<Func<Blog, bool>> blogTerm = t=> blogId != 0 ? t.BlogId == blogId : t.BlogId == t.BlogId;
           var blogTerm = PredicateBuilder.True<Blog>();
            if (blogId != 0){
              blogTerm = blogTerm.And(a=>a.BlogId == blogId);
            }
            return _blogRepository.Get(blogTerm).IncludeChild(a=>a.Posts);
        }

        public void UpdateBlog(Blog blog)
        {
            if (blog != null){
                _blogRepository.Update(blog);
                _unitOfWork.Commit();
            }
        }
    }
}