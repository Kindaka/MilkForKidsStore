using AutoMapper;
using MilkStore_BAL.ModelViews.AccountDTOs;
using MilkStore_BAL.ModelViews.CartDTOs;
using MilkStore_BAL.ModelViews.ChatDTOs;
using MilkStore_BAL.ModelViews.BlogDTOs;
using MilkStore_BAL.ModelViews.FeedbackDTOs;
using MilkStore_BAL.ModelViews.OrderDetailDTOs;
using MilkStore_BAL.ModelViews.OrderDTOs;
using MilkStore_BAL.ModelViews.PaymentDTOs;
using MilkStore_BAL.ModelViews.ProductDTOs;
using MilkStore_BAL.ModelViews.VoucherOfShopDTOs;
using MilkStore_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_BAL.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, UserAuthenticatingDtoResponse>().ReverseMap();
            CreateMap<ProductCategory, CategoryDto>().ReverseMap();
            CreateMap<Product, ProductDtoResponse>().ReverseMap();
            CreateMap<Product, ProductDtoRequest>().ReverseMap();
            CreateMap<Product, ProductDtoUsingFireBaseRequest>().ReverseMap();
            CreateMap<Account, UserRegisterDtoRequest>().ReverseMap();
            CreateMap<VoucherOfShop, VoucherOfShopDtoRequest>().ReverseMap();
            CreateMap<VoucherOfShop, VoucherOfShopDtoResponse>().ReverseMap();
            CreateMap<VoucherOfShop, VoucherOfShopDtoResponseForAdmin>().ReverseMap();
            CreateMap<Cart, CartDtoRequest>().ReverseMap();
            CreateMap<Cart, CartDtoResponse>().ReverseMap();
            CreateMap<Order, OrderDtoResponse>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDtoResponse>().ReverseMap();
            CreateMap<Payment, PaymentDtoResponse>().ReverseMap();
            CreateMap<ChatRequest, MessageDtoRequest>().ReverseMap();
            CreateMap<ChatRequest, MessageDtoResponse>().ReverseMap();
            CreateMap<Feedback, FeedbackDtoRequest>().ReverseMap();
            CreateMap<Feedback, FeedbackDtoResponse>().ReverseMap();
            CreateMap<Feedback, UpdateFeedbackDtoRequest>().ReverseMap();
            CreateMap<Blog, BlogDtoResponse>().ReverseMap();
                       //.ForMember(dest => dest.BlogProductId, opt => opt.MapFrom(src => src.BlogProducts.Select(bp => bp.ProductId)))
                       //.ForMember(dest => dest.BlogProducts, opt => opt.MapFrom(src => src.BlogProducts.Select(bp => new ProductDtoResponse
                       //{
                       //    ProductId = bp.ProductId,
                       //    ProductName = bp.Product.ProductName,
                       //    ProductPrice = (double)bp.Product.ProductPrice
                       //}))); 
            CreateMap<BlogDtoRequest, Blog>();
        }
    }
}
