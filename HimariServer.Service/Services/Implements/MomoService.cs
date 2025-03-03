using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.SettingModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class MomoService : IMomoService
    {
        private readonly MomoSettings _momoSettings;
        public MomoService(IOptions<MomoSettings> momoSettings)
        {
            _momoSettings = momoSettings.Value;
        }
    }
}
