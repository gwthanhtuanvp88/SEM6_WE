use master
create database ReportClaim
go
use ReportClaim
go
create table UserTypes(
	id int primary key identity(1,1),
	name varchar(50),
	description nvarchar(500)
)
go
create table Users(
	id int primary key identity(1,1),
	name nvarchar(50),
	username varchar(50),
	password binary,
	email nvarchar(100),
	address nvarchar(200),
	usertypeid int not null references UserTypes(id),
	status bit
)
go