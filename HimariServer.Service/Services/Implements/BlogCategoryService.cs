using AutoMapper;
using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.BlogCategoryModels;
using HimariServer.Service.BusinessModels.BlogModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HimariServer.Service.Services.Implements
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BlogCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> AddBlogCategory(AddBlogCategoryModel blog)
        {
            if (blog == null)
            {
                throw new IsRequireDataException(MessageConstants.BLOG_CATEROGY_REQUIRE_DATA);
            }


            var blogEntity = _mapper.Map<BlogCategory>(blog);

            await _unitOfWork.BlogCategoryRepository.AddAsync(blogEntity);
            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status201Created,
                Message = MessageConstants.BLOG_CATEGORY_CREATE_SUCCESS,
                Data = _mapper.Map<BlogCategory>(blogEntity)
            };

        }

        public async Task<BaseResponseModel> DeleteBlogCategoryById(int id)
        {
            var blog = await _unitOfWork.BlogCategoryRepository.GetByIdIncludeAsync(id
                                      );
            if (blog == null || blog.IsDeleted)
            {
                throw new NotExistException(MessageConstants.BLOG_CATEGORY_NOT_FOUND);
            }

            _unitOfWork.BlogCategoryRepository.SoftDeleteAsync(blog);
            _unitOfWork.Save();
            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.BLOG_CATEGORY_DELETE_SUCCESS,
            };
        }

        public async Task<BaseResponseModel> GetBlogCategoryById(int id)
        {
            var blog = await _unitOfWork.BlogCategoryRepository.GetByIdIncludeAsync(id);

            if (blog == null || blog.IsDeleted)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.BLOG_CATEGORY_NOT_FOUND,
                };
            }

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.BLOG_CATEGORY_FOUND,
                Data = _mapper.Map<BlogCategoryModel>(blog)
            };
        }

        public async Task<BaseResponseModel> GetBlogsCategoryPaginationAsync(PaginationParameter paginationParameter, bool newestFirst, string? searchTerm)
        {
            string searchKeyword = string.IsNullOrEmpty(searchTerm) ? string.Empty : StringUtils.ConvertToUnSign(searchTerm.ToLower());
            var blog = await _unitOfWork.BlogCategoryRepository.ToPaginationIncludeAsync(
                     paginationParameter,

                     filter: query => !query.IsDeleted && (query.NameUnsign.Contains(searchKeyword)),
                     orderBy: query => newestFirst
                                    ? query.OrderByDescending(x => x.CreatedDate)
                                    : query.OrderBy(x => x.CreatedDate)
                     );
            var listBlog = _mapper.Map<Pagination<BlogCategoryModel>>(blog);

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_LIST_BLOG_CATEGORY_SUCCESS,
                Data = new ModelPaging
                {
                    Data = listBlog,
                    MetaData = new
                    {
                        listBlog.TotalCount,
                        listBlog.PageSize,
                        listBlog.CurrentPage,
                        listBlog.TotalPages,
                        listBlog.HasNext,
                        listBlog.HasPrevious
                    }
                }
            };
        }

        public async Task<BaseResponseModel> UpdateBlogCategory(UpdateBlogCategoryModel newBlog)
        {
            var blog = await _unitOfWork.BlogCategoryRepository.GetByIdAsync(newBlog.Id);
            if (blog == null || blog.IsDeleted)
            {
                throw new NotExistException(MessageConstants.BLOG_CATEGORY_NOT_FOUND);
            }


            _mapper.Map(newBlog, blog);

            _unitOfWork.BlogCategoryRepository.UpdateAsync(blog);
            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.BLOG_CATEGORY_UPDATE_SUCCESS,
                Data = _mapper.Map<BlogCategoryModel>(blog)
            };
        }
    }
}
