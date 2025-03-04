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
        IBlogCategoryRepository  BlogCategoryRepository{get;}
        IBlogRepository BlogRepository {get;}
        IUserRepository UsersRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        IBodyPartRepository BodyPartRepository { get; }
        IBrandRepository BrandRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        ISymptomRepository SymptomRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserDeviceRepository UserDeviceRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IUserNotificationRepository UserNotificationRepository { get; }
        IPartSymptomRepository PartSymptomRepository { get; }
        IChatMessageRepository ChatMessageRepository { get; }
        IOrderRepository OrderRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        int Save();
        void Commit();
        void Rollback();
        Task SaveAsync();
    }
}
