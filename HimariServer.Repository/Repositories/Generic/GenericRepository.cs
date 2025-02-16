using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HimariServer.Repository.Utils;
using HimariServer.Repository.DBContext;

namespace HimariServer.Repository.Repositories.Generic
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(HimariServerContext context)
        {
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.CreatedDate = CommonUtils.GetCurrentTime();
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedDate = CommonUtils.GetCurrentTime();
            }
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.Where(x => !x.IsDeleted).ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            var result = await _dbSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            return result;
        }

        public void PermanentDeletedAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void PermanentDeletedListAsync(List<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public void SoftDeleteAsync(TEntity entity)
        {
            entity.IsDeleted = true;
            _dbSet.Update(entity);
        }

        public void SoftDeleteRangeAsync(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }
            _dbSet.UpdateRange(entities);
        }

        public async Task<Pagination<TEntity>> ToPagination(PaginationParameter paginationParameter)
        {
            var itemCount = await _dbSet.CountAsync();
            var items = await _dbSet.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                                    .Take(paginationParameter.PageSize)
                                    .AsNoTracking()
                                    .ToListAsync();
            var result = new Pagination<TEntity>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }

        public void UpdateAsync(TEntity entity)
        {
            entity.UpdatedDate = CommonUtils.GetCurrentTime();
            _dbSet.Update(entity);
        }

        public async Task<Pagination<TEntity>> ToPaginationIncludeAsync(PaginationParameter paginationParameter,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var itemCount = await query.CountAsync();


            // Implement pagination
            query = query.Skip((paginationParameter.PageIndex - 1) * paginationParameter.PageSize)
                        .Take(paginationParameter.PageSize);

            var items = await query.AsNoTracking().ToListAsync();

            var result = new Pagination<TEntity>(items, itemCount, paginationParameter.PageIndex, paginationParameter.PageSize);

            return result;
        }

        public async Task<TEntity?> GetByIdIncludeAsync(int id,
            Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null,
            Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = _dbSet;

            // Apply include if provided
            if (include != null)
            {
                query = include(query);
            }

            // Apply filter if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }
    }
}
