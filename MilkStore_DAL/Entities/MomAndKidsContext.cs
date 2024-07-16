using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MilkStore_DAL.Entities
{
    public partial class MomAndKidsContext : DbContext
    {
        public MomAndKidsContext()
        {
        }

        public MomAndKidsContext(DbContextOptions<MomAndKidsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Blog> Blogs { get; set; } = null!;
        public virtual DbSet<BlogProduct> BlogProducts { get; set; } = null!;
        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<ImageProduct> ImageProducts { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } = null!;
        public virtual DbSet<VoucherOfShop> VoucherOfShops { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.AccountId).HasColumnName("accountId");

                entity.Property(e => e.Email).HasMaxLength(30);

                entity.Property(e => e.Password).HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("Blog");

                entity.Property(e => e.BlogId).HasColumnName("blogId");

                entity.Property(e => e.BlogContent)
                    .HasMaxLength(350)
                    .HasColumnName("blogContent");

                entity.Property(e => e.BlogImage).HasColumnName("blogImage");

                entity.Property(e => e.BlogTitle)
                    .HasMaxLength(50)
                    .HasColumnName("blogTitle");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<BlogProduct>(entity =>
            {
                entity.HasKey(e => e.BlogId);

                entity.ToTable("BlogProduct");

                entity.Property(e => e.BlogId)
                    .ValueGeneratedNever()
                    .HasColumnName("blogId");

                entity.Property(e => e.BlogProductId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("blogProductId");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Blog)
                    .WithOne(p => p.BlogProduct)
                    .HasForeignKey<BlogProduct>(d => d.BlogId)
                    .HasConstraintName("FK_BlogProduct_Blog");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.BlogProducts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_BlogProduct_Product");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Cart");

                entity.Property(e => e.CartId).HasColumnName("cartId");

                entity.Property(e => e.CartQuantity).HasColumnName("cartQuantity");

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cart_Customer");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Cart_Product");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.AccountId).HasColumnName("accountId");

                entity.Property(e => e.Address).HasMaxLength(30);

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("dob");

                entity.Property(e => e.Phone).HasMaxLength(10);

                entity.Property(e => e.Point).HasColumnName("point");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("userName");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Customer_Account");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.Property(e => e.FeedbackId).HasColumnName("feedbackId");

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.FeedbackContent)
                    .HasMaxLength(250)
                    .HasColumnName("feedbackContent");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.RateNumber).HasColumnName("rateNumber");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Comment_Customer");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Feedbacks)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_Comment_Product");
            });

            modelBuilder.Entity<ImageProduct>(entity =>
            {
                entity.HasKey(e => e.ImageId);

                entity.ToTable("ImageProduct");

                entity.Property(e => e.ImageId).HasColumnName("imageId");

                entity.Property(e => e.ImageProduct1).HasColumnName("imageProduct");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ImageProducts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_ImageProduct_Product");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.CustomerId).HasColumnName("customerId");

                entity.Property(e => e.ExchangedPoint).HasColumnName("exchangedPoint");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("orderDate");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.VoucherId).HasColumnName("voucherId");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Order_Customer");

                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.VoucherId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Order_VoucherOfShop");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.OrderDetailId).HasColumnName("orderDetailId");

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.OrderQuantity).HasColumnName("orderQuantity");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.ProductPrice).HasColumnName("productPrice");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetail_Order");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_OrderDetail_Product");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.PayDate).HasColumnType("datetime");

                entity.Property(e => e.PaymentAmount).HasColumnType("decimal(13, 2)");

                entity.Property(e => e.PaymentMethod).HasMaxLength(100);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_OrderId_Payments");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("productId");

                entity.Property(e => e.ProductCategoryId).HasColumnName("productCategoryId");

                entity.Property(e => e.ProductInfor)
                    .HasMaxLength(250)
                    .HasColumnName("productInfor");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(50)
                    .HasColumnName("productName");

                entity.Property(e => e.ProductPrice).HasColumnName("productPrice");

                entity.Property(e => e.ProductQuatity).HasColumnName("productQuatity");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK_Product_pRODUCTcATEGORY");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("ProductCategory");

                entity.Property(e => e.ProductCategoryId).HasColumnName("productCategoryId");

                entity.Property(e => e.ProductCategoryName)
                    .HasMaxLength(50)
                    .HasColumnName("productCategoryName");
            });

            modelBuilder.Entity<VoucherOfShop>(entity =>
            {
                entity.HasKey(e => e.VoucherId);

                entity.ToTable("VoucherOfShop");

                entity.Property(e => e.VoucherId).HasColumnName("voucherId");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.VoucherQuantity).HasColumnName("voucherQuantity");

                entity.Property(e => e.VoucherValue).HasColumnName("voucherValue");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
