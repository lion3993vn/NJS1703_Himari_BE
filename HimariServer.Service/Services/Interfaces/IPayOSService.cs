using HimariServer.Service.BusinessModels.PayOSModels;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IPayOSService
    {
        Task<string> CreatePaymentUrl(PayOSRequest request);
    }
}
