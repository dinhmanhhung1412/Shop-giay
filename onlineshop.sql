--USE master
--alter database ONLINESHOP set single_user with rollback immediate
--drop database ONLINESHOP

CREATE DATABASE ONLINESHOP
GO

USE ONLINESHOP 
GO

CREATE TABLE [CATEGORY]
(
    CategoryID INT PRIMARY KEY IDENTITY(1, 1),
    CategoryName NVARCHAR(250) UNIQUE NOT NULL,
    MetaKeyword NVARCHAR(50),

    CreatedDate DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE [PRODUCT]
(
    ProductID INT PRIMARY KEY IDENTITY(1, 1),
    ProductName NVARCHAR(250) NOT NULL,
    ProductDescription NVARCHAR(4000),
    ProductPrice DECIMAL(18, 0) NOT NULL,
    PromotionPrice DECIMAL(18, 0) DEFAULT 0,
    Rating INT CHECK (RATING >=0 AND RATING <= 5),
    ShowImage_1 NVARCHAR(4000) DEFAULT N'',
    ShowImage_2 NVARCHAR(4000) DEFAULT N'',
    ProductStock INT DEFAULT 1,
    MetaKeyword NVARCHAR(250),
    ProductStatus BIT,
    CreatedDate DATETIME DEFAULT GETDATE(),

    CategoryID INT CONSTRAINT fk_p_cgid FOREIGN KEY (CategoryID) REFERENCES [CATEGORY](CategoryID) ON DELETE CASCADE NOT NULL
)
GO

CREATE TABLE [PRODUCTIMAGE]
(
    ImageID INT PRIMARY KEY IDENTITY(1, 1),
    DetailImage_1 NVARCHAR(4000) DEFAULT N'',
    DetailImage_2 NVARCHAR(4000) DEFAULT N'',
    DetailImage_3 NVARCHAR(4000) DEFAULT N'',

    ProductID INT CONSTRAINT fk_pi_pdid FOREIGN KEY (ProductID) REFERENCES [PRODUCT](ProductID) ON DELETE CASCADE
)
GO

CREATE TABLE [SIZE]
(
    SizeID INT PRIMARY KEY IDENTITY(1, 1),
    Size NVARCHAR(2),
    CreatedDate DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE [PRODUCTDETAIL]
(
    ProductDetailID INT PRIMARY KEY IDENTITY(1, 1),
    ProductID INT CONSTRAINT fk_pd_pdid FOREIGN KEY (ProductID) REFERENCES [PRODUCT](ProductID) ON DELETE CASCADE,
    SizeID INT REFERENCES [SIZE](SizeID)
)
GO

CREATE TABLE [CUSTOMER]
(
    CustomerID INT PRIMARY KEY IDENTITY(1, 1),
    CustomerUsername NVARCHAR(250) UNIQUE NOT NULL,
    CustomerPassword NVARCHAR(250) NOT NULL,
    CustomerEmail NVARCHAR(250),
    CustomerName NVARCHAR(250) NOT NULL,
    CustomerPhone NVARCHAR(20),
    CustomerAdress NVARCHAR(250),
    CreatedDate DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE [ORDERSTATUS]
(
    StatusID INT PRIMARY KEY IDENTITY(1, 1),
    StatusName NVARCHAR(255),
    CreatedDate DATETIME DEFAULT GETDATE()
)
GO

CREATE TABLE [ORDER]
(
    OrderID INT PRIMARY KEY IDENTITY(1, 1),
    OrderDate DATETIME DEFAULT GETDATE(),
    DeliveryDate DATETIME,
    Total DECIMAL(18, 2),

    OrderStatusID INT CONSTRAINT fk_od_stid FOREIGN KEY (OrderStatusID) REFERENCES [ORDERSTATUS](StatusID),
    CustomerID INT CONSTRAINT fk_od_csid FOREIGN KEY (CustomerID) REFERENCES [CUSTOMER](CustomerID) ON DELETE CASCADE
)
GO

CREATE TABLE [ORDERDETAIL]
(
    DetailID INT PRIMARY KEY IDENTITY(1, 1),

    Quantity INT,
    OrderID INT CONSTRAINT fk_od_odid FOREIGN KEY (OrderID) REFERENCES [ORDER](OrderID) ON DELETE CASCADE,
    SizeID INT REFERENCES [SIZE](SizeID),
    ProductID INT CONSTRAINT fk_od_pdid FOREIGN KEY (ProductID) REFERENCES [PRODUCT](ProductID) ON DELETE CASCADE
)
GO

CREATE TABLE [USER]
(
    UserId INT PRIMARY KEY IDENTITY(1, 1),
    UserUsername NVARCHAR(250) UNIQUE NOT NULL,
    UserPassword NVARCHAR(250) NOT NULL,
    UserName NVARCHAR(250),

    CreatedDate DATETIME
)
GO

INSERT INTO [CATEGORY]
VALUES
    (N'Giày da', N'giay-da', GETDATE()),
    (N'Giày thể thao', N'giay-the-thao', GETDATE()),
    (N'Giày lifestyle', N'giay-lifestyle', GETDATE()),
    (N'Giày boots', N'giay-boots', GETDATE())
GO

INSERT INTO [SIZE]
VALUES
    (34, GETDATE()),
    (35, GETDATE()),
    (36, GETDATE()),
    (37, GETDATE()),
    (38, GETDATE()),
    (39, GETDATE()),
    (40, GETDATE()),
    (41, GETDATE()),
    (42, GETDATE()),
    (43, GETDATE())
GO

INSERT INTO [ORDERSTATUS]
VALUES
    (N'Đang xử lý', GETDATE()),
    (N'Đang giao hàng', GETDATE()),
    (N'Đã giao hàng', GETDATE()),
    (N'Hàng có lỗi', GETDATE()),
    (N'Đã hủy', GETDATE())
GO

INSERT INTO [PRODUCT]
VALUES
	(N'Nike', N'Chất liệu cao cấp,thiết kế nén khí làm chân tiếp đất êm hơn,
                              thích hợp cho nhiều độ tuổi,vải waffle cao cấp bền cho lực kéo nhiều mặt',
		200, 150, 3,
		N'https://res.cloudinary.com/dhi8xksch/image/upload/v1583234400/Thuc-Tap-CSDL/giay-chay-nike-2_dlrisy.jpg',
		N'https://res.cloudinary.com/dhi8xksch/image/upload/v1583234400/Thuc-Tap-CSDL/giay-chay-nike-2_dlrisy.jpg',
		50, N'giay-nike', 1, GETDATE(), 1)

INSERT INTO [PRODUCT]
VALUES
	(N'Adidas', N'Phong cách sắc nét,có phần trên bằng da mềm mại cho hình bóng sạch sẽ,tạo cảm giác thể thao',
		150, null, 4,
		N'https://res.cloudinary.com/dhi8xksch/image/upload/v1583234400/Thuc-Tap-CSDL/giay-chay-nike-2_dlrisy.jpg',
		N'https://res.cloudinary.com/dhi8xksch/image/upload/v1583234400/Thuc-Tap-CSDL/giay-chay-nike-2_dlrisy.jpg',
		20, N'giay-adidas', 1, GETDATE(), 1)

INSERT INTO [PRODUCT]
VALUES
	(N'Converse', N'giày có thiết kế đơn giản, sang trọng, gam màu pastel nhẹ nhàng- mang dấu ấn đặc biệt của Converse,giúp người dùng có được sự thoải mái tối đa khi sử dụng. Chất liệu da sang trọng, dễ vệ sinh',
		25, 20, 1,
		N'https://res.cloudinary.com/dhi8xksch/image/upload/v1583234400/Thuc-Tap-CSDL/giay-chay-nike-2_dlrisy.jpg',
		N'https://res.cloudinary.com/dhi8xksch/image/upload/v1583234400/Thuc-Tap-CSDL/giay-chay-nike-2_dlrisy.jpg',
		15, N'giay-converse', 1, GETDATE(), 1)
INSERT INTO [PRODUCT]
VALUES
	(N'Giày da', N'Lựa chọn một đôi giày SDROLUN đơn giản, lịch sự kết hợp với nhiều trang phục là tiêu chí lựa chọn của cánh đàn ông',
		300, 200, 4,
		N'https://res.cloudinary.com/dhi8xksch/image/upload/v1583234400/Thuc-Tap-CSDL/giay-chay-nike-2_dlrisy.jpg',
		N'https://res.cloudinary.com/dhi8xksch/image/upload/v1583234400/Thuc-Tap-CSDL/giay-chay-nike-2_dlrisy.jpg',
		25, N'giay-da', 1, GETDATE(), 2)
INSERT INTO [PRODUCT]
VALUES
	(N'Giày cao gót', N'giày được thiết kế sang trọng,kiểu dáng thời trang,phù hợp với các bạn trẻ',
		20, null, 5,
		N'https://res.cloudinary.com/dhi8xksch/image/upload/v1583234400/Thuc-Tap-CSDL/giay-chay-nike-2_dlrisy.jpg',
		N'https://res.cloudinary.com/dhi8xksch/image/upload/v1583234400/Thuc-Tap-CSDL/giay-chay-nike-2_dlrisy.jpg',
		40, N'giay-cao-got', 1, GETDATE(), 3)
INSERT INTO [PRODUCT]
VALUES
	(N'Giày boots', N'giày được thiết kế sang trọng,kiểu dáng thời trang,phù hợp với các bạn trẻ',
		300, null, 1,
		N'https://res.cloudinary.com/dhi8xksch/image/upload/v1583234400/Thuc-Tap-CSDL/giay-chay-nike-2_dlrisy.jpg',
		N'https://res.cloudinary.com/dhi8xksch/image/upload/v1583234400/Thuc-Tap-CSDL/giay-chay-nike-2_dlrisy.jpg',
		10, N'giay-boots', 1, GETDATE(), 4)

INSERT INTO [PRODUCTDETAIL]
VALUES
	(1, 1),
	(1, 2),
	(1, 3),
	(1, 4),
	(1, 5),
	(2, 1),
	(2, 2),
	(2, 3),
	(2, 4),
	(2, 5),
	(3, 1),
	(3, 2),
	(3, 3),
	(3, 4),
	(3, 5),
	(4, 1),
	(4, 2),
	(4, 3),
	(4, 4),
	(4, 5),
	(5, 1),
	(5, 2),
	(5, 3),
	(5, 4),
	(5, 5),
	(6, 1),
	(6, 2),
	(6, 3),
	(6, 4),
	(6, 5)

    
INSERT INTO [USER]
VALUES(N'hoang', N'661512447519149825336913615157157122147', N'Nguyễn Hoàng', GETDATE())


--------------------------------------------------
CREATE PROC SelectOrderID
    @CustomerID INT,
    @StatusID INT
AS
BEGIN
    IF @StatusID = 0
    BEGIN
        SELECT [ORDER].OrderID
        FROM [ORDER]
        WHERE [ORDER].CustomerID = @CustomerID
    END
        ELSE
        BEGIN
        SELECT [ORDER].OrderID
        FROM [ORDER]
        WHERE [ORDER].CustomerID = @CustomerID
            AND [OrderStatusID] = @StatusID
    END
END
GO

CREATE PROC SelectOrder
    @CustomerID INT
AS
BEGIN
    SELECT [ORDER].OrderID,
        [ORDER].Total,
        [ORDER].OrderStatusID,
        [ORDERSTATUS].StatusName,
        [ORDER].OrderDate,
        [ORDER].DeliveryDate
    FROM [ORDER] JOIN [ORDERSTATUS] ON [ORDER].OrderStatusID = [ORDERSTATUS].StatusID
    WHERE [ORDER].CustomerID = @CustomerID
END
GO

CREATE PROC SelectOrderProduct
    @OrderID INT
AS
BEGIN
    SELECT
        [PRODUCT].ProductID,
        [PRODUCT].ProductName,
        [CATEGORY].CategoryName,
        [SIZE].Size,
        [ORDERDETAIL].Quantity
    FROM [ORDERDETAIL]
        JOIN [ORDER] ON [ORDERDETAIL].OrderID = [ORDER].OrderID
        JOIN [ORDERSTATUS] ON [ORDER].OrderStatusID = [ORDERSTATUS].StatusID
        JOIN [SIZE] ON [ORDERDETAIL].SizeID = [SIZE].SizeID
        JOIN [PRODUCT] ON [ORDERDETAIL].[ProductID] = [PRODUCT].ProductID
        JOIN [CATEGORY] ON [PRODUCT].CategoryID = [CATEGORY].CategoryID
    WHERE [ORDER].OrderID = @OrderID
END
GO
CREATE TRIGGER tr_update_product_meta
ON [PRODUCT]
AFTER INSERT
AS
BEGIN
    UPDATE PRODUCT
SET [MetaKeyword] = [MetaKeyword] + '-' + CAST([ProductID] AS NVARCHAR(10))
    WHERE [ProductID] = IDENT_CURRENT('PRODUCT')
END
GO

CREATE PROC SelectAllProduct
AS
BEGIN
	SELECT * FROM PRODUCT
	
END

EXEC SelectAllProduct

--------------------------------------------

select * from CUSTOMER

delete from [CATEGORY]
where [CATEGORY].[CategoryID] = 3
delete from [PRODUCTIMAGE]
where [PRODUCTIMAGE].ImageID = 3

 --public void FixEfProviderServicesProblem()
 --        {
 --            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
 --        }
GO


--create read update delete
--read -> tham số truyền vào -> 1 danh sách		-> 0 có		-> trả về 1 list
--							-> lấy chi tiết 1 sp -> id		-> trả về 1 đối tượng
--delete ->					-> truyền id					-> trả về null
--create ->				-> truyền 1 đối tượng			-> trả về id
--update					-> truyền id và 1 đối tượng		-> trả về đối tượng

--crud



-- Thêm sp
CREATE PROC Create_Product @name nvarchar(250), @descript nvarchar(250), 
@price decimal(18,0), @promotionprice,
@img1 nvarchar(250), @img2 nvarchar(250),
@stock int, @meta nvarchar(250), @status bit 
AS
BEGIN
INSERT dbo.PRODUCT
(
    --ProductID - column value is auto-generated
    ProductName,
    ProductDescription,
    ProductPrice,
    PromotionPrice,
    Rating,
    ShowImage_1,
    ShowImage_2,
    ProductStock,
    MetaKeyword,
    ProductStatus,
    CreatedDate,
    CategoryID
)
VALUES
(
    @name, @descript, 
	@price, @promotionprice , NULL 
	@img1, @img2,
	@stock, @meta, @status, getdate(), 0  
)
END 

-- Update sp
CREATE PROC Create_Product @id int, @name nvarchar(250), @descript nvarchar(250), 
@price decimal(18,0), @promotionprice,
@img1 nvarchar(250), @img2 nvarchar(250),
@stock int, @meta nvarchar(250), @status bit 
AS
BEGIN
UPDATE dbo.PRODUCT
SET
    --ProductID - column value is auto-generated
    dbo.PRODUCT.ProductName = @name, 
    dbo.PRODUCT.ProductDescription = @descript, 
    dbo.PRODUCT.ProductPrice = @price,
    dbo.PRODUCT.PromotionPrice = @promotionprice,
    dbo.PRODUCT.ShowImage_1 = @img1,
    dbo.PRODUCT.ShowImage_2 = @img2, 
    dbo.PRODUCT.ProductStock = @stock, 
    dbo.PRODUCT.MetaKeyword = @meta, 
    dbo.PRODUCT.ProductStatus = 0, 
    dbo.PRODUCT.CreatedDate = getdate(), -- DATETIME
    dbo.PRODUCT.CategoryID = 0 
END 
GO

-- Delete sp
CREATE PROC Delete_Product @id int
AS
BEGIN
DELETE dbo.PRODUCT WHERE dbo.PRODUCT.ProductID=@id
END
GO 





