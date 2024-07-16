USE [master]
GO
CREATE DATABASE [MomAndKids]
drop DATABASE [MomAndKids]
USE [MomAndKids]
GO
GO
CREATE TABLE [dbo].[Account](
	[accountId] [int] IDENTITY NOT NULL,
	[roleId] [int] NOT NULL,
	[userName] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](10),
	[Address] [nvarchar](30) ,
	[dob] [date],
	[Email] [nvarchar](30) NOT NULL,
	[password] [nvarchar](30) NOT NULL,
	[status] [Bit] NOT NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[accountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
Go
CREATE TABLE [dbo].[ProductCategory](
	[productCategoryId] [int] IDENTITY NOT NULL,
	[productCategoryName] [nvarchar](50)	NOT NULL,
	[imageProduct] [nvarchar](250) NOT NULL,
	 CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED 
(
	[productCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
GO
CREATE TABLE [dbo].[Product](
	[productId] [int] IDENTITY NOT NULL,
	[productCategoryId] [int] NOT NULL,
	[productName] [nvarchar](50)	NOT NULL,
	[productInfor] [nvarchar](250) NOT NULL,
	[productPrice] [float] NOT NULL,
	[productQuatity] [int] NOT NULL,
	 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[productId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_pRODUCTcATEGORY] FOREIGN KEY([productCategoryId])
REFERENCES [dbo].[ProductCategory] ([productCategoryId])
ON UPDATE CASCADE
ON DELETE CASCADE
Go
GO
CREATE TABLE [dbo].[ImageProduct](
	[imageId] [int] IDENTITY NOT NULL,
	[imageContent] [nvarchar](250)	NOT NULL,
	[productId] [int] NOT NULL,
	[productPrice] [float] NOT NULL,
	[productQuatity] [DateTime] NOT NULL,
	[imageProduct] [nvarchar](250) NOT NULL,
	 CONSTRAINT [PK_ImageProduct] PRIMARY KEY CLUSTERED 
(
	[imageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ImageProduct]  WITH CHECK ADD  CONSTRAINT [FK_ImageProduct_Product] FOREIGN KEY([productId])
REFERENCES [dbo].[Product] ([productId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
GO
CREATE TABLE [dbo].[Cart](
	[cartId] [int] IDENTITY NOT NULL,
	[productId] [int] NOT NULL,
	[accountId] [int] NOT NULL,
	[cartQuantity] [int] NOT NULL,
	[status] [bit] NOT NULL,
	 CONSTRAINT [PK_Cart] PRIMARY KEY CLUSTERED 
(
	[cartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_Cart_Product] FOREIGN KEY([productId])
REFERENCES [dbo].[Product] ([productId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_Cart_Account] FOREIGN KEY([accountId])
REFERENCES [dbo].[Account] ([accountId])
ON UPDATE NO ACTION
ON DELETE NO ACTION
GO
GO
CREATE TABLE [dbo].[Blog](
	[blogId] [int] IDENTITY NOT NULL,
	[accountId] [int] NOT NULL,
	[blogName] [nvarchar](50) NOT NULL,
	[blogContent] [nvarchar](350) NOT NULL,
	[blogImage] [nvarchar](250) NOT NULL,
	[status] [bit] NOT NULL,
	 CONSTRAINT [PK_Blog] PRIMARY KEY CLUSTERED 
(
	[blogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
GO
ALTER TABLE [dbo].[Blog]  WITH CHECK ADD  CONSTRAINT [FK_Blog_Account] FOREIGN KEY([accountId])
REFERENCES [dbo].[Account] ([accountId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
GO
CREATE TABLE [dbo].[Order](
	[orderId] [int] IDENTITY NOT NULL,
	[accountId] [int] NOT NULL,
	[orderDate] [DateTime] NOT NULL,
	[TotalPrice] [float] NOT NULL,
	[status] [int] NOT NULL,
	 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[orderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Account] FOREIGN KEY([accountId])
REFERENCES [dbo].[Account] ([accountId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
CREATE TABLE [dbo].[OrderDetail](
	[orderDetailId] [int] IDENTITY NOT NULL,
	[orderId] [int] NOT NULL,
	[productId] [int] NOT NULL,
	[orderQuantity] [int] NOT NULL,
	[productPrice] [float] NOT NULL,
	[status] [bit] NOT NULL,
	 CONSTRAINT [PK_OrderDetail] PRIMARY KEY CLUSTERED 
(
	[orderDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Product] FOREIGN KEY([productId])
REFERENCES [dbo].[Product] ([productId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Order] FOREIGN KEY([orderId])
REFERENCES [dbo].[Order] ([orderId])
ON UPDATE NO ACTION
ON DELETE NO ACTION
GO
GO
CREATE TABLE [dbo].[Feedback](
	[feedbackId] [int] IDENTITY NOT NULL,
	[accountId] [int] NOT NULL,
	[productId] [int] NOT NULL,
	[content] [nvarchar](250) NOT NULL,
    [rateNumber] [float] NOT NULL,
	[status] [bit] NOT NULL,
	 CONSTRAINT [PK_Feedback] PRIMARY KEY CLUSTERED 
(
	[feedbackId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Comment_Account] FOREIGN KEY([accountId])
REFERENCES [dbo].[Account] ([accountId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Comment_Product] FOREIGN KEY([productId])
REFERENCES [dbo].[Product] ([productId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
create table Payments (
	PaymentId INT IDENTITY(1,1) PRIMARY KEY,
	PaymentMethod NVARCHAR(100) NOT NULL,
	BankCode NVARCHAR(MAX) NOT NULL,
	BankTranNo NVARCHAR(MAX) NOT NULL,
	CardType NVARCHAR(MAX) NOT NULL,
	PaymentInfo NVARCHAR(MAX),
	PayDate DATETIME,
	TransactionNo NVARCHAR(MAX) NOT NULL,
	TransactionStatus INT NOT NULL,
	PaymentAmount DECIMAL(13,2) NOT NULL,
	[orderId] INT,
	CONSTRAINT FK_OrderId_Payments FOREIGN KEY ([orderId]) REFERENCES [dbo].[Order]([orderId])
)
--GO
--CREATE TABLE [dbo].[Rating](
--	[rateId] [int] IDENTITY NOT NULL,
--	[accountId] [int] NOT NULL,
--	[productId] [int] NOT NULL,
--	[rateNumber] [float] NOT NULL,
--	[status] [bit] NOT NULL,
--	 CONSTRAINT [PK_Rating] PRIMARY KEY CLUSTERED 
--(
--	[rateId] ASC
--)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY]
--GO
--GO
--ALTER TABLE [dbo].[Rating]  WITH CHECK ADD  CONSTRAINT [FK_Rating_Account] FOREIGN KEY([accountId])
--REFERENCES [dbo].[Account] ([accountId])
--ON UPDATE CASCADE
--ON DELETE CASCADE
--GO
--GO
--ALTER TABLE [dbo].[Rating]  WITH CHECK ADD  CONSTRAINT [FK_Rating_Product] FOREIGN KEY([productId])
--REFERENCES [dbo].[Product] ([productId])
--ON UPDATE CASCADE
--ON DELETE CASCADE
--GO