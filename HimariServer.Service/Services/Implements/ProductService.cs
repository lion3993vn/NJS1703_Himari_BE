using AutoMapper;
using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.CategoryModels;
using HimariServer.Service.BusinessModels.ProductModels;
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
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> DeleteProductById(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdIncludeAsync(id,
                include: query => query.Include(x => x.Category));
            if (product == null || product.IsDeleted)
            {
                throw new NotExistException(MessageConstants.PRODUCT_NOT_FOUND);
            }

            _unitOfWork.ProductRepository.SoftDeleteAsync(product);
            _unitOfWork.Save();
            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.PRODUCT_DELETE_SUCCESS
            };
        }

        public async Task<BaseResponseModel> GetProductById(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdIncludeAsync(id,
               include: query => query.Include(x => x.Category));

            if (product == null || product.IsDeleted)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.PRODUCT_NOT_FOUND,
                };
            }

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.PRODUCT_FOUND,
                Data = _mapper.Map<ProductModels>(product)
            };
        }
        

        public async Task<BaseResponseModel> GetProductsPaginationAsync(PaginationParameter paginationParameter)
        {
            var product = await _unitOfWork.ProductRepository.ToPaginationIncludeAsync(
               paginationParameter,
               include: query => query.Include(x => x.Category),
               filter: query => !query.IsDeleted
               );
            var listProduct = _mapper.Map<Pagination<ProductModels>>(product);

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_LIST_PRODUCT_SUCCESS,
                Data = new ModelPaging
                {
                    Data = listProduct,
                    MetaData = new
                    {
                        listProduct.TotalCount,
                        listProduct.PageSize,
                        listProduct.CurrentPage,
                        listProduct.TotalPages,
                        listProduct.HasNext,
                        listProduct.HasPrevious
                    }
                }
            };
        }

        public async Task<BaseResponseModel> UpdateProduct(UpdateProductModel newProduct)
        {
            var eProduct = await _unitOfWork.ProductRepository.GetByIdAsync(newProduct.Id);
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync((int) newProduct.CategoryId);
            if (eProduct == null || eProduct.IsDeleted)
            {
                throw new NotExistException(MessageConstants.PRODUCT_NOT_FOUND);
            }
            
            if (category == null || category.IsDeleted)
            {
                throw new NotExistException(MessageConstants.CATEGORY_NOT_FOUND);
            }
            _mapper.Map(newProduct, eProduct);
            //TODO add check brand
            // xoa cai ben duoi 
            eProduct.BrandId = null;



            _unitOfWork.ProductRepository.UpdateAsync(eProduct);
            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.PRODUCT_UPDATE_SUCCESS,
                Data = _mapper.Map<ProductModels>(eProduct)
            };
        }
    }
}
