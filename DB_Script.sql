
CREATE TABLE [dbo].[WebBrowser](
	[BrowserID] [int] IDENTITY(1,1) NOT NULL,
	[BrowserName] [nvarchar](50) NULL,
	[MajorVersion] [int] NULL,
 CONSTRAINT [PK_WebBrowser] PRIMARY KEY CLUSTERED 
(
	[BrowserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Trainer](
	[TrainerId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[YearsOfExperience] [int] NULL,
	[HasBlog] [bit] NULL,
	[BlogURL] [nvarchar](100) NULL,
	[Employer] [nvarchar](20) NULL,
	[RegistrationFee] [int] NULL,
	[BrowserID] [int] NULL,
 CONSTRAINT [PK_Trainer] PRIMARY KEY CLUSTERED 
(
	[TrainerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Trainer]  WITH CHECK ADD  CONSTRAINT [FK_Trainer_WebBrowser] FOREIGN KEY([BrowserID])
REFERENCES [dbo].[WebBrowser] ([BrowserID])
GO

ALTER TABLE [dbo].[Trainer] CHECK CONSTRAINT [FK_Trainer_WebBrowser]
GO

CREATE TABLE [dbo].[Session](
	[SessionID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[TrainerID] [int] NULL,
 CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED 
(
	[SessionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_Trainer] FOREIGN KEY([TrainerID])
REFERENCES [dbo].[Trainer] ([TrainerId])
GO

ALTER TABLE [dbo].[Session] CHECK CONSTRAINT [FK_Session_Trainer]
GO


/****** Object:  UserDefinedTableType [dbo].[SessionType]    Script Date: 6/2/2022 4:18:34 AM ******/
CREATE TYPE [dbo].[SessionType] AS TABLE(
	[title] [nvarchar](50) NULL,
	[description] [nvarchar](max) NULL
)
GO

USE [SimplyLearn]
GO

/****** Object:  StoredProcedure [dbo].[SP_SAVE_TRAINER]    Script Date: 6/2/2022 4:18:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_SAVE_TRAINER] 
(
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@Email NVARCHAR(50),
	@YearsOfExperience INT,
	@HasBlog BIT,
	@BlogURL NVARCHAR(100),
	@BrowserName NVARCHAR(50),
	@BrowserVersion INT,
	@Employer NVARCHAR(20),
	@RegistrationFee INT,
	@Identity INT OUT,
	@SessionsDt SessionType READONLY
)
AS
BEGIN

	DECLARE @BrowserID INT;

	INSERT INTO [dbo].[WebBrowser] ([BrowserName], [MajorVersion])
	VALUES (@BrowserName, @BrowserVersion);

	SET @BrowserID = SCOPE_IDENTITY();

	INSERT INTO [dbo].[Trainer] ([FirstName]
      ,[LastName]
      ,[Email]
      ,[YearsOfExperience]
      ,[HasBlog]
      ,[BlogURL]
      ,[Employer]
      ,[RegistrationFee]
      ,[BrowserID])
	VALUES (@FirstName
	,@LastName
	,@Email
	,@YearsOfExperience
	,@HasBlog
	,@BlogURL
	,@Employer
	,@RegistrationFee
	,@BrowserID)

	SET @Identity = SCOPE_IDENTITY();

	INSERT INTO [dbo].[Session] ([Title], [Description], [TrainerID]) SELECT [title] AS 'Title', [description] AS 'Description', @Identity AS 'TrainerID' FROM @SessionsDt;
END
GO

