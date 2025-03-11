using AutoMapper;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.PaymentModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> GetPaymentInfoByOrderCode(int orderCode)
        {
            var payment = await _unitOfWork.PaymentRepository.GetByOrderCodeAsync(orderCode);

            if (payment == null)
            {
                throw new NotExistException(MessageConstants.PAYMENT_NOT_FOUND);
            }

            return new BaseResponseModel
            {
                StatusCode = 200,
                Message = MessageConstants.GET_PAYMENT_SUCCESS,
                Data = _mapper.Map<PaymentModels>(payment)
            };
        }
    }
}
