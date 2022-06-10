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
ON UPDATE CASCADE
ON DELETE CASCADE
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_Trainer] FOREIGN KEY([TrainerID])
REFERENCES [dbo].[Trainer] ([TrainerId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Session] CHECK CONSTRAINT [FK_Session_Trainer]
GO

CREATE TABLE [dbo].[Certificate](
	[CertificateId] [int] IDENTITY(1,1) NOT NULL,
	[Certificate] [varchar](50) NULL,
	[TrainerId] [int] NOT NULL,
 CONSTRAINT [PK_Certificate] PRIMARY KEY CLUSTERED 
(
	[CertificateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Certificate]  WITH CHECK ADD  CONSTRAINT [FK_Certificate_Certificate] FOREIGN KEY([TrainerId])
REFERENCES [dbo].[Trainer] ([TrainerId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Certificate] CHECK CONSTRAINT [FK_Certificate_Certificate]
GO

CREATE TABLE [dbo].[User](
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


/****** Object:  UserDefinedTableType [dbo].[CetificationType]    Script Date: 6/2/2022 9:53:21 PM ******/
CREATE TYPE [dbo].[CetificationType] AS TABLE(
	[name] [nvarchar](50) NULL
)
GO


/****** Object:  UserDefinedTableType [dbo].[SessionType]    Script Date: 6/2/2022 9:53:33 PM ******/
CREATE TYPE [dbo].[SessionType] AS TABLE(
	[title] [nvarchar](50) NULL,
	[description] [nvarchar](max) NULL
)
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
	@certificationsDt CetificationType READONLY,
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

	INSERT INTO [dbo].[Certificate] ([Certificate], [TrainerId]) SELECT [name] AS 'Certificate', @Identity AS 'TrainerId' FROM @certificationsDt;
END
GO


CREATE VIEW [dbo].[VW_DATA]
AS
SELECT t.TrainerId, t.FirstName, t.LastName, t.Email, t.YearsOfExperience, t.HasBlog, t.BlogURL, t.Employer, t.RegistrationFee, w.BrowserID, w.BrowserName, w.MajorVersion, s.SessionID, s.Title, s.Description, c.[CertificateId], c.[Certificate] FROM [dbo].[Trainer] t
LEFT OUTER JOIN [dbo].[WebBrowser] w ON t.BrowserID = w.BrowserID
LEFT OUTER JOIN [dbo].[Session] s ON t.TrainerId = s.TrainerID
LEFT OUTER JOIN [dbo].[Certificate] c ON t.TrainerId = c.TrainerId;
GO


CREATE PROCEDURE [dbo].[SP_CREATE_USER] 
(
	@Username NVARCHAR(50),
	@Password NVARCHAR(50)
)
AS
BEGIN
	INSERT INTO [dbo].[User] ([Username], [Password]) VALUES (@Username, @Password);
END
GO


CREATE PROCEDURE [dbo].[SP_LOGIN_USER] 
(
	@Username NVARCHAR(50),
	@Password NVARCHAR(50)
)
AS
BEGIN
	SELECT [Username] FROM [dbo].[User] WHERE [Username] = @Username AND [Password] = @Password;
END
GO


EXEC [dbo].[SP_CREATE_USER] 'admin', 'admin'