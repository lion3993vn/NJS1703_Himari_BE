using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IOnnxEmbeddingService
    {
        Task<float[]> GenerateEmbedding(string text);
    }
}
