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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> DeleteCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdIncludeAsync(
                id,
                filter: query => !query.IsDeleted
                );

            if (category == null)
            {
                throw new NotExistException(MessageConstants.CATEGORY_NOT_FOUND);
            }

            _unitOfWork.CategoryRepository.SoftDeleteAsync(category);
            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.CATEGORY_DELETE_SUCCESS
            };
        }

        public async Task<BaseResponseModel> GetCategoriesPaginationAsync(PaginationParameter paginationParameter)
        {
            var category = await _unitOfWork.CategoryRepository.ToPaginationIncludeAsync(
                paginationParameter,
                include: query => query.Include(c => c.ParentCategory),
                filter: query => !query.IsDeleted
                );

            var listCategory = _mapper.Map<Pagination<CategoryModel>>(category);

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_LIST_CATEGORY_SUCCESS,
                Data = new ModelPaging
                {
                    Data = listCategory,
                    MetaData = new
                    {
                        listCategory.TotalCount,
                        listCategory.PageSize,
                        listCategory.CurrentPage,
                        listCategory.TotalPages,
                        listCategory.HasNext,
                        listCategory.HasPrevious
                    }
                }
            };
        }

        public async Task<BaseResponseModel> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdIncludeAsync(
                id,
                include: query => query.Include(c => c.ParentCategory),
                filter: query => !query.IsDeleted
                );

            if (category == null)
            {
                throw new NotExistException(MessageConstants.CATEGORY_NOT_FOUND);
            }

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_CATEGORY_SUCCESS,
                Data = _mapper.Map<CategoryModel>(category)
            };
        }

        public async Task<BaseResponseModel> UpdateCategory(CategoryUpdateModel model)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdIncludeAsync(
                model.id,
                include: query => query.Include(c => c.ParentCategory),
                filter: query => !query.IsDeleted
                );

            if (category == null)
            {
                throw new NotExistException(MessageConstants.CATEGORY_NOT_FOUND);
            }

            if(model.ParentCategoryId != null)
            {
                var parentCategory = await _unitOfWork.CategoryRepository.GetByIdIncludeAsync(
                (int)model.ParentCategoryId,
                include: query => query.Include(c => c.ParentCategory),
                filter: query => !query.IsDeleted
                );

                if (parentCategory == null)
                {
                    throw new NotExistException(MessageConstants.CATEGORY_PARENT_NOT_FOUND);
                }
            }

            _mapper.Map(model, category);
            _unitOfWork.CategoryRepository.UpdateAsync(category);
            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.CATEGORY_UPDATE_SUCCESS,
                Data = _mapper.Map<CategoryModel>(category)
            };
        }
    }
}
