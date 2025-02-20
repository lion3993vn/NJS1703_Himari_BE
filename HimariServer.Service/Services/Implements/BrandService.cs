using AutoMapper;
using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.BrandModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Task<BaseResponseModel> CreateBrand(CreateBrandModel brand)
        {

            var newBrand = _mapper.Map<Brand>(brand);

            _unitOfWork.BrandRepository.UpdateAsync(newBrand);
            _unitOfWork.Save();

            return Task.FromResult(new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.BRAND_CREATE_SUCCESS,
                Data = _mapper.Map<BrandModel>(newBrand)
            });
        }

        public async Task<BaseResponseModel> DeleteBrandById(int id)
        {
            var brand = await _unitOfWork.BrandRepository.GetByIdAsync(id);
            if (brand == null || brand.IsDeleted)
            {
                throw new NotExistException(MessageConstants.BRAND_NOT_FOUND);
            }

            _unitOfWork.BrandRepository.SoftDeleteAsync(brand);
            _unitOfWork.Save();
            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.BRAND_DELETE_SUCCESS
            };
        }

        public async Task<BaseResponseModel> GetBrandById(int id)
        {
            var brand = await _unitOfWork.BrandRepository.GetByIdAsync(id);

            if (brand == null || brand.IsDeleted)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.BRAND_NOT_FOUND,
                };
            }

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.BRAND_FOUND,
                Data = _mapper.Map<BrandModel>(brand)
            };
        }
        

        public async Task<BaseResponseModel> GetBrandsPaginationAsync(PaginationParameter paginationParameter)
        {
            var brand = await _unitOfWork.BrandRepository.ToPaginationIncludeAsync(
               paginationParameter,
               filter: query => !query.IsDeleted
               );
            var listBrand = _mapper.Map<Pagination<BrandModel>>(brand);

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_LIST_BRAND_SUCCESS,
                Data = new ModelPaging
                {
                    Data = listBrand,
                    MetaData = new
                    {
                        listBrand.TotalCount,
                        listBrand.PageSize,
                        listBrand.CurrentPage,
                        listBrand.TotalPages,
                        listBrand.HasNext,
                        listBrand.HasPrevious
                    }
                }
            };
        }

        public async Task<BaseResponseModel> UpdateBrand(BrandModel newBrand)
        {
            var eBrand = await _unitOfWork.BrandRepository.GetByIdAsync(newBrand.Id);
            if (eBrand == null || eBrand.IsDeleted)
            {
                throw new NotExistException(MessageConstants.BRAND_NOT_FOUND);
            }
            _mapper.Map(newBrand, eBrand);

            _unitOfWork.BrandRepository.UpdateAsync(eBrand);
            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.BRAND_UPDATE_SUCCESS,
                Data = _mapper.Map<BrandModel>(eBrand)
            };
        }
    }
}
