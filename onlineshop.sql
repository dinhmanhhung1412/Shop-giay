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
    ProductID INT PRIMARY KEY IDENTITY(1,1),
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
    CustomerEmail NVARCHAR(250) DEFAULT N'',
    CustomerName NVARCHAR(250) NOT NULL,
    CustomerPhone NVARCHAR(20) DEFAULT N'',
    CustomerAdress NVARCHAR(250) DEFAULT N'',
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

 
--------------------------------------------

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
CREATE PROC Create_Product  @name nvarchar(250), @description nvarchar(250), 
@price decimal(18,0), @promotionprice decimal(18,2),
@img1 nvarchar(250), @img2 nvarchar(250),
@stock int, @meta nvarchar(250), @status bit, @cate int
AS
BEGIN
INSERT dbo.PRODUCT
(
    
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
     @name, @description, 
	@price, @promotionprice , 5, 
	@img1, @img2,
	@stock, @meta, @status, getdate(),@cate  
)
END 
GO 
-- Update sp
CREATE PROC Update_Product @id int, @name nvarchar(250), @description nvarchar(250), 
@price decimal(18,0), @promotionprice decimal(18,2),
@img1 nvarchar(250), @img2 nvarchar(250),
@stock int, @meta nvarchar(250), @status bit, @cate int
AS
BEGIN
UPDATE dbo.PRODUCT
SET
    dbo.PRODUCT.ProductName = @name, 
    dbo.PRODUCT.ProductDescription = @description, 
    dbo.PRODUCT.ProductPrice = @price,
    dbo.PRODUCT.PromotionPrice = @promotionprice,
    dbo.PRODUCT.ShowImage_1 = @img1,
    dbo.PRODUCT.ShowImage_2 = @img2, 
    dbo.PRODUCT.ProductStock = @stock, 
    dbo.PRODUCT.MetaKeyword = @meta, 
    dbo.PRODUCT.ProductStatus = @status, 
    dbo.PRODUCT.CreatedDate = getdate(), -- DATETIME
    dbo.PRODUCT.CategoryID = @cate
	WHERE dbo.PRODUCT.ProductID=@id
END
GO
-- Delete sp
CREATE PROC Delete_Product @id int
AS
BEGIN
DELETE dbo.PRODUCT WHERE dbo.PRODUCT.ProductID=@id
END
GO 
CREATE PROC LoadProd_ByID @id int
AS
BEGIN
SELECT * FROM dbo.PRODUCT p
WHERE p.ProductID=@id
END
GO
CREATE PROC ProductList
AS
BEGIN
SELECT * FROM dbo.PRODUCT p
END 
GO 

CREATE PROC Add_ProductDetail @prodID int, @sizeID int
AS
BEGIN
INSERT dbo.PRODUCTDETAIL
(
    --ProductDetailID - column value is auto-generated
    ProductID,
    SizeID
)
VALUES
(
    -- ProductDetailID - INT
    @prodID, -- ProductID - INT
    @sizeID -- SizeID - INT
)
END
GO

CREATE PROC Update_ProductDetail @prodID int, @sizeID int
AS
BEGIN
UPDATE dbo.PRODUCTDETAIL
SET
    dbo.PRODUCTDETAIL.SizeID = @sizeID-- INT
	WHERE dbo.PRODUCTDETAIL.ProductID=@prodID
END
GO

CREATE PROC Delete_ProductDetail @prodID int
AS
BEGIN
DELETE FROM dbo.PRODUCTDETAIL
WHERE dbo.PRODUCTDETAIL.ProductID=@prodID
END
GO 

CREATE PROC LoadSize_ByProdID @prodID int
AS
BEGIN
SELECT * FROM dbo.PRODUCTDETAIL p
WHERE p.ProductID=@prodID
END
GO

