using AutoMapper;
using HimariServer.Repository.Entities;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.PartSymptomModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using HimariServer.Repository.Commons;
using HimariServer.Service.Utils; // Add this using statement

namespace HimariServer.Service.Services.Implements
{
    public class PartSymptomService : IPartSymptomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PartSymptomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> CreatePartSymptom(CreatePartSymptomModel model)
        {
            var partSymptom = _mapper.Map<PartSymptom>(model);
            partSymptom.NameUnsign = StringUtils.ConvertToUnSign(partSymptom.Name);
            await _unitOfWork.PartSymptomRepository.AddAsync(partSymptom);
            await _unitOfWork.SaveAsync();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.PART_SYMPTOM_CREATE_SUCCESS,
                Data = _mapper.Map<PartSymptomModel>(partSymptom)
            };
        }

        public async Task<BaseResponseModel> GetPartSymptomById(int id)
        {
            var partSymptom = await _unitOfWork.PartSymptomRepository.GetByIdAsync(id);
            if (partSymptom == null)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.PART_SYMPTOM_NOT_FOUND
                };
            }

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.PART_SYMPTOM_FOUND,
                Data = _mapper.Map<PartSymptomModel>(partSymptom)
            };
        }

        public async Task<BaseResponseModel> UpdatePartSymptom(PartSymptomModel model)
        {
            var partSymptom = await _unitOfWork.PartSymptomRepository.GetByIdAsync(model.Id);
            if (partSymptom == null)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.PART_SYMPTOM_NOT_FOUND
                };
            }

            _mapper.Map(model, partSymptom);
            _unitOfWork.PartSymptomRepository.UpdateAsync(partSymptom);
            await _unitOfWork.SaveAsync();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.PART_SYMPTOM_UPDATE_SUCCESS,
                Data = _mapper.Map<PartSymptomModel>(partSymptom)
            };
        }

        public async Task<BaseResponseModel> DeletePartSymptomById(int id)
        {
            var partSymptom = await _unitOfWork.PartSymptomRepository.GetByIdAsync(id);
            if (partSymptom == null)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.PART_SYMPTOM_NOT_FOUND
                };
            }

            _unitOfWork.PartSymptomRepository.SoftDeleteAsync(partSymptom);
            await _unitOfWork.SaveAsync();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.PART_SYMPTOM_DELETE_SUCCESS
            };
        }

        public async Task<BaseResponseModel> GetPartSymptomsPaginationAsync(PaginationParameter paginationParameter)
        {
            var partSymptoms = await _unitOfWork.PartSymptomRepository.ToPaginationIncludeAsync(paginationParameter, filter: x => !x.IsDeleted);
            var partSymptomModels = _mapper.Map<Pagination<PartSymptomModel>>(partSymptoms);

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_LIST_PART_SYMPTOM_SERVICE,
                Data = new ModelPaging
                {
                    Data = partSymptomModels,
                    MetaData = new
                    {
                        partSymptomModels.TotalCount,
                        partSymptomModels.PageSize,
                        partSymptomModels.CurrentPage,
                        partSymptomModels.TotalPages,
                        partSymptomModels.HasNext,
                        partSymptomModels.HasPrevious
                    }
                }
            };
        }
    }
}
