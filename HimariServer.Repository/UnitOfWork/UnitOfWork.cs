using HimariServer.Repository.DBContext;
using HimariServer.Repository.Repositories.Implements;
using HimariServer.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HimariServerContext _context;
        private IDbContextTransaction _transaction;
        private IUserRepository _userRepository;
        private IProductRepository _productRepository;
        private IBlogRepository _blogRepository;
        private CategoryRepository _categoryRepository;

        public UnitOfWork(HimariServerContext context) 
        {
            _context = context;
        }

        public IUserRepository UsersRepository 
        {
            get
            {
                return _userRepository ??= new UserRepository(_context);

            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository ??= new CategoryRepository(_context);

            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                return _productRepository ??= new ProductRepository(_context);

            }
        }

        public IBlogRepository BlogRepository
        {
            get
            {
                return _blogRepository ??= new BlogRepository(_context);

            }
        }

        public void Commit()
        {
            try
            {
                _context.SaveChanges();
                _transaction?.Commit();
            }
            catch (Exception)
            {
                _transaction?.Rollback();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
