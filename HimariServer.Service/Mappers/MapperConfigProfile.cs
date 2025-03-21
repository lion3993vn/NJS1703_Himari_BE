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
using HimariServer.Service.BusinessModels.UserDeviceModels;
using HimariServer.Service.BusinessModels.UserModels;
using HimariServer.Service.BusinessModels.PartSymptomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HimariServer.Service.BusinessModels.ChatMessageModels;
using HimariServer.Service.BusinessModels.ProductSymptomModels;
using HimariServer.Service.BusinessModels.PaymentModels;
using HimariServer.Service.BusinessModels.OrderModels;
using HimariServer.Repository.Enums;

namespace HimariServer.Service.Mappers
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile()
        {
            CreateMap<User, UserModel>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role != null ? src.Role.RoleName : null));
            CreateMap<UpdateUserModel, User>();

            // Add mapping for pagination of UserModel (replacing UserListModel)
            CreateMap<Pagination<User>, Pagination<UserModel>>().ConvertUsing<PaginationConverter<User, UserModel>>();

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
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
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
            CreateMap<PartSymptom, PartSymptomModel>().ReverseMap();
            CreateMap<Pagination<PartSymptom>, Pagination<PartSymptomModel>>().ConvertUsing<PaginationConverter<PartSymptom, PartSymptomModel>>();
            CreateMap<CreatePartSymptomModel, PartSymptom>().ReverseMap();

            // UserDevice
            CreateMap<CreateUserDeviceModel, UserDevice>().ReverseMap();
            CreateMap<UserDevice, UserDeviceModel>().ReverseMap();

            // Notification
            MapperNotification();

            CreateMap<ChatMessageModel, ChatMessage>().ReverseMap();
            CreateMap<Pagination<ChatMessage>, Pagination<ChatMessageModel>>().ConvertUsing<PaginationConverter<ChatMessage, ChatMessageModel>>();

            // ProductSymptom
            CreateMap<ProductSymptom, ProductSymptomModel>()
                .ForMember(dest => dest.PartSymptomName, opt => opt.MapFrom(src => src.PartSymptom != null ? src.PartSymptom.Name : null))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductName : null));
            CreateMap<Pagination<ProductSymptom>, Pagination<ProductSymptomModel>>().ConvertUsing<PaginationConverter<ProductSymptom, ProductSymptomModel>>();
            CreateMap<CreateProductSymptomModel, ProductSymptom>().ReverseMap();
            CreateMap<UpdateProductSymptomModel, ProductSymptom>().ReverseMap();

            CreateMap<PaymentModels, Payment>().ReverseMap();

            MapperOrder();

        }

        public void MapperOrder()
        {
            CreateMap<Order, OrderResponseModel>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
                .ForMember(dest => dest.DeliveryStatus, opt => opt.MapFrom(src => (int)src.DeliveryStatus)) 
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName)) 
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.Payments != null && src.Payments.Any()
                    ? src.Payments.FirstOrDefault().Status
                    : PaymentStatus.Pending))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
            CreateMap<Pagination<Order>, Pagination<OrderResponseModel>>().ConvertUsing<PaginationConverter<Order, OrderResponseModel>>();
            //get order không lấy order details
            CreateMap<Order, BasicOrderResponseModel>()
                .ForMember(dest => dest.DeliveryStatus, opt => opt.MapFrom(src => (int)src.DeliveryStatus))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.Payments != null && src.Payments.Any()
                    ? src.Payments.FirstOrDefault().Status
                    : PaymentStatus.Pending))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => (int)src.UserId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
            CreateMap<Pagination<Order>, Pagination<BasicOrderResponseModel>>().ConvertUsing<PaginationConverter<Order, BasicOrderResponseModel>>();

            CreateMap<OrderDetail, OrderDetailsModel>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductName : null))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product != null ? src.Product.ImageUrl : null));
        }

        public void MapperNotification()
        {
            CreateMap<Notification, NotificationRequestModel>().ReverseMap();
            CreateMap<Notification, SystemNotificationModel>().ReverseMap();
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
            CreateMap<Pagination<Notification>, Pagination<NotificationModel>>().ConvertUsing<PaginationConverter<Notification, NotificationModel>>();
            CreateMap<Pagination<Notification>, Pagination<SystemNotificationModel>>().ConvertUsing<PaginationConverter<Notification, SystemNotificationModel>>();
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
