using AutoMapper;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.ProductModels;
using HimariServer.Service.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class ChromaPeriodicService : BackgroundService, IChromaPeriodicService
    {
        private readonly PeriodicTimer _timer = new(TimeSpan.FromMinutes(1));
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IChromaService _chromaService;

        public ChromaPeriodicService(IServiceScopeFactory scopeFactory, IChromaService chromaService)
        {
            _scopeFactory = scopeFactory;
            _chromaService = chromaService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Tính thời gian đến 12h đêm tiếp theo
                var now = DateTime.Now;
                var midnight = now.Date.AddDays(1);
                var delay = midnight - now;

                await Task.Delay(delay, stoppingToken);

                await UpdateChomaDB();
            }
        }

        public async Task UpdateChomaDB()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                var products = await unitOfWork.ProductRepository.GetAllProduct();
                if (!products.Any()) return;

                var productRAGModels = mapper.Map<List<ProductRAGModel>>(products);

                await _chromaService.AddProductsToChromaDB(productRAGModels);
            }
        }
    }
}
