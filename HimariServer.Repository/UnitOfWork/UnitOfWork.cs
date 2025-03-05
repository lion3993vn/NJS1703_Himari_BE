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
        private ICategoryRepository _categoryRepository;
        private IBodyPartRepository _bodyPartRepository;
        private IBrandRepository _brandRepository;
        private ISymptomRepository _symptomRepository;
        private IRoleRepository _roleRepository;
        private IOrderDetailRepository _orderDetailRepository;
        private IBlogCategoryRepository _blogCategoryRepository;
        private IUserDeviceRepository _userDeviceRepository;
        private INotificationRepository _notificationRepository;
        private IUserNotificationRepository _userNotificationRepository;
        private IPartSymptomRepository _partSymptomRepository;
        private IChatMessageRepository _chatMessageRepository;
        private IOrderRepository _orderRepository;
        private IPaymentRepository _paymentRepository;
        private IProductSymptomRepository _productSymptomRepository;

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

        public IBodyPartRepository BodyPartRepository
        {
            get
            {
                return _bodyPartRepository ??= new BodyPartRepository(_context);

            }
        }

        public IBrandRepository BrandRepository
        {
            get
            {
                return _brandRepository ??= new BrandRepository(_context);

            }
        }        
        public IOrderDetailRepository OrderDetailRepository
        {
            get
            {
                return _orderDetailRepository ??= new OrderDetailRepository(_context);

            }
        }        
        
        public ISymptomRepository SymptomRepository
        {
            get
            {
                return _symptomRepository ??= new SymptomRepository(_context);

            }
        }

        public IRoleRepository RoleRepository
        {
            get
            {
                return _roleRepository ??= new RoleRepository(_context);

            }
        }

        public IUserDeviceRepository UserDeviceRepository
        {
            get
            {
                return _userDeviceRepository ??= new UserDeviceRepository(_context);

            }
        }

        public IBlogCategoryRepository BlogCategoryRepository
        {
            get
            {
                return _blogCategoryRepository ??= new BlogCategoryRepository(_context);
            }
        }

        public INotificationRepository NotificationRepository
        {
            get
            {
                return _notificationRepository ??= new NotificationRepository(_context);
            }
        }

        public IUserNotificationRepository UserNotificationRepository
        {
            get
            {
                return _userNotificationRepository ??= new UserNotificationRepository(_context);
            }
        }

        public IPartSymptomRepository PartSymptomRepository
        {
            get
            {
                return _partSymptomRepository ??= new PartSymptomRepository(_context);
            }
        }

        public IChatMessageRepository ChatMessageRepository
        {
            get
            {
                return _chatMessageRepository ??= new ChatMessageRepository(_context);
            }
        }

        public IOrderRepository OrderRepository
        {
            get
            {
                return _orderRepository ??= new OrderRepository(_context);
            }
        }

        public IPaymentRepository PaymentRepository
        {
            get
            {
                return _paymentRepository ??= new PaymentRepository(_context);
            }
        }

        public IProductSymptomRepository ProductSymptomRepository
        {
            get
            {
                return _productSymptomRepository ??= new ProductSymptomRepository(_context);
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
