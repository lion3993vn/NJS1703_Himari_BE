using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Repository.Repositories.Generic
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<List<TEntity>> GetAllAsync();

        Task<TEntity?> GetByIdAsync(int id);

        Task<TEntity> AddAsync(TEntity entity);

        void UpdateAsync(TEntity entity);

        void SoftDeleteAsync(TEntity entity);

        Task AddRangeAsync(List<TEntity> entities);

        void SoftDeleteRangeAsync(List<TEntity> entities);

        void PermanentDeletedAsync(TEntity entity);

        void PermanentDeletedListAsync(List<TEntity> entities);

        Task<Pagination<TEntity>> ToPagination(PaginationParameter paginationParameter);

        Task<Pagination<TEntity>> ToPaginationIncludeAsync(
            PaginationParameter paginationParameter,
            Func<IQueryable<TEntity>,
            IIncludableQueryable<TEntity, object>>? include = null,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<TEntity?> GetByIdIncludeAsync(int id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            Expression<Func<TEntity, bool>> filter = null);
    }
}
