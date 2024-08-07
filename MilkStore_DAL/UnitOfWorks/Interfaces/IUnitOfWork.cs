﻿using Microsoft.EntityFrameworkCore.Storage;
using MilkStore_DAL.Entities;
using MilkStore_DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore_DAL.UnitOfWorks.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<IDbContextTransaction> BeginTransactionAsync();

        Task SaveAsync();

        IGenericRepository<Account> AccountRepository { get; }

        IGenericRepository<Blog> BlogRepository { get; }

        IGenericRepository<Cart> CartRepository { get; }

        IGenericRepository<Feedback> FeedbackRepository { get; }

        IGenericRepository<Order> OrderRepository { get; }

        IGenericRepository<OrderDetail> OrderDetailRepository { get; }

        IGenericRepository<Product> ProductRepository { get; }

        IGenericRepository<ProductCategory> ProductCategoryRepository { get; }

        IGenericRepository<ImageProduct> ImageProductRepository { get; }

        IGenericRepository<Payment> PaymentRepository { get; }

        IGenericRepository<Customer> CustomerRepository { get; }

        IGenericRepository<BlogProduct> BlogProductRepository { get; }

        IGenericRepository<VoucherOfShop> VoucherOfShopRepository { get; }
        IGenericRepository<ChatRequest> ChatRequestRepository { get; }
    }
}
