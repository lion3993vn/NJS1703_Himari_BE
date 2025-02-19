using AutoMapper;
using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.SymptomModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;


namespace HimariServer.Service.Services.Implements
{
    public class SymptomService : ISymptomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SymptomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> CreateSymptom(CreateSymptomModel symptom)
        {
            if (symptom.BodyPartId != null)
            {
                var bodyPart = await _unitOfWork.BodyPartRepository.GetByIdAsync((int)symptom.BodyPartId);
                if (bodyPart == null || bodyPart.IsDeleted)
                {
                    return new BaseResponseModel
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = MessageConstants.BODY_PART_NOT_FOUND,
                    };
                }
            }
            var newSymptom = _mapper.Map<PartSymptom>(symptom);

            _unitOfWork.SymptomRepository.UpdateAsync(newSymptom);
            _unitOfWork.Save();

            return await Task.FromResult(new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.SYMPTOM_CREATE_SUCCESS,
                Data = _mapper.Map<SymptomModel>(newSymptom)
            });
        }

        public async Task<BaseResponseModel> DeleteSymptomById(int id)
        {
            var symptom = await _unitOfWork.SymptomRepository.GetByIdAsync(id);
            if (symptom == null || symptom.IsDeleted)
            {
                throw new NotExistException(MessageConstants.SYMPTOM_NOT_FOUND);
            }

            _unitOfWork.SymptomRepository.SoftDeleteAsync(symptom);
            _unitOfWork.Save();
            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.SYMPTOM_DELETE_SUCCESS
            };
        }

        public async Task<BaseResponseModel> GetSymptomById(int id)
        {
            var symptom = await _unitOfWork.SymptomRepository.GetByIdAsync(id);

            if (symptom == null || symptom.IsDeleted)
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = MessageConstants.SYMPTOM_NOT_FOUND,
                };
            }

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.SYMPTOM_FOUND,
                Data = _mapper.Map<SymptomModel>(symptom)
            };
        }
        

        public async Task<BaseResponseModel> GetSymptomsPaginationAsync(PaginationParameter paginationParameter)
        {
            var symptom = await _unitOfWork.SymptomRepository.ToPaginationIncludeAsync(
               paginationParameter,
               filter: query => !query.IsDeleted
               );
            var listSymptom = _mapper.Map<Pagination<SymptomModel>>(symptom);

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_LIST_SYMPTOM_SUCCESS,
                Data = new ModelPaging
                {
                    Data = listSymptom,
                    MetaData = new
                    {
                        listSymptom.TotalCount,
                        listSymptom.PageSize,
                        listSymptom.CurrentPage,
                        listSymptom.TotalPages,
                        listSymptom.HasNext,
                        listSymptom.HasPrevious
                    }
                }
            };
        }

        public async Task<BaseResponseModel> UpdateSymptom(SymptomModel newSymptom)
        {
            var eSymptom = await _unitOfWork.SymptomRepository.GetByIdAsync(newSymptom.Id);
            if (eSymptom == null || eSymptom.IsDeleted)
            {
                throw new NotExistException(MessageConstants.SYMPTOM_NOT_FOUND);
            }
            if (newSymptom.BodyPartId != null)
            {
                var bodyPart = await _unitOfWork.BodyPartRepository.GetByIdAsync((int)newSymptom.BodyPartId);
                if (bodyPart == null || bodyPart.IsDeleted)
                {
                    return new BaseResponseModel
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = MessageConstants.BODY_PART_NOT_FOUND,
                    };
                }
            }
            _mapper.Map(newSymptom, eSymptom);

            _unitOfWork.SymptomRepository.UpdateAsync(eSymptom);
            _unitOfWork.Save();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.SYMPTOM_UPDATE_SUCCESS,
                Data = _mapper.Map<SymptomModel>(eSymptom)
            };
        }
    }
}
