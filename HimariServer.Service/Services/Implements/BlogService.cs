using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Repositories.Interfaces;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.BlogModels;
using HimariServer.Service.BusinessModels.ProductModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HimariServer.Service.Services.Implements
{
    public class BlogService : IBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public BlogService(IUnitOfWork unitOfWork, IMapper mapper,IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<BaseResponseModel> AddBlog(AddBlogModel blogModel)
        {


            if (blogModel == null)
            {
                throw new IsRequireDataException(MessageConstants.BLOG_REQUIRE_DATA);
            }

            var user = await _unitOfWork.UsersRepository.GetByIdAsync((int)blogModel.UserId);
            if (user == null)
            {
                throw new NotExistException(MessageConstants.ACCOUNT_NOT_EXIST);
            }

            var blogEntity = _mapper.Map<Blog>(blogModel);


            await _unitOfWork.BlogRepository.AddAsync(blogEntity);
            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status201Created,
                Message = MessageConstants.BLOG_CREATE_SUCCESS,
                Data = _mapper.Map<BlogModel>(blogEntity)
            };
        }


        public async Task<BaseResponseModel> DeleteBlogById(int id)
        {
            var blog = await _unitOfWork.BlogRepository.GetByIdIncludeAsync(id,
                           include: query => query.Include(x => x.User));
            if (blog == null || blog.IsDeleted)
            {
                throw new NotExistException(MessageConstants.PRODUCT_NOT_FOUND);
            }

            _unitOfWork.BlogRepository.SoftDeleteAsync(blog);
            _unitOfWork.Save();
            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.BLOG_DELETE_SUCCESS,
            };
        }

        public async Task<BaseResponseModel> GetBlogById(int id)
        {
            var blog = await _unitOfWork.BlogRepository.GetByIdIncludeAsync(id,
                include: query => query.Include(x => x.User).AsNoTracking());

            if (blog == null || blog.IsDeleted)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.BLOG_NOT_FOUND,
                };
            }

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.BLOG_FOUND,
                Data = _mapper.Map<BlogModel>(blog)
            };
        }

        public async Task<BaseResponseModel> GetBlogsPaginationAsync(PaginationParameter paginationParameter)
        {
            var blog = await _unitOfWork.BlogRepository.ToPaginationIncludeAsync(
           paginationParameter,
           include: query => query.Include(x => x.User),
           filter: query => !query.IsDeleted
           );
            var listBlog = _mapper.Map<Pagination<BlogModel>>(blog);

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_LIST_BLOG_SUCCESS,
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

        public async Task<BaseResponseModel> UpdateBlog(int id , UpdateBlogModel newBlog)
        {
            var blog = await _unitOfWork.BlogRepository.GetByIdAsync(id);
            if (blog == null || blog.IsDeleted)
            {
                throw new NotExistException(MessageConstants.PRODUCT_NOT_FOUND);
            }

          
            _mapper.Map(newBlog, blog);

            _unitOfWork.BlogRepository.UpdateAsync(blog);
            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.BLOG_UPDATE_SUCCESS,
                Data = _mapper.Map<BlogModel>(blog)
            };
        }

    
    }
}
