using AutoMapper;
using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Enums;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.CategoryModels;
using HimariServer.Service.BusinessModels.ProductModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.Utils;
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

        public async Task<BaseResponseModel> CreateProduct(CreateProductModel product)
        {
            if (product.CategoryId != null)
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync((int)product.CategoryId);

                if (category == null || category.IsDeleted)
                {
                    throw new NotExistException(MessageConstants.CATEGORY_NOT_FOUND);
                }
            }

            var brand = await _unitOfWork.BrandRepository.GetByIdAsync((int)product.BrandId);
            if (brand == null)
            {
                throw new NotExistException(MessageConstants.BRAND_NOT_FOUND);
            }

            var newProduct = _mapper.Map<Product>(product);
            newProduct.ProductNameUnsign = StringUtils.ConvertToUnSign(newProduct.ProductName);
            await _unitOfWork.ProductRepository.AddAsync(newProduct);
            _unitOfWork.Save();

            var newProductInclude = await _unitOfWork.ProductRepository.GetByIdIncludeAsync(newProduct.Id,
                include: query => query.Include(x => x.Category).Include(x => x.Brand));

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.PRODUCT_CREATE_SUCCESS,
                Data = _mapper.Map<ProductModels>(newProductInclude)
            };
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

        public async Task<BaseResponseModel> GetFeaturedProducts(PaginationParameter paginationParameter)
        {
            var product = await _unitOfWork.ProductRepository.ToPaginationIncludeAsync(
                paginationParameter,
                include: query => query.Include(x => x.Category)
                                       .Include(x => x.OrderDetails)
                                          .ThenInclude(od => od.Order)
                                             .ThenInclude(o => o.Payments)
                                       .Include(x => x.Brand),
                filter: query => !query.IsDeleted,
                orderBy: query => query.OrderByDescending(p =>
                                        p.OrderDetails.Sum(od => od.Quantity))
            );

            var productDict = product.ToDictionary(p => p.Id);

            foreach (var item in listProduct)
            {
                if (productDict.TryGetValue(item.Id, out var productEntity))
                {
                    item.Sold = productEntity.OrderDetails
                        .Where(od => od.Order.Payments != null &&
                               od.Order.Payments.Any(p => p.Status == PaymentStatus.Success))
                        .Sum(od => od.Quantity);
                }
            }


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

        public async Task<BaseResponseModel> GetProductById(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdIncludeAsync(id,
               include: query => query.Include(x => x.Category)
                                      .Include(x => x.Brand)
                                      .Include(x => x.OrderDetails)
                                        .ThenInclude(od => od.Order)
                                            .ThenInclude(o => o.Payments));

            if (product == null || product.IsDeleted)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.PRODUCT_NOT_FOUND,
                };
            }

            var productModel = _mapper.Map<ProductModels>(product);
            
            // Calculate sold count from successfully paid orders
            productModel.Sold = product.OrderDetails
                .Where(od => od.Order.Payments != null && 
                       od.Order.Payments.Any(p => p.Status == Repository.Enums.PaymentStatus.Success))
                .Sum(od => od.Quantity);

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.PRODUCT_FOUND,
                Data = productModel
            };
        }

        public async Task<BaseResponseModel> GetProductsByCategory(PaginationParameter paginationParameter, int categoryId)
        {
            if (categoryId != null)
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);

                if (category == null || category.IsDeleted)
                {
                    throw new NotExistException(MessageConstants.CATEGORY_NOT_FOUND);
                }
            }
            var product = await _unitOfWork.ProductRepository.ToPaginationIncludeAsync(
               paginationParameter,
               include: query => query.Include(x => x.Category)
                                      .Include(x => x.Brand)
                                      .Include(x => x.OrderDetails)
                                        .ThenInclude(od => od.Order)
                                            .ThenInclude(o => o.Payments),
               filter: query => !query.IsDeleted && query.CategoryId == categoryId
               );
               
            var listProduct = _mapper.Map<Pagination<ProductModels>>(product);
            
            // Calculate sold counts for all products
            foreach (var item in listProduct)
            {
                var productEntity = product.FirstOrDefault(p => p.Id == item.Id);
                if (productEntity != null)
                {
                    item.Sold = productEntity.OrderDetails
                        .Where(od => od.Order.Payments != null && 
                               od.Order.Payments.Any(p => p.Status == Repository.Enums.PaymentStatus.Success))
                        .Sum(od => od.Quantity);
                }
            }

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

        public async Task<BaseResponseModel> GetProductsPaginationAsync(PaginationParameter paginationParameter)
        {
            var product = await _unitOfWork.ProductRepository.ToPaginationIncludeAsync(
               paginationParameter,
               include: query => query.Include(x => x.Category)
                                      .Include(x => x.Brand)
                                      .Include(x => x.OrderDetails)
                                        .ThenInclude(od => od.Order)
                                            .ThenInclude(o => o.Payments),
               filter: query => !query.IsDeleted
               );
               
            var listProduct = _mapper.Map<Pagination<ProductModels>>(product);
            
            // Calculate sold counts for all products
            foreach (var item in listProduct)
            {
                var productEntity = product.FirstOrDefault(p => p.Id == item.Id);
                if (productEntity != null)
                {
                    item.Sold = productEntity.OrderDetails
                        .Where(od => od.Order.Payments != null && 
                               od.Order.Payments.Any(p => p.Status == Repository.Enums.PaymentStatus.Success))
                        .Sum(od => od.Quantity);
                }
            }

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
            var eProduct = await _unitOfWork.ProductRepository.GetByIdIncludeAsync(newProduct.Id,
                                    include: query => query.Include(x => x.Category).Include(x => x.Brand));
            if (eProduct == null || eProduct.IsDeleted)
            {
                throw new NotExistException(MessageConstants.PRODUCT_NOT_FOUND);
            }

            var brand = await _unitOfWork.BrandRepository.GetByIdAsync((int)newProduct.BrandId);
            if(brand == null)
            {
                throw new NotExistException(MessageConstants.BRAND_NOT_FOUND);
            }
            if (newProduct.CategoryId != null)
            {
                var category = await _unitOfWork.CategoryRepository.GetByIdAsync((int)newProduct.CategoryId);
                if (category == null || category.IsDeleted)
                {
                    throw new NotExistException(MessageConstants.CATEGORY_NOT_FOUND);
                }
            }
            _mapper.Map(newProduct, eProduct);

            _unitOfWork.ProductRepository.UpdateAsync(eProduct);
            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.PRODUCT_UPDATE_SUCCESS,
                Data = _mapper.Map<ProductModels>(eProduct)
            };
        }

        public async Task<BaseResponseModel> GetProductsByBrand(PaginationParameter paginationParameter, int brandId)
        {
            var brand = await _unitOfWork.BrandRepository.GetByIdAsync(brandId);
            if (brand == null)
            {
                throw new NotExistException(MessageConstants.BRAND_NOT_FOUND);
            }

            var product = await _unitOfWork.ProductRepository.ToPaginationIncludeAsync(
                paginationParameter,
                include: query => query.Include(x => x.Category)
                                      .Include(x => x.Brand)
                                      .Include(x => x.OrderDetails)
                                        .ThenInclude(od => od.Order)
                                            .ThenInclude(o => o.Payments),
                filter: query => !query.IsDeleted && query.BrandId == brandId
            );
            
            var listProduct = _mapper.Map<Pagination<ProductModels>>(product);
            
            // Calculate sold counts for all products
            foreach (var item in listProduct)
            {
                var productEntity = product.FirstOrDefault(p => p.Id == item.Id);
                if (productEntity != null)
                {
                    item.Sold = productEntity.OrderDetails
                        .Where(od => od.Order.Payments != null && 
                               od.Order.Payments.Any(p => p.Status == Repository.Enums.PaymentStatus.Success))
                        .Sum(od => od.Quantity);
                }
            }

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

        public async Task<BaseResponseModel> SearchProductsByKeyword(PaginationParameter paginationParameter, string keyword)
        {
            string searchKeyword = string.IsNullOrEmpty(keyword) ? string.Empty : StringUtils.ConvertToUnSign(keyword.ToLower());
            
            var product = await _unitOfWork.ProductRepository.ToPaginationIncludeAsync(
                paginationParameter,
                include: query => query.Include(x => x.Category)
                                     .Include(x => x.Brand)
                                     .Include(x => x.OrderDetails)
                                       .ThenInclude(od => od.Order)
                                           .ThenInclude(o => o.Payments),
                filter: query => !query.IsDeleted && query.ProductNameUnsign.Contains(searchKeyword)
            );
            
            var listProduct = _mapper.Map<Pagination<ProductModels>>(product);
            
            // Calculate sold counts for all products
            foreach (var item in listProduct)
            {
                var productEntity = product.FirstOrDefault(p => p.Id == item.Id);
                if (productEntity != null)
                {
                    item.Sold = productEntity.OrderDetails
                        .Where(od => od.Order.Payments != null && 
                               od.Order.Payments.Any(p => p.Status == Repository.Enums.PaymentStatus.Success))
                        .Sum(od => od.Quantity);
                }
            }

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
    }
}
