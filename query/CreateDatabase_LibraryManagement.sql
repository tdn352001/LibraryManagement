
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
	Password varchar(100)
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
	Interval Date,
	Money int,
)

create table DetailFee
(
	Id int identity(1,1) primary key,
	IdFee int,
	IdUser int,
	PayDate Date,

	foreign key (IdFee) references Fee(Id),
	foreign key (IdUser) references Users(Id),
)


-- nhà sách
create table BookStore
(
	Id int identity(1,1) primary key,
	Name nvarchar(max),
	Email varchar(100),
	Phone varchar(10),
	Address nvarchar(max),
	CoopDate Date,
	MoreInfo nvarchar(max)
)

-- sách
create table Book
(
	Id int identity(1,1) primary key,
	Name nvarchar(max),
	Author nvarchar(max),
	PublishDate Date,
	Quantity int,
	MoreInfo nvarchar(max)
)



--nhập sách
create table ImportBook
(
	Id int identity(1,1) primary key,
	IdBookStore int,
	ImportDate Date,
	TotalPrice BIGINT,
	foreign key (IdBookStore) references BookStore(Id),
)


create table DetailImport
(
	IdImport int,
	IdBook int,
	Quantity int,
	PriceIn int,

	CONSTRAINT detail_import PRIMARY KEY (IdImport, IdBook),
	foreign key (IdImport) references ImportBook(Id),
	foreign key (IdBook) references Book(Id),
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

alter table Book  add constraint default_value default 0 for Quantity;