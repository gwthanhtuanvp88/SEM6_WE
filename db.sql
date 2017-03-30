use master
go
create database ReportClaim
go
use ReportClaim
go

create table UserType(
	id int primary key identity(1,1),
	name varchar(50) not null,
	description text,
	status bit default 1
)
go

insert into UserType values ('Student','',1)
insert into UserType values ('Coordinator','',1)
insert into UserType values ('Manager','',1)
insert into UserType values ('Administrator','',1)

go 

create table Role(
	id int primary key identity(1,1),
	description varchar(50),
	status bit default 1
)

go

insert into Role values('Claim',1)
insert into Role values('Report',1)

go

create table UserRole(
	id int primary key identity(1,1),
	description varchar(50),
	roleid int not null references Role(id),
	userTypeid int not null references UserType(id),
	status bit default 1
)

go

insert into UserRole values ('', 1,1,1)
insert into UserRole values ('', 1,2,1)

go

create table [User](
	id int primary key identity(1,1),
	username varchar(50) not null,
	password binary(16),
	name nvarchar(50),
	email nvarchar(100) not null,
	phone nvarchar(12),
	address nvarchar(200),
	usertypeid int not null references UserType(id),
	status bit default 1,
	active bit default 1
)

go

insert into [User] values ('thanhtuan',HashBytes('MD5', '123456'), 'Thanh Tuan', 'thanhtuanvp88@gmail.com', '123456789', 'VP', 1, 1, 1)
insert into [User] values ('sang',HashBytes('MD5', '123456'), 'Sang', 'sang@gmail.com', '123456789', 'HB', 2, 1, 1)
insert into [User] values ('hung',HashBytes('MD5', '123456'), 'Hung', 'hung@gmail.com', '123456789', 'HP', 3, 1, 1)
insert into [User] values ('nguyet',HashBytes('MD5', '123456'), 'Nguyet', 'nguyet@gmail.com', '123456789', 'Ha Nam', 4, 1, 1)
insert into [User] values ('minhanh',HashBytes('MD5', '123456'), 'Minh Anh', 'minhanh@gmail.com', '123456789', 'VP', 1, 1, 1)

go

create table Faculty(
	id int primary key identity(1,1),
	name nvarchar(50) not null,
	description text,
	status bit default 1
)

go

insert into Faculty values ('Information technology', '', 1)
insert into Faculty values ('Business', '', 1)

go

create table Student(
	id int primary key identity(1,1),
	facultyid int not null references Faculty(id),
	userid int not null references [User](id),
	status bit default 1
)
go

insert into Student values (1, 1, 1)
insert into Student values (1, 5, 1)
go 

create table Coordinator(
	id int primary key identity(1,1),
	facutyid int not null references Faculty(id),
	userid int not null references [User](id),
	status bit default 1
)

go
insert into Coordinator values (1, 2, 1)
go 

create table Academyyear(
	id int primary key identity(1,1),
	name nvarchar(100) not null,
	startReportDate date,
	closureReportDate date,
	closureEvidenceDate date,
	status bit default 1
)
go
insert into Academyyear values('SUM 2017', '2017-1-1', '2017-5-30', '2017-6-15', 1)
insert into Academyyear values('WIN 2016', '2016-6-30', '2017-1-1', '2017-1-15', 1)
go

create table Assessment(
	id int primary key identity(1,1),
	name nvarchar(100),
	description nvarchar(500),
	academyyearId int references Academyyear(id),
	status bit default 1
)

go
insert into Assessment values('Assignment', '', 1, 1)
insert into Assessment values('Project', '', 1, 1)
insert into Assessment values('Assignment', '', 2, 1)
go
create table Item(
	id int primary key identity(1,1),
	name nvarchar(100) not null,
	description nvarchar(500),
	assessmentId int references Assessment(id),
	status bit default 1
)
go
insert into Item values('Assignment 1', '', 1, 1)
insert into Item values('Assignment 2', '', 1, 1)
insert into Item values('Assignment 3', '', 1, 1)
insert into Item values('Project thery', '', 3, 1)
insert into Item values('Project practical', '', 3, 1)
go

create table Claim(
	id int primary key identity(1,1),
	name nvarchar(50),
	description	text, 
	datesubmited date,
	studentid int not null references Student(id),
	itemId int not null references Item(id), 
	coordinatorId int not null references Coordinator(id),
	dateresult date,
	result tinyint default 0, /*0 means not process yey, 1 means refuse, 2 means accept*/
	status bit default 1
)

go 

create table Evidence(
	id int primary key identity(1,1),
	filename nvarchar(100) not null,
	type tinyint default 1,
	claimid int not null references Claim(id),
	dateUpload date not null,
	status bit default 1
)