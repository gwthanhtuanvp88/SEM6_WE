use ReportClaim

go

-- Faculty
insert into [dbo].[Faculty] (name)
values('facuty1')

insert into [dbo].[Faculty] (name)
values('facuty2')

go 

-- Student

INSERT INTO [dbo].[Student] ([name],[email],[phone],[address],[facultyid])
VALUES ('student 1','student1@mail.com','0987654321','address 1',1)
GO

INSERT INTO [dbo].[Student] ([name],[email],[phone],[address],[facultyid])
VALUES ('student 1','student1@mail.com','0987654321','address 1',1)

-- Acadeny Year

INSERT INTO [dbo].[Academyyear] ([startReportDate] ,[closureReportDate] ,[closureEvidenceDate])
VALUES ('2017-01-12' ,'2017-02-12' ,'2017-02-12')
GO

-- Clain

INSERT INTO [dbo].[Claim] ([name],[description],[studentid],[academyyear])
VALUES ('claim'
		,'Lorem Ipsum is simply dummy text of the printing and typesetting industry.\n Lorem Ipsum has been the industry standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. \n It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. '
		,1,1)
GO

-- Evidence

INSERT INTO [dbo].[Evidence] ([filename],[claimid])
VALUES ('evidence1.txt',2)
GO

-- coordinator

INSERT INTO [dbo].[Coordinator] ([name],[email],[phone],[address],[facutyid])
VALUES ('coordinator1','coordinator1@mail.com','0987654321','address1',1)
GO



-- Claim resutl

INSERT INTO [dbo].[ClaimReult] ([claimid],[coordinatorid],[description])
VALUES (2,1,'description')
GO


 


