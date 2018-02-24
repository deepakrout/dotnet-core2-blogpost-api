using System;
using Microsoft.EntityFrameworkCore;
using Rou.BlogPost.Model.DB;

namespace Rou.BlogPost.Core.Interfaces {
    public interface IUnitOfWork : IDisposable {
        BlogPostDbContext Context { get; }
        void Commit ();
    }
}