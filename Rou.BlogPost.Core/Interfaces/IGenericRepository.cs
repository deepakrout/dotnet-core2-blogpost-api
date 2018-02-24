using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Rou.BlogPost.Core.Interfaces {
    public interface IGenericRepository<T> where T : class {
        IEnumerable<T> Get ();
        IQueryable<T> Get (Expression<Func<T, bool>> predicate);
        void Add (T entity);
        void Delete (T entity);
        void Update (T entity);
    }
}