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

        public IEnumerable<Blog> GetBlogs(int? blogId =0 )
        {
            Expression<Func<Blog, bool>> pred = t=> blogId != 0 ? t.BlogId == blogId : t.BlogId == t.BlogId;
            return _blogRepository.Get(pred).IncludeChild(a=>a.Posts);
        }
    }
}