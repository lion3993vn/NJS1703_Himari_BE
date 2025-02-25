using AutoMapper;
using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Service.BusinessModels.BlogCategoryModels;
using HimariServer.Service.BusinessModels.BlogModels;
using HimariServer.Service.BusinessModels.BodyPartModels;
using HimariServer.Service.BusinessModels.BrandModels;
using HimariServer.Service.BusinessModels.CategoryModels;
using HimariServer.Service.BusinessModels.NotificationModels;
using HimariServer.Service.BusinessModels.ProductModels;
using HimariServer.Service.BusinessModels.SymptomModels;
using HimariServer.Service.BusinessModels.UserDeviceModels;
using HimariServer.Service.BusinessModels.UserModels;
using HimariServer.Service.BusinessModels.PartSymptomModels;
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
            CreateMap<AddCategoryModel, Category>();

            // product
            CreateMap<Product, ProductModels>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : null))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand != null ? src.Brand.BrandName : null));
            CreateMap<Pagination<Product>, Pagination<ProductModels>>().ConvertUsing<PaginationConverter<Product, ProductModels>>();
            CreateMap<UpdateProductModel, Product>().ReverseMap();
            CreateMap<CreateProductModel, Product>().ReverseMap();

            // blog
            CreateMap<Blog, BlogModel>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : null))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User != null ? src.User.Id : (int?)null));
            CreateMap<Pagination<Blog>, Pagination<BlogModel>>().ConvertUsing<PaginationConverter<Blog, BlogModel>>();
            CreateMap<UpdateBlogModel, Blog>().ReverseMap();
            CreateMap<AddBlogModel, Blog>().ReverseMap();

            //Blog Category
            CreateMap<BlogCategory, BlogCategoryModel>().ReverseMap();
            CreateMap<Pagination<BlogCategory>, Pagination<BlogCategoryModel>>().ConvertUsing<PaginationConverter<BlogCategory, BlogCategoryModel>>();
            CreateMap<UpdateBlogCategoryModel, BlogCategory>().ReverseMap();
            CreateMap<AddBlogCategoryModel, BlogCategory>().ReverseMap();

            // body part
            CreateMap<BodyPart, BodyPartModel>().ReverseMap();
            CreateMap<Pagination<BodyPart>, Pagination<BodyPartModel>>().ConvertUsing<PaginationConverter<BodyPart, BodyPartModel>>();
            CreateMap<AddBodyPartModel, BodyPart>().ReverseMap();
            CreateMap<UpdateBodyPartModel, BodyPart>().ReverseMap();

            //brand
            CreateMap<Brand, BrandModel>().ReverseMap();
            CreateMap<Pagination<Brand>, Pagination<BrandModel>>().ConvertUsing<PaginationConverter<Brand, BrandModel>>();
            CreateMap<CreateBrandModel, Brand>().ReverseMap();

            // PartSymptom
            CreateMap<PartSymptom, SymptomModel>().ReverseMap();
            CreateMap<Pagination<PartSymptom>, Pagination<SymptomModel>>().ConvertUsing<PaginationConverter<PartSymptom, SymptomModel>>();
            CreateMap<CreateSymptomModel, PartSymptom>().ReverseMap();

            // PartSymptom
            CreateMap<PartSymptom, PartSymptomModel>().ReverseMap();
            CreateMap<Pagination<PartSymptom>, Pagination<PartSymptomModel>>().ConvertUsing<PaginationConverter<PartSymptom, PartSymptomModel>>();
            CreateMap<CreatePartSymptomModel, PartSymptom>().ReverseMap();

            // UserDevice
            CreateMap<CreateUserDeviceModel, UserDevice>().ReverseMap();
            CreateMap<UserDevice, UserDeviceModel>().ReverseMap();

            // Notification
            MapperNotification();
        }

        public void MapperNotification()
        {
            CreateMap<Notification, NotificationRequestModel>().ReverseMap();
            CreateMap<Notification, NotificationModel>().ReverseMap();
            CreateMap<UserNotification, NotificationModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Notification.Title))
                .ForMember(dest => dest.TitleUnsign, opt => opt.MapFrom(src => src.Notification.TitleUnsign))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Notification.Message))
                .ForMember(dest => dest.Href, opt => opt.MapFrom(src => src.Notification.Href))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Notification.Type))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.Notification.CreatedDate))
                .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => src.IsRead));
            CreateMap<Pagination<UserNotification>, Pagination<NotificationModel>>().ConvertUsing<PaginationConverter<UserNotification, NotificationModel>>();
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
}
