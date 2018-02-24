using Rou.BlogPost.Core.Interfaces;
using Rou.BlogPost.Model.Models;

namespace Rou.BlogPost.Core.Repositories
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository {

        public BlogRepository (IUnitOfWork unitOfWork) : base (unitOfWork) {

        }

    }
}