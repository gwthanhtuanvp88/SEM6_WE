use master
go
drop database ReportClaim
go
create database ReportClaim
go
use ReportClaim
go
create table UserTypes(
	id int primary key identity(1,1),
	name varchar(50),
	description nvarchar(500),
	status bit default 1
)
go
create table Users(
	id int primary key identity(1,1),
	username varchar(50),
	password binary(16),
	name nvarchar(50),
	email nvarchar(100),
	phone nvarchar(12),
	address nvarchar(200),
	usertypeid int not null references UserTypes(id),
	status bit default 1,
	active bit default 1
)

go

create table Faculty(
	id int primary key identity(1,1),
	name nvarchar(50),
	description text,
	status bit default 1
)

go

create table Student(
	id int primary key,
	facultyid int not null references Faculty(id),
	status bit default 1
)

go 
create table Manager(
	id int primary key,
	status bit default 1
)

go 
create table Administration(
	id int primary key ,
	status bit default 1
)

go 
create table Academyyear(
	id int primary key identity(1,1),
	startReportDate date,
	closureReportDate date,
	closureEvidenceDate date,
	status bit default 1
)
go 

create table Coordinator(
	id int primary key,
	name nvarchar(50),
	email nvarchar(100),
	phone nvarchar(15),
	address nvarchar(200),
	facutyid int not null references Faculty(id),
	status bit default 1
)
go

create table Claim(
	id int primary key identity(1,1),
	name nvarchar(50),
	description	text, 
	studentid int not null references Student(id),
	academyyear int not null references AcademyYear(id), 
	coordinatorId int not null references Coordinator(id),
	result tinyint default 0, /*0 means not process yey, 1 means refuse, 2 means accept*/
	status bit default 1
)

go 

create table Evidence(
	id int primary key identity(1,1),
	filename nvarchar(100) not null,
	type tinyint default 1,
	claimid int not null references Claim(id),
	status bit default 1
)





