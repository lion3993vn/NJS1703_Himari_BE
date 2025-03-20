using HimariServer.Service.BusinessModels.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IChromaService
    {
        Task<List<ProductChatModel>> QuerySimilarProducts(string queryText);
        
    }
}
