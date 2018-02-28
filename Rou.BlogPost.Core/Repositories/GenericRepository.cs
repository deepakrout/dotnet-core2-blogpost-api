using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rou.BlogPost.Core.Interfaces;
using Rou.BlogPost.Model.DB;

namespace Rou.BlogPost.Core.Repositories {
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class{
        private readonly IUnitOfWork _unitOfWork;
        public GenericRepository (IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }
        public void Add (T entity) {
            _unitOfWork.Context.Set<T>().Add (entity);
        }

        public void Delete (T entity) {
            T existing = _unitOfWork.Context.Set<T>().Find (entity);
            if (existing != null) _unitOfWork.Context.Set<T>().Remove (existing);
        }

        public IQueryable<T> Get () {
            return _unitOfWork.Context.Set<T>().AsQueryable<T> ();
        }

        public IQueryable<T> Get (Expression<Func<T, bool>> predicate) {
            return _unitOfWork.Context.Set<T>().Where (predicate).AsQueryable<T> ();
        }

        public void Update (T entity) {
            _unitOfWork.Context.Set<T>().Attach (entity);
            _unitOfWork.Context.Entry (entity).State = EntityState.Modified;            
        }
    }
}