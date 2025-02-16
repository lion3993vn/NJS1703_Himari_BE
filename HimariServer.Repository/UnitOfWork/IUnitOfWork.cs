using HimariServer.Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IBlogRepository BlogRepository {get;}
        IUserRepository UsersRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        int Save();
        void Commit();
        void Rollback();
        Task SaveAsync();
    }
}
