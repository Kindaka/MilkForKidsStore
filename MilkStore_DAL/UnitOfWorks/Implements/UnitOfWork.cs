using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MilkStore_DAL.Entities;
using MilkStore_DAL.Repositories.Implements;
using MilkStore_DAL.Repositories.Interfaces;
using MilkStore_DAL.UnitOfWorks.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_DAL.UnitOfWorks.Implements
{
    public class UnitOfWork : IUnitOfWork
    {
        private MomAndKidsContext _context;
        private GenericRepository<Account> _accountRepository;
        private GenericRepository<Blog> _blogRepository;
        private GenericRepository<Cart> _cartRepository;
        private GenericRepository<Feedback> _feedbackRepository;
        private GenericRepository<Order> _orderRepository;
        private GenericRepository<OrderDetail> _orderDetailRepository;
        private GenericRepository<Product> _productRepository;
        private GenericRepository<ProductCategory> _productCategoryRepository;
        private GenericRepository<Rating> _ratingRepository;
        private GenericRepository<Shop> _shopRepository;
        private GenericRepository<VoucherOfshop> _voucherOfshopRepository;

        public UnitOfWork(MomAndKidsContext context)
        {
            _context = context;
        }

        public IGenericRepository<Account> AccountRepository => _accountRepository ??= new GenericRepository<Account>(_context);

        public IGenericRepository<Blog> BlogRepository => _blogRepository ??= new GenericRepository<Blog>(_context);

        public IGenericRepository<Cart> CartRepository => _cartRepository ??= new GenericRepository<Cart>(_context);

        public IGenericRepository<Feedback> FeedbackRepository => _feedbackRepository ??= new GenericRepository<Feedback>(_context);

        public IGenericRepository<Order> OrderRepository => _orderRepository ??= new GenericRepository<Order>(_context);

        public IGenericRepository<OrderDetail> OrderDetailRepository => _orderDetailRepository ??= new GenericRepository<OrderDetail>(_context);

        public IGenericRepository<Product> ProductRepository => _productRepository ??= new GenericRepository<Product>(_context);

        public IGenericRepository<ProductCategory> ProductCategoryRepository => _productCategoryRepository ??= new GenericRepository<ProductCategory>(_context);

        public IGenericRepository<Rating> RatingRepository => _ratingRepository ??= new GenericRepository<Rating>(_context);

        public IGenericRepository<Shop> ShopRepository => _shopRepository ??= new GenericRepository<Shop>(_context);

        public IGenericRepository<VoucherOfshop> VoucherOfShopRepository => _voucherOfshopRepository ??= new GenericRepository<VoucherOfshop>(_context);

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
