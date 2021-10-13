create database LibraryManagement


-- quản trị viên 
create table Admin
(
	Id int identity(1,1) primary key,
	Name nvarchar(max),
	BirthDay Date,
	Email varchar(100),
	Address nvarchar(max),
	Phone varchar(10),
	Username varchar(50),
	Password varchar(12)
)


create table UserStatus
(
	Id int identity(1,1) primary key,
	Name nvarchar(max),
)


-- độc giả
create table Users
(
	Id int identity(1,1) primary key,
	Name nvarchar(max),
	BirthDay Date,
	Email varchar(100),
	Address nvarchar(max),
	Phone varchar(10),
	IdStatus int,
	foreign key (IdStatus) references UserStatus(Id),

)





create table Fee
(
	Id int identity(1,1) primary key,
	Month int,
	Year int
)

create table DetailFee
(
	IdFee int,
	IdUser int,
	Money int,
	PayDate Date,

	CONSTRAINT DetailFee PRIMARY KEY (IdFee, IdUser),
	foreign key (IdFee) references Fee(Id),
	foreign key (IdUser) references Users(Id),
)


-- nhà sách
create table BookStore
(
	Id int identity(1,1) primary key,
	Name nvarchar(max),
	Address nvarchar(max),
	CoopDate Date,
	MoreInfo nvarchar(max)
)

-- sách
create table Book
(
	Id int identity(1,1) primary key,
	Name nvarchar(max),
	Address nvarchar(max),
	PublishDate Date,
	Quantity int,
	PriceIn int,
	PriceOut int,
	MoreInfo nvarchar(max)
)



-- tác giả
create table Writer
(
	Id int identity(1,1) primary key,
	Name nvarchar(max),
	BirthDay Date,
	Description nvarchar(max)
)


-- sách do ai viết
create table Author
(
	IdBook int,
	IdWriter int
	CONSTRAINT Author_pk PRIMARY KEY (IdBook, IdWriter)
	foreign key (IdBook) references Book(Id),
	foreign key (IdWriter) references Writer(Id),
)


--nhập sách
create table ImportBook
(
	Id int identity(1,1) primary key,
	IdBook int,
	IdBookStore int,
	Quantity int,
	ImportDate Date,

	foreign key (IdBook) references Book(Id),
	foreign key (IdBookStore) references BookStore(Id),
)


-- trang thái sách
-- đang mượn
-- đã trả
-- hư hỏng
-- mất
create table StatusBook 
(
	Id int identity(1,1) primary key,
	Name nvarchar(max),
)


-- lưu lại lịch sử mượn sách của độc giả
create table HistoryBook 
(
	Id int identity(1,1) primary key,
	IdUser int,
	IdBook int,
	BorrowedDate Date,
	ReturnDate Date,
	IdStatus int,

	foreign key (IdUser) references Users(Id),
	foreign key (IdBook) references Book(Id),
	foreign key (IdStatus) references StatusBook(Id),
)
