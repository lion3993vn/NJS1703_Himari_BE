using AutoMapper;
using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Service.BusinessModels.CategoryModels;
using HimariServer.Service.BusinessModels.ProductModels;
using HimariServer.Service.BusinessModels.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Mappers
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile()
        {
            CreateMap<User, UserModel>().ReverseMap();

            CreateMap<Category, CategoryModel>()
            .ForMember(dest => dest.ParentCategoryName, opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.CategoryName : null));
            CreateMap<Pagination<Category>, Pagination<CategoryModel>>().ConvertUsing<PaginationConverter<Category, CategoryModel>>();
            CreateMap<CategoryUpdateModel, Category>();

            CreateMap<Product, ProductModels>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName: null));
            CreateMap<Pagination<Product>, Pagination<ProductModels>>().ConvertUsing<PaginationConverter<Product, ProductModels>>();
            CreateMap<UpdateProductModel, Product>().ReverseMap();
        }
    }

    public class PaginationConverter<TSource, TDestination> : ITypeConverter<Pagination<TSource>, Pagination<TDestination>>
    {
        public Pagination<TDestination> Convert(Pagination<TSource> source, Pagination<TDestination> destination, ResolutionContext context)
        {
            var mappedItems = context.Mapper.Map<List<TDestination>>(source);
            return new Pagination<TDestination>(mappedItems, source.TotalCount, source.CurrentPage, source.PageSize);
        }
    }
}
