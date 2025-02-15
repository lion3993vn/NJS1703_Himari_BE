using AutoMapper;
using HimariServer.Repository.Commons;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.CategoryModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
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

        public async Task<BaseResponseModel> GetCategoriesPaginationAsync(PaginationParameter paginationParameter)
        {
            var category = await _unitOfWork.CategoryRepository.ToPaginationIncludeAsync(
                paginationParameter,
                include: query => query.Include(c => c.ParentCategory)
                );

            var listCategory = _mapper.Map<Pagination<CategoryModels>>(category);

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
    }
}