CREATE PROC CategoryList
AS 
BEGIN
SELECT * FROM dbo.CATEGORY c
END
GO 
CREATE PROC SizeList
AS 
BEGIN
SELECT * FROM dbo.SIZE s
END 
GO 
GO 
CREATE PROC Add_Order @cusID int, @total decimal(18,2)
AS
BEGIN
INSERT dbo.[ORDER]
(
    --OrderID - column value is auto-generated
    OrderDate,
    DeliveryDate,
    Total,
    OrderStatusID,
    CustomerID
)
VALUES
(
    -- OrderID - INT
    GETDATE(), -- OrderDate - DATETIME
    GETDATE(), -- DeliveryDate - DATETIME
    @total, -- Total - DECIMAL
    1, -- OrderStatusID - INT
    @cusID -- CustomerID - INT
)
END 
GO 
CREATE PROC Add_OrderDetail @orderID int,@prodID int, @sizeID int, @quantity int
AS
BEGIN
INSERT dbo.ORDERDETAIL
(
    --DetailID - column value is auto-generated
    Quantity,
    OrderID,
    SizeID,
    ProductID
)
VALUES
(
    -- DetailID - int
    @quantity, -- Quantity - int
    @orderID, -- OrderID - int
    @sizeID, -- SizeID - int
    @prodID -- ProductID - int
)
END
GO 
CREATE PROC LoadOrderDetail @orderID int
AS
BEGIN
SELECT * FROM dbo.ORDERDETAIL o
WHERE o.OrderID=@orderID
END
GO 
CREATE PROC Create_Size @sizeName nvarchar(250)
AS
BEGIN
INSERT dbo.SIZE
(
    --SizeID - column value is auto-generated
    Size,
    CreatedDate
)
VALUES
(
    -- SizeID - INT
    @sizeName, -- Size - NVARCHAR
    GETDATE() -- CreatedDate - DATETIME
)
END 
GO 
CREATE PROC Delete_Size @sizeID int
AS
BEGIN
DELETE dbo.SIZE WHERE dbo.SIZE.SizeID=@sizeID
END
GO 
CREATE PROC Update_Size @sizeID int, @sizeName varchar(2)
AS
BEGIN
UPDATE dbo.SIZE
SET
    --SizeID - column value is auto-generated
    dbo.SIZE.Size =@sizeName, -- NVARCHAR
    dbo.SIZE.CreatedDate = GETDATE() -- DATETIME
	WHERE dbo.SIZE.SizeID=@sizeID
END
GO 

CREATE PROC LoadSize_ByID @id int
AS
BEGIN
SELECT * FROM dbo.SIZE s
WHERE s.SizeID=@id
END
GO


CREATE PROC Create_Customer @username nvarchar(250), @pass nchar(250),@name nvarchar(250),
							@phone nvarchar(20),@mail nvarchar(250)
AS
BEGIN
INSERT dbo.CUSTOMER
(
    --CustomerID - column value is auto-generated
    CustomerUsername,
    CustomerPassword,
    CustomerEmail,
    CustomerName,
    CustomerPhone,
    CreatedDate
)
VALUES
(
    -- CustomerID - INT
    @username, -- CustomerUsername - NVARCHAR
    @pass, -- CustomerPassword - NVARCHAR
    @mail, -- CustomerEmail - NVARCHAR
    @name, -- CustomerName - NVARCHAR
    @phone, -- CustomerPhone - NVARCHAR
    GETDATE() -- CreatedDate - DATETIME
)
END 
GO 

CREATE PROC Delete_Customer @id int
AS
BEGIN
DELETE FROM dbo.CUSTOMER 
WHERE dbo.CUSTOMER.CustomerID=@id
END
go

CREATE PROC LoadByUserName @username nvarchar(250)
AS
BEGIN
SELECT *FROM dbo.CUSTOMER c
WHERE c.CustomerUsername=@username
END 
GO

CREATE PROC	Load_Customer
AS 
BEGIN
SELECT * FROM dbo.CUSTOMER c
END
GO

CREATE PROC LoadCustomer_ByID @id int
AS
BEGIN
SELECT * FROM dbo.CUSTOMER c
WHERE c.CustomerID =@id
END
GO


CREATE PROC Login_Admin @username nvarchar(250),@pass nvarchar(250)
AS
BEGIN
	DECLARE @count int
	DECLARE @res bit
	SELECT @count = count(*) FROM dbo.[USER] u 
	WHERE u.UserUsername=@username AND u.UserPassword=@pass
	IF @count > 0 
		SET @res=1
	ELSE
		SET @res=0
	RETURN @res
END
GO

