using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.DashboardModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponseModel> GetRevenue()
        {
            DateTime currentDate = DateTime.Now;
            int currentMonth = currentDate.Month;
            int previousMonth = currentDate.Month == 1 ? 12 : currentDate.Month - 1;

            int currentMonthRevenue = await _unitOfWork.OrderRepository.GetTotalPriceByMonth(currentMonth);
            int previousMonthRevenue = await _unitOfWork.OrderRepository.GetTotalPriceByMonth(previousMonth);

            double percentageChange = 0;
            bool isIncrease = false;

            if (previousMonthRevenue > 0)
            {
                percentageChange = Math.Abs(((double)(currentMonthRevenue - previousMonthRevenue) / previousMonthRevenue) * 100);
                isIncrease = currentMonthRevenue >= previousMonthRevenue;
            }
            else if (currentMonthRevenue > 0)
            {
                percentageChange = 100;
                isIncrease = true;
            }

            // Create revenue model with formatted values
            var revenueModel = new RevenueModel
            {
                Revenue = FormatToMillions(currentMonthRevenue),
                Percent = FormatPercentage(percentageChange),
                IsIncrease = isIncrease
            };

            // Create and return response
            return new BaseResponseModel
            {
                StatusCode = 200,
                Message = MessageConstants.GET_REVENUE_SUCCESS,
                Data = revenueModel
            };
        }

        private string FormatToMillions(double value)
        {
            if (value == 0)
                return "0M";

            double inMillions = value / 1000000.0;
            
            if (value < 1000000 && value >= 1000)
            {
                inMillions = value / 1000000.0;
            }
            
            return $"{inMillions:0.0}M";
        }

        private string FormatPercentage(double value)
        {
            if (value == Math.Floor(value))
            {
                return value.ToString("0");
            }
            else
            {
                return value.ToString("0.0");
            }
        }
    }
}
