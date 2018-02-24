using Rou.BlogPost.Core.Interfaces;
using Rou.BlogPost.Model.Models;

namespace Rou.BlogPost.Core.Repositories {
    public class PostRepository : GenericRepository<Post>, IPostRepository {
        public PostRepository (IUnitOfWork unitOfWork) : base (unitOfWork) { }
    }
}