CREATE PROC Create_Category @name nvarchar(250),@cate nvarchar(250)
AS
BEGIN
INSERT dbo.CATEGORY
(
    --CategoryID - column value is auto-generated
    CategoryName,
    MetaKeyword,
    CreatedDate
)
VALUES
(
    -- CategoryID - INT
    @name, -- CategoryName - NVARCHAR
    @cate, -- MetaKeyword - NVARCHAR
    GETDATE() -- CreatedDate - DATETIME
)
END
GO

CREATE PROC Delete_Category @id int
AS
BEGIN
DELETE dbo.CATEGORY WHERE dbo.CATEGORY.CategoryID=@id
END
GO 

CREATE PROC Edit_Category @id int, @name nvarchar(250), @meta nvarchar(250)
AS
BEGIN
UPDATE dbo.CATEGORY
SET
    --CategoryID - column value is auto-generated
    dbo.CATEGORY.CategoryName = @name, -- NVARCHAR
    dbo.CATEGORY.MetaKeyword = @meta, -- NVARCHAR
    dbo.CATEGORY.CreatedDate = GETDATE() -- DATETIME
	WHERE dbo.CATEGORY.CategoryID=@id
END
GO 

CREATE PROC LoadMeta_ByID @id int
AS
BEGIN 
SELECT * FROM dbo.CATEGORY c
WHERE c.CategoryID=@id
END 
GO

CREATE PROC Load_Order
AS
BEGIN 
SELECT * FROM dbo.[ORDER] o
END 
GO 

CREATE PROC LoadOrderStatus
AS
BEGIN
SELECT * FROM dbo.ORDERSTATUS o
END

CREATE PROC Cancel_Order @orderID int
AS
BEGIN 
UPDATE dbo.[ORDER]
SET
    dbo.[ORDER].OrderStatusID = 5
WHERE dbo.[ORDER].OrderID=@orderID
END 
GO

CREATE PROC Change_Order @orderID int, @statusID int
AS
BEGIN
UPDATE dbo.[ORDER]
SET
    dbo.[ORDER].OrderStatusID = @statusID
	WHERE dbo.[ORDER].OrderID=@orderID
END
GO 

CREATE PROC Load_CustomerOrder @cusID int
AS
BEGIN
SELECT *FROM dbo.[ORDER] o
WHERE o.CustomerID=@cusID
END
GO

CREATE PROC LoadByMeta_Cate @meta nvarchar(250)
AS
BEGIN 
SELECT * FROM dbo.CATEGORY c
WHERE c.MetaKeyword=@meta
END
GO 

CREATE PROC LoadByMeta_Prod @meta nvarchar(250)
AS
BEGIN
SELECT * FROM dbo.PRODUCT p
WHERE p.MetaKeyword=@meta
END
GO
SELECT * FROM dbo.PRODUCT p
SELECT * FROM dbo.PRODUCTDETAIL p

go
CREATE PROC SelectPaging
    @PageSize INT,
    @PageIndex INT,
    @Sort nvarchar(10),
    @Search nvarchar(255) = NULL,
    @Cate INT = NULL
AS
BEGIN
    IF(@Cate IS NULL)
    BEGIN
        SELECT *
        FROM [PRODUCT]
        WHERE(@Search IS NULL OR [PRODUCT].[ProductName] LIKE '%'+@Search+'%')
        ORDER BY
        CASE 
            WHEN @Sort = 'name_desc' THEN ProductName END DESC,
        CASE 
            WHEN @Sort = 'name_asc' THEN ProductName END ASC,
        CASE 
            WHEN @Sort = 'price_desc' THEN ProductPrice END DESC,
        CASE 
            WHEN @Sort = 'price_asc' THEN ProductName END ASC
        OFFSET @PageSize * @PageIndex ROWS
        FETCH NEXT @PageSize ROWS ONLY
    END
    ELSE
    BEGIN
        SELECT *
        FROM [PRODUCT]
        WHERE [PRODUCT].CategoryID = @Cate
        ORDER BY
        CASE 
            WHEN @Sort = 'name_desc' THEN ProductName END DESC,
        CASE 
            WHEN @Sort = 'name_asc' THEN ProductName END ASC,
        CASE 
            WHEN @Sort = 'price_desc' THEN ProductPrice END DESC,
        CASE 
            WHEN @Sort = 'price_asc' THEN ProductName END ASC
        OFFSET @PageSize * @PageIndex ROWS
        FETCH NEXT @PageSize ROWS ONLY
    END
END
