using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Repository.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<bool> IsContainProduct(int categoryId);
        Task<List<int>> GetProductIdsByPartSymptomAsync(string partSymptomName);
    }
}
