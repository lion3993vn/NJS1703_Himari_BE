using AutoMapper;
using HimariServer.Repository.Enums;
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
        private readonly IMapper _mapper;

        public DashboardService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

        public async Task<BaseResponseModel> GetNewProduct()
        {
            DateTime currentDate = DateTime.Now;
            int currentMonth = currentDate.Month;
            int previousMonth = currentDate.Month == 1 ? 12 : currentDate.Month - 1;
            int currentYear = currentDate.Year;
            int previousYear = currentDate.Month == 1 ? currentDate.Year - 1 : currentDate.Year;

            int currentMonthProducts = await _unitOfWork.ProductRepository.GetProductCountByMonth(currentMonth, currentYear);
            int previousMonthProducts = await _unitOfWork.ProductRepository.GetProductCountByMonth(previousMonth, previousYear);

            double percentageChange = 0;
            bool isIncrease = false;

            if (previousMonthProducts > 0)
            {
                percentageChange = Math.Abs(((double)(currentMonthProducts - previousMonthProducts) / previousMonthProducts) * 100);
                isIncrease = currentMonthProducts >= previousMonthProducts;
            }
            else if (currentMonthProducts > 0)
            {
                percentageChange = 100;
                isIncrease = true;
            }

            // Create new product model with formatted values
            var newProductModel = new NewProductModel
            {
                QuantityProduct = currentMonthProducts,
                Percent = FormatPercentage(percentageChange),
                IsIncrease = isIncrease
            };

            // Create and return response
            return new BaseResponseModel
            {
                StatusCode = 200,
                Message = MessageConstants.GET_NEW_PRODUCT_SUCCESS,
                Data = newProductModel
            };
        }

        public async Task<BaseResponseModel> GetRevenueWithListMonth()
        {
            List<(int Year, int Month)> lastSixMonths = new List<(int, int)>();

            DateTime currentDate = DateTime.Now;

            for (int i = 0; i < 6; i++)
            {
                int month = currentDate.Month - i;
                int year = currentDate.Year;

                if (month <= 0)
                {
                    month += 12;
                    year -= 1;
                }

                lastSixMonths.Add((year, month));
            }

            List<RevenueByMonthModel> data = new();

            foreach(var item in lastSixMonths)
            {
                var revenue = await _unitOfWork.OrderRepository.GetTotalPriceByMonthAndYear(item.Month, item.Year);

                data.Add(new RevenueByMonthModel
                {
                    Month = "Tháng " + item.Month,
                    Revenue = revenue,
                });
            }

            return new BaseResponseModel
            {
                StatusCode = 200,
                Message = MessageConstants.GET_REVENUE_WITH_LIST_MONTH_SUCCESS,
                Data = data
            };
        }

        public async Task<BaseResponseModel> GetOrderWithRevenue()
        {
            var resultSuccess = await _unitOfWork.OrderRepository.GetTotalPriceWithDeliveryStatus(DeliveryStatus.Delivered);
            var resultFailPayment = await _unitOfWork.OrderRepository.GetTotalPriceWithPaymentStatus(PaymentStatus.Failed);
            var resultFailShip = await _unitOfWork.OrderRepository.GetTotalPriceWithDeliveryStatus(DeliveryStatus.Cancelled);

            List<OrderWithRevenue> data = new();
            data.Add(new OrderWithRevenue
            {
                Status = "Thành công",
                Revenue = resultSuccess,
            });
            data.Add(new OrderWithRevenue
            {
                Status = "Thất bại",
                Revenue = resultFailPayment + resultFailShip,
            });

            return new BaseResponseModel
            {
                StatusCode = 200,
                Message = MessageConstants.GET_ORDER_WITH_REVENUE_STATUS_SUCCESS,
                Data = data,
            };
        }

        public async Task<BaseResponseModel> GetLowQuantityProduct()
        {
            var products = await _unitOfWork.ProductRepository.GetLowQuantityProduct();
            return new BaseResponseModel
            {
                StatusCode = 200,
                Message = MessageConstants.GET_LOW_QUANTITY_PRODUCT_SUCCESS,
                Data = _mapper.Map<List<LowQuantityProductModel>>(products)
            };
        }
    }
}
