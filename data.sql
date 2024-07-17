USE [MomAndKids]
GO

-- Insert sample data into Account table
INSERT INTO [dbo].[Account] (roleId, Email, password, status) VALUES 
(1, 'admin@example.com', 'adminpassword', 1),
(2, 'staff@example.com', 'staffpassword', 1),
(3, 'customer1@example.com', 'customer1password', 1),
(3, 'customer2@example.com', 'customer2password', 1);

-- Insert sample data into Customer table
INSERT INTO [dbo].[Customer] (accountId, userName, Phone, Address, dob, point, status) VALUES 
(3, 'Customer One', '1234567890', '123 Main St', '1990-01-01', 100, 1),
(4, 'Customer Two', '0987654321', '456 Elm St', '1992-02-02', 200, 1);

-- Insert sample data into ProductCategory table
INSERT INTO [dbo].[ProductCategory] (productCategoryName) VALUES 
('Vinamilk'),
('Nutifood'),
('Abbott');

-- Insert sample data into Product table
INSERT INTO [dbo].[Product] (productCategoryId, productName, productInfor, productPrice, productQuatity) VALUES 
(1, 'Milk 1', 'Use for kids.', 100000, 100),
(2, 'Milk 2', 'Use for both adults and kids.', 340000, 50),
(3, 'Milk 3', 'Use for adults only.', 240000, 200);

-- Insert sample data into Cart table
INSERT INTO [dbo].[Cart] (productId, customerId, cartQuantity, status) VALUES 
(1, 1, 2, 1),
(2, 1, 1, 1),
(3, 2, 3, 1);

-- Insert sample data into Blog table
INSERT INTO [dbo].[Blog] (blogTitle, blogContent, blogImage, status) VALUES 
('First Blog', 'This is the first blog post.', NULL, 1),
('Second Blog', 'This is the second blog post.', NULL, 1);

-- Insert sample data into BlogProduct table
INSERT INTO [dbo].[BlogProduct] (blogId, productId, status) VALUES 
(1, 1, 1),
(2, 2, 1);

-- Insert sample data into VoucherOfShop table
INSERT INTO [dbo].[VoucherOfShop] (voucherValue, StartDate, voucherQuantity, EndDate, status) VALUES 
(10.0, '2024-01-01', 100, '2024-12-31', 1),
(15.0, '2024-01-01', 50, '2024-12-31', 1);

-- Insert sample data into Feedback table
INSERT INTO [dbo].[Feedback] (customerId, productId, feedbackContent, rateNumber, status) VALUES 
(1, 1, 'Great product!', 5.0, 1),
(2, 2, 'Good quality.', 4.5, 1);
GO
