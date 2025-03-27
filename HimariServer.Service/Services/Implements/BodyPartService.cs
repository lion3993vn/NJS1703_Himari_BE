using AutoMapper;
using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.BodyPartModels;
using HimariServer.Service.BusinessModels.CategoryModels;
using HimariServer.Service.BusinessModels.ProductModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.Utils;
using MailKit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HimariServer.Service.Services.Implements
{
    public class BodyPartService : IBodyPartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BodyPartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> AddBodyPart(AddBodyPartModel model)
        {
            var bodyPart = _mapper.Map<Repository.Entities.BodyPart>(model);
            bodyPart.BodyPartNameUnsign = StringUtils.ConvertToUnSign(bodyPart.BodyPartName);
            await _unitOfWork.BodyPartRepository.AddAsync(bodyPart);
            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.ADD_BODY_PART_SUCCESS,
                Data = _mapper.Map<BodyPartModel>(bodyPart)
            };
        }

        public async Task<BaseResponseModel> DeleteBodyPartByIdAsync(int id)
        {
            var bodyPart = await _unitOfWork.BodyPartRepository.GetByIdIncludeAsync(id, filter: query => !query.IsDeleted);

            if (bodyPart == null)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.BODY_PART_NOT_FOUND,
                };
            }
            if (await _unitOfWork.PartSymptomRepository.IsContainPartSymptom(id))
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = MessageConstants.BODY_PART_HAS_PART_SYMPTOM
                };
            }


            _unitOfWork.BodyPartRepository.SoftDeleteAsync(bodyPart);
            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.BODY_PART_DELETE_SUCCESS,
            };
        }

        public async Task<BaseResponseModel> GetBodyPartByIdAsync(int id)
        {
            var bodyPart = await _unitOfWork.BodyPartRepository.GetByIdIncludeAsync(id, filter: query => !query.IsDeleted);

            if(bodyPart == null)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.BODY_PART_NOT_FOUND,
                };
            }

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_BODY_PART_SUCCESS,
                Data = _mapper.Map<BodyPartModel>(bodyPart)
            };
        }

        public async Task<BaseResponseModel> GetBodyPartsPaginationAsync(PaginationParameter paginationParameter, bool newestFirst, string? searchTerm)
        {
            string searchKeyword = string.IsNullOrEmpty(searchTerm) ? string.Empty : StringUtils.ConvertToUnSign(searchTerm.ToLower());
            var bodyParts = await _unitOfWork.BodyPartRepository.ToPaginationIncludeAsync(paginationParameter,
            filter: query => !query.IsDeleted && query.BodyPartNameUnsign.Contains(searchKeyword),
            orderBy: query => newestFirst
                    ? query.OrderByDescending(x => x.CreatedDate)
                    : query.OrderBy(x => x.CreatedDate)
            );

            var listBodyPart = _mapper.Map<Pagination<BodyPartModel>>(bodyParts);

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_LIST_BODY_PART_SUCCESS,
                Data = new ModelPaging
                {
                    Data = listBodyPart,
                    MetaData = new
                    {
                        listBodyPart.TotalCount,
                        listBodyPart.PageSize,
                        listBodyPart.CurrentPage,
                        listBodyPart.TotalPages,
                        listBodyPart.HasNext,
                        listBodyPart.HasPrevious
                    }
                }
            };
        }

        public async Task<BaseResponseModel> UpdateBodyPart(UpdateBodyPartModel model)
        {
            var bodyPart = await _unitOfWork.BodyPartRepository.GetByIdIncludeAsync(model.Id,
                filter: query => !query.IsDeleted);

            if(bodyPart == null)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.BODY_PART_NOT_FOUND
                };
            }

            _mapper.Map(model, bodyPart);
            bodyPart.BodyPartNameUnsign = StringUtils.ConvertToUnSign(bodyPart.BodyPartName);
            _unitOfWork.BodyPartRepository.UpdateAsync(bodyPart);
            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.UPDATE_BODY_PART_SUCCESS,
                Data = _mapper.Map<BodyPartModel>(bodyPart)
            };
        }
    }
}
