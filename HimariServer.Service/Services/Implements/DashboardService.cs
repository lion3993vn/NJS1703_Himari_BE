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

        public async Task<BaseResponseModel> GetNewOrder()
        {
            DateTime currentDate = DateTime.Now;
            int currentMonth = currentDate.Month;
            int previousMonth = currentDate.Month == 1 ? 12 : currentDate.Month - 1;
            int currentYear = currentDate.Year;
            int previousYear = currentDate.Month == 1 ? currentDate.Year - 1 : currentDate.Year;

            int currentMonthOrders = await _unitOfWork.OrderRepository.GetTotalOrder(currentMonth, currentYear);
            int previousMonthOrders = await _unitOfWork.OrderRepository.GetTotalOrder(previousMonth, previousYear);

            double percentageChange = 0;
            bool isIncrease = false;

            if (previousMonthOrders > 0)
            {
                percentageChange = Math.Abs(((double)(currentMonthOrders - previousMonthOrders) / previousMonthOrders) * 100);
                isIncrease = currentMonthOrders >= previousMonthOrders;
            }
            else if (currentMonthOrders > 0)
            {
                percentageChange = 100;
                isIncrease = true;
            }

            // Create new order model with formatted values
            var newOrderModel = new NewOrderModel
            {
                QuantityOrder = currentMonthOrders,
                Percent = FormatPercentage(percentageChange),
                IsIncrease = isIncrease
            };

            // Create and return response
            return new BaseResponseModel
            {
                StatusCode = 200,
                Message = MessageConstants.GET_NEW_ORDER_SUCCESS,
                Data = newOrderModel
            };
        }

        public async Task<BaseResponseModel> GetNewUser()
        {
            DateTime currentDate = DateTime.Now;
            int currentMonth = currentDate.Month;
            int previousMonth = currentDate.Month == 1 ? 12 : currentDate.Month - 1;
            int currentYear = currentDate.Year;
            int previousYear = currentDate.Month == 1 ? currentDate.Year - 1 : currentDate.Year;

            int currentMonthUsers = await _unitOfWork.UsersRepository.GetUserCountByMonth(currentMonth, currentYear);
            int previousMonthUsers = await _unitOfWork.UsersRepository.GetUserCountByMonth(previousMonth, previousYear);

            double percentageChange = 0;
            bool isIncrease = false;

            if (previousMonthUsers > 0)
            {
                percentageChange = Math.Abs(((double)(currentMonthUsers - previousMonthUsers) / previousMonthUsers) * 100);
                isIncrease = currentMonthUsers >= previousMonthUsers;
            }
            else if (currentMonthUsers > 0)
            {
                percentageChange = 100;
                isIncrease = true;
            }

            // Create new user model with formatted values
            var newUserModel = new NewUserModel
            {
                QuantityUser = currentMonthUsers,
                Percent = FormatPercentage(percentageChange),
                IsIncrease = isIncrease
            };

            // Create and return response
            return new BaseResponseModel
            {
                StatusCode = 200,
                Message = MessageConstants.GET_NEW_USER_SUCCESS,
                Data = newUserModel
            };
        }
    }
}
