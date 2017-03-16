use master
go
drop database ReportClaim
go
create database ReportClaim
go
use ReportClaim
go

create table UserType(
	id int primary key identity(1,1),
	name varchar(50),
	description text,
	status bit default 1
)

go 

create table Role(
	id int primary key identity(1,1),
	description varchar(50),
	status bit default 1
)

go

create table UserRole(
	id int primary key identity(1,1),
	description varchar(50),
	roleid int not null references Role(id),
	userTypeid int not null references UserType(id),
	status bit default 1
)

go

create table [User](
	id int primary key identity(1,1),
	username varchar(50),
	password binary(16),
	name nvarchar(50),
	email nvarchar(100),
	phone nvarchar(12),
	address nvarchar(200),
	usertypeid int not null references UserType(id),
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
	id int primary key identity(1,1),
	facultyid int not null references Faculty(id),
	userid int not null references [User](id),
	status bit default 1
)

go 

create table Coordinator(
	id int primary key identity(1,1),
	facutyid int not null references Faculty(id),
	userid int not null references [User](id),
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

create table Claim(
	id int primary key identity(1,1),
	name nvarchar(50),
	description	text, 
	studentid int not null references Student(id),
	academyyearid int not null references AcademyYear(id), 
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