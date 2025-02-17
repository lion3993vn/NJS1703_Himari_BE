using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.ResultModels
{
    public class ModelPaging
    {
        public object? Data { get; set; } = new object();
        public object? MetaData { get; set; } = new object();
    }
}
