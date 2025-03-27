using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;

namespace HimariServer.Repository.Repositories.Interfaces
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
       Task<bool> IsContainBlog(int blogCategoryId);
    }
}
