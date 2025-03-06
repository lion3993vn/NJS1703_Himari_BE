using AutoMapper;
using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.ProductSymptomModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class ProductSymptomService : IProductSymptomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductSymptomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> CreateProductSymptom(CreateProductSymptomModel productSymptom)
        {
            if (productSymptom.ProductId != null)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync((int)productSymptom.ProductId);
                if (product == null || product.IsDeleted)
                {
                    return new BaseResponseModel
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = MessageConstants.PRODUCT_NOT_FOUND,
                    };
                }
            }

            if (productSymptom.PartSymptomId != null)
            {
                var partSymptom = await _unitOfWork.PartSymptomRepository.GetByIdAsync((int)productSymptom.PartSymptomId);
                if (partSymptom == null || partSymptom.IsDeleted)
                {
                    return new BaseResponseModel
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = MessageConstants.SYMPTOM_NOT_FOUND,
                    };
                }
            }

            var newProductSymptom = _mapper.Map<ProductSymptom>(productSymptom);

            await _unitOfWork.ProductSymptomRepository.AddAsync(newProductSymptom);
            _unitOfWork.Save();

            var newProductSymptomInclude = await _unitOfWork.ProductSymptomRepository.GetByIdIncludeAsync(newProductSymptom.Id,
                include: query => query.Include(x => x.Product).Include(x => x.PartSymptom));

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.PRODUCT_SYMPTOM_CREATE_SUCCESS,
                Data = _mapper.Map<ProductSymptomModel>(newProductSymptomInclude)
            };
        }

        public async Task<BaseResponseModel> DeleteProductSymptomById(int id)
        {
            var productSymptom = await _unitOfWork.ProductSymptomRepository.GetByIdAsync(id);
            if (productSymptom == null || productSymptom.IsDeleted)
            {
                throw new NotExistException(MessageConstants.PRODUCT_SYMPTOM_NOT_FOUND);
            }

            _unitOfWork.ProductSymptomRepository.SoftDeleteAsync(productSymptom);
            _unitOfWork.Save();
            
            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.PRODUCT_SYMPTOM_DELETE_SUCCESS
            };
        }

        public async Task<BaseResponseModel> GetProductSymptomById(int id)
        {
            var productSymptom = await _unitOfWork.ProductSymptomRepository.GetByIdIncludeAsync(id,
                include: query => query.Include(x => x.Product).Include(x => x.PartSymptom));

            if (productSymptom == null || productSymptom.IsDeleted)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.PRODUCT_SYMPTOM_NOT_FOUND,
                };
            }

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.PRODUCT_SYMPTOM_FOUND,
                Data = _mapper.Map<ProductSymptomModel>(productSymptom)
            };
        }

        public async Task<BaseResponseModel> GetProductSymptomsByPartSymptomId(PaginationParameter paginationParameter, int partSymptomId)
        {
            var partSymptom = await _unitOfWork.PartSymptomRepository.GetByIdAsync(partSymptomId);
            if (partSymptom == null || partSymptom.IsDeleted)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.SYMPTOM_NOT_FOUND,
                };
            }

            var productSymptoms = await _unitOfWork.ProductSymptomRepository.ToPaginationIncludeAsync(
                paginationParameter,
                include: query => query.Include(x => x.Product).Include(x => x.PartSymptom),
                filter: query => !query.IsDeleted && query.PartSymptomId == partSymptomId
            );

            var listProductSymptom = _mapper.Map<Pagination<ProductSymptomModel>>(productSymptoms);

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_LIST_PRODUCT_SYMPTOM_SUCCESS,
                Data = new ModelPaging
                {
                    Data = listProductSymptom,
                    MetaData = new
                    {
                        listProductSymptom.TotalCount,
                        listProductSymptom.PageSize,
                        listProductSymptom.CurrentPage,
                        listProductSymptom.TotalPages,
                        listProductSymptom.HasNext,
                        listProductSymptom.HasPrevious
                    }
                }
            };
        }

        public async Task<BaseResponseModel> GetProductSymptomsByProductId(PaginationParameter paginationParameter, int productId)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null || product.IsDeleted)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.PRODUCT_NOT_FOUND,
                };
            }

            var productSymptoms = await _unitOfWork.ProductSymptomRepository.ToPaginationIncludeAsync(
                paginationParameter,
                include: query => query.Include(x => x.Product).Include(x => x.PartSymptom),
                filter: query => !query.IsDeleted && query.ProductId == productId
            );

            var listProductSymptom = _mapper.Map<Pagination<ProductSymptomModel>>(productSymptoms);

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_LIST_PRODUCT_SYMPTOM_SUCCESS,
                Data = new ModelPaging
                {
                    Data = listProductSymptom,
                    MetaData = new
                    {
                        listProductSymptom.TotalCount,
                        listProductSymptom.PageSize,
                        listProductSymptom.CurrentPage,
                        listProductSymptom.TotalPages,
                        listProductSymptom.HasNext,
                        listProductSymptom.HasPrevious
                    }
                }
            };
        }

        public async Task<BaseResponseModel> GetProductSymptomsPaginationAsync(PaginationParameter paginationParameter)
        {
            var productSymptoms = await _unitOfWork.ProductSymptomRepository.ToPaginationIncludeAsync(
                paginationParameter,
                include: query => query.Include(x => x.Product).Include(x => x.PartSymptom),
                filter: query => !query.IsDeleted
            );

            var listProductSymptom = _mapper.Map<Pagination<ProductSymptomModel>>(productSymptoms);

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_LIST_PRODUCT_SYMPTOM_SUCCESS,
                Data = new ModelPaging
                {
                    Data = listProductSymptom,
                    MetaData = new
                    {
                        listProductSymptom.TotalCount,
                        listProductSymptom.PageSize,
                        listProductSymptom.CurrentPage,
                        listProductSymptom.TotalPages,
                        listProductSymptom.HasNext,
                        listProductSymptom.HasPrevious
                    }
                }
            };
        }

        public async Task<BaseResponseModel> UpdateProductSymptom(UpdateProductSymptomModel productSymptom)
        {
            var existingProductSymptom = await _unitOfWork.ProductSymptomRepository.GetByIdAsync(productSymptom.Id);
            if (existingProductSymptom == null || existingProductSymptom.IsDeleted)
            {
                throw new NotExistException(MessageConstants.PRODUCT_SYMPTOM_NOT_FOUND);
            }

            if (productSymptom.ProductId != null)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync((int)productSymptom.ProductId);
                if (product == null || product.IsDeleted)
                {
                    return new BaseResponseModel
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = MessageConstants.PRODUCT_NOT_FOUND,
                    };
                }
            }

            if (productSymptom.PartSymptomId != null)
            {
                var partSymptom = await _unitOfWork.PartSymptomRepository.GetByIdAsync((int)productSymptom.PartSymptomId);
                if (partSymptom == null || partSymptom.IsDeleted)
                {
                    return new BaseResponseModel
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = MessageConstants.SYMPTOM_NOT_FOUND,
                    };
                }
            }

            _mapper.Map(productSymptom, existingProductSymptom);

            _unitOfWork.ProductSymptomRepository.UpdateAsync(existingProductSymptom);
            _unitOfWork.Save();

            var updatedProductSymptom = await _unitOfWork.ProductSymptomRepository.GetByIdIncludeAsync(existingProductSymptom.Id,
                include: query => query.Include(x => x.Product).Include(x => x.PartSymptom));

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.PRODUCT_SYMPTOM_UPDATE_SUCCESS,
                Data = _mapper.Map<ProductSymptomModel>(updatedProductSymptom)
            };
        }
    }
}
