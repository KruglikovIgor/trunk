USE [master]
IF db_id('KPK2') IS NOT NULL DROP DATABASE KPK2
CREATE DATABASE [KPK2] COLLATE Cyrillic_General_CI_AS
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [KPK2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [KPK2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [KPK2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [KPK2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [KPK2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [KPK2] SET ARITHABORT OFF 
GO
ALTER DATABASE [KPK2] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [KPK2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [KPK2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [KPK2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [KPK2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [KPK2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [KPK2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [KPK2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [KPK2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [KPK2] SET  DISABLE_BROKER 
GO
ALTER DATABASE [KPK2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [KPK2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [KPK2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [KPK2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [KPK2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [KPK2] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [KPK2] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [KPK2] SET  MULTI_USER 
GO
ALTER DATABASE [KPK2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [KPK2] SET DB_CHAINING OFF 
GO
USE [KPK2]
GO
/****** Object:  UserDefinedFunction [dbo].[FullNameShortForm]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin Oleg
-- Create date: 30/11/2012
-- Description:	Возвращает сокращенную форму записи исполнителя
-- =============================================
CREATE FUNCTION [dbo].[FullNameShortForm]
(
	@EmployeeId int
)
RETURNS nvarchar(100)
AS
BEGIN	
	declare @result nvarchar(100)
	select @result = EmployeeFirstName + case when LEN(EmployeeSecondName) > 0 then ' ' + SUBSTRING(EmployeeSecondName, 1, 1) + '.' else '' end + case when LEN(EmployeeLastName) > 0 then SUBSTRING(EmployeeLastName, 1, 1) + '.' else '' end from Employee where @EmployeeId = EmployeeId
	return @result
END

GO
/****** Object:  UserDefinedFunction [dbo].[GetCurrentLangId]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin O.A.
-- Create date: 02.03.2018
-- Description:	Возвращает Id текущего языка
-- =============================================
CREATE FUNCTION [dbo].[GetCurrentLangId]
(
)
RETURNS int
AS
BEGIN
	DECLARE @LangId int

	SELECT @LangId=Id
	FROM dbo.Lang
	WHERE (ShortName = (SELECT ShortName FROM dbo.Lang WHERE (Id = 0))) AND (Id > 0)
	
	RETURN @LangId
END

GO
/****** Object:  UserDefinedFunction [dbo].[IsOkSelectedParameter]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[IsOkSelectedParameter]
(
	@TestId int,
	@ParamId int	
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @result int
	SET @result = 2

	-- Add the T-SQL statements to compute the return value here
	SELECT @result = case when (Result is NULL ) then 2
						when (Result <= MaxValue) AND (Result >= MinValue) then 1
					 else 0 end
	from Parameters join MeasuringParam ON Parameters.ParamId = MeasuringParam.ParamId
	where TestId = @TestId AND Parameters.ParamId = @ParamId

	-- Return the result of the function
	RETURN @result

END

GO
/****** Object:  Table [dbo].[Version]    Script Date: 06.09.2021 11:03:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Version](
	[Id] [int] IDENTITY(1,1) NOT NULL,

	[DBVersion] [nchar](10) NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Version] ON 

GO
INSERT [dbo].[Version] ([Id], [DBVersion]) VALUES (1, N'2.0.6')
GO
SET IDENTITY_INSERT [dbo].[Version] OFF

GO
/****** Object:  Table [dbo].[ACType]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ACType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ACTypeName] [nvarchar](30) NOT NULL,
 CONSTRAINT [PK_HelicopterType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AdditionalParameter]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdditionalParameter](
	[TestId] [int] NOT NULL,
	[ParamId] [int] NOT NULL,
	[Value] [nvarchar](100) NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AdditionalParameterReference]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdditionalParameterReference](
	[ParamId] [int] IDENTITY(1,1) NOT NULL,
	[ParamName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_AdditionalParameterReference] PRIMARY KEY CLUSTERED 
(
	[ParamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Calibration]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Calibration](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Channel] [int] NOT NULL,
	[IdealValue] [decimal](18, 5) NOT NULL,
	[Value] [int] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Channel]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Channel](
	[ModuleId] [int] NOT NULL,
	[ChannelNum] [int] NOT NULL,
	[ChannelName] [nvarchar](50) NOT NULL,
	[Note] [nvarchar](254) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Composition]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Composition](
	[TestId] [int] NOT NULL,
	[BlockId] [int] NOT NULL,
	[No] [nvarchar](12) NULL,
	[TotalRun] [int] NULL,
	[LastRun] [int] NULL,
 CONSTRAINT [PK_Composition_1] PRIMARY KEY CLUSTERED 
(
	[TestId] ASC,
	[BlockId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CompositionReference]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompositionReference](
	[DeviceId] [int] NOT NULL,
	[BlockId] [int] NOT NULL,
	[BlockName] [nvarchar](100) NULL,
	[BlockShortName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_CompositionReference] PRIMARY KEY CLUSTERED 
(
	[DeviceId] ASC,
	[BlockId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ControlTask]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ControlTask](
	[Name] [nvarchar](50) NOT NULL,
	[Number] [int] NOT NULL,
	[Amplitude] [int] NOT NULL,
	[Shift] [int] NOT NULL,
	[Note] [nvarchar](254) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DescriptionParam]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DescriptionParam](
	[ParamID] [int] IDENTITY(1,1) NOT NULL,
	[Note] [nvarchar](100) NULL,
	[ParamName] [nvarchar](50) NOT NULL,
	[SetMinValue] [real] NULL,
	[SetMaxValue] [real] NULL,
	[UnitsMeasurement] [nvarchar](10) NULL,
	[COM_Port] [nvarchar](50) NOT NULL,
	[Commands] [nvarchar](500) NOT NULL,
	[TimeOut] [int] NULL,
 CONSTRAINT [PK_DescriptionParam] PRIMARY KEY CLUSTERED 
(
	[ParamID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DeviceType]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceType](
	[DeviceTypeId] [int] IDENTITY(1,1) NOT NULL,
	[DeviceName] [nvarchar](200) NOT NULL,
	[DeviceShortName] [nvarchar](20) NULL,
 CONSTRAINT [PK_DeviceType] PRIMARY KEY CLUSTERED 
(
	[DeviceTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DocumentLink]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DocumentLink](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeviceId] [int] NOT NULL,
	[TestReferenceId] [int] NOT NULL,
	[Text] [varchar](200) NOT NULL,
	[Comment] [varchar](200) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeNumber] [varchar](50) NULL,
	[EmployeeFirstName] [varchar](50) NULL,
	[EmployeeSecondName] [varchar](50) NULL,
	[EmployeeLastName] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[Image] [image] NULL,
	[Note] [varchar](50) NULL,
	[PostId] [int] NULL,
 CONSTRAINT [PK_Executors] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HeaderPrint]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HeaderPrint](
	[Date] [nvarchar](50) NULL,
	[Number] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[HelicopterMark] [nvarchar](50) NULL,
	[HelicopterSign] [nvarchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Lang]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lang](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShortName] [nchar](2) NOT NULL,
	[FullName] [nvarchar](50) NULL,
 CONSTRAINT [PK_Lang] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LTRSignal]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LTRSignal](
	[SignalId] [int] IDENTITY(1,1) NOT NULL,
	[Slot] [int] NOT NULL,
	[Channel] [int] NOT NULL,
	[Range] [float] NOT NULL,
	[Alone] [bit] NOT NULL CONSTRAINT [DF_LTRSignal_Alone]  DEFAULT ((0)),
	[ACDC] [bit] NOT NULL CONSTRAINT [DF_LTRSignal_ACDC]  DEFAULT ((0)),
	[Note] [nvarchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MeasuringParam]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeasuringParam](
	[TestId] [int] NOT NULL,
	[ParamId] [int] NOT NULL,
	[Result] [decimal](18, 5) NULL,
 CONSTRAINT [PK_MeasuringParam] PRIMARY KEY CLUSTERED 
(
	[TestId] ASC,
	[ParamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Module]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Module](
	[ModuleId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleName] [varchar](50) NULL,
	[PortId] [int] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Parameters]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parameters](
	[ParamId] [int] IDENTITY(1,1) NOT NULL,
	[ParamName] [nvarchar](50) NOT NULL,
	[DeviceId] [int] NOT NULL,
	[Command] [nvarchar](50) NULL,
	[Koef] [float] NOT NULL CONSTRAINT [DF_Command_Koef]  DEFAULT ((1)),
	[MinValue] [float] NULL,
	[MaxValue] [float] NULL,
	[Precision] [int] NOT NULL CONSTRAINT [DF_Parameters_Precision]  DEFAULT ((2)),
	[Note] [nvarchar](100) NULL,
	[AddCommand] [nvarchar](50) NULL,
 CONSTRAINT [PK_Parameters] PRIMARY KEY CLUSTERED 
(
	[ParamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ParamPrint]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ParamPrint](
	[Param] [nvarchar](200) NULL,
	[Units] [nvarchar](50) NULL,
	[Standard] [nvarchar](75) NULL,
	[Fact] [nvarchar](50) NULL,
	[FontBold] [bit] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PassportValues]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PassportValues](
	[TestId] [int] NOT NULL,
	[ParamId] [int] NOT NULL,
	[Value] [decimal](18, 5) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PassportValuesReference]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PassportValuesReference](
	[DeviceTypeId] [int] NOT NULL,
	[ParamId] [int] NOT NULL,
	[Number] [int] NOT NULL,
	[Name] [nvarchar](300) NULL,
	[DefaultValue] [decimal](18, 5) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Port]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Port](
	[PortID] [int] IDENTITY(1,1) NOT NULL,
	[PortName] [nvarchar](50) NULL,
	[DataBits] [int] NOT NULL CONSTRAINT [DF__Port__DataBits__7CD98669]  DEFAULT ((8)),
	[BaudRate] [int] NOT NULL CONSTRAINT [DF__Port__BaudRate__7DCDAAA2]  DEFAULT ((115200)),
	[Parity] [int] NOT NULL CONSTRAINT [DF__Port__Parity__7BE56230]  DEFAULT ((0)),
	[StopBits] [float] NOT NULL CONSTRAINT [DF__Port__StopBits__7AF13DF7]  DEFAULT ((1)),
	[ReadTimeout] [int] NOT NULL CONSTRAINT [DF_Port_ReadTimeout]  DEFAULT ((0)),
	[WriteTimeout] [int] NOT NULL CONSTRAINT [DF_Port_WriteTimeout]  DEFAULT ((0)),
 CONSTRAINT [PK_Port] PRIMARY KEY CLUSTERED 
(
	[PortID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Post]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Post](
	[PostId] [int] IDENTITY(1,1) NOT NULL,
	[PostName] [varchar](50) NOT NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED 
(
	[PostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Report]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeviceTypeId] [int] NOT NULL,
	[Item] [int] NOT NULL,
	[SubItem] [int] NOT NULL,
	[OrderNumber] [int] NULL,
	[CheckName] [nvarchar](200) NULL,
	[Units] [nvarchar](50) NULL,
	[Requirement] [nvarchar](75) NULL,
	[ParamId] [int] NULL,
 CONSTRAINT [PK_Report] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TablesToTranslate]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TablesToTranslate](
	[TableId] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [nvarchar](50) NULL,
	[IdColumn] [nvarchar](50) NULL,
	[TranslateColumn] [nvarchar](50) NULL,
 CONSTRAINT [PK_TablesToTranslate] PRIMARY KEY CLUSTERED 
(
	[TableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Testing]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Testing](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DeviceTypeId] [int] NOT NULL,
	[ProductNumber] [nvarchar](20) NOT NULL,
	[MakingDate] [smalldatetime] NOT NULL,
	[HelicopterMark] [nvarchar](20) NULL,
	[HelicopterSign] [nvarchar](20) NULL,
	[HelicopterNumber] [nvarchar](20) NULL,
	[TestDate] [smalldatetime] NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[Note] [nvarchar](max) NULL,
	[TOFormId] [int] NULL,
	[TotalRun] [int] NULL,
	[LastRun] [int] NULL,
	[Result] [bit] NULL,
	[IsManualResult] [bit] NOT NULL,
 CONSTRAINT [PK_Block] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TOForm]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TOForm](
	[TOFormId] [int] IDENTITY(1,1) NOT NULL,
	[TOFormName] [varchar](150) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Translate]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Translate](
	[LangId] [int] NOT NULL,
	[TableId] [int] NOT NULL,
	[PropId] [int] NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Translate] PRIMARY KEY CLUSTERED 
(
	[LangId] ASC,
	[TableId] ASC,
	[PropId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TranslateEquals]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TranslateEquals](
	[LangId] [int] NULL,
	[OriginalText] [nvarchar](50) NULL,
	[TranslatedText] [nvarchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[PassportValuesReferenceLang]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[PassportValuesReferenceLang]
AS
SELECT     dbo.PassportValuesReference.DeviceTypeId, dbo.PassportValuesReference.ParamId, dbo.PassportValuesReference.Number, ISNULL(dbo.Translate.Text, 
                      dbo.PassportValuesReference.Name) AS Name, dbo.PassportValuesReference.DefaultValue
FROM         dbo.PassportValuesReference LEFT OUTER JOIN
                      dbo.Lang ON dbo.Lang.Id = dbo.GetCurrentLangId() LEFT OUTER JOIN
                      dbo.Translate ON dbo.Translate.PropId = dbo.PassportValuesReference.ParamId AND dbo.Translate.TableId = 3 AND dbo.Translate.LangId = dbo.Lang.Id

GO
/****** Object:  View [dbo].[ReportLang]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ReportLang]
AS
SELECT     dbo.Report.Id, dbo.Report.DeviceTypeId, dbo.Report.Item, dbo.Report.SubItem, dbo.Report.OrderNumber, ISNULL(t1.Text, dbo.Report.CheckName) 
                      AS CheckName, ISNULL(tUnit.TranslatedText, dbo.Report.Units) AS Units, ISNULL(t2.Text, dbo.Report.Requirement) AS Requirement, 
                      dbo.Report.ParamId
FROM         dbo.Report LEFT OUTER JOIN
                      dbo.Lang ON dbo.Lang.Id = dbo.GetCurrentLangId() LEFT OUTER JOIN
                      dbo.Translate AS t1 ON t1.PropId = dbo.Report.Id AND t1.TableId = 1 AND t1.LangId = dbo.Lang.Id LEFT OUTER JOIN
                      dbo.Translate AS t2 ON t2.PropId = dbo.Report.Id AND t2.TableId = 2 AND t2.LangId = dbo.Lang.Id LEFT OUTER JOIN
                      dbo.TranslateEquals AS tUnit ON tUnit.LangId = dbo.Lang.Id AND dbo.Report.Units = tUnit.OriginalText

GO

SET IDENTITY_INSERT [dbo].[ACType] ON 

GO
INSERT [dbo].[ACType] ([Id], [ACTypeName]) VALUES (1, N'Ми-8')
INSERT [dbo].[ACType] ([Id], [ACTypeName]) VALUES (2, N'Ми-17')
INSERT [dbo].[ACType] ([Id], [ACTypeName]) VALUES (3, N'Ми-24')
INSERT [dbo].[ACType] ([Id], [ACTypeName]) VALUES (4, N'Ми-35М')
INSERT [dbo].[ACType] ([Id], [ACTypeName]) VALUES (5, N'Ми-26')
INSERT [dbo].[ACType] ([Id], [ACTypeName]) VALUES (6, N'Ми-28Н')
INSERT [dbo].[ACType] ([Id], [ACTypeName]) VALUES (7, N'Ка-27')
INSERT [dbo].[ACType] ([Id], [ACTypeName]) VALUES (8, N'Ка-28')
INSERT [dbo].[ACType] ([Id], [ACTypeName]) VALUES (9, N'Ка-29')
INSERT [dbo].[ACType] ([Id], [ACTypeName]) VALUES (10, N'Ка-31Р')
INSERT [dbo].[ACType] ([Id], [ACTypeName]) VALUES (11, N'Ка-52')
INSERT [dbo].[ACType] ([Id], [ACTypeName]) VALUES (12, N'Ка-226')
INSERT [dbo].[ACType] ([Id], [ACTypeName]) VALUES (13, N'Ил-76')
GO
SET IDENTITY_INSERT [dbo].[ACType] OFF
GO
SET IDENTITY_INSERT [dbo].[AdditionalParameterReference] ON 

GO
INSERT [dbo].[AdditionalParameterReference] ([ParamId], [ParamName]) VALUES (1, N'TypeDiss32')
INSERT [dbo].[AdditionalParameterReference] ([ParamId], [ParamName]) VALUES (2, N'Modification')
INSERT [dbo].[AdditionalParameterReference] ([ParamId], [ParamName]) VALUES (4, N'CorrectionCode1')
INSERT [dbo].[AdditionalParameterReference] ([ParamId], [ParamName]) VALUES (5, N'CorrectionCode2')
INSERT [dbo].[AdditionalParameterReference] ([ParamId], [ParamName]) VALUES (6, N'CorrectionCode3')
INSERT [dbo].[AdditionalParameterReference] ([ParamId], [ParamName]) VALUES (7, N'TypeVSS')
INSERT [dbo].[AdditionalParameterReference] ([ParamId], [ParamName]) VALUES (8, N'CheckTemperature')
INSERT [dbo].[AdditionalParameterReference] ([ParamId], [ParamName]) VALUES (9, N'TypeBlock5')

GO
SET IDENTITY_INSERT [dbo].[AdditionalParameterReference] OFF
GO
SET IDENTITY_INSERT [dbo].[Calibration] ON 

GO
INSERT [dbo].[Calibration] ([Id], [ModuleId], [Channel], [IdealValue], [Value]) VALUES (1, 5, 0, CAST(39764.00000 AS Decimal(18, 5)), 15000)
INSERT [dbo].[Calibration] ([Id], [ModuleId], [Channel], [IdealValue], [Value]) VALUES (2, 5, 1, CAST(2620.00000 AS Decimal(18, 5)), 1000)
INSERT [dbo].[Calibration] ([Id], [ModuleId], [Channel], [IdealValue], [Value]) VALUES (3, 5, 1, CAST(10574.00000 AS Decimal(18, 5)), 4000)
INSERT [dbo].[Calibration] ([Id], [ModuleId], [Channel], [IdealValue], [Value]) VALUES (4, 5, 2, CAST(2632.00000 AS Decimal(18, 5)), 1000)
INSERT [dbo].[Calibration] ([Id], [ModuleId], [Channel], [IdealValue], [Value]) VALUES (5, 5, 2, CAST(10582.00000 AS Decimal(18, 5)), 4000)
INSERT [dbo].[Calibration] ([Id], [ModuleId], [Channel], [IdealValue], [Value]) VALUES (11, 5, 1, CAST(21174.00000 AS Decimal(18, 5)), 8000)
INSERT [dbo].[Calibration] ([Id], [ModuleId], [Channel], [IdealValue], [Value]) VALUES (12, 5, 1, CAST(31804.00000 AS Decimal(18, 5)), 12000)
INSERT [dbo].[Calibration] ([Id], [ModuleId], [Channel], [IdealValue], [Value]) VALUES (13, 5, 1, CAST(39781.00000 AS Decimal(18, 5)), 15000)
INSERT [dbo].[Calibration] ([Id], [ModuleId], [Channel], [IdealValue], [Value]) VALUES (14, 5, 2, CAST(21182.00000 AS Decimal(18, 5)), 8000)
INSERT [dbo].[Calibration] ([Id], [ModuleId], [Channel], [IdealValue], [Value]) VALUES (15, 5, 2, CAST(31784.00000 AS Decimal(18, 5)), 12000)
INSERT [dbo].[Calibration] ([Id], [ModuleId], [Channel], [IdealValue], [Value]) VALUES (16, 5, 2, CAST(40265.00000 AS Decimal(18, 5)), 15200)
INSERT [dbo].[Calibration] ([Id], [ModuleId], [Channel], [IdealValue], [Value]) VALUES (18, 5, 0, CAST(31787.00000 AS Decimal(18, 5)), 12000)
INSERT [dbo].[Calibration] ([Id], [ModuleId], [Channel], [IdealValue], [Value]) VALUES (19, 5, 0, CAST(21185.00000 AS Decimal(18, 5)), 8000)
INSERT [dbo].[Calibration] ([Id], [ModuleId], [Channel], [IdealValue], [Value]) VALUES (20, 5, 0, CAST(10582.00000 AS Decimal(18, 5)), 4000)
INSERT [dbo].[Calibration] ([Id], [ModuleId], [Channel], [IdealValue], [Value]) VALUES (21, 5, 0, CAST(2628.00000 AS Decimal(18, 5)), 1000)
GO
SET IDENTITY_INSERT [dbo].[Calibration] OFF
GO
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 0, N'TK_15', N'Реле Токи Кристаллов  (ДИСС-15)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 1, N'Equivalent', N'Эквивалент (ДИСС-15)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 2, N'Band_2', N'Полоса-2 (Шум для ДИСС-15)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 3, N'RRU', N'РРУ (ДИСС-15)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 12, N'Block_8', N'Блок 8')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 4, N'High_15', N'"Высокое" (ДИСС-15)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 5, N'UNCh_15', N'УНЧ для КЗ (ДИСС-15)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 6, N'Task_15', N'КЗ (ДИСС-15)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 13, N'Course', N'КУРС (ДИСС-15,-32)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 14, N'Pitch', N'ТАНГАЖ (ДИСС-15,-32)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 15, N'Heel', N'КРЕН (ДИСС-15,-32)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 20, N'UNCh_32', N'УНЧ для КЗ (ДИСС-32)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 8, N'Task_1_32', N'КЗ-1 (ДИСС-32)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 9, N'Task_2_32', N'КЗ-2 (ДИСС-32)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 10, N'Task_3_32', N'КЗ-3 (ДИСС-32)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 11, N'Task_4_32', N'КЗ-4 (ДИСС-32)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 16, N'TK_32', N'Токи кристаллов (ДИСС-32)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (7, 17, N'High_32', N'"Высокое" (ДИСС-32)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 21, N'Izdelie', N'Изделие (ДИСС-15)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 0, N'Capture_1', N'Захват 1')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 1, N'Capture_2', N'Захват 2')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 2, N'Capture_3', N'Захват 3')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 3, N'GoodCondition', N'Исправность')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 4, N'Memory_1', N'Память 1')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 5, N'Memory_2', N'Память 2')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 8, N'BPK_On', N'БПК включён')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 9, N'Flight', N'Сигнал "Полёт"')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 10, N'Refuse_VCh', N'Откач ВЧ')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 11, N'Refuse_VSS', N'Отказ ВСС')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 12, N'Capture', N'Захват')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 22, N'Refuse_BVK', N'Отказ БВК')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 14, N'DPSM', N'Исчисление ДПСМ')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 16, N'VSS_Good', N'Исправность ВСС')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 17, N'Block8_Off', N'Блок 8 отключён')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 18, N'DissOn', N'ДИСС Включен(ДИСС-32)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 19, N'High', N'Высокое(ДИСС-32)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (10, 5, N'Calculus', N'Счисл.ДИСС (ДИСС-32-90А)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (10, 3, N'Disconnect', N'Откл.коорд (ДИСС-32-90А)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (10, 4, N'Navigation', N'НАВИГАЦИЯ (ДИСС-32-90А)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (10, 1, N'BVK_Refuse', N'Отключение БВК')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (10, 6, N'DPSM_32', N'Счисл.ДПСМ(ДИСС-32-90А)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (10, 2, N'BVK_Good', N'Исправн.БВК(ДИСС-32-90А)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 20, N'HV', N'Высокое напряжение (ДИСС-15)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (6, 13, N'Refuse', N'Отказ (ДИСС-15)')
INSERT [dbo].[Channel] ([ModuleId], [ChannelNum], [ChannelName], [Note]) VALUES (10, 0, N'Block8_32', N'Откл.Бл.8 (ДИСС-32-90А)')
GO

INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (1, 1, NULL, N'ВЧ')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (1, 2, NULL, N'НЧ')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (1, 3, NULL, N'Бл. 5')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (1, 4, NULL, N'Бл. 5И')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (1, 5, NULL, N'Бл. 6')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (1, 6, NULL, N'Бл. 7')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (1, 7, NULL, N'Бл. 8')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (1, 8, NULL, N'Прибор ПК')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (1, 9, N'Картографический индикатор', N'Бл. 10')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (1, 10, NULL, N'СК КИ')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (1, 11, NULL, N'Бл. 9М')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (2, 1, NULL, N'ВЧ')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (2, 2, NULL, N'ВСС')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (2, 3, NULL, N'НП-2')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (2, 4, NULL, N'БВК')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (2, 5, NULL, N'Бл. 6')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (2, 6, NULL, N'Инд. УС ПС №1')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (2, 7, NULL, N'Инд. УС ПС №2')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (2, 8, NULL, N'Бл. 8 №1')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (2, 9, NULL, N'Бл. 8 №2')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (2, 10, NULL, N'Прибор БПК')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (2, 11, NULL, N'Прибор КС')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (2, 12, NULL, N'Бл. 10')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (2, 13, NULL, N'СК КИ')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (2, 14, NULL, N'Рама (ВСС, НП-2)')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (2, 15, NULL, N'Рама (БВК)')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (3, 1, NULL, N'ВЧ')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (3, 2, NULL, N'НЧ')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (3, 3, NULL, N'БС-3М')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (3, 4, NULL, N'Рама БС-3М')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (3, 5, NULL, N'Инд. ИП2/1')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (3, 6, NULL, N'Инд. И')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (4, 1, NULL, N'ВЧ')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (4, 2, NULL, N'Бл. БС-28М')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (5, 1, NULL, N'ВЧ')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (5, 2, NULL, N'ВСС')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (5, 3, NULL, N'НП-2')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (5, 4, NULL, N'Бл. 6')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (5, 5, NULL, N'Инд. УС ПС №1')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (5, 6, NULL, N'Прибор БПК')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (5, 7, NULL, N'Прибор КС')
INSERT [dbo].[CompositionReference] ([DeviceId], [BlockId], [BlockName], [BlockShortName]) VALUES (5, 8, NULL, N'Рама (ВСС, НП-2)')
GO

INSERT [dbo].[ControlTask] ([Name], [Number], [Amplitude], [Shift], [Note]) VALUES (N'Task_W_1_15', 1, 107, 101, N'Контрольная задача 1 (ДИСС-15) - Б.')
INSERT [dbo].[ControlTask] ([Name], [Number], [Amplitude], [Shift], [Note]) VALUES (N'Task_W_2_15', 2, 107, 101, N'Контрольная задача 2 (ДИСС-15) - Б.')
INSERT [dbo].[ControlTask] ([Name], [Number], [Amplitude], [Shift], [Note]) VALUES (N'Task_W_3_15', 3, 107, 101, N'Контрольная задача 3 (ДИСС-15) - Б.')
INSERT [dbo].[ControlTask] ([Name], [Number], [Amplitude], [Shift], [Note]) VALUES (N'Task_W_4_15', 4, 107, 101, N'Контрольная задача 4 (ДИСС-15) - Б.')
INSERT [dbo].[ControlTask] ([Name], [Number], [Amplitude], [Shift], [Note]) VALUES (N'Task_W_5_15', 5, 107, 101, N'Контрольная задача 5 (ДИСС-15) - Б.')
INSERT [dbo].[ControlTask] ([Name], [Number], [Amplitude], [Shift], [Note]) VALUES (N'Task_W_6_15', 6, 107, 101, N'Контрольная задача 6 (ДИСС-15) - Б.')
INSERT [dbo].[ControlTask] ([Name], [Number], [Amplitude], [Shift], [Note]) VALUES (N'Task_W_1_32', 9, 72, 128, N'Контрольная задача 1 (ДИСС-32) - Б.')
INSERT [dbo].[ControlTask] ([Name], [Number], [Amplitude], [Shift], [Note]) VALUES (N'Task_1_15', 1, 200, 165, N'Контрольная задача 1 вкл (ДИСС-15)-Атт.')
INSERT [dbo].[ControlTask] ([Name], [Number], [Amplitude], [Shift], [Note]) VALUES (N'Task_4_15', 4, 255, 192, N'Контрольная задача 4 вкл (ДИСС-15)-Атт.')
INSERT [dbo].[ControlTask] ([Name], [Number], [Amplitude], [Shift], [Note]) VALUES (N'Task_2_15', 2, 255, 192, N'Контрольная задача 2 вкл (ДИСС-15)-Атт.')
GO
SET IDENTITY_INSERT [dbo].[DescriptionParam] ON 

GO
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (209, N'СД 1-1 (для ДИСС-32)', N'SD_1_1', 0.6, 1.8, N'B', N'LTR11:1', N'[usb:13]2,5;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (210, N'СД 1-2 (для ДИСС-32)', N'SD_1_2', 0.6, 1.8, N'B', N'LTR11:2', N'[usb:13]2,5;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (211, N'СД 2-1 (для ДИСС-32)', N'SD_2_1', 0.6, 1.8, N'B', N'LTR11:3', N'[usb:13]2,5;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (212, N'СД 2-2 (для ДИСС-32)', N'SD_2_2', 0.6, 1.8, N'B', N'LTR11:4', N'[usb:13]2,5;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (213, N'СД 3-1 (для ДИСС-32)', N'SD_3_1', 0.6, 1.8, N'B', N'LTR11:5', N'[usb:13]2,5;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (214, N'СД 3-2 (для ДИСС-32)', N'SD_3_2', 0.6, 1.8, N'B', N'LTR11:6', N'[usb:13]2,5;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (215, N'Шум УНЧ 1-1 (ДИСС-32) - атт.', N'UNCh_1_1_Noise', 30, 100, N'мВ', N'LTR22:1', N'[usb:15]1;1000;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (216, N'Шум УНЧ 1-2 (ДИСС-32) - атт.', N'UNCh_1_2_Noise', 30, 100, N'мВ', N'LTR22:2', N'[usb:15]1;1000;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (217, N'Шум УНЧ 2-1 (ДИСС-32) - атт.', N'UNCh_2_1_Noise', 30, 100, N'мВ', N'LTR22:3', N'[usb:15]1;1000;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (218, N'Шум УНЧ 2-2 (ДИСС-32) - атт.', N'UNCh_2_2_Noise', 30, 100, N'мВ', N'LTR22:4', N'[usb:15]1;1000;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (219, N'Шум УНЧ 3-1 (ДИСС-32) - атт.', N'UNCh_3_1_Noise', 30, 100, N'мВ', N'LTR22:1', N'[usb:16]1;1000;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (220, N'Шум УНЧ 3-2 (ДИСС-32) - атт.', N'UNCh_3_2_Noise', 30, 100, N'мВ', N'LTR22:2', N'[usb:16]1;1000;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (221, N'МОД 1 (ДИСС-32)', N'MOD_1', 2.9, 4.3, N'V', N'LTR11:7', N'[usb:13]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (222, N'МОД 2 (ДИСС-32)', N'MOD_2', -4.3, -2.9, N'V', N'LTR11:8', N'[usb:13]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (223, N'ГЛПД 1', N'GLPD_1', 2.4, 3.5, N'V', N'LTR11:9', N'[usb:13]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (224, N'ГЛПД 2', N'GLPD_2', 2.4, 3.5, N'V', N'LTR11:10', N'[usb:13]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (225, N'Ток ГД', N'I_GD', -2.9, -1.7, N'V', N'LTR11:11', N'[usb:13]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (226, N'Ток магнетрона (ДИСС-15)', N'I_M', -300, 300, N'V', N'LTR11:12', N'[usb:13]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (227, N'УНЧ 1-1', N'UNCh_1_1', 0.7, 1.7, N'B', N'LTR22:1', N'[usb:15]10;1;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (228, N'УНЧ 1-2', N'UNCh_1_2', 0.7, 1.7, N'B', N'LTR22:2', N'[usb:15]10;1;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (229, N'УНЧ 2-1', N'UNCh_2_1', 0.7, 1.7, N'B', N'LTR22:3', N'[usb:15]10;1;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (230, N'УНЧ 2-2', N'UNCh_2_2', 0.7, 1.7, N'B', N'LTR22:4', N'[usb:15]10;1;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (231, N'УНЧ 3-1', N'UNCh_3_1', 0.7, 1.7, N'B', N'LTR22:1', N'[usb:16]10;1;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (232, N'УНЧ 3-2', N'UNCh_3_2', 0.7, 1.7, N'B', N'LTR22:2', N'[usb:16]10;1;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (244, N'Wx_PKV (ДИСС15, ДИСС32)', N'Wx_PKV', -10, 10, N'B', N'LTR114:1', N'[usb:11]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (245, N'Wy_PKV (ДИСС15, ДИСС32)', N'Wy_PKV', -10, 10, N'B', N'LTR114:2', N'[usb:11]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (246, N'Wz_PKV (ДИСС15, ДИСС32)', N'Wz_PKV', -10, 10, N'B', N'LTR114:3', N'[usb:11]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (252, N'Wz_PKV (ДИСС15) Обн.', N'PKV_Z_Zero', -230, 230, N'мВ', N'LTR114:5', N'[usb:11]10;1000', 250)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (253, N'Wx_NKV (ДИСС15) Обн.', N'NKV_X_Zero', -40, 40, N'мВ', N'LTR114:4', N'[usb:11]10;1000', 250)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (254, N'Wy_NKV (ДИСС-32) Обн.', N'NKV_Y_Zero', -30, 30, N'мВ', N'LTR114:2', N'[usb:11]0,4;1000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (255, N'Wz_NKV (ДИСС15) Обн.', N'NKV_Z_Zero', -80, 80, N'мВ', N'LTR114:6', N'[usb:11]10;1000', 250)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (263, N'W (7V) (ДИСС-32)', N'W_7V', -300, 300, N'B', N'LTR114:13', N'[usb:11]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (264, N'Контроль P СВЧ (ДИСС-15)', N'P_SVCh', -300, 300, NULL, N'LTR11:15', N'[usb:12]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (265, N'+27V (ДИСС15) ', N'Plus_27B', 26.125, 28.875, N'B', N'LTR11:8', N'[usb:12]10;5', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (266, N'+5V (ДИСС-32)', N'Plus_5B', 4.75, 5.25, N'B', N'LTR11:1', N'[usb:12]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (267, N'-20V (ДИСС15) ', N'Minus_20B', -21, -19, N'B', N'LTR11:6', N'[usb:12]10;3', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (268, N'-27V (ДИСС15) ', N'Minus_27B', -28.35, -25.65, N'B', N'LTR11:9', N'[usb:12]10;5', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (269, N'+6V(ДИСС-32)', N'Plus_6B', 6, 6.6, N'B', N'LTR11:2', N'[usb:12]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (270, N'-6V (ДИСС15) ', N'Minus_6B', -6.615, -5.67, N'B', N'LTR11:3', N'[usb:12]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (271, N'+27V_sl(ДИСС-15)', N'Plus_27B_Sl', 25.65, 28.35, N'B', N'LTR11:11', N'[usb:12]10;5', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (272, N'-27V_op(ДИСС-15)', N'Minus_27B_Op', -28.35, -25.65, N'B', N'LTR11:12', N'[usb:12]10;5', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (273, N'd27V_op(ДИСС-15)', N'Delta_27B', -81, 81, N'mV', N'LTR114:15', N'[usb:11]0,4;6000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (274, N'+15V(ДИСС-32)', N'Plus_15B', 14.25, 15.75, N'B', N'LTR11:13', N'[usb:12]10;3', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (275, N'-15V(ДИСС-32)', N'Minus_15B', -15.75, -14.25, N'B', N'LTR11:14', N'[usb:12]10;3', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (276, N'd15V(ДИСС-32)', N'Delta_15B', -35, 35, N'мВ', N'LTR114:16', N'[usb:11]0,4;4000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (277, N'+12V (ДИСС15) ', N'Plus_12B', 11.97, 13.23, N'B', N'LTR11:4', N'[usb:12]10;2', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (278, N'-12V (ДИСС15) ', N'Minus_12B', -13.23, -11.97, N'B', N'LTR11:5', N'[usb:12]10;2', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (279, N'СВЧ 80 В (ВН)  (ДИСС-15)', N'BH', -57, -46, NULL, N'LTR11:7', N'[usb:12]10;8,910', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (280, N'ТКМ 1 (ДИСС-15)', N'TKM_1', -4.2, -3, NULL, N'LTR11:13', N'[usb:13]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (281, N'ТКМ 2 (ДИСС-15)', N'TKM_2', 3, 4.2, NULL, N'LTR11:14', N'[usb:13]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (282, N'a(L) (ДИСС15, ДИСС32) ', N'Alfa', -300, 300, NULL, N'LTR114:14', N'[usb:11]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (284, N'Ток K 1-1 (ДИСС-15 калибровка)', N'__1_1_15', 0.25, 1.6, N'mV', N'LTR11:1', N'[usb:14]0,6;1000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (285, N'Ток K 2-1 (ДИСС-15 калибровка)', N'I_K_2_1_15', 0.25, 1.6, N'mV', N'LTR11:3', N'[usb:14]0,6;1000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (286, N'Ток K 3-1 (ДИСС-15 калибровка)', N'I_K_3_1_15', 0.25, 1.6, N'mV', N'LTR11:5', N'[usb:14]0,6;1000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (287, N'Ток K 1-2 (ДИСС-15 калибровка)', N'I_K_1_2_15', -0.25, -1.6, N'mV', N'LTR11:2', N'[usb:14]0,6;1000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (288, N'Ток K 2-2 (ДИСС-15 калибровка)', N'I_K_2_2_15', -0.25, -1.6, N'mV', N'LTR11:4', N'[usb:14]0,6;1000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (289, N'Ток K 3-2 (ДИСС-15 калибровка)', N'I_K_3_2_15', -0.25, -1.6, N'mV', N'LTR11:6', N'[usb:14]0,6;1000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (290, N'Ток K 1-1 (ДИСС-32 калибровка) Высокое вкл.', N'I_K_1_1_On', -300, 300, N'mV', N'LTR11:7', N'[usb:14]0,6;1000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (291, N'Ток K 2-1 (ДИСС-32 калибровка) Высокое вкл.', N'I_K_2_1_On', -300, 300, N'mV', N'LTR11:9', N'[usb:14]0,6;1000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (292, N'Ток K 3-1 (ДИСС-32 калибровка) Высокое вкл.', N'I_K_3_1_On', -300, 300, N'mV', N'LTR11:11', N'[usb:14]0,6;1000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (293, N'Ток K 1-2 (ДИСС-32 калибровка) Высокое вкл.', N'I_K_1_2_On', -300, 300, N'mV', N'LTR11:8', N'[usb:14]0,6;1000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (294, N'Ток K 2-2 (ДИСС-32 калибровка) Высокое вкл.', N'I_K_2_2_On', -300, 300, N'mV', N'LTR11:10', N'[usb:14]0,6;1000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (295, N'Ток K 3-2 (ДИСС-32 калибровка) Высокое вкл.', N'I_K_3_2_On', -300, 300, N'mV', N'LTR11:12', N'[usb:14]0,6;1000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (296, N'Захват 1', N'Capture_1', NULL, NULL, NULL, N'COM5', N'[get:0]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (297, N'Захват 2', N'Capture_2', NULL, NULL, NULL, N'COM5', N'[get:1]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (298, N'Захват 3', N'Capture_3', NULL, NULL, NULL, N'COM5', N'[get:2]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (299, N'Исправность', N'GoodCondition', NULL, NULL, NULL, N'COM5', N'[get:3]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (300, N'Память 1', N'Memory_1', NULL, NULL, NULL, N'COM5', N'[get:4]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (301, N'Память 2', N'Memory_2', NULL, NULL, NULL, N'COM5', N'[get:5]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (302, N'БПК включён', N'BPK_On', NULL, NULL, NULL, N'COM5', N'[get:8]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (303, N'Сигнал "Полёт"', N'Flay', NULL, NULL, NULL, N'COM5', N'[get:9]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (304, N'Отказ ВЧ', N'Refuse_VCh', NULL, NULL, NULL, N'COM5', N'[get:10]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (305, N'Отказ ВСС ', N'Refuse_BCC', NULL, NULL, NULL, N'COM5', N'[get:11]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (306, N'Захват ', N'Capture', NULL, NULL, NULL, N'COM5', N'[get:12]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (307, N'Отказ БВК', N'Refuse_BVK', NULL, NULL, NULL, N'COM5', N'[get:22]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (308, N'Счисление ДПСМ', N'DPSM', NULL, NULL, NULL, N'COM5', N'[get:14]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (309, N'Исправность BCC', N'BCC_Good', NULL, NULL, NULL, N'COM5', N'[get:16]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (310, N'Откл. Бл.8', N'Block8_Off', NULL, NULL, NULL, N'COM5', N'[get:17]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (311, N'ДИСС Включен(ДИСС-32)', N'Diss_Included', NULL, NULL, NULL, N'COM5', N'[get:18]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (312, N'Высокое(ДИСС-32)', N'High', NULL, NULL, NULL, N'COM5', N'[get:19]0x0A;0x40', 120)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (313, N'Все выходы отключить(дискр.вых)', N'All_Off', NULL, NULL, NULL, N'COM4', N'[two]0x0A;0x34', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (314, N'Включить реле Токи Кристаллов  (ДИСС-15)', N'TK_On_15', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x80', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (315, N'Выключить реле Токи Кристаллов   (ДИСС-15)', N'TK_Off_15', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x00', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (316, N'Включить Эквивалент (ДИСС-15)', N'Equivalent', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x81', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (317, N'Включить Антенна (ДИСС-15)', N'Antenna', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x01', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (318, N'Включить Полоса-1 (Шум для ДИСС-15)', N'Band_1', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x02', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (319, N'Включить Полоса-2 (Шум для ДИСС-15)', N'Band_2', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x82', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (320, N'Включить РРУ (ДИСС-15)', N'RRU_On', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x83', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (321, N'Выключить РРУ (ДИСС-15)', N'RRU_Off', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x03', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (322, N'Включить Блок 8', N'Block_8_On', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x8C', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (323, N'Выключить Блок 8', N'Block_8_Off', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x0C', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (324, N'"Высокое" выключить (ДИСС-15)', N'Higt_Off_15', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x84', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (325, N'"Высокое" включить (ДИСС-15)', N'Higt_On_15', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x04', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (326, N'Включить УНЧ для КЗ (ДИСС-15)', N'UNCh_On_15', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x85', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (327, N'Выключить УНЧ для КЗ (ДИСС-15)', N'UNCh_Off_15', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x05', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (328, N'Включить КЗ для ДИСС-15', N'Task_On_15', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x86', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (329, N'Выключить КЗ для ДИСС-15', N'Task_Off_15', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x06', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (330, N'КУРС Включить (ДИСС-15,-32)', N'Course_On', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x8D', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (331, N'КУРС Выключить (ДИСС-15,-32)', N'Course_Off', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x0D', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (332, N'ТАНГАЖ Включить (ДИСС-15,-32)', N'Pitch_On', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x8E', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (333, N'ТАНГАЖ Выключить (ДИСС-15,-32)', N'Pitch_Off', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x0E', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (334, N'КРЕН Включить (ДИСС-15,-32)', N'Heel_On', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x8F', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (335, N'КРЕН Выключить (ДИСС-15,-32)', N'Heel_Off', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x0F', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (338, N'Включить УНЧ для КЗ (ДИСС-32)', N'UNCh_On_32', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x94', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (339, N'Выключить УНЧ для КЗ (ДИСС-32)', N'UNCh_Off_32', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x14', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (340, N'КЗ-1. Включить. (ДИСС-32)', N'Task_1_On_32', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x88', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (341, N'КЗ-1. Выключить. (ДИСС-32)', N'Task_1_Off_32', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x08', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (342, N'КЗ-2. Включить. (ДИСС-32)', N'Task_2_On_32', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x89', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (343, N'КЗ-2. Выключить. (ДИСС-32)', N'Task_2_Off_32', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x09', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (344, N'КЗ-3. Включить. (ДИСС-32)', N'Task_3_On_32', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x8A', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (345, N'КЗ-3. Выключить. (ДИСС-32)', N'Task_3_Off_32', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x0A', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (346, N'КЗ-4. Включить. (ДИСС-32)', N'Task_4_On_32', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x8B', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (347, N'КЗ-4. Выключить. (ДИСС-32)', N'Task_4_Off_32', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x0B', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (348, N'Включить Токи кристаллов (ДИСС-32)', N'TK_On_32', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x90', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (349, N'Выключить Токи кристаллов (ДИСС-32)', N'TK_Off_32', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x10', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (350, N'"ВЫСОКОЕ" Выключить. (ДИСС-32)', N'Higt_Off_32', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x91', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (351, N'"ВЫСОКОЕ"  Включить. (ДИСС-32)', N'Higt_On_32', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x11', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (359, N'УНЧ 1-1 (Шум-15) - Полоса I', N'UNCh_1_1_Noise_I', 10, 120, NULL, N'LTR22:1', N'[usb:15]1;1000;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (373, N'УНЧ 1-2 (Шум-15) - Полоса I', N'UNCh_1_2_Noise_I', 10, 120, NULL, N'LTR22:2', N'[usb:15]1;1000;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (374, N'УНЧ 2-1 (Шум-15) - Полоса I', N'UNCh_2_1_Noise_I', 10, 120, NULL, N'LTR22:3', N'[usb:15]1;1000;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (375, N'УНЧ 2-2 (Шум-15) - Полоса I', N'UNCh_2_2_Noise_I', 10, 120, NULL, N'LTR22:4', N'[usb:15]1;1000;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (376, N'УНЧ 3-1 (Шум-15) - Полоса I', N'UNCh_3_1_Noise_I', 10, 120, NULL, N'LTR22:1', N'[usb:16]1;1000;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (377, N'УНЧ 3-2 (Шум-15) - Полоса I', N'UNCh_3_2_Noise_I', 10, 120, NULL, N'LTR22:2', N'[usb:16]1;1000;AC', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (378, N'УНЧ 1-1 (Шум-15) - Полоса II', N'UNCh_1_1_Noise_II', 0, 130, NULL, N'COM100', N'[UNCh_1_1_Noise_I]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (379, N'УНЧ 1-2 (Шум-15) - Полоса II', N'UNCh_1_2_Noise_II', 0, 130, NULL, N'COM100', N'[UNCh_1_2_Noise_I]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (380, N'УНЧ 2-1 (Шум-15) - Полоса II', N'UNCh_2_1_Noise_II', 0, 130, NULL, N'COM100', N'[UNCh_2_1_Noise_I]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (381, N'УНЧ 2-2 (Шум-15) - Полоса II', N'UNCh_2_2_Noise_II', 0, 130, NULL, N'COM100', N'[UNCh_2_2_Noise_I]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (382, N'УНЧ 3-1 (Шум-15) - Полоса II', N'UNCh_3_1_Noise_II', 0, 130, NULL, N'COM100', N'[UNCh_3_1_Noise_I]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (383, N'УНЧ 3-2 (Шум-15) - Полоса II', N'UNCh_3_2_Noise_II', 0, 130, NULL, N'COM100', N'[UNCh_3_2_Noise_I]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (384, N'Ток К 1-1 (ДИСС-15)', N'I_K_1_1', 0.25, 1.6, NULL, N'СОМ100', N'[I_K_1_1_15]100', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (385, N'Ток К 1-2 (ДИСС-15)', N'I_K_1_2', -1.6, -0.25, NULL, N'COM100', N'[I_K_1_2_15]100', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (386, N'Ток К 2-1 (ДИСС-15)', N'I_K_2_1', 0.25, 1.6, NULL, N'COM100', N'[I_K_2_1_15]100', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (387, N'Ток К 2-2 (ДИСС-15)', N'I_K_2_2', -1.6, -0.25, NULL, N'COM100', N'[I_K_2_2_15]100', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (388, N'Ток К 3-1 (ДИСС-15)', N'I_K_3_1', 0.25, 1.6, NULL, N'COM100', N'[I_K_3_1_15]100', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (389, N'Ток К 3-2 (ДИСС-15)', N'I_K_3_2', -1.6, -0.25, NULL, N'COM100', N'[I_K_3_2_15]100', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (390, N'Ток К 1 канал (ДИСС-15) разброс в паре', N'I_K_1', 0, 0.65, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (391, N'Ток К 2 канал (ДИСС-15) разброс в паре', N'I_K_2', 0, 0.65, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (392, N'Ток К 3 канал (ДИСС-15) разброс в паре', N'I_K_3', 0, 0.65, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (393, N'Ток K 1-1 (ДИСС-32) Высокое выключено', N'I_K_1_1_Off', -300, 300, NULL, N'COM100', N'[I_K_1_1_On]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (394, N'Ток K 1-2 (ДИСС-32) Высокое выключено', N'I_K_1_2_Off', -300, 300, NULL, N'COM100', N'[I_K_1_2_On]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (395, N'Ток K 2-1 (ДИСС-32) Высокое выключено', N'I_K_2_1_Off', -300, 300, NULL, N'COM100', N'[I_K_2_1_On]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (396, N'Ток K 2-2 (ДИСС-32) Высокое выключено', N'I_K_2_2_Off', -300, 300, NULL, N'COM100', N'[I_K_2_2_On]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (397, N'Ток K 3-1 (ДИСС-32) Высокое выключено', N'I_K_3_1_Off', -300, 300, NULL, N'COM100', N'[I_K_3_1_On]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (398, N'Ток K 3-2 (ДИСС-32) Высокое выключено', N'I_K_3_2_Off', -300, 300, NULL, N'COM100', N'[I_K_3_2_On]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (399, N'Ток K 1-1 (ДИСС-32) Дельта вкл-выкл', N'I_K_1_1_Delta', 20, 160, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (400, N'Ток K 1-2 (ДИСС-32) Дельта вкл-выкл', N'I_K_1_2_Delta', 20, 160, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (401, N'Ток K 2-1 (ДИСС-32) Дельта вкл-выкл', N'I_K_2_1_Delta', 20, 160, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (402, N'Ток K 2-1 (ДИСС-32) Дельта вкл-выкл', N'I_K_2_2_Delta', 20, 160, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (403, N'Ток K 3-1 (ДИСС-32) Дельта вкл-выкл', N'I_K_3_1_Delta', 20, 160, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (404, N'Ток K 3-2 (ДИСС-32) Дельта вкл-выкл', N'I_K_3_2_Delta', 20, 160, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (405, N'Ток К 1 канал (ДИСС-32) разброс в паре', N'I_K_1_Abs', 0, 45, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (406, N'Ток К 2 канал (ДИСС-32) разброс в паре', N'I_K_2_Abs', 0, 45, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (407, N'Ток К 3 канал (ДИСС-32) разброс в паре', N'I_K_3_Abs', 0, 45, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (408, N'Ток магнетрона (ДИСС-15)', N'I_Mgn', 68, 74, NULL, N'COM100', N'[I_M]0,05', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (409, N'Контроль P СВЧ (ДИСС-15) ', N'SVCh_P', 0.5, 5, NULL, N'COM100', N'[P_SVCh]0,2', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (410, N'+27В (ДИСС-32) ', N'Plus_27B_32', 24.3, 29.7, N'B', N'COM100', N'[Plus_27B]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (411, N'-20B (ДИСС-32)', N'Minus_20B_32', -21, -19, N'B', N'COM100', N'[Minus_20B]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (412, N'-27В (ДИСС-32)', N'Minus_27B_32', -28.35, -25.65, N'B', N'COM100', N'[Minus_27B]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (413, N'-6В (ДИСС-32)', N'Minus_6B_32', -6.6, -6, N'B', N'COM100', N'[Minus_6B]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (414, N'+12В (ДИСС-32)', N'Plus_12B_32', 11.95, 13.05, N'B', N'COM100', N'[Plus_12B]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (415, N'-12В (ДИСС-32)', N'Minus_12B_32', -13.05, -11.95, N'B', N'COM100', N'[Minus_12B]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (416, N'ВН (ДИСС-15)', N'BH_Get', NULL, NULL, NULL, N'COM5', N'[get:20]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (417, N'Запуск ответов для Tablo (№ порта из [get])', N'Tablo_On', NULL, NULL, NULL, N'COM5', N'[port]0x0A;0x41;0xF0', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (418, N'Остановка ответов для Tablo (211 mS)', N'Tablo_Off', NULL, NULL, NULL, N'COM5', N'[port]0x0A;0x41;0x00', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (419, N'Выставить СКТ 0 градусов (ДИСС-32)', N'SKT_0_32', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0x00;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (420, N'Выставить СКТ 5 градусов (ДИСС-32)', N'SKT_5_32', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0x05;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (421, N'Выставить СКТ 11 градусов (ДИСС-32)', N'SKT_11_32', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0x0B;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (422, N'Выставить СКТ 12 градусов (ДИСС-32)', N'SKT_12_32', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0x0C;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (423, N'Выставить СКТ 15 градусов (ДИСС-32)', N'SKT_15_32', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0x0F;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (424, N'Выставить СКТ 45 градусов (ДИСС-32)', N'SKT_45_32', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0xE1;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (425, N'Выставить СКТ 135 градусов (ДИСС-32)', N'SKT_135_32', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x01;0x3B;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (426, N'Выставить СКТ 345 градусов (ДИСС-32)', N'SKT_345_32', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x01;0x59;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (427, N'Выставить СКТ 348 градусов (ДИСС-32)', N'SKT_348_32', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x01;0x5C;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (428, N'Выставить СКТ 349 градусов (ДИСС-32)', N'SKT_349_32', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x01;0x5D;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (429, N'Выставить СКТ 355 градусов (ДИСС-32)', N'SKT_355_32', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x01;0x63;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (431, N'Выключаем плату СКТ, т.е. совсем', N'SKT_Off', NULL, NULL, NULL, N'COM3', N'[port]0x0A;0x87;0x00', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (438, N'Контрольная задача 1 вкл (ДИСС-15)-Атт.', N'Task_1_15', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x01;0xC8;0xA5', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (439, N'Контрольная задача 2 вкл (ДИСС-15)-Атт.', N'Task_2_15', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x02;0xFF;0xC0', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (440, N'Контрольная задача 3 вкл (ДИСС-15)-Атт.', N'Task_3_15', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x03;0xFF;0xC0', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (441, N'Контрольная задача 4 вкл (ДИСС-15)-Атт.', N'Task_4_15', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x04;0xFF;0xC0', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (442, N'Контрольная задача 5 вкл (ДИСС-15)-Атт.', N'Task_5_15', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x05;0xFF;0xC0', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (443, N'Контрольная задача 6 вкл (ДИСС-15)-Атт.', N'Task_6_15', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x06;0xFF;0xC0', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (444, N'Контрольная задача 1 вкл (ДИСС-32)-Атт.', N'Task_1_32', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x07;0xFF;0xC0', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (445, N'Контрольная задача 2 вкл (ДИСС-32)-Атт.', N'Task_2_32', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x08;0xFF;0xC0', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (446, N'Выключить любую контрольную задачу', N'Task_All_Off', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x00;0x00;0x00', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (447, N'"Аттестация СКТ" - 30 градусов', N'SKT_30', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0x1E;0xFF', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (448, N'"Аттестация СКТ" - 60 градусов', N'SKT_60', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0x3C;0xFF', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (449, N'"Аттестация СКТ" - 90 градусов', N'SKT_90', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0x5A;0xFF', 100)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (450, N'"Аттестация СКТ" - 120 градусов', N'SKT_120', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0x78;0xFF', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (451, N'"Аттестация СКТ" - 150 градусов', N'SKT_150', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0x96;0xFF', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (452, N'"Аттестация СКТ" - 180 градусов', N'SKT_180', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0xB4;0xFF', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (453, N'"Аттестация СКТ" - 210 градусов', N'SKT_210', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0xD2;0xFF', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (454, N'"Аттестация СКТ" - 240 градусов', N'SKT_240', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0xF0;0xFF', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (455, N'"Аттестация СКТ" - 270 градусов', N'SKT_270', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x01;0x0E;0xFF', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (456, N'"Аттестация СКТ" - 300 градусов', N'SKT_300', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x01;0x2C;0xFF', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (457, N'"Аттестация СКТ" - 330 градусов', N'SKT_330', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x01;0x4A;0xFF', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (467, N'Ток по 27В (ДИСС-15)', N'I_27V_15', 0, 16.5, N'A', N'COM8', N'[27V_U]MC?', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (468, N'Задаем сигналы(их ID)для записи в БД (15)', N'Limit_15', NULL, NULL, NULL, N'COM100', N'27V_I;Set_115_On', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (469, N'Задаем сигналы(их ID)для записи в БД (32)', N'Limit_32', NULL, NULL, NULL, N'COM100', N'Set_115_Norma', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (470, N'Ввод частоты "Магнетрон и ТКМ" (для ID)', N'Manual_TKM_Freq', 13327, 13335, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (471, N'Ввод мощности "Магнетрон и ТКМ" (для ID)', N'Manual_TKM_Power', 2, 10, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (472, N'БРЕД машинистов', N'proverka_vch', NULL, NULL, NULL, N'COM8', N'[portAll]Higt_On_15;Task_Off_15', 200)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (473, N'ВПЕРЕД КЗ-1 (ДИСС-15)', N'W_Forward_15', 14.45, 19.45, NULL, N'COM100', N'[Wx_PKV]9,9377;-0,062', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (474, N'НАЗАД КЗ-2 (ДИСС-15)', N'W_Back_15', 14.45, 19.45, NULL, N'COM100', N'[Wx_PKV]-9,9377;0,062', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (475, N'ВВЕРХ КЗ-1 (ДИСС-15)', N'W_Up_15', 4.15, 5.15, NULL, N'COM100', N'[Wy_PKV]-2,053;-0,145', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (476, N'ВНИЗ КЗ-3 (ДИСС-15)', N'W_Down_15', 4.15, 5.15, NULL, N'COM100', N'[Wy_PKV]2,053;0,145', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (477, N'ВПРАВО КЗ-2 (ДИСС-15)', N'W_Right_15', 14.95, 18.95, NULL, N'COM100', N'[Wz_PKV]4,667;0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (478, N'ПС-215 для КЗ-4 (ДИСС-15) и КЗ-5', N'Spd_Task4_15', 133, 139, NULL, N'COM100', N'[W_7V]79,5;0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (479, N'ПС-215 для КЗ-6 (ДИСС-15)', N'Spd_Task6_15', 212, 218, NULL, N'COM100', N'[W_7V]79,5;0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (480, N'ВПЕРЕД ручками КЗ-1 (ДИСС-15)', N'Mnl_Forward_15', 14.5, 19.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (481, N'НАЗАД ручками КЗ-2 (ДИСС-15)', N'Mnl_Back_15', 14.5, 19.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (482, N'ВВЕРХ ручками КЗ-1 (ДИСС-15)', N'Mnl_Up_15', 4, 5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (483, N'ВНИЗ ручками КЗ-3 (ДИСС-15)', N'Mnl_Down_15', 4, 5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (484, N'ВПРАВО ручками КЗ-2 (ДИСС-15)', N'Mnl_Right_15', 14.5, 19.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (485, N'ПС-215 ручками для КЗ-4 (ДИСС-15)', N'Mnl_Spd_Task4_15', 211.5, 218.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (486, N'ПС-215 ручками для КЗ-6 (ДИСС-15)', N'Mnl_Spd_Task6_15', 211.5, 218.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (487, N'УС 0 КЗ-4 (ДИСС-15)', N'Alfa_0_15', -0.45, 0.45, NULL, N'COM100', N'[Alfa]9,55;0;9,55;0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (488, N'УС +18 КЗ-5 (ДИСС-15)', N'Alfa_18Plus', 17.4, 19.1, NULL, N'COM100', N'[Alfa]9,55;0;9,55;0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (489, N'УС -18 КЗ-6 (ДИСС-15)', N'Alfa_18Minus', -19.1, -17.4, NULL, N'COM100', N'[Alfa]9,55;0;9,55;0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (490, N'УС 0 ручками КЗ-4 (ДИСС-15)', N'Mnl_Alfa_0_15', -1, 1, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (491, N'УС +18 ручками КЗ-5 (ДИСС-15)', N'Mnl_Alfa_18Plus', 17, 20, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (492, N'УС -18 ручками КЗ-6 (ДИСС-15)', N'Mnl_Alfa_18Minus', -20, -17, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (496, N'Загорание табло "ПАМЯТЬ" (ДИСС-15)', N'Memory_Time_15', 0.5, 1.5, NULL, N'COM100', N'0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (497, N'"ПАМЯТЬ" путевая скорость', N'Memory_W_15', -4.5, 4.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (498, N'"ПАМЯТЬ" угол сноса', N'Memory_Alfa_15', -3.5, 3.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (499, N'Wxr нулевая составляющая скорости (ДИСС-15)', N'Wxr_Zero_15', -70, 70, NULL, N'COM100', N'[NKV_X_Zero]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (500, N'Wzr нулевая составляющая скорости (ДИСС-15)', N'Wzr_Zero_15', -70, 70, NULL, N'COM100', N'[NKV_Z_Zero]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (501, N'Wyr нулевая составляющая скорости (ДИСС-15)', N'Wyr_Zero_15', -220, 220, NULL, N'COM100', N'[PKV_Z_Zero]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (502, N'"БПК" ВПЕРЕД (ДИСС-15)', N'BPK_15_Forward', 14.5, 19.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (503, N'"БПК" ВБОК (ДИСС-15)', N'BPK_15_Side', -2.5, 2.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (504, N'"БПК" ВВЕРХ (ДИСС-15)', N'BPK_15_Up', 4, 5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (505, N'"БПК" НАЗАД (ДИСС-15)', N'BPK_15_Back', 14.5, 19.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (506, N'"БПК" ВПРАВО (ДИСС-15)', N'BPK_15_Right', 14.5, 19.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (507, N'"БПК" ВВЕРХ(положение 2) (ДИСС-15)', N'BPK_15_Up_2', 2.5, 3.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (508, N'"БПК" Путевая скорость (ДИСС-15)', N'BPK_15_W', 132.5, 139.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (509, N'"БПК" Угол сноса (ДИСС-15)', N'BPK_15_Alfa', -1, 1, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (510, N'"БПК" W>=50 (да/нет) (ДИСС-15)', N'BPK_15_Signal', NULL, NULL, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (511, N'"БПК" время вкл.табло П (ДИСС-15)', N'BPK_15_Time', 0.5, 2.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (512, N'"БПК" Дельта W (ДИСС-15)', N'BPK_15_Delta_W', -4.5, 4.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (513, N'"БПК" Дельта Альфа (ДИСС-15)', N'BPK_15_Delta_Alfa', -3.5, 3.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (514, N'"БПК" РАБОТА (да/нет) (ДИСС-15)', N'BPK_15_Work', NULL, NULL, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (515, N'"БПК" Положение 4 (да/нет) (ДИСС-15)', N'BPK_15_Memory', NULL, NULL, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (516, N'Блок8 путь ВПЕРЕД (курс 45)', N'Block8_Forward', 7, 9, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (517, N'Блок8 уклонение ВЛЕВО (курс 45)', N'Block8_Left', 7, 9, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (518, N'Блок8 путь НАЗАД (курс 135)', N'Block8_Back', 7, 9, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (519, N'Блок8 уклонение ВПРАВО (курс 135)', N'Block8_Right', 7, 9, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (520, N'Wxr Тангаж 15 (ДИСС-15)', N'Tangazh15_X_15', -3.5, -1.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (521, N'Wyr Тангаж 15 (ДИСС-15)', N'Tangazh15_Y_15', 0.8, 1.7, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (522, N'Wzr Крен 15 (ДИСС-15)', N'Kren15_Z_15', 1.5, 3.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (523, N'Wyr Крен 15 (ДИСС-15)', N'Kren15_Y_15', -1.7, -0.8, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (524, N'Wxr Тангаж 345 (ДИСС-15)', N'Tangazh345_X_15', 1.5, 3.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (525, N'Wyr Тангаж 345 (ДИСС-15)', N'Tangazh345_Y_15', -1.7, -0.8, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (526, N'Wzr Крен 345 (ДИСС-15)', N'Kren345_Z_15', -3.5, -1.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (527, N'Wyr Крен 345 (ДИСС-15)', N'Kren345_Y_15', 0.8, 1.7, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (528, N'"Режим К" путевая скорость (ДИСС-15)', N'W_K_15', 302.5, 309.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (529, N'"Режим К" угол сноса (ДИСС-15)', N'Alfa_K_15', 14, 16, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (530, N'"Режим Море" увеличение путевой скорости (15)', N'W_Sea_15', 1.7, 2.3, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (532, N'Погрешность пути ВПЕРЕД Блока-8 (15)', N'Block8_15_Forward', 49.3, 52.3, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (533, N'Погрешность уклонения ВЛЕВО Блока-8 (15)', N'Block8_15_Left', 15.6, 18.2, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (534, N'Изделие ДИСС-15', N'Izdelie', NULL, NULL, NULL, N'COM5', N'[get:21]0x0A;0x40', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (535, N'Контрольная задача 1 вкл (ДИСС-15) - Б.', N'Task_W_1_15', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x01;0x6B;0x65', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (536, N'Контрольная задача 2 вкл (ДИСС-15) - Б.', N'Task_W_2_15', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x02;0x6B;0x65', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (537, N'Контрольная задача 3 вкл (ДИСС-15) - Б.', N'Task_W_3_15', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x03;0x6B;0x65', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (538, N'Контрольная задача 4 вкл (ДИСС-15) - Б.', N'Task_W_4_15', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x04;0x6B;0x65', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (539, N'Контрольная задача 5 вкл (ДИСС-15) - Б.', N'Task_W_5_15', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x05;0x6B;0x65', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (540, N'Контрольная задача 6 вкл (ДИСС-15) - Б.', N'Task_W_6_15', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x06;0x6B;0x65', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (541, N'Контрольная задача 1 вкл (ДИСС-32) - Б.', N'Task_W_1_32', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x09;0x48;0x80', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (542, N'Контрольная задача 2 вкл (ДИСС-32) - Б.', N'Task_W_2_32', NULL, NULL, NULL, N'COM6', N'[task]0x0A;0x60;0x09;0x48;0x80', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (543, N'Скорость отработки Wy (ДИСС-15)', N'Spd_Wy_15', 0.25, 100, NULL, N'COM100', N'0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (544, N'Скорость отработки Wx (ДИСС-15)', N'Spd_Wx_15', 2.5, 100, NULL, N'COM100', N'0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (545, N'Скорость отработки Wz (ДИСС-15)', N'Spd_Wz_15', 2.5, 100, NULL, N'COM100', N'0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (546, N'Скорость отработки угла сноса (ДИСС-15)', N'Spd_YC_15', 1.1, 100, NULL, N'COM100', N'0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (547, N'Скорость отработки ПС (ДИСС-15)', N'Spd_PC_15', 2.5, 100, NULL, N'COM100', N'0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (548, N'Время включения ВН (ДИСС-15)', N'BH_Time_15', 30, 90, NULL, N'COM100', N'Команды не определены', NULL)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (551, N'Частота СВЧ колебаний (ДИСС-32)', N'Mnl_F_32', 13317.5, 13332.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (552, N'Погрешность Wxr НКВ для КЗ-1 (ДИСС-32)', N'NKV_X_T1', 410, 530, NULL, N'COM100', N'[Wx_NKV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (553, N'Погрешность Wxr НКВ для КЗ-2 (ДИСС-32)', N'NKV_X_T2', -530, -410, NULL, N'COM100', N'[Wx_NKV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (554, N'Погрешность Wzr НКВ для КЗ-1 (ДИСС-32)', N'NKV_Z_T1', 1.37, 1.77, NULL, N'COM100', N'[Wz_NKV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (555, N'Погрешность Wzr НКВ для КЗ-2 (ДИСС-32)', N'NKV_Z_T2', -1.77, -1.37, NULL, N'COM100', N'[Wz_NKV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (556, N'Погрешность Wyr НКВ для КЗ-1 (ДИСС-32)', N'NKV_Y_T1', -3.7, -2.5, NULL, N'COM100', N'[Wy_NKV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (557, N'Погрешность Wyr НКВ для КЗ-2 (ДИСС-32)', N'NKV_Y_T2', 2.5, 3.7, NULL, N'COM100', N'[Wy_NKV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (558, N'Погрешность Wxr ПКВ для КЗ-1 (ДИСС-32)', N'PKV_X_T1', -5.31, -4.11, NULL, N'COM100', N'[Wx_PKV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (559, N'Погрешность Wxr ПКВ для КЗ-2 (ДИСС-32)', N'PKV_X_T2', 4.11, 5.31, NULL, N'COM100', N'[Wx_PKV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (560, N'Погрешность Wzr ПКВ для КЗ-1 (ДИСС-32)', N'PKV_Z_T1', -5.31, -4.11, NULL, N'COM100', N'[Wz_PKV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (561, N'Погрешность Wzr ПКВ для КЗ-2 (ДИСС-32)', N'PKV_Z_T2', 4.11, 5.31, NULL, N'COM100', N'[Wz_PKV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (562, N'Погрешность Wyr ПКВ для КЗ-1 (ДИСС-32)', N'PKV_Y_T1', -3.7, -2.5, NULL, N'COM100', N'[Wy_PKV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (563, N'Погрешность Wyr ПКВ для КЗ-2 (ДИСС-32)', N'PKV_Y_T2', 2.5, 3.7, NULL, N'COM100', N'[Wy_PKV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (564, N'Погрешность Wxr СПСВ для КЗ-1 (ДИСС-32)', N'SPSV_X_T1', -590, -450, NULL, N'COM100', N'[Wx_SPSV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (565, N'Погрешность Wxr СПСВ для КЗ-2 (ДИСС-32)', N'SPSV_X_T2', 450, 590, NULL, N'COM100', N'[Wx_SPSV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (566, N'Погрешность Wzr СПСВ для КЗ-1 (ДИСС-32)', N'SPSV_Z_T1', -590, -450, NULL, N'COM100', N'[Wz_SPSV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (567, N'Погрешность Wzr СПСВ для КЗ-2 (ДИСС-32)', N'SPSV_Z_T2', 450, 590, NULL, N'COM100', N'[Wz_SPSV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (568, N'Погрешность Wyr СПСВ для КЗ-1 (ДИСС-32)', N'SPSV_Y_T1', 2.05, 3.25, NULL, N'COM100', N'[Wy_SPSV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (569, N'Погрешность Wyr СПСВ для КЗ-2 (ДИСС-32)', N'SPSV_Y_T2', -3.25, -2.05, NULL, N'COM100', N'[Wy_SPSV]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (570, N'Погрешность Wxr Тестер для КЗ-1 (ДИСС-32)', N'Test_X_T1', 600, 1000, NULL, N'COM100', N'[Wx_TEST]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (571, N'Погрешность Wxr Тестер для КЗ-2 (ДИСС-32)', N'Test_X_T2', 90, 490, NULL, N'COM100', N'[Wx_TEST]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (572, N'Погрешность Wzr Тестер для КЗ-1 (ДИСС-32)', N'Test_Z_T1', 3.27, 3.67, NULL, N'COM100', N'[Wz_TEST]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (573, N'Погрешность Wzr Тестер для КЗ-2 (ДИСС-32)', N'Test_Z_T2', 2.33, 2.73, NULL, N'COM100', N'[Wz_TEST]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (574, N'Погрешность Wyr Тестер для КЗ-1 (ДИСС-32)', N'Test_Y_T1', 1.67, 2.47, NULL, N'COM100', N'[Wy_TEST]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (575, N'Погрешность Wyr Тестер для КЗ-2 (ДИСС-32)', N'Test_Y_T2', 3.53, 4.33, NULL, N'COM100', N'[Wy_TEST]', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (576, N'Только для отображения в КЗ-2 (ДИСС-32)', N'Task2_X_32', 14.5, 19.5, NULL, N'COM100', N'[Wx_PKV]0,277780092611883', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (577, N'Только для отображения в КЗ-2 (ДИСС-32)', N'Task2_Z_32', 14.5, 19.5, NULL, N'COM100', N'[Wz_PKV]0,277780092611883', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (578, N'Только для отображения в КЗ-2 (ДИСС-32)', N'Task2_Y_32', 2.6, 3.6, NULL, N'COM100', N'[Wy_PKV]1,00000830006889', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (579, N'Погрешность индикации Назад-17 (ДИСС-32)', N'Mnl_Back_32', -2.5, 2.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (580, N'Погрешность индикации Вправо-17 (ДИСС-32)', N'Mnl_Right_32', -2.5, 2.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (581, N'Погрешность индикации Вверх-3,1 (ДИСС-32)', N'Mnl_Up_32', -0.5, 0.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (582, N'Погрешность индикации Вперед-17 (ДИСС-32)', N'Mnl_Forward_32', -2.5, 2.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (583, N'Погрешность индикации Влево-17 (ДИСС-32)', N'Mnl_Left_32', -2.5, 2.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (584, N'Погрешность индикации Вниз-3,1 (ДИСС-32)', N'Mnl_Down_32', -0.5, 0.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (585, N'Тангаж +12 X (ДИСС-32)', N'Pitch_Plus12_X', NULL, NULL, NULL, N'COM100', N'1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (586, N'Тангаж +12 Z (ДИСС-32)', N'Pitch_Plus12_Z', NULL, NULL, NULL, N'COM100', N'0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (587, N'Тангаж +12 Y (ДИСС-32)', N'Pitch_Plus12_Y', NULL, NULL, NULL, N'COM100', N'-1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (588, N'Тангаж -12 X (ДИСС-32)', N'Pitch_Minus12_X', NULL, NULL, NULL, N'COM100', N'-1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (589, N'Тангаж -12 Z (ДИСС-32)', N'Pitch_Minus12_Z', NULL, NULL, NULL, N'COM100', N'0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (590, N'Тангаж -12 Y (ДИСС-32)', N'Pitch_Minus12_Y', NULL, NULL, NULL, N'COM100', N'1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (591, N'Крен +12 X (ДИСС-32)', N'Heel_Plus12_X', NULL, NULL, NULL, N'COM100', N'0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (592, N'Крен +12 Z (ДИСС-32)', N'Heel_Plus12_Z', NULL, NULL, NULL, N'COM100', N'1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (593, N'Крен +12 Y (ДИСС-32)', N'Heel_Plus12_Y', NULL, NULL, NULL, N'COM100', N'-1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (594, N'Крен -12 X (ДИСС-32)', N'Heel_Minus12_X', NULL, NULL, NULL, N'COM100', N'0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (595, N'Крен -12 Z (ДИСС-32)', N'Heel_Minus12_Z', NULL, NULL, NULL, N'COM100', N'-1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (596, N'Крен -12 Y (ДИСС-32)', N'Heel_Minus12_Y', NULL, NULL, NULL, N'COM100', N'1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (600, N'Комментарий отсутствует', N'UNCh_On_15_111', NULL, NULL, NULL, N'COM4', N'[port]0x0A;0x33;0x86', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (601, N'Режим "Память" ПС (КЗ-3 ДИСС-32)', N'ModeMemory_PC', -3, 3, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (602, N'Режим "Память" УС (КЗ-3 ДИСС-32)', N'ModeMemory_YC', -3, 3, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (603, N'Режим "М-С" (КЗ-4 ДИСС-32)', N'Mode_M_C', 1.8, 2.2, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (604, N'Режим "М-Б" (КЗ-4 ДИСС-32)', N'Mode_M_B', 0.8, 1.2, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (605, N'ПС1(-127) КЗ-3 (ДИСС-32)', N'Spd_Task3_32', -2, 2, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (606, N'ПС2(-258) КЗ-4 (ДИСС-32)', N'Spd_Task4_32', -3, 3, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (607, N'Ручками ПС1(-127) КЗ-3 (ДИСС-32)', N'Mnl_Spd_Task3_32', -2.5, 2.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (608, N'Ручками ПС2(-258) КЗ-4 (ДИСС-32)', N'Mnl_Spd_Task4_32', -3.5, 3.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (609, N'УС 0 КЗ-3 (ДИСС-32)', N'Alfa_0_32', -0.5, 0.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (610, N'УС 9,5 КЗ-4 (ДИСС-32)', N'Alfa_9_32', -0.5, 0.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (611, N'Ручками УС 0 КЗ-3 (ДИСС-32)', N'Mnl_Alfa_0_32', -1, 1, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (612, N'Ручками УС 9,5 КЗ-4 (ДИСС-32)', N'Mnl_Alfa_9_32', -1, 1, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (613, N'Только для отображения в КЗ-1 (ДИСС-32)', N'Task1_X_32', 14.5, 19.5, NULL, N'COM100', N'[Wx_PKV]-0,277780092611883', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (614, N'Только для отображения в КЗ-1 (ДИСС-32)', N'Task1_Z_32', 14.5, 19.5, NULL, N'COM100', N'[Wz_PKV]-0,277780092611883', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (615, N'Только для отображения в КЗ-1 (ДИСС-32)', N'Task1_Y_32', 2.6, 3.6, NULL, N'COM100', N'[Wy_PKV]-1,00000830006889', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (616, N'ПС 127 КЗ-3 (ДИСС-32)', N'Task3_PC_32', 125, 129, NULL, N'COM100', N'[W_7V]-0,0251', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (617, N'УС 0 КЗ-3 (ДИСС-32)', N'Task3_Y0_32', -0.7, 0.7, NULL, N'COM100', N'[Alfa]0,4545454545', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (618, N'ПС 258 КЗ-4 (ДИСС-32)', N'Task4_PC_32', 255, 261, NULL, N'COM100', N'[W_7V]-0,0251', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (619, N'УС 9,5 КЗ-3 (ДИСС-32)', N'Task4_Y9_32', 8.8, 10.2, NULL, N'COM100', N'[Alfa]-0,4545454545', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (620, N'Измеряем U на новом БП 27В', N'27V_U', NULL, NULL, NULL, N'COM8', N'[27V_U]MV?', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (621, N'Измеряем I на новом БП 27В', N'27V_I', 0, 16.5, NULL, N'COM8', N'[27V_U]MC?', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (622, N'Включаем новый БП 27В', N'Incl_27V', NULL, NULL, NULL, N'COM8', N'[27V_Y]OUT 1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (623, N'Выставляем на БП 27V (ДИСС-32)', N'27V_On', NULL, NULL, NULL, N'COM8', N'[27V_Y]PV 27.5', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (624, N'Выключить БП 27V (ДИСС-32)', N'27V_Off', NULL, NULL, NULL, N'COM8', N'[27V_Y]OUT 0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (625, N'27V+10% (29,7B) (ДИСС-32)', N'27V_Plus', NULL, NULL, NULL, N'COM8', N'[27V_Y]PV 30.2', 5)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (626, N'27V-10% (24,3B) (ДИСС-32)', N'27V_Minus', NULL, NULL, NULL, N'COM8', N'[27V_Y]PV 24.8', 5)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (629, N'27,5+5% (28,875B) (ДИСС-15)', N'27_5_Plus', NULL, NULL, NULL, N'COM8', N'[27V_Y]PV 29.875', 5)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (630, N'27,5-5% (26,125B) (ДИСС-15)', N'27_5_Minus', NULL, NULL, NULL, N'COM8', N'[27V_Y]PV 28.125', 5)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (632, N'Выставляем на БП 27,5V (ДИСС-15)', N'27_5_On', NULL, NULL, NULL, N'COM8', N'[27V_Y]PV 28.5', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (633, N'Выключить БП 27,5V (ДИСС-15)', N'27_5_Off', NULL, NULL, NULL, N'COM8', N'[27V_Y]OUT 0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (634, N'БПК (ДИСС-32) Вперед-17', N'BPK_Forward_32', -2.5, 2.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (635, N'БПК (ДИСС-32) Влево-17', N'BPK_Left_32', -2.5, 2.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (636, N'БПК (ДИСС-32) Вниз-3', N'BPK_Down_32', -0.5, 0.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (637, N'БПК (ДИСС-32) Назад-17', N'BPK_Back_32', -2.5, 2.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (638, N'БПК (ДИСС-32) Вправо-17', N'BPK_Right_32', -2.5, 2.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (639, N'БПК (ДИСС-32) Вверх-3', N'BPK_Up_32', -0.5, 0.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (640, N'БПК (ДИСС-32) ПС1-127', N'BPK_PC1_32', -2.5, 2.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (641, N'БПК (ДИСС-32) УС0', N'BPK_YC0_32', -1, 1, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (642, N'БПК (ДИСС-32) ПС2-258', N'BPK_PC2_32', -3.5, 3.5, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (643, N'БПК (ДИСС-32) УС 9,5', N'BPK_YC9_32', -1, 1, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (646, N'Коэф.для расчёта Р (ДИСС-15)', N'Koef_P', NULL, NULL, NULL, N'COM100', N'28,59', NULL)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (647, N'Шум УНЧ 1-1 (ДИСС-32) - раб.', N'UNCh_1_1_Noise32', 30, 100, NULL, N'COM100', N'[UNCh_1_1_Noise]1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (648, N'Шум УНЧ 1-2 (ДИСС-32) - раб.', N'UNCh_1_2_Noise32', 30, 100, NULL, N'COM100', N'[UNCh_1_2_Noise]1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (649, N'Шум УНЧ 2-1 (ДИСС-32) - раб.', N'UNCh_2_1_Noise32', 30, 100, NULL, N'COM100', N'[UNCh_2_1_Noise]1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (650, N'Шум УНЧ 2-2 (ДИСС-32) - раб.', N'UNCh_2_2_Noise32', 30, 100, NULL, N'COM100', N'[UNCh_2_2_Noise]1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (651, N'Шум УНЧ 3-1 (ДИСС-32) - раб.', N'UNCh_3_1_Noise32', 30, 100, NULL, N'COM100', N'[UNCh_3_1_Noise]1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (652, N'Шум УНЧ 3-2 (ДИСС-32) - раб.', N'UNCh_3_2_Noise32', 30, 100, NULL, N'COM100', N'[UNCh_3_2_Noise]1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (654, N'Wx_NKV (ДИСС32) Обн.', N'NKV_X_Zero32', -10, 10, N'мВ', N'LTR114:4', N'[usb:11]0,4;1000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (655, N'Wz_NKV (ДИСС32) Обн.', N'NKV_Z_Zero32', -15, 15, N'мВ', N'LTR114:6', N'[usb:11]0,4;1000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (656, N'Выставить СКТ 0 градусов (ДИСС-15)', N'SKT_0', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0x00;0xFF', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (657, N'Выставить СКТ 15 градусов (ДИСС-15)', N'SKT_15', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0x0F;0xFF', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (658, N'Выставить СКТ 45 градусов (ДИСС-15)', N'SKT_45', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0xE1;0xFF', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (659, N'Выставить СКТ 135 градусов (ДИСС-15)', N'SKT_135', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x01;0x3B;0xFF', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (660, N'Выставить СКТ 345 градусов (ДИСС-15)', N'SKT_345', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x01;0x59;0xFF', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (662, N'ДИСС32 Отсутствие сигналов П1,П2,П,ПОИСК', N'Point23_32_1', NULL, NULL, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (663, N'ДИСС32 Отсут. запомин. при +/- 5град', N'Point23_32_2', NULL, NULL, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (664, N'ДИСС32 Отсут. запомин. при +/- 15град(М-Б)', N'Point23_32_3', NULL, NULL, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (665, N'ДИСС32 Отсут. запом. при +/- 15град(ВПЕРЕД)', N'Point23_32_4', NULL, NULL, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (666, N'ДИСС32.Блок8(п.24) ВПЕРЕД', N'Point24_X', 62, 65, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (667, N'ДИСС32.Блок8(п.24) ВЛЕВО', N'Point24_Z', 9.5, 11.7, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (668, N'ДИСС32.Блок8(п.25) ВПЕРЕД', N'Point25_X', 20.4, 30, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (669, N'ДИСС32.Блок8(п.25) ВЛЕВО', N'Point25_Z', -0.6, 0.6, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (670, N'ДИСС32.Блок8(п.25) ВПЕРЕД КУРС +10', N'Point25_X_Course', NULL, NULL, NULL, N'COM100', N'-1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (671, N'ДИСС32.Блок8(п.25) ВЛЕВО КУРС +10', N'Point25_Z_Course', NULL, NULL, NULL, N'COM100', N'1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (674, N'Включить обнуление Zорт(ДИСС32-90А)', N'Zero_Zopt_On', NULL, NULL, NULL, N'PCI', N'[pci-set:1]1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (675, N'Выключить обнуление Zорт(ДИСС32-90А)', N'Zero_Zopt_Off', NULL, NULL, NULL, N'PCI', N'[pci-set:0]1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (676, N'Курс +10 (п.25 ДИСС32)', N'SKT_10', NULL, NULL, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (689, N'W НКВ (ДИСС32-90А) обнуление W', N'NKV_W_Zero', -10, 10, N'мВ', N'LTR114:13', N'[usb:11]10;1000', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (690, N'Zорт ПКВ (ДИСС 32-90А)_1', N'Z_Zero_32', -0.1, 0.1, N'В', N'LTR11:16', N'[usb:13]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (691, N'Zорт ПКВ (ДИСС 32-90А)_2', N'Z_Task_32', -0.44, 0.44, NULL, N'LTR11:16', N'[usb:13]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (692, N'Z ток ПНП (ДИСС 32-90А)', N'Z_PNP_32', -59, 59, N'мкА', N'LTR11:16', N'[usb:12]10;1,26', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (693, N'ПОИСК (ДИСС-32-90А)', N'Search', NULL, NULL, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (694, N'Табло П,Выкл. (ДИСС-32-90А)', N'Tablo_P', NULL, NULL, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (695, N'КОНТР.ДИСС (ДИСС-32-90А)', N'Control_Diss', NULL, NULL, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (696, N'Сигнал Счисл.ДИСС (ДИСС-32-90А)', N'Calculus', NULL, NULL, NULL, N'PCI', N'[pci-Get]5', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (697, N'Сигнал Откл.коорд (ДИСС-32-90А)', N'Disconnect', NULL, NULL, NULL, N'PCI', N'[pci-Get]3', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (698, N'Сигнал НАВИГАЦИЯ (ДИСС-32-90А)', N'Navigat', NULL, NULL, NULL, N'PCI', N'[pci-Get]4', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (699, N'Сигнал Откл.Бл.8 (ДИСС-32-90А)', N'Block8_32', NULL, NULL, NULL, N'PCI', N'[pci-Get]1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (700, N'Сигнал Счисл.ДПСМ(ДИСС-32-90А)', N'DPSM_32', NULL, NULL, NULL, N'PCI', N'[pci-Get]6', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (701, N'Сигнал Исправн.БВК(ДИСС-32-90А)', N'BVK_Good', NULL, NULL, NULL, N'PCI', N'[pci-Get]2', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (702, N'Отказ БВК Включить(ДИСС-32-90А)', N'BVK_On_32', NULL, NULL, NULL, N'PCI', N'[pci-set:1]2', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (703, N'Отказ БВК Отключить(ДИСС-32-90А)', N'BVK_Off_32', NULL, NULL, NULL, N'PCI', N'[pci-set:0]2', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (704, N'Дельта 27В(ДИСС-15) ', N'Delta_27B_15', 0, 0.3, NULL, N'COM100', N'[Delta_27B]270', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (706, N'Инициализация нового БП 27В', N'Init_27V', NULL, NULL, NULL, N'COM8', N'[27V_Y]ADR 6', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (707, N'Выставляем 0В на новом БП 27В', N'27V_0B', NULL, NULL, NULL, N'COM8', N'[27V_Y]PV 0.0', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (715, N'(Aktor)БП115В - установить в 115В', N'Set_115_Norma', 109.25, 120.75, NULL, N'COM7', N'[aktor]A;115', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (718, N'(Aktor)БП115В- разрешение программ. вкл/выкл', N'Set_115_Permit', NULL, NULL, NULL, N'COM7', N'[aktor]A', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (719, N'(Aktor)БП115В - установить в 109,25В (32)', N'Set_115_32_Minus', NULL, NULL, NULL, N'COM7', N'[aktor]A;109,25', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (720, N'(Aktor)БП115В - установить в 120,75В (32)', N'Set_115_32_Plus', NULL, NULL, NULL, N'COM7', N'[aktor]A;120,75', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (721, N'(Aktor)БП115В - установить в 110,4В (15)', N'Set_115_15_Minus', NULL, NULL, NULL, N'COM7', N'[aktor]A;110,4', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (722, N'(Aktor)БП115В - установить в 119,6В (15)', N'Set_115_15_Plus', NULL, NULL, NULL, N'COM7', N'[aktor]A;119,6', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (723, N'(Aktor)БП115В - измеряем U', N'115V_Read_U', NULL, NULL, N'B', N'COM7', N'[aktorU]A;U', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (724, N'(Aktor)БП115В - измеряем I', N'115V_Read_I', NULL, NULL, N'A', N'COM7', N'[aktorU]A;I', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (725, N'(Aktor)Включение БП 115В', N'Set_115_On', 110.4, 119.6, NULL, N'COM7', N'[aktor]On', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (726, N'(Aktor)Выключение БП 115В', N'Set_115_Off', NULL, NULL, NULL, N'COM7', N'[aktor]Off', 20)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (728, N'(Aktor)БП115В - измеряем I через L_Card', N'115V_Read_I_Card', 0, 2, N'A', N'LTR11:15', N'[usb:14]10;1', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (729, N'(Aktor)БП115В - измеряем U через L_Card', N'115V_Read_U_Card', 110.4, 119.6, N'B', N'LTR11:16', N'[usb:14]10;50', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (730, N'Задать угол в SKT', N'SKT_Angle', NULL, NULL, NULL, N'COM3', N'[skt]0x0A;0x86;0x00;0x00;0x00;0xFF', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (731, N'Отказ (ДИСС15)', N'Otkaz_15', NULL, NULL, NULL, N'COM5', N'[get:13]0x0A;0x40', 20)
GO
SET IDENTITY_INSERT [dbo].[DescriptionParam] OFF
GO
SET IDENTITY_INSERT [dbo].[DeviceType] ON 

GO
INSERT [dbo].[DeviceType] ([DeviceTypeId], [DeviceName], [DeviceShortName]) VALUES (1, N'доплеровский измеритель ДИСС-15', N'ДИСС-15')
INSERT [dbo].[DeviceType] ([DeviceTypeId], [DeviceName], [DeviceShortName]) VALUES (2, N'доплеровский измеритель ДИСС-32-90', N'ДИСС-32-90')
INSERT [dbo].[DeviceType] ([DeviceTypeId], [DeviceName], [DeviceShortName]) VALUES (3, N'доплеровский измеритель ДИСС-013', N'ДИСС-013')
INSERT [dbo].[DeviceType] ([DeviceTypeId], [DeviceName], [DeviceShortName]) VALUES (4, N'доплеровский измеритель ДИСС-32-28', N'ДИСС-32-28')
INSERT [dbo].[DeviceType] ([DeviceTypeId], [DeviceName], [DeviceShortName]) VALUES (5, N'доплеровский измеритель ДИСС-32', N'ДИСС-32')
INSERT [dbo].[DeviceType] ([DeviceTypeId], [DeviceName], [DeviceShortName]) VALUES (6, N'доплеровский измеритель ДИСС-32-28М', N'ДИСС-32-28М')
GO
SET IDENTITY_INSERT [dbo].[DeviceType] OFF
GO
SET IDENTITY_INSERT [dbo].[DocumentLink] ON 

GO
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (1, 1, 1, N'РЭ, пункт РО 110.30.00с; РК Книга 2, часть 3 пункт 2.2.4', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (2, 1, 2, N'РЭ, пункт РО 110.30.00с; РК Книга 2, часть 3 пункт 2.2.4', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (3, 1, 3, N'РЭ, пункт РО 110.30.00с', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (4, 1, 4, N'РЭ, пункт РО 110.30.00d', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (5, 1, 5, N'РЭ, пункт РО 110.30.00d', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (6, 1, 6, N'РЭ, пункт РО 110.30.00d', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (7, 1, 7, N'РЭ, пункт РО 110.30.00d', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (8, 1, 8, N'РЭ, пункт РО 110.30.00d', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (9, 1, 9, N'РЭ, пункт РО 110.30.00d', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (10, 1, 10, N'РЭ, пункт РО 110.30.00f', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (11, 1, 11, N'РЭ, пункт РО 110.30.00g', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (12, 1, 12, N'РЭ пункт РО 110.30.00е', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (13, 1, 13, N'РЭ пункт РО 110.30.00i', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (14, 1, 14, N'РЭ пункт РО 110.30.00j', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (15, 1, 15, N'РЭ пункт РО 110.30.00l', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (16, 1, 16, N'РЭ пункт РО 110.30.00k', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (17, 1, 17, N'РК Книга 2, часть 3 пункт 2.2.21', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (18, 1, 18, N'РЭ пункт РО 110.30.00l', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (19, 1, 19, N'РК Книга 2, часть 3 пункт 2.2.23', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (20, 1, 20, N'РЭ пункт РО 110.30.00l', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (21, 1, 21, N'РЭ пункт РО 110.30.00o', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (22, 1, 22, N'РЭ пункт РО 110.30.00n', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (23, 1, 23, N'РЭ пункт РО 110.30.00k', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (24, 1, 24, N'РЭ пункт РО 110.30.00p', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (25, 1, 25, N'РК Книга 2, часть 3 пункт 2.2.25', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (26, 1, 26, N'', NULL)
INSERT [dbo].[DocumentLink] ([Id], [DeviceId], [TestReferenceId], [Text], [Comment]) VALUES (27, 1, 27, N'', NULL)
GO
SET IDENTITY_INSERT [dbo].[DocumentLink] OFF
GO
SET IDENTITY_INSERT [dbo].[Employee] ON 

GO
INSERT [dbo].[Employee] ([EmployeeId], [EmployeeNumber], [EmployeeFirstName], [EmployeeSecondName], [EmployeeLastName], [IsActive], [Image], [Note], [PostId]) VALUES (81, N'1', N'Субботин', N'Олег', N'Анатольевич', 0, NULL, N'', 1)
INSERT [dbo].[Employee] ([EmployeeId], [EmployeeNumber], [EmployeeFirstName], [EmployeeSecondName], [EmployeeLastName], [IsActive], [Image], [Note], [PostId]) VALUES (82, N'2', N'Иванов', N'Иван', N'Иванович', 1, NULL, N'', 1)
GO
SET IDENTITY_INSERT [dbo].[Employee] OFF
GO
SET IDENTITY_INSERT [dbo].[Lang] ON 

GO
INSERT [dbo].[Lang] ([Id], [ShortName], [FullName]) VALUES (0, N'RU', N'Текущий язык')
INSERT [dbo].[Lang] ([Id], [ShortName], [FullName]) VALUES (2, N'EN', N'Английский язык')
GO
SET IDENTITY_INSERT [dbo].[Lang] OFF
GO
SET IDENTITY_INSERT [dbo].[LTRSignal] ON 

GO
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (1, 12, 8, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (2, 12, 4, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (3, 12, 5, 10, 1, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (4, 12, 2, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (5, 12, 3, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (6, 12, 6, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (7, 12, 13, 10, 1, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (8, 12, 14, 10, 1, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (9, 11, 16, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (10, 12, 1, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (11, 12, 9, 10, 1, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (12, 12, 12, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (13, 13, 9, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (14, 13, 10, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (15, 13, 11, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (16, 13, 7, 10, 1, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (17, 13, 8, 10, 1, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (18, 14, 1, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (19, 14, 3, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (20, 14, 5, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (21, 14, 2, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (22, 14, 4, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (23, 14, 6, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (24, 14, 7, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (25, 14, 9, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (26, 14, 11, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (27, 13, 1, 10, 1, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (28, 13, 2, 10, 1, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (29, 13, 3, 10, 1, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (30, 13, 4, 10, 1, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (31, 13, 5, 10, 1, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (32, 13, 6, 10, 1, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (33, 15, 1, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (34, 15, 2, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (35, 15, 3, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (36, 15, 4, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (37, 16, 1, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (38, 16, 2, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (39, 14, 8, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (40, 14, 10, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (41, 14, 12, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (42, 11, 4, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (43, 11, 4, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (44, 11, 6, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (45, 11, 6, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (46, 11, 2, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (47, 11, 13, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (48, 11, 1, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (49, 11, 2, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (50, 11, 3, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (51, 11, 14, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (52, 14, 15, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (53, 14, 16, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (54, 12, 7, 10, 1, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (55, 13, 12, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (56, 12, 15, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (57, 13, 13, 10, 1, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (58, 13, 14, 10, 1, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (59, 12, 11, 10, 1, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (60, 11, 15, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (61, 11, 5, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (62, 13, 16, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (63, 12, 16, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (64, 13, 5, 10, 1, 1, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (65, 13, 6, 10, 1, 1, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (66, 13, 7, 10, 1, 1, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (67, 14, 1, 10, 0, 1, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (68, 14, 2, 10, 0, 1, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (69, 14, 3, 10, 0, 1, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (70, 14, 4, 10, 0, 1, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (71, 14, 5, 10, 0, 1, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (72, 14, 6, 10, 0, 1, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (73, 12, 10, 10, 0, 1, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (74, 11, 7, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (75, 11, 8, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (76, 11, 9, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (77, 11, 10, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (78, 11, 11, 10, 0, 0, NULL)
INSERT [dbo].[LTRSignal] ([SignalId], [Slot], [Channel], [Range], [Alone], [ACDC], [Note]) VALUES (79, 11, 12, 10, 0, 0, NULL)
GO
SET IDENTITY_INSERT [dbo].[LTRSignal] OFF
GO
INSERT [dbo].[MeasuringParam] ([TestId], [ParamId], [Result]) VALUES (-1, 127, CAST(10.06000 AS Decimal(18, 5)))
GO
SET IDENTITY_INSERT [dbo].[Module] ON 

GO
INSERT [dbo].[Module] ([ModuleId], [ModuleName], [PortId]) VALUES (1, N'Aktor115V', 8)
INSERT [dbo].[Module] ([ModuleId], [ModuleName], [PortId]) VALUES (2, N'Lambda40V', 2)
INSERT [dbo].[Module] ([ModuleId], [ModuleName], [PortId]) VALUES (4, N'KZ', 7)
INSERT [dbo].[Module] ([ModuleId], [ModuleName], [PortId]) VALUES (5, N'SKT', 4)
INSERT [dbo].[Module] ([ModuleId], [ModuleName], [PortId]) VALUES (6, N'DiscretInputs', 6)
INSERT [dbo].[Module] ([ModuleId], [ModuleName], [PortId]) VALUES (7, N'DiscretOutputs', 5)
INSERT [dbo].[Module] ([ModuleId], [ModuleName], [PortId]) VALUES (9, N'LTR', 1)
INSERT [dbo].[Module] ([ModuleId], [ModuleName], [PortId]) VALUES (10, N'Adv1760', 0)
GO
SET IDENTITY_INSERT [dbo].[Module] OFF
GO
SET IDENTITY_INSERT [dbo].[Parameters] ON 

GO
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (1, N'115V_Read_U', 1, N'A', 1, 109, 121, 2, N'Измерение 115V на Aktor', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (2, N'Plus_27V_32', 9, N'1', 5, 24.3, 29.7, 2, N'Измерение 27В (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (3, N'Plus_27V', 9, N'1', 5, 26.125, 28.875, 3, N'[АТТ][ДИСС15] Измерение 27В', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (4, N'Plus_12V_32', 9, N'2', 2, 11.95, 13.05, 2, N'+12В (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (5, N'Plus_12V', 9, N'2', 2, 11.97, 13.23, 2, N'[АТТ][ДИСС15] +12В', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (6, N'Minus_12V', 9, N'3', 2, -13.23, -11.97, 2, N'[АТТ][ДИСС15] -12В', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (7, N'Minus_12V_32', 9, N'3', 2, -13.05, -11.95, 2, N'-12В (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (8, N'Plus_6V', 9, N'4', 1, 6, 6.6, 3, N'[АТТ][ДИСС32] +6В', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (9, N'Minus_6V', 9, N'5', 1, -6.615, -5.67, 3, N'[АТТ][ДИСС15] -6В', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (10, N'Minus_6V_32', 9, N'5', 1, -6.6, -6, 3, N'-6В (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (11, N'Minus_20V', 9, N'6', 3, -21, -19, 2, N'[АТТ] -20В', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (12, N'Plus_15V', 9, N'7', 3, 14.25, 15.75, 2, N'[АТТ] +15В', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (13, N'Minus_15V', 9, N'8', 3, -15.75, -14.25, 2, N'[АТТ] -15В', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (14, N'Delta_15V', 9, N'9', 4000, -35, 35, 2, N'[АТТ] Дельта 15В', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (15, N'Plus_5V', 9, N'10', 1, 4.75, 5.25, 3, N'[АТТ] +5', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (16, N'Minus_27V', 9, N'11', 5, -28.35, -25.65, 2, N'[АТТ] -27В', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (17, N'Minus_27V_Op', 9, N'12', 5, -28.35, -25.65, 2, N'[АТТ] Опорное -27В', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (18, N'GLPD_1', 9, N'13', 1, 2.4, 3.5, 3, N'[АТТ] ГЛПД1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (19, N'GLPD_2', 9, N'14', 1, 2.4, 3.5, 3, N'[АТТ] ГЛПД2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (20, N'I_GD', 9, N'15', 1, -2.9, -1.7, 3, N'[АТТ] Ток ГД', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (21, N'MOD_1', 9, N'16', 1, 2.9, 4.3, 3, N'[АТТ] МОД1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (22, N'MOD_2', 9, N'17', 1, -4.9, -2.3, 3, N'[АТТ] МОД2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (23, N'I_K_1_1', 9, N'18', 1, -300, 300, 2, N'[АТТ][ДИСС15] Ток К1-1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (24, N'I_K_2_1', 9, N'19', 1, -300, 300, 2, N'[АТТ][ДИСС15] Ток К2-1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (25, N'I_K_3_1', 9, N'20', 1, -300, 300, 2, N'[АТТ][ДИСС15] Ток К3-1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (26, N'I_K_1_2', 9, N'21', 1, -300, 300, 2, N'[АТТ][ДИСС15] Ток K1-2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (27, N'I_K_2_2', 9, N'22', 1, -300, 300, 2, N'[АТТ][ДИСС15] Ток К2-2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (28, N'I_K_3_2', 9, N'23', 1, -300, 300, 2, N'[АТТ][ДИСС15] Ток К3-2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (29, N'I_K_1_1_32', 0, N'299', 1000, -300, 300, 1, N'[ДИСС32] Ток К1-1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (30, N'I_K_2_1_32', 0, N'300', 1000, -300, 300, 1, N'[ДИСС32] Ток К2-1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (31, N'I_K_3_1_32', 0, N'301', 1000, -300, 300, 1, N'[ДИСС32] Ток К3-1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (32, N'SD_1_1', 9, N'27', 1, 0.6, 1.8, 3, N'[АТТ] СД 1-1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (33, N'SD_1_2', 9, N'28', 1, 0.6, 1.8, 3, N'[АТТ] СД 1-2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (34, N'SD_2_1', 9, N'29', 1, 0.6, 1.8, 3, N'[АТТ] СД 2-1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (35, N'SD_2_2', 9, N'30', 1, 0.6, 1.8, 3, N'[АТТ] СД 2-2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (36, N'SD_3_1', 9, N'31', 1, 0.6, 1.8, 3, N'[АТТ] СД 3-1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (37, N'SD_3_2', 9, N'32', 1, 0.6, 1.8, 3, N'[АТТ] СД 3-2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (38, N'UNCh_1_1_Noise32', 0, N'76', 1000, 30, 100, 2, N'Шум УНЧ 1-1 (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (39, N'UNCh_1_2_Noise32', 0, N'77', 1000, 30, 100, 2, N'Шум УНЧ 1-2 (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (40, N'UNCh_2_1_Noise32', 0, N'78', 1000, 30, 100, 2, N'Шум УНЧ 2-1 (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (41, N'UNCh_2_2_Noise32', 0, N'79', 1000, 30, 100, 2, N'Шум УНЧ 2-2 (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (42, N'UNCh_3_1_Noise32', 0, N'80', 1000, 30, 100, 2, N'Шум УНЧ 3-1 (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (43, N'UNCh_3_2_Noise32', 0, N'81', 1000, 30, 100, 2, N'Шум УНЧ 3-2 (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (44, N'I_K_Delta', 9, N'', 1, 20, 160, 1, N'Ток К Делта (ДИСС32)', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (45, N'I_K_Abs', 9, N'', 1, 0, 45, 1, N'Ток К Abs (ДИСС32)', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (46, N'I_K_1_2_32', 0, N'302', 1000, -300, 300, 1, N'[ДИСС32] Ток К1-2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (47, N'I_K_2_2_32', 0, N'303', 1000, -300, 300, 1, N'[ДИСС32] Ток К2-2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (48, N'I_K_3_2_32', 0, N'304', 1000, -300, 300, 1, N'[ДИСС32] Ток К3-2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (49, N'NKV_X_Zero', 9, N'42', 1000, -40, 40, 1, N'Wx NKV (ДИСС15) Обн.', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (50, N'NKV_X_Zero32', 9, N'43', 1000, -10, 10, 1, N'Wx NKV (ДИСС32) Обн.', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (51, N'NKV_Z_Zero', 9, N'44', 1000, -80, 80, 1, N'Wz NKV (ДИСС15) Обн.', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (52, N'NKV_Z_Zero32', 9, N'45', 1000, -15, 15, 1, N'Wz NKV (ДИСС32) Обн.', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (53, N'NKV_Y_Zero', 9, N'46', 1000, -30, 30, 1, N'Wy NKV (ДИСС-32) Обн.', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (54, N'NKV_W_Zero', 9, N'47', 1000, -10, 10, 1, N'W НКВ (ДИСС32-90А) обнуление W', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (55, N'Wx_PKV', 9, N'48', 1, -10, 10, 1, N'[АТТ] Wx PKV', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (56, N'Wy_PKV', 9, N'49', 1, -10, 10, 1, N'[АТТ] Wy PKV', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (57, N'Wz_PKV', 9, N'50', 1, -10, 10, 1, N'[АТТ] Wz PKV', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (58, N'Task1_X_32', 9, N'48', -3.59996999999999, 14.5, 19.5, 1, N'Только для отображения в КЗ-1 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (59, N'Task1_Y_32', 9, N'49', -0.9999917, 2.6, 3.6, 1, N'Только для отображения в КЗ-1 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (60, N'Task1_Z_32', 9, N'50', -3.59996999999999, 14.5, 19.5, 1, N'Только для отображения в КЗ-1 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (61, N'W_7V', 9, N'47', 1, -300, 300, 1, N'[АТТ][ДИСС32] W (7V)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (62, N'Task3_PC_32', 9, N'47', -39.8406374501992, 125, 129, 1, N'ПС 127 КЗ-3 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (63, N'Alfa', 9, N'51', 1, -300, 300, 1, N'[АТТ] a(L)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (64, N'Task3_Y0_32', 9, N'51', 2.2, -0.7, 0.7, 1, N'УС 0 КЗ-3 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (65, N'Task4_PC_32', 9, N'47', -39.8406374501992, 255, 261, 1, N'ПС 258 КЗ-4 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (66, N'Task4_Y9_32', 9, N'51', -2.2, 8.8, 10.2, 1, N'УС 9,5 КЗ-3 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (67, N'115V_Read_I_LCard', 9, N'52', 1, 0, 6, 3, N'(Aktor)БП115В - измеряем I через LCard', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (68, N'115V_Read_U_LCard', 9, N'53', 50, 110.4, 119.6, 2, N'(Aktor)БП115В - измеряем U через LCard', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (69, N'HV', 9, N'54', 8.88, -57, -46, 2, N'[АТТ][ДИСС15] СВЧ 80 В (ВН)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (70, N'I_Mgn', 9, N'55', 20, 68, 74, 2, N'Ток магнетрона (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (71, N'I_M', 9, N'55', 1, -300, 300, 2, N'[АТТ][ДИСС15] Ток магнетрона', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (72, N'P_SVCh', 9, N'56', 1000, -300, 300, 2, N'Контроль P СВЧ (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (73, N'SVCh_P', 9, N'56', 5, 0.5, 5, 2, N'Контроль P СВЧ (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (74, N'TKM_1', 9, N'57', 1, -4.2, -3, 3, N'[АТТ][ДИСС15] ТКМ 1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (75, N'TKM_2', 9, N'58', 1, 3, 4.2, 3, N'[АТТ][ДИСС15] ТКМ 2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (76, N'UNCh_1_1', 9, N'33', 1, 0.8, 1.7, 3, N'УНЧ 1-1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (77, N'UNCh_1_2', 9, N'34', 1, 0.8, 1.7, 3, N'УНЧ 1-2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (78, N'UNCh_2_1', 9, N'35', 1, 0.8, 1.7, 3, N'УНЧ 2-1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (79, N'UNCh_2_2', 9, N'36', 1, 0.8, 1.7, 3, N'УНЧ 2-2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (80, N'UNCh_3_1', 9, N'37', 1, 0.8, 1.7, 3, N'УНЧ 3-1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (81, N'UNCh_3_2', 9, N'38', 1, 0.8, 1.7, 3, N'УНЧ 3-2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (82, N'Plus_27V_Sl', 9, N'59', 5, 25.65, 28.35, 2, N'[АТТ] [ДИСС15] +27V_sl', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (83, N'Delta_27V', 9, N'60', 6000, -81, 81, 2, N'[АТТ] d27V_op(ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (84, N'Delta_27V_15', 9, N'60', 22.222222222222221, 0, 0.3, 2, N'Дельта 27В(ДИСС-15) ', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (85, N'UNCh_1_1_Noise_I', 9, N'33', 1000, 10, 120, 1, N'УНЧ 1-1 (Шум-15) - Полоса I', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (86, N'UNCh_1_2_Noise_I', 9, N'34', 1000, 10, 120, 1, N'УНЧ 1-2 (Шум-15) - Полоса I', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (87, N'UNCh_2_1_Noise_I', 9, N'35', 1000, 10, 120, 1, N'УНЧ 2-1 (Шум-15) - Полоса I', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (88, N'UNCh_2_2_Noise_I', 9, N'36', 1000, 10, 120, 1, N'УНЧ 2-2 (Шум-15) - Полоса I', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (89, N'UNCh_3_1_Noise_I', 9, N'37', 1000, 10, 120, 1, N'УНЧ 3-1 (Шум-15) - Полоса I', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (90, N'UNCh_3_2_Noise_I', 9, N'38', 1000, 10, 120, 1, N'УНЧ 3-2 (Шум-15) - Полоса I', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (91, N'UNCh_1_1_Noise_II', 9, N'33', 1000, 0, 130, 1, N'УНЧ 1-1 (Шум-15) - Полоса II', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (92, N'UNCh_1_2_Noise_II', 9, N'34', 1000, 0, 130, 1, N'УНЧ 1-2 (Шум-15) - Полоса II', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (93, N'UNCh_2_1_Noise_II', 9, N'35', 1000, 0, 130, 1, N'УНЧ 2-1 (Шум-15) - Полоса II', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (94, N'UNCh_2_2_Noise_II', 9, N'36', 1000, 0, 130, 1, N'УНЧ 2-2 (Шум-15) - Полоса II', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (95, N'UNCh_3_1_Noise_II', 9, N'37', 1000, 0, 130, 1, N'УНЧ 3-1 (Шум-15) - Полоса II', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (96, N'UNCh_3_2_Noise_II', 9, N'38', 1000, 0, 130, 1, N'УНЧ 3-2 (Шум-15) - Полоса II', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (97, N'I_K_1', 9, N'', 1, 0, 0.4, 2, N'Ток К 1 (ДИСС-15) разброс в паре', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (98, N'W_Forward_15', 9, N'48', 9.9377, 14.45, 19.45, 2, N'ВПЕРЕД КЗ-1 (ДИСС-15)', N'-0.062')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (99, N'W_Up_15', 9, N'49', -2.053, 4.15, 5.15, 2, N'ВВЕРХ КЗ-1 (ДИСС-15)', N'-0.145')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (100, N'W_Right_15', 9, N'50', 4.667, 14.95, 18.95, 2, N'ВПРАВО КЗ-2 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (101, N'Alfa_0_15', 9, N'51', 9.55, -0.75, 0.75, 2, N'УС 0 КЗ-4 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (102, N'W_Back_15', 9, N'48', -9.9377, 14.45, 19.45, 2, N'НАЗАД КЗ-1 (ДИСС-15)', N'0.062')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (103, N'W_Down_15', 9, N'49', 2.053, 4.15, 5.15, 2, N'ВНИЗ КЗ-1 (ДИСС-15)', N'0.145')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (104, N'Spd_Task4_15', 9, N'47', 79.5, 133, 139, 2, N'ПС-215 для КЗ-4 (ДИСС-15) и КЗ-5', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (105, N'Spd_Task6_15', 9, N'47', 79.5, 212, 218, 2, N'ПС-215 для КЗ-6 и КЗ-5 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (106, N'Alfa_18Plus', 9, N'51', 9.55, 17.6666, 19.16, 1, N'УС +18 КЗ-5 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (107, N'Alfa_18Minus', 9, N'51', 9.55, -19.16, -17.6666, 2, N'УС -18 КЗ-6 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (108, N'Wxr_Zero_15', 9, N'42', 1000, -70, 70, 2, N'Wxr нулевая составляющая скорости (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (109, N'Wzr_Zero_15', 9, N'44', 1000, -70, 70, 2, N'Wzr нулевая составляющая скорости (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (110, N'Wyr_Zero_15', 9, N'61', 1000, -220, 220, 2, N'Wyr нулевая составляющая скорости (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (111, N'PKV_Z_Zero', 9, N'61', 1000, -230, 230, 1, N'Wz_PKV (ДИСС15) Обн.', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (112, N'Memory_Alfa_15', 9, N'', 1, -3.5, 3.5, 2, N'"ПАМЯТЬ" угол сноса', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (113, N'Memory_W_15', 9, N'', 1, -4.5, 4.5, 1, N'"ПАМЯТЬ" путевая скорость', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (114, N'Z_Zero_32', 9, N'62', 1, -0.1, 0.1, 3, N'Zорт ПКВ (ДИСС 32-90А)_1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (115, N'Z_PNP_32', 0, N'306', 1000, 88, 206, 3, N'Z ток ПНП (ДИСС 32-90А)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (116, N'27V_I', 2, N'', 1, 0, 16.5, 1, N'Ток 27В', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (117, N'I_K_2', 9, N'', 1, 0, 0.4, 2, N'Ток К 2 (ДИСС-15) разброс в паре', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (118, N'I_K_3', 9, N'', 1, 0, 0.4, 2, N'Ток К 3 (ДИСС-15) разброс в паре', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (119, N'Manual_TKM_Freq', 9, N'', 1, 13312, 13336, 1, N'Ввод частоты "Магнетрон и ТКМ" (для ID)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (120, N'Manual_TKM_Power', 9, N'', 1, 2, 10, 1, N'Ввод мощности "Магнетрон и ТКМ" (для ID)', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (121, N'I_K_1_1_15', 9, N'18', 10, 0.4, 1.0, 3, N'Ток К1-1 (ДИСС15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (122, N'I_K_2_1_15', 9, N'19', 10, 0.4, 1.0, 3, N'Ток К2-1 (ДИСС15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (123, N'I_K_3_1_15', 9, N'20', 10, 0.4, 1.0, 3, N'Ток К3-1 (ДИСС15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (124, N'I_K_1_2_15', 9, N'21', 10, -1.0, -0.4, 3, N'Ток К1-2 (ДИСС15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (125, N'I_K_2_2_15', 9, N'22', 10, -1.0, -0.4, 3, N'Ток К2-2 (ДИСС15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (126, N'I_K_3_2_15', 9, N'23', 10, -1.0, -0.4, 3, N'Ток К3-2 (ДИСС15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (127, N'BH_Time_15', 0, N'', 1, 30, 90, 2, N'Время включения ВН (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (128, N'Mnl_Forward_15', 0, N'', 1, 14.5, 19.5, 2, N'ВПЕРЕД вручную КЗ-1 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (129, N'Mnl_Back_15', 0, N'', 1, 14.5, 19.5, 2, N'НАЗАД вручную КЗ-2 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (130, N'Mnl_Up_15', 0, N'', 1, 4, 5, 2, N'ВВЕРХ вручную КЗ-1 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (131, N'Mnl_Down_15', 0, N'', 1, 4, 5, 2, N'ВНИЗ вручную КЗ-3 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (132, N'Mnl_Right_15', 0, N'', 1, 14.5, 19.5, 2, N'ВПРАВО вручную КЗ-2 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (133, N'Mnl_Spd_Task4_15', 0, N'', 1, 132.5, 139.5, 2, N'ПС-136 вручную для КЗ-4 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (134, N'Mnl_Spd_Task6_15', 0, N'', 1, 211.5, 218.5, 2, N'ПС-215 вручную для КЗ-6 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (135, N'Mnl_Alfa_0_15', 0, N'', 1, -1, 1, 2, N'УС 0 вручную КЗ-4 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (136, N'Spd_Wy_15', 0, N'', 1, 0.25, 100, 2, N'Скорость отработки Wy (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (137, N'Spd_Wx_15', 0, N'', 1, 2.5, 100, 2, N'Скорость отработки Wx (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (138, N'Spd_Wz_15', 0, N'', 1, 2.5, 100, 2, N'Скорость отработки Wz (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (139, N'Spd_YC_15', 0, N'', 1, 1.1, 100, 2, N'Скорость отработки угла сноса (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (140, N'Spd_PC_15', 0, N'', 1, 2.5, 100, 2, N'Скорость отработки ПС (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (141, N'Block8_15_Forward', 0, N'', 1, 49.3, 52.3, 2, N'Погрешность пути ВПЕРЕД Блока-8 (15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (142, N'Block8_15_Left', 0, N'', 1, 15.8, 18.0, 2, N'Погрешность уклонения ВЛЕВО Блока-8 (15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (143, N'Tangazh15_X_15', 0, N'', 1, -3.5, -1.5, 2, N'Wxr Тангаж 15 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (144, N'Tangazh15_Y_15', 0, N'', 1, 0.8, 1.7, 2, N'Wyr Тангаж 15 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (145, N'Kren15_Z_15', 0, N'', 1, 1.5, 3.5, 2, N'Wzr Крен 15 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (146, N'Kren15_Y_15', 0, N'', 1, -1.7, -0.8, 2, N'Wyr Крен 15 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (147, N'Tangazh345_X_15', 0, N'', 1, 1.5, 3.5, 2, N'Wxr Тангаж 345 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (148, N'Tangazh345_Y_15', 0, N'', 1, -1.7, -0.8, 2, N'Wyr Тангаж 345 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (149, N'Kren345_Z_15', 0, N'', 1, -3.5, -1.5, 2, N'Wzr Крен 345 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (150, N'Kren345_Y_15', 0, N'', 1, 0.8, 1.7, 2, N'Wyr Крен 345 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (151, N'W_Sea_15', 0, N'', 1, 1.7, 2.3, 2, N'"Режим Море" увеличение путевой скорости (15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (152, N'Block8_Forward', 0, N'', 1, 7, 9, 2, N'Блок8 путь ВПЕРЕД (курс 45)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (153, N'Block8_Left', 0, N'', 1, 7, 9, 2, N'Блок8 уклонение ВЛЕВО (курс 45)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (154, N'Block8_Back', 0, N'', 1, 7, 9, 2, N'Блок8 путь НАЗАД (курс 135)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (155, N'Block8_Right', 0, N'', 1, 7, 9, 2, N'Блок8 уклонение ВПРАВО (курс 135)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (156, N'Memory_Time_15', 0, N'', 1, 0.5, 2.5, 2, N'Загорание табло "ПАМЯТЬ" (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (157, N'Memory_W_15', 0, N'', 1, -4.5, 4.5, 2, N'"ПАМЯТЬ" путевая скорость', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (158, N'Memory_Alfa_15', 0, N'', 1, -3.0, 3.0, 2, N'"ПАМЯТЬ" угол сноса', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (159, N'W_K_15', 0, N'', 1, 302.5, 309.5, 2, N'"Режим К" путевая скорость (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (160, N'Alfa_K_15', 0, N'', 1, 14, 16, 2, N'"Режим К" угол сноса (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (161, N'BPK_15_Forward', 0, N'', 1, 14.5, 19.5, 2, N'"БПК" ВПЕРЕД (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (162, N'BPK_15_Side', 0, N'', 1, -2.5, 2.5, 2, N'"БПК" ВБОК (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (163, N'BPK_15_Up', 0, N'', 1, 4, 5, 2, N'"БПК" ВВЕРХ (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (164, N'BPK_15_Back', 0, N'', 1, 14.5, 19.5, 2, N'"БПК" НАЗАД (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (165, N'BPK_15_Right', 0, N'', 1, 14.5, 19.5, 2, N'"БПК" ВПРАВО (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (166, N'BPK_15_Up_2', 0, N'', 1, 2.5, 3.5, 2, N'"БПК" ВВЕРХ(положение 2) (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (167, N'BPK_15_W', 0, N'', 1, 132.5, 139.5, 2, N'"БПК" Путевая скорость (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (168, N'BPK_15_Alfa', 0, N'', 1, -1, 1, 2, N'"БПК" Угол сноса (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (169, N'BPK_15_Signal', 0, N'', 1, -1000, 1000, 2, N'"БПК" W>=50 (да/нет) (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (170, N'BPK_15_Time', 0, N'', 1, 0.5, 2.5, 2, N'"БПК" время вкл.табло П (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (171, N'BPK_15_Delta_W', 0, N'', 1, -4.5, 4.5, 2, N'"БПК" Дельта W (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (172, N'BPK_15_Delta_Alfa', 0, N'', 1, -3.5, 3.5, 2, N'"БПК" Дельта Альфа (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (173, N'BPK_15_Work', 0, N'', 1, -1000, 1000, 2, N'"БПК" РАБОТА (да/нет) (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (174, N'BPK_15_Memory', 0, N'', 1, -1000, 1000, 2, N'"БПК" Положение 4 (да/нет) (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (175, N'Mnl_Alfa_18Minus', 0, N'', 1, -20, -17, 2, N'УС -18 вручную КЗ-6 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (176, N'Mnl_Alfa_18Plus', 0, N'', 1, 17, 20, 2, N'УС +18 вручную КЗ-5 (ДИСС-15)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (177, N'Mnl_Forward_32', 0, N'', 1, 14.5, 19.5, 2, N'Погрешность индикации Вперед-17 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (178, N'Mnl_Left_32', 0, N'', 1, 14.5, 19.5, 2, N'Погрешность индикации Влево-17 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (179, N'Mnl_Down_32', 0, N'', 1, 2.6, 3.6, 2, N'Погрешность индикации Вниз-3,1 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (180, N'Mnl_Back_32', 0, N'', 1, 14.5, 19.5, 2, N'Погрешность индикации Назад-17 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (181, N'Mnl_Right_32', 0, N'', 1, 14.5, 19.5, 2, N'Погрешность индикации Вправо-17 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (182, N'Mnl_Up_32', 0, N'', 1, 2.6, 3.6, 2, N'Погрешность индикации Вверх-3,1 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (183, N'Mnl_Spd_Task3_32', 0, N'', 1, 124.5, 129.5, 2, N'Вручную ПС1(-127) КЗ-3 (УС ПС №1)(ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (184, N'Mnl_Alfa_0_32', 0, N'', 1, -1, 1, 2, N'Вручную УС 0 КЗ-3 (УС ПС №1)(ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (185, N'Point25_X', 0, N'', 1, 20.4, 30, 2, N'ДИСС32.Блок8(п.25) ВПЕРЕД', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (186, N'Point25_Z', 0, N'', 1, -0.6, 0.6, 2, N'ДИСС32.Блок8(п.25) ВЛЕВО', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (187, N'Mnl_Spd_Task4_32', 0, N'', 1, 254.5, 261.5, 2, N'Вручную ПС2(-258) КЗ-4 (УС ПС №1)(ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (188, N'Mnl_Alfa_9_32', 0, N'', 1, 8.5, 10.5, 2, N'Вручную УС 9,5 КЗ-4 (УС ПС №1)(ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (189, N'Z_Task_32', 9, N'62', 1, 1.52, 2.4, 2, N'Zорт ПКВ (ДИСС 32-90А)_2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (190, N'Mode_M_C', 0, N'', 1, 1.8, 2.2, 2, N'Режим "М-С" (КЗ-4 ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (191, N'Mode_M_B', 0, N'', 1, 0.8, 1.2, 2, N'Режим "М-Б" (КЗ-4 ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (192, N'I_K_1_1_Delta', 0, N'', 1, 20, 160, 1, N'Ток К 1-1 Делта (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (193, N'I_K_1_2_Delta', 0, N'', 1, 20, 160, 1, N'Ток К 1-2 Делта (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (194, N'I_K_2_1_Delta', 0, N'', 1, 20, 160, 1, N'Ток К 2-1 Делта (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (195, N'I_K_2_2_Delta', 0, N'', 1, 20, 160, 1, N'Ток К 2-2 Делта (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (196, N'I_K_3_1_Delta', 0, N'', 1, 20, 160, 1, N'Ток К 3-1 Делта (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (197, N'I_K_3_2_Delta', 0, N'', 1, 20, 160, 1, N'Ток К 3-2 Делта (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (198, N'I_K_1_Abs', 0, N'', 1, 0, 45, 1, N'Ток К 1 Abs (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (199, N'I_K_2_Abs', 0, N'', 1, 0, 45, 1, N'Ток К 2 Abs (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (200, N'I_K_3_Abs', 0, N'', 1, 0, 45, 1, N'Ток К 3 Abs (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (201, N'Task2_X_32', 9, N'48', 3.59996999999999, 14.5, 19.5, 1, N'Только для отображения в КЗ-2 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (202, N'Task2_Y_32', 9, N'49', 0.9999917, 2.6, 3.6, 1, N'Только для отображения в КЗ-2 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (203, N'Task2_Z_32', 9, N'50', 3.59996999999999, 14.5, 19.5, 1, N'Только для отображения в КЗ-2 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (204, N'ModeMemory_PC', 0, N'', 1, -3, 3, 1, N'Режим "Память" ПС (КЗ-3 ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (205, N'ModeMemory_YC', 0, N'', 1, -3, 3, 1, N'Режим "Память" УС (КЗ-3 ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (206, N'Refuse_VCh', 0, N'', 1, 1, 1, 1, N'Отказ ВЧ', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (207, N'Refuse_VSS', 0, N'', 1, 1, 1, 1, N'Отказ ВСС ', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (208, N'Search', 0, N'', 1, 1, 1, 1, N'ПОИСК (ДИСС-32-90А)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (209, N'Display_P', 0, N'', 1, 1, 1, 1, N'Табло П,Выкл. (ДИСС-32-90А)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (210, N'Flight', 0, N'', 1, 1, 1, 1, N'Сигнал "Полёт"', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (211, N'Control_Diss', 0, N'', 1, 1, 1, 1, N'КОНТР.ДИСС (ДИСС-32-90А)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (212, N'GoodCondition', 0, N'', 1, 1, 1, 1, N'Исправность', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (213, N'CalcDiss', 0, N'', 1, 1, 1, 1, N'Сигнал Счисл.ДИСС (ДИСС-32-90А)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (214, N'CalcDPSM', 0, N'', 1, 1, 1, 1, N'Сигнал Счисл.ДПСМ (ДИСС-32-90А)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (215, N'Refuse_BVK', 0, N'', 1, 1, 1, 1, N'Отказ БВК', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (216, N'PositionOff', 0, N'', 1, 1, 1, 1, N'Сигнал Откл.коорд (ДИСС-32-90А)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (217, N'BPK_Forward_32', 0, N'', 1, 14.5, 19.5, 1, N'БПК (ДИСС-32) Вперед-17', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (218, N'BPK_Left_32', 0, N'', 1, 14.5, 19.5, 1, N'БПК (ДИСС-32) Влево-17', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (219, N'BPK_Down_32', 0, N'', 1, 2.6, 3.6, 1, N'БПК (ДИСС-32) Вниз-3', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (220, N'BPK_Back_32', 0, N'', 1, 14.5, 19.5, 1, N'БПК (ДИСС-32) Назад-17', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (221, N'BPK_Right_32', 0, N'', 1, 14.5, 19.5, 1, N'БПК (ДИСС-32) Вправо-17', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (222, N'BPK_Up_32', 0, N'', 1, 2.6, 3.6, 1, N'БПК (ДИСС-32) Вверх-3', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (223, N'BPK_PC1_32', 0, N'', 1, 124.5, 129.5, 1, N'БПК (ДИСС-32) ПС1-127', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (224, N'BPK_YC0_32', 0, N'', 1, -1, 1, 1, N'БПК (ДИСС-32) УС0', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (225, N'BPK_PC2_32', 0, N'', 1, 254.5, 261.5, 1, N'БПК (ДИСС-32) ПС2-258', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (226, N'BPK_YC9_32', 0, N'', 1, 8.5, 10.5, 1, N'БПК (ДИСС-32) УС 9,5', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (227, N'Point24_X', 0, N'', 1, 62, 65, 1, N'ДИСС32.Блок8(п.24) ВПЕРЕД', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (228, N'Point24_Z', 0, N'', 1, 9.5, 11.7, 1, N'ДИСС32.Блок8(п.24) ВЛЕВО', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (229, N'Heel_Plus12_X', 0, N'', 1, 0, 0, 1, N'Крен +12 X (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (230, N'Heel_Minus12_X', 0, N'', 1, 0, 0, 1, N'Крен -12 X (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (231, N'Pitch_Plus12_X', 0, N'', 1, 0, 0, 1, N'Тангаж +12 X (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (232, N'Heel_Plus12_Z', 0, N'', 1, 0, 0, 1, N'Крен +12 Z (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (233, N'Heel_Minus12_Z', 0, N'', 1, 0, 0, 1, N'Крен -12 Z (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (234, N'Pitch_Plus12_Z', 0, N'', 1, 0, 0, 1, N'Тангаж +12 Z (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (235, N'Pitch_Minus12_Z', 0, N'', 1, 0, 0, 1, N'Тангаж -12 Z (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (236, N'Heel_Plus12_Y', 0, N'', 1, 0, 0, 1, N'Крен +12 Y (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (237, N'Heel_Minus12_Y', 0, N'', 1, 0, 0, 1, N'Крен -12 Y (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (238, N'Pitch_Plus12_Y', 0, N'', 1, 0, 0, 1, N'Тангаж +12 Y (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (239, N'Pitch_Minus12_Y', 0, N'', 1, 0, 0, 1, N'Тангаж -12 Y (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (240, N'Gen_Sin_Limit', 5, N'100', 0, 0, 0, 0, N'Настройки генератора синусов', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (241, N'TK9_W', 0, N'', 1, 971, 1021, 0, N'Путевая скорость (ДИСС13 ТК9)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (242, N'TK9_A', 0, N'', 1, -22, -18, 0, N'Угол сноса (ДИСС13 ТК9)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (243, N'Ampl_F1', 9, N'64', 1, 4, 1000, 1, N'Амплитуда импульсов F1 (ДИСС13 ТК9)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (244, N'Ampl_F2', 9, N'65', 1, 4, 1000, 1, N'Амплитуда импульсов F2 (ДИСС13 ТК9)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (245, N'Ampl_F3', 9, N'66', 1, 4, 1000, 1, N'Амплитуда импульсов F2 (ДИСС13 ТК9)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (246, N'Dev_F1', 0, N'', 1, 0, 7, 0, N'Девиация импульсов F1 (ДИСС13 ТК9)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (247, N'Dev_F2', 0, N'', 1, 0, 7, 0, N'Девиация импульсов F2 (ДИСС13 ТК9)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (248, N'Dev_F3', 0, N'', 1, 0, 7, 0, N'Девиация импульсов F3 (ДИСС13 ТК9)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (249, N'Dur_F1', 0, N'', 1, 0, 10, 0, N'Длительность импульсов F1 (ДИСС13 ТК9)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (250, N'Dur_F2', 0, N'', 1, 0, 10, 0, N'Длительность импульсов F2 (ДИСС13 ТК9)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (251, N'Dur_F3', 0, N'', 1, 0, 10, 0, N'Длительность импульсов F3 (ДИСС13 ТК9)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (252, N'I_SVCh1_1', 9, N'67', 1000, 240, 340, 0, N'Ток СВЧ 1-1 (ТК9 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (253, N'I_SVCh1_2', 9, N'68', 1000, 240, 340, 0, N'Ток СВЧ 1-2 (ТК9 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (254, N'I_SVCh2_1', 9, N'69', 1000, 240, 340, 0, N'Ток СВЧ 2-1 (ТК9 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (255, N'I_SVCh2_2', 9, N'70', 1000, 240, 340, 0, N'Ток СВЧ 2-2 (ТК9 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (256, N'I_SVCh3_1', 9, N'71', 1000, 240, 340, 0, N'Ток СВЧ 3-1 (ТК9 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (257, N'I_SVCh3_2', 9, N'72', 1000, 240, 340, 0, N'Ток СВЧ 3-2 (ТК9 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (258, N'PowerSVCh', 0, N'', 1, 260, 10000, 0, N'Мощность СВЧ-колебаний (ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (259, N'PhaseIndex', 0, N'', 1, 2.1, 2.9, 2, N'Индекс фазовой модуляции (ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (260, N'PTime', 0, N'', 1, 2.3, 4.3, 1, N'Время перехода в режим П (ТК11 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (261, N'FreqSVCh', 0, N'', 1, 8780, 8820, 1, N'Частота СВЧ-колебаний (ТК5 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (262, N'HV515V', 0, N'', 1, 511, 519, 1, N'Повышенное напряжение питания при 515В (ТК8 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (263, N'LV515V', 0, N'', 1, 511, 519, 1, N'Пониженное напряжение питания при 515В (ТК8 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (264, N'HV400V', 0, N'', 1, 398, 402, 1, N'Повышенное напряжение питания при 400В (ТК8 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (265, N'LV400V', 0, N'', 1, 398, 402, 1, N'Пониженное напряжение питания при 400В (ТК8 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (266, N'HV6p3V', 0, N'', 1, 5.9, 6.7, 2, N'Повышенное напряжение питания при 6,3В (ТК8 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (267, N'LV6p3V', 0, N'', 1, 5.9, 6.7, 2, N'Пониженное напряжение питания при 6,3В (ТК8 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (268, N'PLightCatch', 0, N'', 1, 1, 1, 0, N'Загорание табло П в режиме "Захват" (ТК11 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (269, N'TK17_Smes1', 0, N'', 1, 25, 60, 0, N'СМЕС 1 (ТК17 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (270, N'TK17_Smes2', 0, N'', 1, 25, 60, 0, N'СМЕС 2 (ТК17 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (271, N'TK17_Geter', 0, N'', 1, 20, 70, 0, N'ГЕТЕР (ТК17 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (272, N'TK17_UNChOut', 0, N'', 1, 25, 95, 0, N'ВЫХ УНЧ (ТК17 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (273, N'TK17_Klistr', 0, N'', 1, 40, 80, 0, N'КЛИСТР (ТК17 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (274, N'TK17_W2', 0, N'', 1, -16, 16, 0, N'Путевая скорость задачи 2 (ТК17 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (275, N'TK17_A2', 0, N'', 1, -1.5, 1.5, 1, N'Угол сноса задачи 2 (ТК17 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (276, N'TK17_W2M', 0, N'', 1, 1.1, 1.5, 2, N'Путевая скорость задачи 2 в режиме "Море" (ТК17 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (277, N'TK17_A2M', 0, N'', 1, 0, 0, 1, N'Угол сноса задачи 2 в режиме "Море" (ТК17 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (278, N'TK17_W1', 0, N'', 1, -12.1, 12.1, 0, N'Путевая скорость задачи 1 (ТК17 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (279, N'TK17_A1', 0, N'', 1, -1.5, 1.5, 1, N'Угол сноса задачи 1 (ТК17 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (280, N'TK17_W3', 0, N'', 1, 315.5, 325.5, 0, N'Путевая скорость задачи 3 (ТК17 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (281, N'TK17_A3', 0, N'', 1, -1.5, 1.5, 1, N'Угол сноса задачи 3 (ТК17 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (282, N'TK17_PLight', 0, N'', 1, 1, 1, 0, N'Загорание табло табло П (ТК17 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (283, N'MedFiltrKoef', 4, N'', 1, 0, 0, 1, N'Компенсация медианного фильтра (ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (284, N'Smes1_Done', 0, N'', 1, 1, 1, 1, N'Исправность смесителя 1 (ТК13 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (285, N'Smes2_Done', 0, N'', 1, 1, 1, 1, N'Исправность смесителя 2 (ТК13 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (286, N'Speed_15_On', 0, N'', 1, 971, 1021, 1, N'Скорость 15-е включение (ТК13 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (287, N'Speed_30_On', 0, N'', 1, 971, 1021, 1, N'Скорость 30-е включение (ТК13 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (288, N'Speed_50_On', 0, N'', 1, 971, 1021, 1, N'Скорость 50-е включение (ТК13 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (289, N'Angle_15_On', 0, N'', 1, -22, -18, 1, N'Угол сноса 15-е включение (ТК13 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (290, N'Angle_30_On', 0, N'', 1, -22, -18, 1, N'Угол сноса 30-е включение (ТК13 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (291, N'Angle_50_On', 0, N'', 1, -22, -18, 1, N'Угол сноса 50-е включение (ТК13 ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (292, N'VSS_10V', 9, N'73', 2, 0, 20, 2, N'[АТТ] 10В ВСС', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (293, N'Wx_SPSV', 9, N'74', 1, 0, 0, 2, N'[АТТ] Wx СПСВ', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (294, N'Wy_SPSV', 9, N'75', 1, 0, 0, 2, N'[АТТ] Wy СПСВ', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (295, N'Wz_SPSV', 9, N'76', 1, 0, 0, 2, N'[АТТ] Wz СПСВ', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (296, N'Wx_Tester', 9, N'77', 1, 0, 0, 2, N'[АТТ] Wx ТЕСТЕР', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (297, N'Wy_Tester', 9, N'78', 1, 0, 0, 2, N'[АТТ] Wy ТЕСТЕР', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (298, N'Wz_Tester', 9, N'79', 1, 0, 0, 2, N'[АТТ] Wz ТЕСТЕР', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (299, N'I_K_1_1_32A', 9, N'24', 1, -300, 300, 2, N'[АТТ][ДИСС32] Ток К1-1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (300, N'I_K_2_1_32A', 9, N'25', 1, -300, 300, 2, N'[АТТ][ДИСС32] Ток К2-1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (301, N'I_K_3_1_32A', 9, N'26', 1, -300, 300, 2, N'[АТТ][ДИСС32] Ток К3-1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (302, N'I_K_1_2_32A', 9, N'39', 1, -300, 300, 2, N'[АТТ][ДИСС32] Ток K1-2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (303, N'I_K_2_2_32A', 9, N'40', 1, -300, 300, 2, N'[АТТ][ДИСС32] Ток К2-2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (304, N'I_K_3_2_32A', 9, N'41', 1, -300, 300, 2, N'[АТТ][ДИСС32] Ток К3-2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (305, N'P_SVCh_A', 9, N'56', 1, -10, 10, 3, N'[АТТ] P_SVCh', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (306, N'Z_PNP_32A', 9, N'63', 1.25, -10, 10, 3, N'[АТТ] Z_PNP_32', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (307, N'PKV_Z_Zero_A', 9, N'62', 1, -10, 10, 3, N'[АТТ] PKV_Z_Zero', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (308, N'Plus60Vst', 9, N'54', 9, 0, 100, 1, N'+60В ст (ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (309, N'ContrHV', 9, N'6', 3, 0, 100, 1, N'Контр. ВН -(11-17)В (ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (310, N'Minus27Vst', 9, N'11', 5, 0, 100, 1, N'-27В ст (ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (311, N'Plus12Vst', 9, N'2', 2, 0, 100, 1, N'+12,6В ст (ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (312, N'Minus12Vst', 9, N'3', 2, 0, 100, 1, N'-12,6В ст (ДИСС13)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (313, N'Wx_NKV', 9, N'42', 1, 0, 10, 3, N'[АТТ] Wx НКВ', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (314, N'Wy_NKV', 9, N'46', 1, 0, 10, 3, N'[АТТ] Wy НКВ', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (315, N'Wz_NKV', 9, N'44', 1, 0, 10, 3, N'[АТТ] Wz НКВ', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (316, N'Pitch_Minus12_X', 0, N'', 1, 0, 0, 1, N'Тангаж -12 X (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (317, N'Mnl_F_32', 0, N'', 1, 13316, 13334, 1, N'Частота СВЧ колебаний (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (319, N'Task3_PC2_32', 9, N'55', -39.8406374501992, 125, 129, 1, N'ПС2 127 КЗ-3 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (320, N'Task3_Y20_32', 9, N'61', 2.2, -0.7, 0.7, 1, N'УС 0 КЗ-3 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (321, N'MapCorner', 9, N'59', 1, 1, 1, 0, N'Map Corner (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (322, N'ModeMemory_PC2', 0, N'', 1, -3, 3, 1, N'Режим "Память" ПС2 (КЗ-3 ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (323, N'ModeMemory_YC2', 0, N'', 1, -3, 3, 1, N'Режим "Память" УС2 (КЗ-3 ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (324, N'Task4_PC2_32', 9, N'55', -39.8406374501992, 255, 261, 1, N'ДИСС32.УСПС2.КЗ4.Ск 258', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (325, N'Task4_Y29_32', 9, N'61', -2.2, 8.8, 10.2, 1, N'ДИСС32.УСПС2.КЗ3.Угол 9,5', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (326, N'Mode_M_C_2', 0, N'', 1, 1.8, 2.2, 2, N'Режим "М-С" (КЗ-4 ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (327, N'Mode_M_B_2', 0, N'', 1, 0.8, 1.2, 2, N'Режим "М-Б" (КЗ-4 ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (328, N'Mnl_Spd2_Task4_32', 0, N'', 1, 254.5, 261.5, 2, N'Вручную ПС2(-258) КЗ-4 (УС ПС №2)(ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (329, N'Mnl_Alfa2_9_32', 0, N'', 1, 8.5, 10.5, 2, N'Вручную УС 9,5 КЗ-4 (УС ПС №2)(ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (330, N'Point24_X_2', 0, N'', 1, 62, 65, 1, N'ДИСС32.Блок8Гео(п.24) ВПЕРЕД', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (331, N'Point24_Z_2', 0, N'', 1, 9.5, 11.7, 1, N'ДИСС32.Блок8 Гео(п.24) ВЛЕВО', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (332, N'Mnl_Spd2_Task3_32', 0, N'', 1, 124.5, 129.5, 2, N'Вручную ПС1(-127) КЗ-3 (УС ПС №2)(ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (333, N'Mnl_Alfa2_0_32', 0, N'', 1, -1, 1, 2, N'Вручную УС 0 КЗ-3 (УС ПС №2)(ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (336, N'AistKZ4Alpha', 9, N'27', -3, -0.5, 0.5, 2, N'ДИСС15.АИСТ.КЗ-4.Альфа', N'15')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (337, N'AistKZ4W', 0, N'12', -3.3, -12.6, -9.6, 2, N'ДИСС15.АИСТ.КЗ-4.W', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (338, N'AistKZ5Alpha', 9, N'27', -3, 5.4, 7.4, 2, N'ДИСС15.АИСТ.КЗ-5.Альфа', N'15')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (339, N'AistKZ6Alpha', 9, N'27', -3, -7.4, -5.4, 2, N'ДИСС15.АИСТ.КЗ-6.Альфа', N'15')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (340, N'AistKZ6W', 0, N'12', -3.3, -20.1, -15.1, 2, N'ДИСС15.АИСТ.КЗ-6.W', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (341, N'AistAMove', 0, N'', 1, 1, 1, 0, N'ДИСС15.АИСТ.Изм. Альфа до Закр. Шторки', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (342, N'AistAClose', 0, N'', 1, 1, 1, 0, N'ДИСС15.АИСТ.Изм. Альфа после Закр. Шторки', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (344, N'SAU_KZ6_A', 9, N'30', 1, -1.05, -0.45, 3, N'ДИСС15.САУ.КЗ6.А', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (346, N'SAU_KZ5_A', 9, N'30', 1, 0.45, 1.05, 3, N'ДИСС15.САУ.КЗ5.А', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (347, N'SAU_KZ1_Wxr', 0, N'33', 3.3, -1000, -5, 1, N'ДИСС15.САУ.КЗ1.Wxr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (348, N'SAU_KZ1_Wyr', 0, N'298', -3.3, 4.5, 8.5, 2, N'ДИСС15.САУ.КЗ1.Wyr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (350, N'SAU_Work_Equiv', 0, N'', 1, 1, 1, 1, N'ДИСС15.САУ.Испр. от Эквивалент', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (351, N'SAU_Work_Heel', 0, N'', 1, 1, 1, 1, N'ДИСС15.САУ.Испр. от Крен', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (352, N'SAU_Work_Pitch', 0, N'', 1, 1, 1, 1, N'ДИСС15.САУ.Испр. от Тангаж', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (353, N'SAU_Work_HV', 0, N'', 1, 1, 1, 1, N'ДИСС15.САУ.Испр. от ВН', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (354, N'SAU_KZ3_Wxr', 0, N'33', 3.3, 5, 1000, 1, N'ДИСС15.САУ.КЗ3.Wxr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (355, N'SAU_KZ3_Wyr', 0, N'298', -3.3, -8.5, -4.5, 1, N'ДИСС15.САУ.КЗ3.Wyr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (356, N'SU_KZ1_Wxr', 9, N'29', 1, -0.136, -0.096, 3, N'ДИСС15.СУ.КЗ1.Wxr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (357, N'SU_KZ2_Wzr', 9, N'30', 1, 0.125, 0.165, 3, N'ДИСС15.СУ.КЗ2.Wzr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (358, N'SU_KZ1_Wyr', 9, N'31', 1, -3.559, -3.059, 3, N'ДИСС15.СУ.КЗ1.Wyr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (359, N'SU_KZ4_Wyr', 9, N'31', 1, -0.1, 0.1, 2, N'ДИСС15.СУ.KZ4.Wyr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (360, N'DVS_B8_Forward', 0, N'', 1, 25, 35, 1, N'ДИСС15.ДВС.Блок8.Вперёд', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (361, N'DVS_B8_ToSide', 0, N'', 1, 0, 1, 1, N'ДИСС15.ДВС.Блок8.Вбок', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (362, N'DVS_B10_Up', 0, N'', 1, 22.5, 37.5, 2, N'ДИСС15.ДВС.Блок10.Вверх', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (363, N'DVS_B10_ToSide', 0, N'', 1, 0, 1.5, 2, N'ДИСС15.ДВС.Блок10.Вбок', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (364, N'DVS_MapCorner', 0, N'', 1, 1, 1, 0, N'ДИСС15.ДВС.Граница карты', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (365, N'Landysh_KZ2_Wxr', 0, N'296', -3, -0.85, -0.45, 3, N'ДИСС15.Ландыш.КЗ2.Wxr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (366, N'Landysh_KZ2_Wzr', 0, N'297', -3, 0.45, 0.85, 3, N'ДИСС15.Ландыш.КЗ2.Wzr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (367, N'Landysh_KZ4_Wxr', 0, N'296', -3, 4.5, 6.5, 2, N'ДИСС15.Ландыш.КЗ4.Wxr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (368, N'Landysh_KZ4_Wzr', 0, N'297', -3, -0.1, 0.1, 2, N'ДИСС15.Ландыш.КЗ4.Wzr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (369, N'Landysh_KZ5_Wzr', 0, N'297', -3, 1.9, 2.9, 2, N'ДИСС15.Ландыш.КЗ5.Wzr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (370, N'Landysh_KZ6_Wxr', 0, N'296', -3, 6.5, 9.5, 2, N'ДИСС15.Ландыш.КЗ6.Wxr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (371, N'Landysh_KZ6_Wzr', 0, N'297', -3, -2.9, -1.9, 2, N'ДИСС15.Ландыш.КЗ6.Wzr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (372, N'Landysh_StatorX', 0, N'', 1, 8, 16, 1, N'ДИСС15.Ландыш.СтаторX', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (373, N'Landysh_StatorZ', 0, N'', 1, 8, 16, 1, N'ДИСС15.Ландыш.СтаторZ', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (374, N'SAU_KZ3_Wzr', 0, N'33', 3.3, -1, 1, 1, N'ДИСС15.САУ.КЗ3.Wzr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (375, N'SAU_KZ2_Wzr', 0, N'33', 5.4, -1000, -8, 1, N'ДИСС15.САУ.КЗ2.Wzr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (376, N'B10_VisirPosition', 0, N'', 1, 1, 1, 1, N'ДИСС32.Блок10.Положение визира', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (377, N'ModeMemory', 0, N'', 1, 1, 1, 1, N'[ДИСС3228] Режим "Память"', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (378, N'ModeAC', 0, N'', 1, 1, 1, 1, N'[ДИСС3228] Режим "АС"', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (379, N'ModeContrBS', 0, N'', 1, 1, 1, 1, N'[ДИСС3228] Режим "Контр. БС"', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (380, N'ModeContrVCh', 0, N'', 1, 1, 1, 1, N'[ДИСС3228] Режим "Контр. ВЧ"', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (381, N'MOD1_3228', 0, N'21', 1, 2.7, 4.5, 2, N'[ДИСС3228] МОД1(+)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (382, N'MOD2_3228', 0, N'22', 1, -4.5, -2.7, 2, N'[ДИСС3228] МОД2(-)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (383, N'UHFFrequency', 0, N'', 1, 13316, 13334, 0, N'[ДИСС3228] Частота СВЧ', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (384, N'MagnetronTraining', 0, NULL, 1, 0, 1, 2, N'Тренировка магнетрона', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (385, N'Mnl_Spd_Task3_32_FlightVSS', 0, N'', 1, 123.5, 130.5, 2, N'Вручную ПС1(-127) КЗ-3 (УС ПС №1)(ДИСС-32) Штатный ВСС', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (386, N'Mnl_Spd_Task4_32_FlightVSS', 0, N'', 1, 253, 263, 2, N'Вручную ПС2(-258) КЗ-4 (УС ПС №1)(ДИСС-32) Штатный ВСС', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (387, N'BPK_PC1_32_FlightVSS', 0, N'', 1, 123.5, 130.5, 1, N'БПК (ДИСС-32) ПС1-127 Штатный ВСС', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (388, N'BPK_PC2_32_FlightVSS', 0, N'', 1, 253, 263, 1, N'БПК (ДИСС-32) ПС2-258 Штатный ВСС', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (389, N'MOD_1_Diss32', 0, N'21', 1, 2.7, 4.5, 3, N'[АТТ] МОД1 для ДИСС-32', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (390, N'MOD_2_Diss32', 0, N'22', 1, -4.5, -2.7, 3, N'[АТТ] МОД2 для ДИСС-32', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (391, N'NKV_X_Zero32_Diss32', 0, N'50', 1, -40, 40, 1, N'Wx NKV (ДИСС32) Обн. для ДИСС-32', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (392, N'NKV_Z_Zero32_Diss32', 0, N'52', 1, -80, 80, 1, N'Wz NKV (ДИСС32) Обн. для ДИСС-32', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (393, N'NKV_Y_Zero_Diss32', 0, N'53', 1, -300, 300, 1, N'Wy NKV (ДИСС-32) Обн. для ДИСС-32', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (394, N'SU_KZ5_Wxr', 9, N'77', 1, 0.6, 0.9, 3, N'ДИСС15.СУ.КЗ5.Wxг', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (395, N'SU_KZ5_Wzr', 9, N'78', 1, -0.4, 0.2, 3, N'ДИСС15.СУ.КЗ5.Wzг', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (396, N'PKV_X_Zero32', 9, N'48', 1000, -330, 330, 1, N'Wx ПКВ (ДИСС-32) Обнуление', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (397, N'PKV_Y_Zero32', 9, N'49', 1000, -300, 300, 1, N'Wy ПКВ (ДИСС-32) Обнуление', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (398, N'PKV_Z_Zero32', 9, N'50', 1000, -230, 230, 1, N'Wz ПКВ (ДИСС-32) Обнуление', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (399, N'Wx_NKV_Task1_SystemDiss32', 9, N'42', 1, 0.41, 0.53, 3, N'Wx НКВ (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (400, N'Wy_NKV_Task1_SystemDiss32', 9, N'46', 1, -3.7, -2.5, 3, N'Wy НКВ (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (401, N'Wz_NKV_Task1_SystemDiss32', 9, N'44', 1, 1.37, 1.77, 3, N'Wz НКВ (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (402, N'Wx_PKV_Task1_SystemDiss32', 9, N'48', 1, -5.31, -4.11, 3, N'Wx ПКВ (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (403, N'Wy_PKV_Task1_SystemDiss32', 9, N'49', 1, -3.7, -2.5, 2, N'Wy ПКВ Задача 1 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (404, N'Wz_PKV_Task1_SystemDiss32', 9, N'50', 1, -5.31, -4.11, 3, N'Wz ПКВ Задача 1 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (405, N'Wx_SPSV_Task1_SystemDiss32', 9, N'74', 1, -0.59, -0.45, 3, N'Wx СПСВ Задача 1 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (406, N'Wy_SPSV_Task1_SystemDiss32', 9, N'75', 1, 2.05, 3.25, 3, N'Wy СПСВ Задача 1 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (407, N'Wz_SPSV_Task1_SystemDiss32', 9, N'76', 1, -0.59, -0.45, 3, N'Wz СПСВ Задача 1 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (408, N'Wx_Tester_Task1_SystemDiss32', 9, N'77', 1, 0.6, 1, 3, N'Wx Тестер Задача 1 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (409, N'Wy_Tester_Task1_SystemDiss32', 9, N'78', 1, 1.67, 2.47, 3, N'Wy Тестер Задача 1 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (410, N'Wz_Tester_Task1_SystemDiss32', 9, N'79', 1, 3.27, 3.67, 3, N'Wz Тестер Задача 1 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (411, N'Wx_NKV_Task2_SystemDiss32', 9, N'42', 1, -0.53, -0.41, 3, N'Wx НКВ Задача 2 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (412, N'Wy_NKV_Task2_SystemDiss32', 9, N'46', 1, 2.5, 3.7, 3, N'Wy НКВ Задача 2 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (413, N'Wz_NKV_Task2_SystemDiss32', 9, N'44', 1, -1.77, -1.37, 3, N'Wz НКВ Задача 2 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (414, N'Wx_PKV_Task2_SystemDiss32', 9, N'48', 1, 4.11, 5.31, 3, N'Wx ПКВ Задача 2 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (415, N'Wy_PKV_Task2_SystemDiss32', 9, N'49', 1, 2.5, 3.7, 3, N'Wy ПКВ Задача 2 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (416, N'Wz_PKV_Task2_SystemDiss32', 9, N'50', 1, 4.11, 5.31, 3, N'Wz ПКВ Задача 2 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (417, N'Wx_SPSV_Task2_SystemDiss32', 9, N'74', 1, 0.45, 0.59, 3, N'Wx СПСВ Задача 2 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (418, N'Wy_SPSV_Task2_SystemDiss32', 9, N'75', 1, -3.25, -2.05, 3, N'Wy СПСВ Задача 2 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (419, N'Wz_SPSV_Task2_SystemDiss32', 9, N'76', 1, 0.45, 0.59, 3, N'Wz СПСВ Задача 2 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (420, N'Wx_Tester_Task2_SystemDiss32', 9, N'77', 1, 0.09, 0.49, 3, N'Wx Тестер Задача 2 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (421, N'Wy_Tester_Task2_SystemDiss32', 9, N'78', 1, 3.53, 4.33, 3, N'Wy Тестер Задача 2 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (422, N'Wz_Tester_Task2_SystemDiss32', 9, N'79', 1, 2.33, 2.73, 3, N'Wz Тестер Задача 2 (ДИСС-32) в борт. системы', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (423, N'VisualControlMemorize', 0, NULL, 1, 1, 1, 1, N'Визуальный контроль режима "ЗАПОМИНАНИЕ" (ДИСС-32)', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (424, N'HV485V', 0, NULL, 1, 481, 489, 1, N'Повышенное напряжение питания при 485В (ТК8 ДИСС13)', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (425, N'LV485V', 0, NULL, 1, 481, 489, 1, N'Пониженное напряжение питания при 485В (ТК8 ДИСС13)', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (428, N'Flight_Diss3228', 9, N'23', 1, 1, 1, 2, N'Сигнал "Полёт" для ДИСС-3228', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (429, N'AistKZZeroAlpha', 9, N'27', -3, -30, 30, 2, N'ДИСС15.АИСТ.КЗ-Zero.Альфа', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (430, N'LandyshStatorValue', 0, N'33', 1000, -300, 300, 1, N'ДИСС15Г.ЛАНДЫШ.Напряжение на статоре', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (431, N'SU_KZ1_Wyr2', 9, N'28', 1, 2.65, 3.05, 2, N'ДИСС15.СУ.КЗ1.Wyr', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (432, N'SAU_Workability', 0, N'', 1, 1, 1, 1, N'ДИСС15.САУ.Сигнал исправность', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (433, N'Plus_5V_32_28', 9, N'56', 1, 4.75, 5.25, 3, N'ДИСС32-28 +5В вторичные', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (434, N'Capture_Diss3228', 9, N'58', 1, 1, 1, 1, N'ДИСС32-28 Сигнал Захват', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (435, N'Current_U27V_Diss3228M', 0, N'', 1, 0, 5, 1, N'ДИСС32-28М Ток по =27В', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (436, N'U12V_Diss3228M', 0, N'4', 1, 11.4, 12.6, 2, N'ДИСС32-28М (+12В)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (437, N'UMinus_12V_Diss3228M', 0, N'7', 1, -12.6, -11.4, 2, N'ДИСС32-28М (-12В)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (438, N'HFFrequency_Ch1_3_Diss3228M', 0, N'', 1, 13317, 13323, 1, N'ДИСС32-28М СВЧ Колебания (Канал 1,3)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (439, N'HFFrequency_Ch2_Diss3228M', 0, N'', 1, 13317, 13323, 1, N'ДИСС32-28М СВЧ Колебания (Канал 2)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (440, N'U9V_Diss3228M', 9, N'45', 1, 8.55, 9.45, 3, N'ДИСС32-28М (+9В)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (441, N'Indication_Diss3228M', 0, N'', 1, 1, 1, 1, N'ДИСС32-28М Индикация', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (442, N'UNCh_1_1_Diss3228M', 9, N'33', 1, 1, 3, 1, N'Шум УНЧ 1-1 (ДИСС3228М)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (443, N'UNCh_1_2_Diss3228M', 9, N'34', 1, 1, 3, 1, N'Шум УНЧ 1-2 (ДИСС3228М)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (444, N'UNCh_2_1_Diss3228M', 9, N'35', 1, 1, 3, 1, N'Шум УНЧ 2-1 (ДИСС3228М)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (445, N'UNCh_2_2_Diss3228M', 9, N'36', 1, 1, 3, 1, N'Шум УНЧ 2-2 (ДИСС3228М)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (446, N'UNCh_3_1_Diss3228M', 9, N'37', 1, 1, 3, 1, N'Шум УНЧ 3-1 (ДИСС3228М)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (447, N'UNCh_3_2_Diss3228M', 9, N'38', 1, 1, 3, 1, N'Шум УНЧ 3-2 (ДИСС3228М)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (448, N'RefuseMCO_Diss3228M', 9, N'6', 1, 1, 1, 1, N'Отказ МЦО (ДИСС-32-28М)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (449, N'RefuseUSK_Diss3228M', 9, N'54', 1, 1, 1, 1, N'Отказ УСК (ДИСС-32-28М)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (450, N'RefuseMP_Diss3228M', 9, N'73', 1, 1, 1, 1, N'Отказ МП (ДИСС-32-28М)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (451, N'RefuseMAO_Diss3228M', 9, N'11', 1, 1, 1, 1, N'Отказ МАО (ДИСС-32-28М)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (452, N'Plus_5V_Diss3228M', 9, N'18', 1, 4.75, 5.25, 3, N'[АТТ] +5В (ДИСС-32-28М)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (453, N'Delta_15V_Att', 0, N'14', 0.001, 0, 0, 3, N'Дельта 15В в В для аттестации', NULL)
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (454, N'Delta_27V_Att', 0, N'83', 0.001, 0, 0, 3, N'Дельта 27В в В для аттестации', NULL)

INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (455, N'BPK_PC1_2_32', 0, N'', 1, 124.5, 129.5, 1, N'БПК (ДИСС-32) ПС1-127', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (456, N'BPK_YC0_2_32', 0, N'', 1, -1, 1, 1, N'БПК (ДИСС-32) УС0', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (457, N'BPK_PC2_2_32', 0, N'', 1, 254.5, 261.5, 1, N'БПК (ДИСС-32) ПС2-258', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (458, N'BPK_YC9_2_32', 0, N'', 1, 8.5, 10.5, 1, N'БПК (ДИСС-32) УС 9,5', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (459, N'BPK_PC1_2_32_FlightVSS', 0, N'', 1, 123.5, 130.5, 1, N'БПК (ДИСС-32) ПС1-127 Штатный ВСС', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (460, N'BPK_PC2_2_32_FlightVSS', 0, N'', 1, 253, 263, 1, N'БПК (ДИСС-32) ПС2-258 Штатный ВСС', N'')

GO
SET IDENTITY_INSERT [dbo].[Parameters] OFF
GO
INSERT [dbo].[PassportValues] ([TestId], [ParamId], [Value]) VALUES (261, 274, CAST(300.00000 AS Decimal(18, 5)))
INSERT [dbo].[PassportValues] ([TestId], [ParamId], [Value]) VALUES (261, 275, CAST(10.00000 AS Decimal(18, 5)))
INSERT [dbo].[PassportValues] ([TestId], [ParamId], [Value]) VALUES (261, 278, CAST(400.00000 AS Decimal(18, 5)))
INSERT [dbo].[PassportValues] ([TestId], [ParamId], [Value]) VALUES (264, 274, CAST(300.00000 AS Decimal(18, 5)))
INSERT [dbo].[PassportValues] ([TestId], [ParamId], [Value]) VALUES (264, 275, CAST(20.10000 AS Decimal(18, 5)))
INSERT [dbo].[PassportValues] ([TestId], [ParamId], [Value]) VALUES (264, 278, CAST(500.00000 AS Decimal(18, 5)))
INSERT [dbo].[PassportValuesReference] ([DeviceTypeId], [ParamId], [Number], [Name], [DefaultValue]) VALUES (3, 274, 1, N'Скорость (задача 2)', NULL)
INSERT [dbo].[PassportValuesReference] ([DeviceTypeId], [ParamId], [Number], [Name], [DefaultValue]) VALUES (3, 275, 2, N'Угол отклонения (задача 2)', NULL)
INSERT [dbo].[PassportValuesReference] ([DeviceTypeId], [ParamId], [Number], [Name], [DefaultValue]) VALUES (3, 278, 0, N'Скорость (задача 1)', NULL)
GO
SET IDENTITY_INSERT [dbo].[Port] ON 

GO
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (1, N'не определено', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (2, N'COM1', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (3, N'COM2', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (4, N'COM3', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (5, N'COM4', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (6, N'COM5', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (7, N'COM6', 8, 9600, 0, 1, 900, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (8, N'COM7', 8, 9600, 0, 1, 900, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (9, N'COM8', 8, 9600, 0, 1, 900, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (10, N'COM9', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (11, N'COM10', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (12, N'COM11', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (13, N'COM12', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (14, N'COM13', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (15, N'COM14', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (16, N'COM15', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (17, N'COM16', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (18, N'COM17', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (19, N'COM18', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (20, N'COM19', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (21, N'COM20', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (22, N'COM21', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (23, N'COM22', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (24, N'COM23', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (25, N'COM24', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (26, N'COM25', 8, 9600, 0, 1, 300, 100)
INSERT [dbo].[Port] ([PortID], [PortName], [DataBits], [BaudRate], [Parity], [StopBits], [ReadTimeout], [WriteTimeout]) VALUES (27, N'COM26', 8, 9600, 0, 1, 300, 100)
GO
SET IDENTITY_INSERT [dbo].[Port] OFF
GO
SET IDENTITY_INSERT [dbo].[Post] ON 

GO
INSERT [dbo].[Post] ([PostId], [PostName], [IsActive]) VALUES (1, N'инженер', 1)
GO
SET IDENTITY_INSERT [dbo].[Post] OFF
GO
SET IDENTITY_INSERT [dbo].[Report] ON 

GO
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (1, 1, 1, 1, 1, N'Питающие напряжения:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (2, 1, 1, 2, NULL, N'     постоянного тока', N'В', N'27,5 ± 1,375', 3)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (3, 1, 1, 4, NULL, N'     переменного тока', N'В', N'115 ± 4,6', 68)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (4, 1, 2, 1, 2, N'Токи, потребляемые по цепям питания:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (5, 1, 2, 2, NULL, N'     = 27,5 В', N'А', N'не более 16,5', 116)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (6, 1, 2, 3, NULL, N'     ~ 115 B', N'А', N'не более 2', 67)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (7, 1, 3, 1, 3, N'Напряжение вторичных источников питания:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (8, 1, 3, 2, NULL, N'     12,6 В', N'В', N'12,6 ± 0,63', 5)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (9, 1, 3, 3, NULL, N'     -12,6 B', N'В', N'-12,6 ± 0,63', 6)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (10, 1, 3, 4, NULL, N'     -6,3 B', N'В', N'от -5,67 до -6,62', 9)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (11, 1, 3, 5, NULL, N'     -20 B', N'В', N'-20 ± 1', 11)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (12, 1, 3, 6, NULL, N'     -27 B', N'В', N'-27 ± 1,35', 16)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (13, 1, 3, 7, NULL, N'     -27 В опорн.', N'В', N'-27 ± 1,35', 17)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (14, 1, 3, 8, NULL, N'     27 В след.', N'В', N'27 ± 1,35', 82)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (15, 1, 4, 1, 4, N'Разность абсолютных значений напряжений -27 В ст.опорн. и +27 В ст.след.', N'%', N'не более 0,3', 84)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (16, 1, 5, 1, 5, N'Время задержки включения высокого напряжения на магнетроне', N'сек', N'30 - 90', 127)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (17, 1, 6, 1, 6, N'Напряжение на магнетроне', N'В', N'от -46 до -57', 69)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (18, 1, 7, 1, 7, N'Ток магнетрона', N'мА', N'68 - 74', 70)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (19, 1, 8, 1, 8, N'Токи кристалических смесителей:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (20, 1, 8, 2, NULL, N'          Канал I - 1', N'мА', N'от 0,4 до 1,0', 121)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (21, 1, 8, 3, NULL, N'          Канал I - 2', N'мА', N'от -0,4 до -1,0', 124)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (22, 1, 8, 4, NULL, N'          Канал II - 1', N'мА', N'от 0,4 до 1,0', 122)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (23, 1, 8, 5, NULL, N'          Канал II - 2', N'мА', N'от -0,4 до -1,0', 125)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (24, 1, 8, 6, NULL, N'          Канал III - 1', N'мА', N'от 0,4 до 1,0', 123)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (25, 1, 8, 7, NULL, N'          Канал III - 2', N'мА', N'от -0,4 до -1,0', 126)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (26, 1, 8, 8, 0, N'     разброс в паре:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (27, 1, 8, 9, NULL, N'          Канал I', N'мА', N'не более 0,4', 97)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (28, 1, 8, 10, NULL, N'          Канал II', N'мА', N'не более 0,4', 117)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (29, 1, 8, 11, NULL, N'          Канал III', N'мА', N'не более 0,4', 118)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (30, 1, 9, 1, 9, N'"Контрольная" мощность магнетрона', N'мА', N'не менее 0,5', 73)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (31, 1, 10, 1, 10, N'Частота СВЧ', N'МГц', N'13 312 - 13 336', 119)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (32, 1, 11, 1, 11, N'Мощность СВЧ', N'Вт', N'2 - 10', 120)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (33, 1, 12, 1, 12, N'Напряжение шумов и уровень напряжения сигналов на выходах УНЧ:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (34, 1, 12, 2, 0, N'     Шум, Полоса I (допускаются колебания      ±20 мВ)', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (35, 1, 12, 3, NULL, N'          Канал I - 1', N'мВ', N'10 - 120', 85)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (36, 1, 12, 4, NULL, N'          Канал I - 2', N'мВ', N'10 - 120', 86)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (37, 1, 12, 5, NULL, N'          Канал II - 1', N'мВ', N'10 - 120', 87)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (38, 1, 12, 6, NULL, N'          Канал II - 2', N'мВ', N'10 - 120', 88)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (39, 1, 12, 7, NULL, N'          Канал III - 1', N'мВ', N'10 - 120', 89)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (40, 1, 12, 8, NULL, N'          Канал III - 2', N'мВ', N'10 - 120', 90)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (41, 1, 12, 9, 0, N'     Шум, Полоса II (допускаются колебания     ±20 мВ)', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (42, 1, 12, 10, NULL, N'          Канал I - 1', N'мВ', N'более Полосы I' + CHAR(10) + N'и не более 130', 91)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (43, 1, 12, 11, NULL, N'          Канал I - 2', N'мВ', N'более Полосы I' + CHAR(10) + N'и не более 130', 92)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (44, 1, 12, 12, NULL, N'          Канал II - 1', N'мВ', N'более Полосы I' + CHAR(10) + N'и не более 130', 93)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (45, 1, 12, 13, NULL, N'          Канал II - 2', N'мВ', N'более Полосы I' + CHAR(10) + N'и не более 130', 94)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (46, 1, 12, 14, NULL, N'          Канал III - 1', N'мВ', N'более Полосы I' + CHAR(10) + N'и не более 130', 95)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (47, 1, 12, 15, NULL, N'          Канал III - 2', N'мВ', N'более Полосы I' + CHAR(10) + N'и не более 130', 96)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (48, 1, 12, 16, 0, N'     Напряжение сигналов', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (49, 1, 12, 17, NULL, N'          Канал I - 1', N'В', N'0,8 - 1,7', 76)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (50, 1, 12, 18, NULL, N'          Канал I - 2', N'В', N'0,8 - 1,7', 77)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (51, 1, 12, 19, NULL, N'          Канал II - 1', N'В', N'0,8 - 1,7', 78)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (52, 1, 12, 20, NULL, N'          Канал II - 2', N'В', N'0,8 - 1,7', 79)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (53, 1, 12, 21, NULL, N'          Канал III - 1', N'В', N'0,8 - 1,7', 80)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (54, 1, 12, 22, NULL, N'          Канал III - 2', N'В', N'0,8 - 1,7', 81)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (55, 1, 13, 1, 13, N'Значения составляющих скорости при их обнулении:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (56, 1, 13, 2, NULL, N'     Wxг', N'мВ', N'не более ±70', 108)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (57, 1, 13, 3, NULL, N'     Wzг', N'мВ', N'не более ±70', 109)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (58, 1, 13, 4, NULL, N'     Wyг', N'мВ', N'не более ±220', 110)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (59, 1, 14, 1, 14, N'Точность вычисления и индикации составляющих вектора скорости (допускаются колебания ±0,5 км/ч по Wxг и Wzг и ±0,05 м/с по Wyг):', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (60, 1, 14, 2, 0, N'     по КПК-2:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (61, 1, 14, 3, NULL, N'          ВПЕРЕД (Wxг)', N'км/ч', N'16,95 ± 2,5', 98)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (62, 1, 14, 4, NULL, N'          НАЗАД (Wxг)', N'км/ч', N'16,95 ± 2,5', 102)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (63, 1, 14, 5, NULL, N'          ВПРАВО (Wzг)', N'км/ч', N'16,95 ± 2,0', 100)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (64, 1, 14, 6, NULL, N'          ВВЕРХ (Wyг)', N'м/с', N'4,65 ± 0,5', 99)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (65, 1, 14, 7, NULL, N'          ВНИЗ (Wyг)', N'м/с', N'4,65 ± 0,5', 103)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (66, 1, 14, 8, 0, N'     по блоку 6:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (67, 1, 14, 9, NULL, N'          ВПЕРЕД (Wxг)', N'км/ч', N'17 ± 2,5', 128)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (68, 1, 14, 10, NULL, N'          НАЗАД (Wxг)', N'км/ч', N'17 ± 2,5', 129)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (69, 1, 14, 11, NULL, N'          ВПРАВО (Wzг)', N'км/ч', N'17 ± 2,5', 132)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (70, 1, 14, 12, NULL, N'          ВВЕРХ (Wyг)', N'м/с', N'4,5 ± 0,5', 130)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (71, 1, 14, 13, NULL, N'          ВНИЗ (Wyг)', N'м/с', N'4,5 ± 0,5', 131)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (72, 1, 15, 1, 15, N'Скорость отработки составляющих вектора скорости блоком 6:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (73, 1, 15, 2, NULL, N'     Wxг', N'км/ч /с', N'не менее 2,5', 137)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (74, 1, 15, 3, NULL, N'     Wzг', N'км/ч /с', N'не менее 2,5', 138)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (75, 1, 15, 4, NULL, N'     Wyг', N'м/с /с', N'не менее 0,25', 136)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (76, 1, 16, 1, 16, N'Точность вычисления и индикации угла сноса (допускаются колебания ±30΄):', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (77, 1, 16, 2, 0, N'     по КПК-2:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (78, 1, 16, 3, NULL, N'          УС 0', N'град', N'0 ± 45΄', 101)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (79, 1, 16, 4, NULL, N'          УС +18', N'град', N'18°25΄ ± 45΄', 106)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (80, 1, 16, 5, NULL, N'          УС -18', N'град', N'-18°25΄ ± 45΄', 107)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (81, 1, 16, 6, 0, N'     по блоку 7:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (82, 1, 16, 7, NULL, N'          УС 0', N'град', N'0 ± 1°', 135)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (83, 1, 16, 8, NULL, N'          УС +18', N'град', N'18°30΄±1°30΄', 176)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (84, 1, 16, 9, NULL, N'          УС -18', N'град', N'-18°30΄±1°30΄', 175)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (85, 1, 17, 1, 17, N'Скорость отработки указателем угла сноса участка в 15 градусов по шкале блока 7', N'град/с', N'не менее 1,1', 139)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (86, 1, 18, 1, 18, N'Точность вычисления и индикации путевой скорости (допускаются колебания ±0,5 км/ч):', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (87, 1, 18, 2, NULL, N'     ПС 215 (по КПК-2)', N'км/ч', N'215 ± 3', 105)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (88, 1, 18, 3, NULL, N'     ПС 215 (по блоку 7)', N'км/ч', N'215 ± 3,5', 134)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (89, 1, 19, 1, 19, N'Скорость отработки участка в 100 км/ч счетчиком путевой скорости блока 7', N'км/ч /с', N'не менее 2,5', 140)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (90, 1, 20, 1, 20, N'Точность вычисления и индикации пройденного пути и бокового уклонения (показания счетчиков блока 8):', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (91, 1, 20, 2, NULL, N'     пройденный путь ВПЕРЕД', N'км', N'50,8 ± 1,5', 141)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (92, 1, 20, 3, NULL, N'     боковое уклонение ВЛЕВО', N'км', N'16,9 ± 1,1', 142)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (93, 1, 21, 1, 21, N'Связь аппаратуры по углам (величины изменения продольной, поперечной и вертикальной составляющих вектора скорости) на значениях:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (94, 1, 21, 2, 0, N'     Θ (тангаж) = 15°:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (95, 1, 21, 3, NULL, N'          Wxг', N'км/ч', N'      уменьш.       на 1,5 - 3,5', 143)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (96, 1, 21, 4, NULL, N'          Wyг', N'м/с', N'      увелич.         на 0,8 - 1,7', 144)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (97, 1, 21, 5, 0, N'     γ (крен) = 15°:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (98, 1, 21, 6, NULL, N'          Wzг', N'км/ч', N'      увелич.         на 1,5 - 3,5', 145)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (99, 1, 21, 7, NULL, N'          Wyг', N'м/с', N'      уменьш.        на 0,8 - 1,7', 146)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (100, 1, 21, 8, 0, N'     Θ (тангаж) = 345°:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (101, 1, 21, 9, NULL, N'          Wxг', N'км/ч', N'      увелич.         на 1,5 - 3,5', 147)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (102, 1, 21, 10, NULL, N'          Wyг', N'м/с', N'      уменьш.        на 0,8 - 1,7', 148)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (103, 1, 21, 11, 0, N'     γ (крен) = 345°:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (104, 1, 21, 12, NULL, N'          Wzг', N'км/ч', N'      уменьш.        на 1,5 - 3,5', 149)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (105, 1, 21, 13, NULL, N'          Wyг', N'м/с', N'      увелич.         на 0,8 - 1,7', 150)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (106, 1, 22, 1, 22, N'Калибровка аппаратуры в режиме "Море" (изменение путевой скорости относительно режима "Суша")', N'%', N'увелич. на 2±0,3', 151)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (107, 1, 23, 1, 23, N'Отработка блоком 8 контрольной задачи УС 0 на значениях:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (108, 1, 23, 2, 0, N'     ψ (курс) = 45°:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (109, 1, 23, 3, NULL, N'          пройденный путь ВПЕРЕД', N'км', N'8 ± 1', 152)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (110, 1, 23, 4, NULL, N'          боковое уклонение ВЛЕВО', N'км', N'8 ± 1', 153)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (111, 1, 23, 5, 0, N'     ψ (курс) = 135°:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (112, 1, 23, 6, NULL, N'          пройденный путь НАЗАД', N'км', N'8 ± 1', 154)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (113, 1, 23, 7, NULL, N'          боковое уклонение ВПРАВО', N'км', N'8 ± 1', 155)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (114, 1, 24, 1, 24, N'Работа аппаратуры в режиме "ПАМЯТЬ":', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (115, 1, 24, 2, NULL, N'     загорание табло П', N'с', N'1,5 ± 1', 156)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (116, 1, 24, 3, NULL, N'     путевая скорость', N'км/ч', N'±4,5', 157)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (117, 1, 24, 4, NULL, N'     угол сноса', N'град', N'±3,0°', 158)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (118, 1, 25, 1, 25, N'Точность индикации в режиме "К":', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (119, 1, 25, 2, NULL, N'     путевая скорость', N'км/ч', N'306 ± 3,5', 159)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (120, 1, 25, 3, NULL, N'     угол сноса ВЛЕВО', N'град', N'15° ± 1°', 160)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (121, 1, 26, 1, 26, N'Точностные характеристики выходных параметров от прибора ПК при последовательных положениях переключателя:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (122, 1, 26, 2, 0, N'     Положение 1:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (123, 1, 26, 3, NULL, N'          ВПЕРЕД', N'км/ч', N'17 ± 2,5', 161)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (124, 1, 26, 4, NULL, N'          ВБОК', N'км/ч', N'0 ± 2,5', 162)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (125, 1, 26, 5, NULL, N'          ВВЕРХ', N'м/с', N'4,5 ± 0,5', 163)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (126, 1, 26, 6, 0, N'     Положение 2:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (127, 1, 26, 7, NULL, N'          НАЗАД', N'км/ч', N'17 ± 2,5', 164)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (128, 1, 26, 8, NULL, N'          ВПРАВО', N'км/ч', N'17 ± 2,5', 165)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (129, 1, 26, 9, NULL, N'          ВВЕРХ', N'м/с', N'3 ± 0,5', 166)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (130, 1, 26, 10, 0, N'     Положение 3:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (131, 1, 26, 11, NULL, N'          путевая скорость', N'км/ч', N'136 ± 3,5', 167)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (132, 1, 26, 12, NULL, N'          угол сноса', N'град', N'0 ± 1°', 168)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (133, 1, 26, 13, NULL, N'          W≥50 км/ч', N'—', N'на бл.6 вкл. табло ВЫК., на бл.7 откр. шторка', 169)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (134, 1, 26, 14, 0, N'     Положение 4:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (135, 1, 26, 15, NULL, N'          время вкл. табло П', N'с', N'1,5 ± 1', 170)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (136, 1, 26, 16, NULL, N'       изменения относительно положения 3:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (137, 1, 26, 17, NULL, N'          путевая скорость', N'км/ч', N'не более ±4,5', 171)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (138, 1, 26, 18, NULL, N'          угол сноса', N'град', N'не более ±3,5°', 172)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (139, 1, 26, 19, 0, N'     Положение РАБОТА', N'—', N'на пр. ПК должны выключ. табло М и КОНТР.', 173)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (140, 1, 26, 20, 0, N'     Положение 4', N'—', N'должны включ. табло М и КОНТР. и выключ. табло РАБОТА', 174)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (141, 1, 27, 1, 27, N'Напряжение смещения на модуляторных диодах (на разъеме Ш21 блока ВЧ относит. контакта 30):', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (142, 1, 27, 2, NULL, N'     27 (ТКМ 1)', N'В', N'-3,6 ± 0,6', 74)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (143, 1, 27, 3, NULL, N'     26 (ТКМ 2)', N'В', N'3,6 ± 0,6', 75)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (144, 2, 1, 1, 1, N'Питающие напряжения:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (145, 2, 1, 2, NULL, N'     постоянного тока', N'В', N'27 ± 2,7', 2)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (146, 2, 1, 3, NULL, N'     переменного тока', N'В', N'115 ± 6', 1)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (147, 2, 11, 1, 11, N'Точность индикации составляющих скорости блоком 6 в режиме контрольных задач:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (148, 2, 11, 10, NULL, N'          ВПЕРЕД - 17 (Wxг)', N'км/ч', N'17 ± 2,5', 177)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (149, 2, 11, 11, NULL, N'          ВЛЕВО - 17 (Wzг)', N'км/ч', N'17 ± 2,5', 178)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (150, 2, 11, 12, NULL, N'          ВНИЗ - 3,1 (Wyг)', N'м/с', N'3,1 ± 0,5', 179)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (151, 2, 11, 13, NULL, N'          НАЗАД - 17 (Wxг)', N'км/ч', N'17 ± 2,5', 180)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (152, 2, 11, 14, NULL, N'          ВПРАВО - 17 (Wzг)', N'км/ч', N'17 ± 2,5', 181)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (153, 2, 11, 15, NULL, N'          ВВЕРХ - 3,1 (Wyг)', N'м/с', N'3,1 ± 0,5', 182)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (154, 2, 12, 1, 12, N'Точность вычисления и индикации путевой скорости:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (155, 2, 12, 2, NULL, N'     КПК-2 (УС ПС №1):    ПС1 - 127', N'км/ч', N'127 ± 2,0', 62)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (156, 2, 12, 3, NULL, N'                     ПС2 - 258', N'км/ч', N'258 ± 3,0', 65)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (157, 2, 12, 6, NULL, N'     УС ПС №1:    ПС1 - 127', N'км/ч', N'127 ± 2,5', 183)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (158, 2, 12, 7, NULL, N'                        ПС2 - 258', N'км/ч', N'258 ± 3,5', 187)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (159, 2, 13, 1, 13, N'Точность вычисления и индикации угла сноса:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (160, 2, 13, 2, NULL, N'     КПК-2 (УС ПС №1):    УС 0', N'град', N'0 ± 0,7', 64)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (161, 2, 13, 3, NULL, N'                     УС 9,5 (ВЛЕВО)', N'град', N'9,5 ± 0,7', 66)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (162, 2, 13, 6, NULL, N'     УС ПС №1:    УС 0', N'град', N'0 ± 1,0', 184)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (163, 2, 13, 7, NULL, N'                        УС 9,5 (ВЛЕВО)', N'град', N'9,5 ± 1,0', 188)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (164, 2, 15, 1, 15, N'Увеличение путевой скорости в режиме "Море":', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (165, 2, 15, 3, NULL, N'     М - С', N'%', N'2 ± 0,2', 190)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (166, 2, 15, 4, NULL, N'     М - Б', N'%', N'1 ± 0,2', 191)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (167, 2, 14, 1, 14, N'Работа аппаратуры в режиме "Память":', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (168, 2, 14, 3, NULL, N'     ПС', N'км/ч', N'0 ± 3', 204)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (169, 2, 14, 4, NULL, N'     УС', N'град', N'0 ± 3', 205)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (170, 2, 20, 1, 20, N'Связь аппаратуры по углам (величины изменения продольной, поперечной и вертикальной составляющих вектора скорости) на значениях:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (171, 2, 20, 2, 0, N'     Θ (тангаж) = 12°', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (172, 2, 20, 3, NULL, N'          Wxг', N'км/ч', N'увелич.', 231)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (173, 2, 20, 4, NULL, N'          Wzг', N'км/ч', N'без измен.', 234)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (174, 2, 20, 5, NULL, N'          Wyг', N'м/с', N'уменьш.', 238)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (175, 2, 20, 6, 0, N'     Θ (тангаж) = -12°', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (176, 2, 20, 7, NULL, N'          Wxг', N'км/ч', N'уменьш.', 316)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (177, 2, 20, 8, NULL, N'          Wzг', N'км/ч', N'без измен.', 235)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (178, 2, 20, 9, NULL, N'          Wyг', N'м/с', N'увелич.', 239)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (179, 2, 20, 10, 0, N'     γ (крен) = 12°', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (180, 2, 20, 11, NULL, N'          Wxг', N'км/ч', N'без измен.', 229)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (181, 2, 20, 12, NULL, N'          Wzг', N'км/ч', N'увелич.', 232)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (182, 2, 20, 13, NULL, N'          Wyг', N'м/с', N'уменьш.', 236)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (183, 2, 20, 14, 0, N'     γ (крен) = -12°', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (184, 2, 20, 15, NULL, N'          Wxг', N'км/ч', N'без измен.', 230)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (185, 2, 20, 16, NULL, N'          Wzг', N'км/ч', N'уменьш.', 233)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (186, 2, 20, 17, NULL, N'          Wyг', N'м/с', N'увелич.', 237)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (187, 2, 16, 1, 16, N'Точность вычисления и индикации выходных параметров при проверке от БПК:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (188, 2, 16, 2, 0, N'     Вперед 17, Влево 17, Вниз 3', N'—', N'по блоку 6', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (189, 2, 16, 3, NULL, N'          Wxг', N'км/ч', N'17 ± 2,5', 217)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (190, 2, 16, 4, NULL, N'          Wzг', N'км/ч', N'17 ± 2,5', 218)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (191, 2, 16, 5, NULL, N'          Wyг', N'м/с', N'3,1 ± 0,5', 219)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (192, 2, 16, 6, 0, N'     Назад 17, Вправо 17, Вверх 3', N'—', N'по блоку 6', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (193, 2, 16, 7, NULL, N'          Wxг', N'км/ч', N'17 ± 2,5', 220)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (194, 2, 16, 8, NULL, N'          Wzг', N'км/ч', N'17 ± 2,5', 221)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (195, 2, 16, 9, NULL, N'          Wyг', N'м/с', N'3,1 ± 0,5', 222)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (196, 2, 16, 10, 0, N'     Скорость 127, Снос 0', N'—', N'по инд. УС ПС', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (197, 2, 2, 1, 2, N'Напряжение вторичных источников питания:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (198, 2, 2, 2, NULL, N'     12,6 B', N'В', N'12,6 ± 0,65', 4)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (199, 2, 2, 3, NULL, N'     -12,6 B', N'В', N'-12,6 ± 0,65', 7)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (200, 2, 2, 4, NULL, N'     6,3 B', N'В', N'6,3 ± 0,3', 8)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (201, 2, 2, 5, NULL, N'     -6,3 B', N'В', N'-6,3 ± 0,3', 10)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (202, 2, 2, 6, NULL, N'     -20 B', N'В', N'-20 ± 1', 11)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (203, 2, 2, 7, NULL, N'     15 В след.', N'В', N'15 ± 0,75', 12)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (204, 2, 2, 8, NULL, N'     -15 В опорн.', N'В', N'-15 ± 0,75', 13)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (205, 2, 2, 9, NULL, N'     5 B', N'В', N'5 ± 0,25', 15)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (206, 2, 2, 10, NULL, N'     -27 B', N'В', N'-27 ± 1,35', 16)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (207, 2, 3, 1, 3, N'Разность модулей вторичных напряжений    Δ15 В', N'мВ', N'от -35 до +35', 14)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (208, 2, 4, 1, 4, N'Контрольное напряжение генератора ГЛПД:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (209, 2, 4, 2, NULL, N'     ГЛПД 1', N'В', N'от 2,4 до 3,5', 18)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (210, 2, 4, 3, NULL, N'     ГЛПД 2', N'В', N'от 2,4 до 3,5', 19)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (211, 2, 5, 1, 5, N'Контрольное напряжение, пропорциональное общему току генераторных диодов', N'В', N'от -1,7 до -2,9', 20)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (212, 2, 6, 1, 6, N'Частота СВЧ', N'МГц', N'13325 ± 9', 317)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (213, 2, 7, 1, 7, N'Напряжение смещения на модуляторных диодах:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (214, 2, 7, 2, NULL, N'     МОД 1', N'В', N'от +2,3 до +4,9', 21)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (215, 2, 7, 3, NULL, N'     МОД 2', N'B', N'от -2,3 до -4,9', 22)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (216, 2, 8, 1, 8, N'Разность напряжений смещения на диодах смесителей "ТОК К":', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (217, 2, 8, 2, NULL, N'          Канал I-1', N'мВ', N'90 ± 70', 192)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (218, 2, 8, 3, NULL, N'          Канал I-2', N'мВ', N'90 ± 70', 193)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (219, 2, 8, 4, NULL, N'          Канал II-1', N'мВ', N'90 ± 70', 194)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (220, 2, 8, 5, NULL, N'          Канал II-2', N'мВ', N'90 ± 70', 195)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (221, 2, 8, 6, NULL, N'          Канал III-1', N'мВ', N'90 ± 70', 196)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (222, 2, 8, 7, NULL, N'          Канал III-2', N'мВ', N'90 ± 70', 197)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (223, 2, 8, 8, 0, N'     разброс в паре:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (224, 2, 8, 9, NULL, N'          Канал I', N'мВ', N'не более 45', 198)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (225, 2, 8, 10, NULL, N'          Канал II', N'мВ', N'не более 45', 199)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (226, 2, 8, 11, NULL, N'          Канал III', N'мВ', N'не более 45', 200)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (227, 2, 9, 1, 9, N'Напряжение смещения на СД:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (228, 2, 9, 2, NULL, N'     Канал I-1', N'В', N'от 0,6 до 1,8', 32)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (229, 2, 9, 3, NULL, N'     Канал I-2', N'В', N'от 0,6 до 1,8', 33)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (230, 2, 9, 4, NULL, N'     Канал II-1', N'В', N'от 0,6 до 1,8', 34)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (231, 2, 9, 5, NULL, N'     Канал II-2', N'В', N'от 0,6 до 1,8', 35)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (232, 2, 9, 6, NULL, N'     Канал III-1', N'В', N'от 0,6 до 1,8', 36)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (233, 2, 9, 7, NULL, N'     Канал III-2', N'В', N'от 0,6 до 1,8', 37)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (234, 2, 10, 1, 10, N'Напряжение шумов на выходах УНЧ:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (235, 2, 10, 2, NULL, N'     Канал I-1', N'мВ', N'от 30 до 100', 38)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (236, 2, 10, 3, NULL, N'     Канал I-2', N'мВ', N'от 30 до 100', 39)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (237, 2, 10, 4, NULL, N'     Канал II-1', N'мВ', N'от 30 до 100', 40)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (238, 2, 10, 5, NULL, N'     Канал II-2', N'мВ', N'от 30 до 100', 41)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (239, 2, 10, 6, NULL, N'     Канал III-1', N'мВ', N'от 30 до 100', 42)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (240, 2, 10, 7, NULL, N'     Канал III-2', N'мВ', N'от 30 до 100', 43)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (241, 2, 21, 1, 21, N'Выходные напряжения блока ВСС при обнулении составляющих вектора скорости:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (242, 2, 21, 2, NULL, N'     Wxг НКВ', N'мВ', N'не более ±10', 50)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (243, 2, 21, 3, NULL, N'     Wzг НКВ', N'мВ', N'не более ±15', 52)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (244, 2, 21, 4, NULL, N'     Wyг НКВ', N'мВ', N'не более ±30', 53)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (245, 2, 21, 5, NULL, N'     W НКВ', N'мВ', N'не более ±10', 54)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (246, 2, 16, 11, NULL, N'         ПС №1', N'км/ч', N'127 ± 2,5', 223)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (247, 2, 16, 12, NULL, N'         УС №1', N'град', N'0 ± 1', 224)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (248, 2, 16, 15, 0, N'     Скорость 258, Снос 9,5 (ВЛЕВО)', N'—', N'по инд. УС ПС', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (249, 2, 16, 16, NULL, N'         ПС №1', N'км/ч', N'258 ± 3,5', 225)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (250, 2, 16, 17, NULL, N'         УС №1', N'град', N'9,5 ± 1', 226)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (251, 2, 24, 1, 24, N'Работа световой сигнализации изделия:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (252, 2, 24, 2, NULL, N'     КОНТРОЛЬ ДИСС', NULL, N'Соотв. РЭ', 211)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (253, 2, 24, 3, NULL, N'     ПОИСК', NULL, N'Соотв. РЭ', 208)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (254, 2, 24, 4, NULL, N'     ИСПРАВНОСТЬ', NULL, N'Соотв. РЭ', 212)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (255, 2, 24, 5, NULL, N'     ПОЛЕТ', NULL, N'Соотв. РЭ', 210)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (256, 2, 17, 1, 17, N'Точность вычисления и индикации пройденного пути и бокового уклонения (показания счетчиков блока 8 Ортодромического):', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (257, 2, 17, 2, NULL, N'     пройденный путь ВПЕРЕД', N'км', N'63,5 ± 1,5', 227)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (258, 2, 17, 3, NULL, N'     боковое уклонение ВЛЕВО', N'км', N'10,6 ± 1,1', 228)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (259, 2, 24, 6, NULL, N'     ОТКАЗ ВЧ', NULL, N'Соотв. РЭ', 206)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (260, 2, 24, 7, NULL, N'     ОТКАЗ ВСС ', NULL, N'Соотв. РЭ', 207)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (261, 2, 24, 8, NULL, N'     Табло П, Выкл.(на УСПС)', NULL, N'Соотв. РЭ', 209)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (262, 2, 25, 1, 25, N'Выдача каналом координат сигналов:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (263, 2, 25, 2, NULL, N'     СЧИСЛЕНИЕ ОТ ДИСС', NULL, N'Соотв. РЭ', 213)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (264, 2, 25, 3, NULL, N'     СЧИСЛЕНИЕ ОТ ДПСМ ', NULL, N'Соотв. РЭ', 214)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (265, 2, 25, 4, NULL, N'     ИСПРАВНОСТЬ БВК (ОТКАЗ БВК)', NULL, N'Соотв. РЭ', 215)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (266, 2, 22, 1, 22, N'Точность выдачи во внешние системы напряжения Zорт (ПВК):', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (267, 2, 22, 2, NULL, N'     в режиме контрольной задачи', N'В', N'1,96 ± 0,44', 189)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (268, 2, 22, 3, NULL, N'     в режиме обнуления', N'В', N'0 ± 0,1', 114)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (269, 2, 23, 1, 23, N'Точность выдачи во внешние системы сигнала постоянного тока Zорт (ПНП)', N'мкА', N'147 ± 59', 115)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (270, 2, 11, 2, NULL, N'     КПК-2:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (271, 2, 11, 3, NULL, N'          ВПЕРЕД - 17 (Wxг)', N'км/ч', N'17 ± 2,5', 58)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (272, 2, 11, 4, NULL, N'          ВЛЕВО - 17 (Wzг)', N'км/ч', N'17 ± 2,5', 60)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (273, 2, 11, 5, NULL, N'          ВНИЗ - 3,1 (Wyг)', N'м/c', N'3,1 ± 0,5', 59)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (274, 2, 11, 6, NULL, N'          НАЗАД - 17 (Wxг)', N'км/ч', N'17 ± 2,5', 201)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (275, 2, 11, 7, NULL, N'          ВПРАВО - 17 (Wzг)', N'км/ч', N'17 ± 2,5', 203)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (276, 2, 11, 8, NULL, N'          ВВЕРХ - 3,1 (Wyг)', N'м/c', N'3,1 ± 0,5', 202)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (277, 2, 11, 9, NULL, N'     блок 6:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (278, 2, 25, 5, NULL, N'     ОТКЛЮЧЕНИЕ КООРДИНАТ', NULL, N'Соотв. РЭ', 216)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (279, 3, 1, 1, 1, N'1. Частота СВЧ колебаний', N'МГц', N'8800 ± 20', 261)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (280, 3, 2, 1, 2, N'2. Мощность СВЧ колебаний', N'мВт', N'не менее 260', 258)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (281, 3, 3, 1, 3, N'3. Индекс фазовой модуляции', N'—', N'от 2,1 до 2,9', 259)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (282, 3, 4, 5, NULL, N'    при 515В:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (283, 3, 4, 6, NULL, N'        - при повышенном напряжении питания', N'В', N'515 ± 4', 262)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (284, 3, 4, 7, NULL, N'        - при пониженном напряжении питания', N'В', N'515 ± 4', 263)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (285, 3, 4, 8, NULL, N'    при 400В:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (286, 3, 4, 9, NULL, N'        - при повышенном напряжении питания', N'В', N'400 ± 2', 264)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (287, 3, 4, 10, NULL, N'        - при пониженном напряжении питания', N'В', N'400 ± 2', 265)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (288, 3, 4, 11, NULL, N'    при 6,3В:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (289, 3, 4, 12, NULL, N'        - при повышенном напряжении питания', N'В', N'6,3 ± 0,4', 266)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (290, 3, 4, 13, NULL, N'        - при пониженном напряжении питания', N'В', N'6,3 ± 0,4', 267)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (291, 3, 5, 2, NULL, N'    путевая скорость', N'км/ч', N'996 ± 25', 241)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (292, 3, 5, 3, NULL, N'    угол сноса', N'град', N'-20 ± 2', 242)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (293, 3, 5, 4, NULL, N'    амплитуда имульсов:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (294, 3, 5, 5, NULL, N'         F1', N'В', N'не менее 4', 243)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (295, 3, 5, 6, NULL, N'         F2', N'В', N'не менее 4', 244)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (296, 3, 5, 7, NULL, N'         F3', N'В', N'не менее 4', 245)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (297, 3, 5, 8, NULL, N'    девиация импульсов:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (298, 3, 5, 9, NULL, N'         F1', N'%', N'не более 7%', 246)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (299, 3, 5, 10, NULL, N'         F2', N'%', N'не более 7%', 247)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (300, 3, 5, 11, NULL, N'         F3', N'%', N'не более 7%', 248)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (301, 3, 5, 12, NULL, N'    длительность импульсов:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (302, 3, 5, 13, NULL, N'         F1', N'мкс', N'не более 10', 249)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (303, 3, 5, 14, NULL, N'         F2', N'мкс', N'не более 10', 250)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (304, 3, 5, 15, NULL, N'         F3', N'мкс', N'не более 10', 251)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (305, 3, 5, 16, NULL, N'    запитка:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (306, 3, 5, 17, NULL, N'         Мод 1', N'мА', N'240 - 340', 252)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (307, 3, 5, 18, NULL, N'         Мод 2', N'мА', N'240 - 340', 254)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (308, 3, 5, 19, NULL, N'         Мод 3', N'мА', N'240 - 340', 256)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (310, 3, 6, 1, 6, N'6. Время перехода в режим П', N'с', N'2,3 - 4,3', 260)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (311, 3, 8, 1, 8, N'8. Встроенный контроль:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (312, 3, 8, 2, NULL, N'    Смес 1', N'дел', N'25 - 60', 269)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (313, 3, 8, 3, NULL, N'    Смес 2', N'дел', N'25 - 60', 270)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (314, 3, 8, 4, NULL, N'    Гетер', N'дел', N'20 - 70', 271)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (315, 3, 8, 5, NULL, N'    Вых. УНЧ', N'дел', N'25 - 95', 272)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (316, 3, 8, 6, NULL, N'    Клистр', N'дел', N'40 - 80 или зашкал', 273)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (317, 3, 8, 7, NULL, N'    задача 2:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (318, 3, 8, 8, NULL, N'        - скорость', N'км/ч', N'p2 ± 16', 274)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (319, 3, 8, 9, NULL, N'        - угол сноса', N'град', N'p2 ± 1,5', 275)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (320, 3, 8, 10, NULL, N'        - скорость на море', N'%', N'увелич. на 1,3±0,2', 276)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (321, 3, 8, 11, NULL, N'    задача 1:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (322, 3, 8, 12, NULL, N'        - скорость', N'км/ч', N'p1 ± 12,1', 278)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (323, 3, 8, 13, NULL, N'        - угол сноса', N'град', N'0 ± 1,5', 279)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (324, 3, 8, 14, NULL, N'    задача 3:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (325, 3, 8, 15, NULL, N'        - скорость', N'км/ч', N'320,5 ± 5', 280)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (326, 3, 8, 16, NULL, N'        - угол сноса', N'град', N'0 ± 1,5', 281)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (327, 3, 8, 17, NULL, N'    состояние табло П', N'—', N'горит', 282)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (328, 3, 7, 1, 7, N'7. Прогон измерителя:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (329, 3, 7, 2, NULL, N'    исправность Смес1', N'—', N'исправно', 284)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (330, 3, 7, 3, NULL, N'    исправность Смес2', N'—', N'исправно', 285)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (331, 3, 7, 4, NULL, N'    15-ое включение:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (332, 3, 7, 5, NULL, N'        - скорость', N'км/ч', N'996 ± 25', 286)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (333, 3, 7, 6, NULL, N'        - угол сноса', N'град', N'-20 ± 2', 289)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (334, 3, 7, 7, NULL, N'    30-ое включение:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (335, 3, 7, 8, NULL, N'        - скорость', N'км/ч', N'996 ± 25', 287)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (336, 3, 7, 9, NULL, N'        - угол сноса', N'град', N'-20 ± 2', 290)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (337, 3, 7, 10, NULL, N'    50-е включение:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (338, 3, 7, 11, NULL, N'        - скорость', N'км/ч', N'996 ± 25', 288)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (339, 3, 7, 12, NULL, N'        - угол сноса', N'град', N'-20 ± 2', 291)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (340, 2, 12, 8, NULL, N'     УС ПС №2:    ПС1 - 127', N'км/ч', N'127 ± 2,5', 332)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (341, 2, 12, 9, NULL, N'                        ПС2 - 258', N'км/ч', N'258 ± 3,5', 328)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (342, 2, 13, 8, NULL, N'     УС ПС №2:    УС 0', N'град', N'0 ± 1,0', 333)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (343, 2, 13, 9, NULL, N'                        УС 9,5 (ВЛЕВО)', N'град', N'9,5 ± 1,0', 329)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (344, 2, 14, 2, NULL, N'УС ПС №1:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (345, 2, 14, 5, NULL, N'УС ПС №2:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (346, 2, 14, 6, NULL, N'     ПС', N'км/ч', N'0 ± 3', 322)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (347, 2, 14, 7, NULL, N'     УС', N'град', N'0 ± 3', 323)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (351, 2, 15, 6, NULL, N'     М - С', N'%', N'2 ± 0,2', 326)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (352, 2, 15, 7, NULL, N'     М - Б', N'%', N'1 ± 0,2', 327)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (353, 2, 15, 2, NULL, N'УС ПС №1:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (354, 2, 15, 5, NULL, N'УС ПС №2:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (355, 2, 12, 4, NULL, N'     КПК-2 (ПС УС №2):    ПС1 - 127', N'км/ч', N'127 ± 2,0', 319)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (356, 2, 12, 5, NULL, N'                     ПС2 - 258', N'км/ч', N'258 ± 3,0', 324)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (357, 2, 13, 4, NULL, N'     КПК-2 (УС ПС №2):    УС 0', N'град', N'0 ± 0,7', 320)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (358, 2, 13, 5, NULL, N'                     УС 9,5 (ВЛЕВО)', N'град', N'9,5 ± 0,7', 325)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (359, 2, 18, 1, 18, N'Точность вычисления и индикации пройденного пути и бокового уклонения (показания счетчиков блока 8 Географического):', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (360, 2, 18, 2, NULL, N'     пройденный путь ВПЕРЕД', N'км', N'63,5 ± 1,5', 330)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (361, 2, 18, 3, NULL, N'     боковое уклонение ВЛЕВО', N'км', N'10,6 ± 1,1', 331)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (362, 2, 25, 6, NULL, N'     ГРАНИЦА КАРТЫ', NULL, N'Соотв. РЭ', 321)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (363, 1, 28, 1, 28, N'Связь аппаратуры ДИСС с изделием "Аист"', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (364, 1, 28, 2, 0, N' КЗ-4:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (365, 1, 28, 3, NULL, N'  Напряжение, пропорциональное α', N'В', N'0 ± 0,5', 336)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (366, 1, 28, 4, NULL, N'  Напряжение, пропорциональное W', N'В', N'-11,1 ± 1,5', 337)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (367, 1, 28, 5, 0, N' КЗ-6:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (368, 1, 28, 6, NULL, N'  Напряжение, пропорциональное α', N'В', N'-6,4 ± 1', 339)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (369, 1, 28, 7, NULL, N'  Напряжение, пропорциональное W', N'В', N'-17,6 ± 2,5', 340)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (370, 1, 28, 8, 0, N' КЗ-5: Напряжение, пропорциональное α', N'В', N'6,4 ± 1', 338)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (371, 1, 29, 1, 29, N'Связь аппаратуры ДИСС с изделием САУ', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (372, 1, 29, 2, 0, N' КЗ-6: Напряжение, пропорциональное α', N'В', N'-0,75 ± 0,3', 344)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (373, 1, 29, 3, 0, N' КЗ-5: Напряжение, пропорциональное α', N'В', N'0,75 ± 0,3', 346)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (374, 1, 29, 4, 0, N' КЗ-1:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (375, 1, 29, 5, NULL, N'  Напряжение, пропорциональное Wхг', N'В', N'не более -5', 347)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (376, 1, 29, 6, NULL, N'  Напряжение, пропорциональное Wуг', N'В', N'6,5 ± 2', 348)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (377, 1, 29, 7, 0, N' КЗ-3:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (378, 1, 29, 8, NULL, N'  Напряжение, пропорциональное Wxг', N'В', N'не менее 5', 354)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (379, 1, 29, 9, NULL, N'  Напряжение, пропорциональное Wyг', N'В', N'-6,5 ± 2', 355)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (380, 1, 29, 10, NULL, N'  Напряжение, пропорциональное Wzг', N'В', N'0 ± 1', 374)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (381, 1, 29, 11, 0, N' КЗ-2: Напряжение, пропорциональное Wzг', N'В', N'не более -8', 375)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (382, 1, 30, 1, 30, N'Связь аппаратуры ДИСС с изделием СУ', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (383, 1, 30, 4, NULL, N'  Напряжение, пропорциональное Wхг', N'В', N'-0,116 ± 0,02', 356)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (384, 1, 30, 4, 0, N' КЗ-2: Напряжение, пропорциональное Wzг', N'В', N'0,145 ± 0,02', 357)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (385, 1, 30, 3, NULL, N'  Напряжение, пропорциональное Wуг(х)', N'В', N'-3,309 ± 0,25', 358)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (387, 1, 31, 1, 31, N' Связь аппаратуры ДИСС с изделием "Ландыш"', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (388, 1, 31, 2, 0, N' КЗ-2:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (389, 1, 31, 3, NULL, N'  Напряжение, пропорциональное Wхг', N'В', N'-0,65 ± 0,2', 365)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (390, 1, 31, 4, NULL, N'  Напряжение, пропорциональное Wzг', N'В', N'0,65 ± 0,2', 366)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (391, 1, 31, 5, 0, N' КЗ-4:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (392, 1, 31, 6, NULL, N'  Напряжение, пропорциональное Wхг', N'В', N'5,5 ± 1', 367)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (393, 1, 31, 7, NULL, N'  Напряжение, пропорциональное Wzг', N'В', N'0 ± 0,1', 368)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (394, 1, 31, 8, 0, N' КЗ-5: Напряжение, пропорциональное Wzг', N'В', N'2,4 ± 0,5', 369)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (395, 1, 31, 9, 0, N' КЗ-6:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (396, 1, 31, 10, NULL, N'  Напряжение, пропорциональное W=215 км/ч', N'В', N'8 ± 1,5', 370)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (397, 1, 31, 11, NULL, N'  Напряжение, пропорциональное Wzг', N'В', N'-2,4 ± 0,5', 371)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (398, 1, 32, 1, 32, N'Связь аппаратуры ДИСС с датчиком ДВС-24', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (399, 1, 32, 2, 0, N' Блок 8:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (400, 1, 32, 3, NULL, N'  пройденный путь ВПЕРЁД', N'км', N'30 ± 5', 360)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (401, 1, 32, 4, NULL, N'  пройденный путь ВБОК', N'км', N'0 ± 1', 361)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (402, 1, 32, 5, 0, N' Блок 10:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (403, 1, 32, 6, NULL, N'  смещение визира ВВЕРХ', N'мм', N'30 ± 7,5', 362)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (404, 1, 32, 7, NULL, N'  смещение визира ВБОК', N'мм', N'0 ± 1,5', 363)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (405, 1, 32, 8, 0, N' Загорание табло ГРАНИЦА КАРТЫ', N'—', N'должно загореться', 364)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (408, 2, 19, 1, 19, N'Положение центра визира Блока 10', NULL, N'В зоне площадки 2', 376)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (409, 4, 1, 1, 1, N'Питающие напряжения:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (410, 4, 1, 2, NULL, N' постоянного тока', N'В', N'27 ± 2,7', 2)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (411, 4, 1, 3, NULL, N' переменного тока', N'В', N'115 ± 6', 1)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (412, 4, 2, 1, 2, N'Напряжение вторичных источников питания:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (413, 4, 2, 2, NULL, N' +12,6 B', N'В', N'12,6 ± 0,65', 4)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (414, 4, 2, 3, NULL, N' -12,6 B', N'В', N'-12,6 ± 0,65', 7)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (415, 4, 2, 4, NULL, N' +6,3 B', N'В', N'6,3 ± 0,3', 8)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (416, 4, 2, 5, NULL, N' -6,3 B', N'В', N'-6,3 ± 0,3', 10)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (417, 4, 2, 6, NULL, N' -20 B', N'В', N'-20 ± 1', 11)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (420, 4, 2, 9, NULL, N' +5 B', N'В', N'5 ± 0,25', 433)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (422, 4, 4, 1, 4, N'Контрольное напряжение генератора ПМ:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (423, 4, 4, 2, NULL, N' ПМ1', N'В', N'от 2,4 до 3,5', 18)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (424, 4, 4, 3, NULL, N' ПМ2', N'В', N'от 2,4 до 3,5', 19)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (425, 4, 5, 1, 5, N'Контрольное напряжение, пропорциональное общему току ПМ', N'В', N'от -1,7 до -2,9', 20)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (426, 4, 6, 1, 6, N'Частота СВЧ колебаний', N'МГц', N'13325 ± 7,5', 383)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (427, 4, 7, 1, 7, N'Напряжение смещения на модуляторных диодах:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (428, 4, 7, 2, NULL, N' МОД 1 (+)', N'В', N'от +2,7 до +4,5', 381)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (429, 4, 7, 3, NULL, N' МОД 2 (-)', N'B', N'от -2,7 до -4,5', 382)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (430, 4, 8, 1, 8, N'Разность напряжений смещения ТК:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (431, 4, 8, 2, NULL, N' Канал 1-1', N'мВ', N'90 ± 70', 192)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (432, 4, 8, 3, NULL, N' Канал 2-1', N'мВ', N'90 ± 70', 193)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (433, 4, 8, 4, NULL, N' Канал 1-2', N'мВ', N'90 ± 70', 194)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (434, 4, 8, 5, NULL, N' Канал 2-2', N'мВ', N'90 ± 70', 195)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (435, 4, 8, 6, NULL, N' Канал 1-3', N'мВ', N'90 ± 70', 196)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (436, 4, 8, 7, NULL, N' Канал 2-3', N'мВ', N'90 ± 70', 197)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (437, 4, 8, 8, 0, N' разброс в паре:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (438, 4, 8, 9, NULL, N'  Канал 1', N'мВ', N'не более 45', 198)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (439, 4, 8, 10, NULL, N'  Канал 2', N'мВ', N'не более 45', 199)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (440, 4, 8, 11, NULL, N'  Канал 3', N'мВ', N'не более 45', 200)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (441, 4, 9, 1, 9, N'Напряжение смещения на СД:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (442, 4, 9, 2, NULL, N' Канал 1-1', N'В', N'от 0,6 до 1,8', 32)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (443, 4, 9, 3, NULL, N' Канал 2-1', N'В', N'от 0,6 до 1,8', 33)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (444, 4, 9, 4, NULL, N' Канал 1-2', N'В', N'от 0,6 до 1,8', 34)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (445, 4, 9, 5, NULL, N' Канал 2-2', N'В', N'от 0,6 до 1,8', 35)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (446, 4, 9, 6, NULL, N' Канал 1-3', N'В', N'от 0,6 до 1,8', 36)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (447, 4, 9, 7, NULL, N' Канал 2-3', N'В', N'от 0,6 до 1,8', 37)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (448, 4, 10, 1, 10, N'Напряжение шумов на выходах УНЧ:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (449, 4, 10, 2, NULL, N' Канал 1-1', N'мВ', N'от 30 до 100', 38)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (450, 4, 10, 3, NULL, N' Канал 2-1', N'мВ', N'от 30 до 100', 39)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (451, 4, 10, 4, NULL, N' Канал 1-2', N'мВ', N'от 30 до 100', 40)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (452, 4, 10, 5, NULL, N' Канал 2-2', N'мВ', N'от 30 до 100', 41)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (453, 4, 10, 6, NULL, N' Канал 1-3', N'мВ', N'от 30 до 100', 42)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (454, 4, 10, 7, NULL, N' Канал 2-3', N'мВ', N'от 30 до 100', 43)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (455, 4, 11, 1, 11, N'Режим "Память"', N'—', N'Соотв. НТП', 377)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (456, 4, 12, 1, 12, N'Режим "АС"', N'—', N'Соотв. НТП', 378)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (457, 4, 13, 1, 13, N'Режим "Контр. БС"', N'—', N'Соотв. НТП', 379)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (458, 4, 14, 1, 14, N'Режим "Контр. ВЧ"', N'—', N'Соотв. НТП', 380)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (459, 1, 33, 1, 33, N'Тренировка магнетрона', N'—', N'—', 384)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (460, 5, 1, 1, 1, N'Питающие напряжения:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (461, 5, 1, 2, NULL, N'     постоянного тока', N'В', N'27 ± 2,7', 2)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (462, 5, 1, 3, NULL, N'     переменного тока', N'В', N'115 ± 6', 1)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (463, 5, 11, 1, 11, N'Точность индикации составляющих скорости блоком 6 в режиме контрольных задач:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (464, 5, 11, 10, NULL, N'          ВПЕРЕД - 17 (Wxг)', N'км/ч', N'17 ± 2,5', 177)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (465, 5, 11, 11, NULL, N'          ВЛЕВО - 17 (Wzг)', N'км/ч', N'17 ± 2,5', 178)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (466, 5, 11, 12, NULL, N'          ВНИЗ - 3,1 (Wyг)', N'м/с', N'3,1 ± 0,5', 179)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (467, 5, 11, 13, NULL, N'          НАЗАД - 17 (Wxг)', N'км/ч', N'17 ± 2,5', 180)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (468, 5, 11, 14, NULL, N'          ВПРАВО - 17 (Wzг)', N'км/ч', N'17 ± 2,5', 181)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (469, 5, 11, 15, NULL, N'          ВВЕРХ - 3,1 (Wyг)', N'м/с', N'3,1 ± 0,5', 182)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (470, 5, 12, 1, 12, N'Точность вычисления и индикации путевой скорости:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (471, 5, 12, 2, NULL, N'     КПК-2 (УС ПС №1):    ПС1 - 127', N'км/ч', N'127 ± 2,0', 62)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (472, 5, 12, 3, NULL, N'                     ПС2 - 258', N'км/ч', N'258 ± 3,0', 65)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (473, 5, 12, 6, NULL, N'     УС ПС  №1:    ПС1 - 127', N'км/ч', N'127 ± 2,5', 183)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (474, 5, 12, 7, NULL, N'                        ПС2 - 258', N'км/ч', N'258 ± 3,5', 187)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (475, 5, 13, 1, 13, N'Точность вычисления и индикации угла сноса:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (476, 5, 13, 2, NULL, N'     КПК-2 (УС ПС  №1):    УС 0', N'град', N'0 ± 0,7', 64)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (477, 5, 13, 3, NULL, N'                     УС 9,5 (ВЛЕВО)', N'град', N'9,5 ± 0,7', 66)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (478, 5, 13, 6, NULL, N'     УС ПС  №1:    УС 0', N'град', N'0 ± 1,0', 184)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (479, 5, 13, 7, NULL, N'                        УС 9,5 (ВЛЕВО)', N'град', N'9,5 ± 1,0', 188)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (480, 5, 15, 1, 15, N'Увеличение путевой скорости в режиме "Море":', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (481, 5, 15, 3, NULL, N'     М - С', N'%', N'2 ± 0,2', 190)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (482, 5, 15, 4, NULL, N'     М - Б', N'%', N'1 ± 0,2', 191)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (483, 5, 14, 1, 14, N'Работа аппаратуры в режиме "Память":', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (484, 5, 14, 3, NULL, N'     ПС', N'км/ч', N'0 ± 3', 204)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (485, 5, 14, 4, NULL, N'     УС', N'град', N'0 ± 3', 205)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (486, 5, 17, 1, 17, N'Связь аппаратуры по углам (величины изменения продольной, поперечной и вертикальной составляющих вектора скорости) на значениях:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (487, 5, 17, 2, 0, N'     Θ (тангаж) = 12°', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (488, 5, 17, 3, NULL, N'          Wxг', N'км/ч', N'увелич.', 231)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (489, 5, 17, 4, NULL, N'          Wzг', N'км/ч', N'без измен.', 234)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (490, 5, 17, 5, NULL, N'          Wyг', N'м/с', N'уменьш.', 238)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (491, 5, 17, 6, 0, N'     Θ (тангаж) = -12°', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (492, 5, 17, 7, NULL, N'          Wxг', N'км/ч', N'уменьш.', 316)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (493, 5, 17, 8, NULL, N'          Wzг', N'км/ч', N'без измен.', 235)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (494, 5, 17, 9, NULL, N'          Wyг', N'м/с', N'увелич.', 239)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (495, 5, 17, 10, 0, N'     γ (крен) = 12°', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (496, 5, 17, 11, NULL, N'          Wxг', N'км/ч', N'без измен.', 229)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (497, 5, 17, 12, NULL, N'          Wzг', N'км/ч', N'увелич.', 232)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (498, 5, 17, 13, NULL, N'          Wyг', N'м/с', N'уменьш.', 236)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (499, 5, 17, 14, 0, N'     γ (крен) = -12°', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (500, 5, 17, 15, NULL, N'          Wxг', N'км/ч', N'без измен.', 230)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (501, 5, 17, 16, NULL, N'          Wzг', N'км/ч', N'уменьш.', 233)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (502, 5, 17, 17, NULL, N'          Wyг', N'м/с', N'увелич.', 237)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (503, 5, 16, 1, 16, N'Точность вычисления и индикации выходных параметров при проверке от БПК:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (504, 5, 16, 2, 0, N'     Вперед 17, Влево 17, Вниз 3', N'—', N'по блоку 6', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (505, 5, 16, 3, NULL, N'          Wxг', N'км/ч', N'17 ± 2,5', 217)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (506, 5, 16, 4, NULL, N'          Wzг', N'км/ч', N'17 ± 2,5', 218)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (507, 5, 16, 5, NULL, N'          Wyг', N'м/с', N'3,1 ± 0,5', 219)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (508, 5, 16, 6, 0, N'     Назад 17, Вправо 17, Вверх 3', N'—', N'по блоку 6', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (509, 5, 16, 7, NULL, N'          Wxг', N'км/ч', N'17 ± 2,5', 220)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (510, 5, 16, 8, NULL, N'          Wzг', N'км/ч', N'17 ± 2,5', 221)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (511, 5, 16, 9, NULL, N'          Wyг', N'м/с', N'3,1 ± 0,5', 222)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (512, 5, 16, 10, 0, N'     Скорость 127, Снос 0', N'—', N'по инд. УС ПС', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (513, 5, 2, 1, 2, N'Напряжение вторичных источников питания:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (514, 5, 2, 2, NULL, N'     12,6 B', N'В', N'12,6 ± 0,65', 4)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (515, 5, 2, 3, NULL, N'     -12,6 B', N'В', N'-12,6 ± 0,65', 7)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (516, 5, 2, 4, NULL, N'     6,3 B', N'В', N'6,3 ± 0,3', 8)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (517, 5, 2, 5, NULL, N'     -6,3 B', N'В', N'-6,3 ± 0,3', 10)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (518, 5, 2, 6, NULL, N'     -20 B', N'В', N'-20 ± 1', 11)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (519, 5, 2, 7, NULL, N'     15 В след.', N'В', N'15 ± 0,75', 12)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (520, 5, 2, 8, NULL, N'     -15 В опорн.', N'В', N'-15 ± 0,75', 13)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (521, 5, 2, 9, NULL, N'     5 B', N'В', N'5 ± 0,25', 15)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (522, 5, 2, 10, NULL, N'     -27 B', N'В', N'-27 ± 1,35', 16)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (523, 5, 3, 1, 3, N'Разность модулей вторичных напряжений    Δ15 В', N'мВ', N'от -35 до +35', 14)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (524, 5, 4, 1, 4, N'Контрольное напряжение генератора ГЛПД:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (525, 5, 4, 2, NULL, N'     ГЛПД 1', N'В', N'от 2,4 до 3,5', 18)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (526, 5, 4, 3, NULL, N'     ГЛПД 2', N'В', N'от 2,4 до 3,5', 19)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (527, 5, 5, 1, 5, N'Контрольное напряжение, пропорциональное общему току генераторных диодов', N'В', N'от -1,7 до -2,9', 20)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (528, 5, 6, 1, 6, N'Частота СВЧ колебаний', N'МГц', N'13325 ± 9', 317)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (529, 5, 7, 1, 7, N'Напряжение смещения на модуляторных диодах:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (530, 5, 7, 2, NULL, N'     МОД 1', N'В', N'от +2,7 до +4,5', 389)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (531, 5, 7, 3, NULL, N'     МОД 2', N'B', N'от -2,7 до -4,5', 390)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (532, 5, 8, 1, 8, N'Разность напряжений смещения ТК:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (533, 5, 8, 2, NULL, N'          Канал I-1', N'мВ', N'90 ± 70', 192)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (534, 5, 8, 3, NULL, N'          Канал I-2', N'мВ', N'90 ± 70', 193)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (535, 5, 8, 4, NULL, N'          Канал II-1', N'мВ', N'90 ± 70', 194)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (536, 5, 8, 5, NULL, N'          Канал II-2', N'мВ', N'90 ± 70', 195)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (537, 5, 8, 6, NULL, N'          Канал III-1', N'мВ', N'90 ± 70', 196)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (538, 5, 8, 7, NULL, N'          Канал III-2', N'мВ', N'90 ± 70', 197)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (539, 5, 8, 8, 0, N'     разброс в паре:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (540, 5, 8, 9, NULL, N'          Канал I', N'мВ', N'не более 45', 198)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (541, 5, 8, 10, NULL, N'          Канал II', N'мВ', N'не более 45', 199)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (542, 5, 8, 11, NULL, N'          Канал III', N'мВ', N'не более 45', 200)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (543, 5, 9, 1, 9, N'Напряжение смещения на СД:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (544, 5, 9, 2, NULL, N'     Канал I-1', N'В', N'от 0,6 до 1,8', 32)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (545, 5, 9, 3, NULL, N'     Канал I-2', N'В', N'от 0,6 до 1,8', 33)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (546, 5, 9, 4, NULL, N'     Канал II-1', N'В', N'от 0,6 до 1,8', 34)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (547, 5, 9, 5, NULL, N'     Канал II-2', N'В', N'от 0,6 до 1,8', 35)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (548, 5, 9, 6, NULL, N'     Канал III-1', N'В', N'от 0,6 до 1,8', 36)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (549, 5, 9, 7, NULL, N'     Канал III-2', N'В', N'от 0,6 до 1,8', 37)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (550, 5, 10, 1, 10, N'Напряжение шумов на выходах УНЧ:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (551, 5, 10, 2, NULL, N'     Канал I-1', N'мВ', N'от 30 до 100', 38)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (552, 5, 10, 3, NULL, N'     Канал I-2', N'мВ', N'от 30 до 100', 39)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (553, 5, 10, 4, NULL, N'     Канал II-1', N'мВ', N'от 30 до 100', 40)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (554, 5, 10, 5, NULL, N'     Канал II-2', N'мВ', N'от 30 до 100', 41)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (555, 5, 10, 6, NULL, N'     Канал III-1', N'мВ', N'от 30 до 100', 42)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (556, 5, 10, 7, NULL, N'     Канал III-2', N'мВ', N'от 30 до 100', 43)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (557, 5, 18, 1, 18, N'Выходные напряжения блока ВСС при обнулении составляющих вектора скорости:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (558, 5, 18, 2, NULL, N'     Wxг НКВ', N'мВ', N'не более ±40', 391)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (559, 5, 18, 3, NULL, N'     Wzг НКВ', N'мВ', N'не более ±80', 392)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (560, 5, 18, 4, NULL, N'     Wyг НКВ', N'мВ', N'не более ±300', 393)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (562, 5, 16, 11, NULL, N'         ПС', N'км/ч', N'127 ± 2,5', 223)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (563, 5, 16, 12, NULL, N'         УС', N'град', N'0 ± 1', 224)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (564, 5, 16, 13, 0, N'     Скорость 258, Снос 9,5 (ВЛЕВО)', N'—', N'по инд. УС ПС', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (565, 5, 16, 14, NULL, N'         ПС', N'км/ч', N'258 ± 3,5', 225)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (566, 5, 16, 15, NULL, N'         УС', N'град', N'9,5 ± 1', 226)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (567, 5, 20, 1, 20, N'Работа световой сигнализации изделия:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (568, 5, 20, 2, NULL, N'     КОНТРОЛЬ ДИСС', NULL, N'Соотв. РЭ', 211)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (569, 5, 20, 3, NULL, N'     ПОИСК', NULL, N'Соотв. РЭ', 208)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (570, 5, 20, 4, NULL, N'     ИСПРАВНОСТЬ', NULL, N'Соотв. РЭ', 212)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (571, 5, 20, 5, NULL, N'     ПОЛЕТ', NULL, N'Соотв. РЭ', 210)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (575, 5, 20, 6, NULL, N'     ОТКАЗ ВЧ', NULL, N'Соотв. РЭ', 206)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (576, 5, 20, 7, NULL, N'     ОТКАЗ ВСС ', NULL, N'Соотв. РЭ', 207)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (577, 5, 20, 8, NULL, N'     Табло П, Выкл.(на УСПС)', NULL, N'Соотв. РЭ', 209)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (586, 5, 11, 2, NULL, N'     КПК-2:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (587, 5, 11, 3, NULL, N'          ВПЕРЕД - 17 (Wxг)', N'км/ч', N'17 ± 2,5', 58)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (588, 5, 11, 4, NULL, N'          ВЛЕВО - 17 (Wzг)', N'км/ч', N'17 ± 2,5', 60)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (589, 5, 11, 5, NULL, N'          ВНИЗ - 3,1 (Wyг)', N'м/c', N'3,1 ± 0,5', 59)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (590, 5, 11, 6, NULL, N'          НАЗАД - 17 (Wxг)', N'км/ч', N'17 ± 2,5', 201)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (591, 5, 11, 7, NULL, N'          ВПРАВО - 17 (Wzг)', N'км/ч', N'17 ± 2,5', 203)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (592, 5, 11, 8, NULL, N'          ВВЕРХ - 3,1 (Wyг)', N'м/c', N'3,1 ± 0,5', 202)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (593, 5, 11, 9, NULL, N'     блок 6:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (599, 5, 14, 2, NULL, N'УС ПС  №1:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (605, 5, 15, 2, NULL, N'УС ПС  №1:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (619, 5, 18, 5, NULL, N'     Wxг ПКВ', N'мВ', N'не более ±330', 396)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (620, 5, 18, 6, NULL, N'     Wzг ПКВ', N'мВ', N'не более ±230', 398)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (621, 5, 18, 7, NULL, N'     Wyг ПКВ', N'мВ', N'не более ±300', 397)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (622, 5, 21, 9, 0, N'Визуальный контроль работы режима "ЗАПОМИНАНИЕ"', NULL, N'Соотв. РЭ', 423)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (623, 5, 19, 1, 19, N'Выдача Wx, Wy, Wz в бортовые системы:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (624, 5, 19, 2, NULL, N' при задаче 1:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (625, 5, 19, 3, NULL, N'  Wxг НКВ', N'В', N'0,47 ± 0,06', 399)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (626, 5, 19, 4, NULL, N'  Wzг НКВ', N'В', N'1,57 ± 0,20', 401)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (627, 5, 19, 5, NULL, N'  Wyг НКВ', N'В', N'-3,1 ± 0,6', 400)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (628, 5, 19, 6, NULL, N'  Wxг ПКВ', N'В', N'-4,71 ± 0,60', 402)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (629, 5, 19, 7, NULL, N'  Wzг ПКВ', N'В', N'-4,71 ± 0,60', 404)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (630, 5, 19, 8, NULL, N'  Wyг ПКВ', N'В', N'-3,1 ± 0,6', 403)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (631, 5, 19, 9, NULL, N'  Wxг СПСВ', N'В', N'-0,52 ± 0,07', 405)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (632, 5, 19, 10, NULL, N'  Wzг СПСВ', N'В', N'-0,52 ± 0,07', 407)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (633, 5, 19, 11, NULL, N'  Wyг СПСВ', N'В', N'2,65 ± 0,60', 406)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (634, 5, 19, 12, NULL, N'  Wxг Тестер', N'В', N'0,8 ± 0,2', 408)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (635, 5, 19, 13, NULL, N'  Wzг Тестер', N'В', N'3,47 ± 0,20', 410)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (636, 5, 19, 14, NULL, N'  Wyг Тестер', N'В', N'2,07 ± 0,40', 409)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (637, 5, 19, 15, NULL, N' при задаче 2:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (638, 5, 19, 16, NULL, N'  Wxг НКВ', N'В', N'-0,47 ± 0,06', 411)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (639, 5, 19, 17, NULL, N'  Wzг НКВ', N'В', N'-1,57 ± 0,20', 413)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (640, 5, 19, 18, NULL, N'  Wyг НКВ', N'В', N'3,1 ± 0,6', 412)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (641, 5, 19, 19, NULL, N'  Wxг ПКВ', N'В', N'4,71 ± 0,60', 414)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (642, 5, 19, 20, NULL, N'  Wzг ПКВ', N'В', N'4,71 ± 0,60', 416)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (643, 5, 19, 21, NULL, N'  Wyг ПКВ', N'В', N'3,1 ± 0,6', 415)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (644, 5, 19, 22, NULL, N'  Wxг СПСВ', N'В', N'0,52 ± 0,07', 417)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (645, 5, 19, 23, NULL, N'  Wzг СПСВ', N'В', N'0,52 ± 0,07', 419)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (646, 5, 19, 24, NULL, N'  Wyг СПСВ', N'В', N'-2,65 ± 0,60', 418)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (647, 5, 19, 25, NULL, N'  Wxг Тестер', N'В', N'0,29 ± 0,20', 420)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (648, 5, 19, 26, NULL, N'  Wzг Тестер', N'В', N'2,53 ± 0,20', 422)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (649, 5, 19, 27, NULL, N'  Wyг Тестер', N'В', N'3,93 ± 0,40', 421)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (650, 3, 4, 1, 4, N'4. Стабильность выходных напряжений', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (651, 3, 4, 2, NULL, N'    при 485В:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (652, 3, 4, 3, NULL, N'        - при повышенном напряжении питания', N'В', N'485 ± 4', 424)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (653, 3, 4, 4, NULL, N'        - при пониженном напряжении питания', N'В', N'485 ± 4', 425)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (657, 3, 5, 1, 5, N'5. Параметры выходных импульсов и токов', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (659, 1, 30, 2, 0, N' КЗ-1:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (660, 1, 30, 5, 0, N' КЗ-4: Напряжение, пропорциональное Wyг', N'В', N'0 ± 0,1', 359)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (661, 1, 29, 12, 0, N' Работоспособность сигнала "Исправность"', N'—', N'соответствует', 432)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (662, 6, 1, 1, 1, N'Питающее напряжение постоянного тока', N'В', N'27 ± 2,7', 2)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (663, 6, 2, 1, 2, N'Сила постоянного тока', N'А', N'Не более 5', 435)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (664, 6, 3, 1, 3, N'Напряжение вторичных источников питания:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (665, 6, 3, 2, NULL, N'     +12 B', N'В', N'12 ± 0,6', 436)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (666, 6, 3, 3, NULL, N'     -12 B', N'В', N'-12 ± 0,6', 437)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (667, 6, 3, 6, NULL, N'     +9 B', N'В', N'9 ± 0,45', 440)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (668, 6, 3, 4, NULL, N'     +15 В', N'В', N'15 ± 0,75', 12)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (669, 6, 3, 5, NULL, N'     -15 В', N'В', N'-15 ± 0,75', 13)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (670, 6, 3, 7, NULL, N'     +5 B', N'В', N'5 ± 0,25', 452)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (671, 6, 6, 1, 6, N'Напряжение шумов на выходах УНЧ:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (672, 6, 6, 2, NULL, N'     Канал 1 - Выход УНЧ 1', N'В', N'от 1 до 3', 442)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (673, 6, 6, 3, NULL, N'     Канал 1 - Выход УНЧ 2', N'В', N'от 1 до 3', 443)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (674, 6, 6, 4, NULL, N'     Канал 2 - Выход УНЧ 1', N'В', N'от 1 до 3', 444)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (675, 6, 6, 5, NULL, N'     Канал 2 - Выход УНЧ 2', N'В', N'от 1 до 3', 445)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (676, 6, 6, 6, NULL, N'     Канал 3 - Выход УНЧ 1', N'В', N'от 1 до 3', 446)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (677, 6, 6, 7, NULL, N'     Канал 3 - Выход УНЧ 2', N'В', N'от 1 до 3', 447)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (678, 6, 4, 1, 4, N'Частота СВЧ колебаний:', N'—', N'—', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (679, 6, 7, 1, 7, N'Режим "АС"', N'—', N'Соотв. НТП', 378)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (680, 6, 8, 1, 8, N'Режим "Контр. БС"', N'—', N'Соотв. НТП', 379)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (681, 6, 9, 1, 9, N'Режим "Контр. ВЧ"', N'—', N'Соотв. НТП', 380)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (682, 6, 4, 2, NULL, N'    Канал 1,3', N'МГц', N'13320 ± 3', 438)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (683, 6, 5, 1, 5, N'Проверка индикации', N'—', N'Соотв. НТП', 441)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (684, 6, 4, 3, NULL, N'    Канал 2', N'МГц', N'13320 ± 3', 439)


INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (685, 2, 16, 13, NULL, N'         ПС №2', N'км/ч', N'127 ± 2,5', 455)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (686, 2, 16, 14, NULL, N'         УС №2', N'град', N'0 ± 1', 456)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (687, 2, 16, 18, NULL, N'         ПС №2', N'км/ч', N'258 ± 3,5', 457)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (688, 2, 16, 19, NULL, N'         УС №2', N'град', N'9.5 ± 1', 458)

GO
SET IDENTITY_INSERT [dbo].[Report] OFF
GO
SET IDENTITY_INSERT [dbo].[TablesToTranslate] ON 

GO
INSERT [dbo].[TablesToTranslate] ([TableId], [TableName], [IdColumn], [TranslateColumn]) VALUES (1, N'Report', N'Id', N'CheckName')
GO
INSERT [dbo].[TablesToTranslate] ([TableId], [TableName], [IdColumn], [TranslateColumn]) VALUES (2, N'Report', N'Id', N'Requirement')
GO
INSERT [dbo].[TablesToTranslate] ([TableId], [TableName], [IdColumn], [TranslateColumn]) VALUES (3, N'PassportValuesReference', N'ParamId', N'Name')
GO
SET IDENTITY_INSERT [dbo].[TablesToTranslate] OFF
GO
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 1, N'Power voltages:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 2, N'     direct current')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 3, N'     alternating current  ')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 4, N'Currents consumed in power circuits:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 5, N'     = 27.5 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 6, N'     ~ 115 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 7, N'Voltages of secondary power sources:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 8, N'     12,6 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 9, N'     -12,6 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 10, N'     -6,3 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 11, N'     -20 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 12, N'     -27 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 13, N'     -27 V ref.')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 14, N'     27 V ts.')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 15, N'Difference in absolute values of voltages -27V st.ref. and +27Vst. ts.')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 16, N'Delay time of high voltage at the magnetron')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 17, N'Voltage at magnetron')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 18, N'Current of magnetron')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 19, N'Currents of crystal mixers:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 20, N'          Channel I - 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 21, N'          Channel I - 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 22, N'          Channel II - 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 23, N'          Channel II - 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 24, N'          Channel III - 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 25, N'          Channel III - 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 26, N'     scatter in pair:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 27, N'          Channel I')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 28, N'          Channel II')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 29, N'          Channel III')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 30, N'Reference power of magnetron')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 31, N'Frequency of magnetron')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 32, N'Power of magnetron')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 33, N'Noise voltage and level of signal voltages at LFA outputs:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 34, N'     Noise, Band I (allowable oscillations       ±20 mV):')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 35, N'          Channel I - 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 36, N'          Channel I - 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 37, N'          Channel II - 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 38, N'          Channel II - 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 39, N'          Channel III - 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 40, N'          Channel III - 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 41, N'     Noise, Band II (allowable oscillations       ±20 mV):')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 42, N'          Channel I - 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 43, N'          Channel I - 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 44, N'          Channel II - 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 45, N'          Channel II - 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 46, N'          Channel III - 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 47, N'          Channel III - 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 48, N'     Voltage of signals')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 49, N'          Channel I - 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 50, N'          Channel I - 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 51, N'          Channel II - 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 52, N'          Channel II - 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 53, N'          Channel III - 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 54, N'          Channel III - 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 55, N'Values of nulled velocity components:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 59, N'Accuracy of calculation and indication of velocity vector components (allowed oscillations ±0.5 km/h for Wxг and Wzг;  ±0.05 m/s for Wyг): ')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 60, N'     КПК-2:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 61, N'          FORWARD (Wxг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 62, N'          BACKWARD (Wxг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 63, N'          RIGHT (Wzг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 64, N'          UP (Wyг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 65, N'          DOWN (Wyг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 66, N'     unit 6:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 67, N'          FORWARD (Wxг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 68, N'          BACK (Wxг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 69, N'          RIGHT (Wzг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 70, N'          UP (Wyг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 71, N'          DOWN (Wyг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 72, N'Unit 6 response rate for velocity vector components:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 76, N'Accuracy of calculation and indication of drift angle (allowed oscillations  ±30΄):')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 77, N'     КПК-2:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 78, N'          DA 0')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 79, N'          DA +18')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 80, N'          DA -18')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 81, N'     unit 7:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 82, N'          DA 0')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 83, N'          DA +18')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 84, N'          DA -18')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 85, N'Drift angle indicator response rate at 15 degress sector of the unit 7 scale')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 86, N'Accuracy of calculation and indication of ground speed (allowed oscillations  ±0.5 km/h):')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 87, N'     GS 215 (КПК-2)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 88, N'     GS 215 (unit 7)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 89, N'Unit 7 ground speed counter response rate at 100 km/h sector:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 90, N'Accuracy of calculation and indication of distance flown and side deviation (indications of counters in unit 8):')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 91, N'     distance flown FORWARD')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 92, N'     side deviation LEFT')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 93, N'Values of longitudinal, lateral and vertical components of velocity vector at:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 94, N'     Θ (pitch) = 15°:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 97, N'     γ (bank) = 15°:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 100, N'     Θ (pitch) = 345°:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 103, N'     γ (bank) = 345°:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 106, N'Calibration in SEA mode (changing of ground speed in reference to TERRAIN mode) ')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 107, N'Unit 8 response to check task DA 0 for values:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 108, N'     ψ (heading) = 45°:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 109, N'           distance flown FORWARD')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 110, N'           side deviation LEFT')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 111, N'     ψ (heading) = 135°:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 112, N'           distance flown BACKWARD')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 113, N'          боковое уклонение RIGHT')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 114, N'Operation in MEMORY mode:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 115, N'     П light illuminated')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 116, N'     ground speed')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 117, N'     drift angle')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 118, N'Indication accuracy in "К" mode:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 119, N'     ground speed')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 120, N'     drift angle LEFT')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 121, N'Accuracy characteristics of output parameters of ПК instrument for successive positions of the switch:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 122, N'     Position 1:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 123, N'          FORWARD')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 124, N'          SIDE')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 125, N'          UP')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 126, N'     Position 2:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 127, N'          BACKWARD')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 128, N'          RIGHT')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 129, N'          UP')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 130, N'     Position 3:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 131, N'          ground speed')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 132, N'          drift angle')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 133, N'          W≥50 km/h')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 134, N'     Position 4:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 135, N'          П light engagement time')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 136, N'              changes in reference to Position 3:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 137, N'          ground speed')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 138, N'          drift angle')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 139, N'     Position OPERATION')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 140, N'     Position 4')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 141, N'Bias voltage at modulator diodes (at Ш21 connector of HF unit in ref.to contact 30):')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 142, N'     27 (MD 1)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 143, N'     26 (MD 2)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 144, N'Power voltages:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 145, N'     direct current')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 146, N'     alternating current')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 147, N'Unit 6 error of velocity components indication in check task mode:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 148, N'          FORWARD - 17 (Wxг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 149, N'          LEFT - 17 (Wzг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 150, N'          DOWN - 3,1 (Wyг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 151, N'          BACK - 17 (Wxг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 152, N'          RIGHT - 17 (Wzг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 153, N'          UP - 3,1 (Wyг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 154, N'Error in calculation and indication of ground speed:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 159, N'Error in calculation and indication of drift angle:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 161, N'                     УС 9,5 (LEFT)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 163, N'                        УС 9,5 (LEFT)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 164, N'Increase of ground speed in SEA mode:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 167, N'Operation in MEMORY mode:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 168, N'     GS')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 169, N'     DA')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 170, N'Communication of equipment in corners (values of longitudinal, lateral and vertical components of velocity vector) at:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 171, N'     Θ (pitch) = 12°')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 175, N'     Θ (pitch) = -12°')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 179, N'     γ (bank) = 12°')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 183, N'     γ (bank) = -12°')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 187, N'Error in calculation and indication of W during testing from AMP:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 188, N'     Forward 17, Left 17, Down 3')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 192, N'     Back 17, Right 17, Up 3')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 196, N'     Speed 127, Drift 0')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 197, N'Voltages of secondary power sources:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 198, N'     12,6 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 199, N'     -12,6 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 200, N'     6,3 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 201, N'     -6,3 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 202, N'     -20 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 203, N'     15 V ts.')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 204, N'     -15 V ref.')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 205, N'     5 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 206, N'     -27 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 207, N'Difference of secondary voltages modules    Δ15 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 208, N'Reference voltage of IMPATT oscillator:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 209, N'     IMPATT 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 210, N'     IMPATT 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 211, N'Reference voltage proprotional to total current of generator diodes')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 212, N'SHF oscillations frequency')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 213, N'Bias voltage at modulator diodes:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 214, N'     MOD 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 215, N'     MOD 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 216, N'Difference of TK bias voltages:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 217, N'          Channel I-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 218, N'          Channel I-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 219, N'          Channel II-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 220, N'          Channel II-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 221, N'          Channel III-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 222, N'          Channel III-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 223, N'     scatter in pair:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 224, N'          Channel I')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 225, N'          Channel II')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 226, N'          Channel III')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 227, N'Bias voltage at SD:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 228, N'     Channel I-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 229, N'     Channel I-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 230, N'     Channel II-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 231, N'     Channel II-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 232, N'     Channel III-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 233, N'     Channel III-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 234, N'Noise voltage at LFA outputs:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 235, N'     Channel I-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 236, N'     Channel I-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 237, N'     Channel II-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 238, N'     Channel II-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 239, N'     Channel III-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 240, N'     Channel III-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 241, N'Output voltages of VCC unit with velocity vector components nulled: ')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 246, N'         GS')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 247, N'         DA')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 248, N'     Speed 258, Drift 9.5 (LEFT)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 249, N'         GS')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 250, N'         DA')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 251, N'Operation of light warning:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 252, N'     DOPP CTRL')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 253, N'     SEARCH')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 254, N'     OK')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 255, N'     FLIGHT')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 256, N'Error in calculation and indication of distance flown and side deviation (indications of counters in unit 8, great circle):')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 257, N'     distance flown FORWARD')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 258, N'     side deviation LEFT')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 259, N'     ВЧ FAIL')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 260, N'     ВСС FAIL')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 261, N'     П light off (at GSDA indicator ) ')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 262, N'Signals coordinates generated by channel:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 263, N'     RECKONING FROM DOPP')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 264, N'     RECKONING FROM ДПСМ ')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 265, N'     БВК OK (БВК FAIL)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 266, N'Error while delivering Zорт (ПВК) voltage in the outer systems:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 267, N'     check task mode ')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 268, N'     nulling mode')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 269, N'Error while delivering Zорт (ПНП) direct current signal in the outer systems')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 271, N'          FORWARD - 17 (Wxг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 272, N'          LEFT - 17 (Wzг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 273, N'          DOWN - 3,1 (Wyг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 274, N'          BACK - 17 (Wxг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 275, N'          RIGHT - 17 (Wzг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 276, N'          UP - 3,1 (Wyг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 277, N'     unit 6:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 278, N'     COORDINATES ARE DISABLED')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 279, N'Frequency measured ')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 280, N'Power measured')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 281, N'Phase modulation index')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 282, N'  515 V:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 283, N'    - at increased power voltage')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 284, N'    - at decreased power voltage')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 285, N'  400 V:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 286, N'    - at increased power voltage ')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 287, N'    - at decreased power voltage')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 288, N'  6.3 V:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 289, N'    - at increased power voltage ')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 290, N'    - at decreased power voltage')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 291, N'    ground speed')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 292, N'    drift angle')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 293, N'  pulses amplitude:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 297, N'  pulses deviation:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 301, N'  pulses duration:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 305, N'  powering:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 306, N'    Mod 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 307, N'    Mod 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 308, N'    Mod 3')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 309, N'ТК11 П Light')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 310, N'П light illumination time')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 312, N'  Mixer 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 313, N'  Mixer 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 314, N'  Heter')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 315, N'  LFA output')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 316, N'  Klystr')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 317, N'Task 2:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 318, N'  - speed')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 319, N'  - drift angle')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 320, N'  - speed over the sea')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 321, N'Task 1:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 322, N'  - speed')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 323, N'  - drift angle')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 324, N'Task 3:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 325, N'  - speed')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 326, N'  - drift angle')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 327, N'П light')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 329, N'  Mix1 OK')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 330, N'  Mix2 OK')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 331, N'  15th turn-on:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 332, N'    - speed')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 333, N'    - drift angle')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 334, N'  30th turn-on:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 335, N'    - speed')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 336, N'    - drift angle')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 337, N'  50th turn-on:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 338, N'    - speed')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 339, N'    - drift angle')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 343, N'                        УС 9,5 (LEFT)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 346, N'     GS')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 347, N'     DA')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 358, N'                     УС 9,5 (LEFT)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 359, N'Error in calculation and indication of the distance flown and the side deviation (readings of counters from unit 8 Geographic):')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 360, N'     distance flown FORWARD')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 361, N'     side deviation LEFT')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 362, N'     MAP BOUNDARY')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 363, N'DOPP - AIST connection ')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 364, N' CT-4:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 365, N'  Voltage proportional to α')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 366, N'  Voltage proportional to W')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 367, N' CT-6:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 368, N'  Voltage proportional to α')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 369, N'  Voltage proportional to W')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 370, N' CT-5: Voltage proportional to α')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 371, N'DOPP - SAU connection')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 372, N' CT-6: Voltage proportional to α')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 373, N' CT-5: Voltage proportional to α')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 374, N' CT-1:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 375, N'  Voltage proportional to Wхг')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 376, N'  Voltage proprotinal to Wуг')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 377, N' CT-3:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 378, N'  Voltage proprotional to Wxг')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 379, N'  Voltage proportional to Wyг')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 380, N'  Voltage proprotional to Wzг')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 381, N' CT-2: Voltage proportional to Wzг')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 382, N' DOPP - SU connection')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 383, N' CT-1: Voltage proportional to Wхг')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 384, N' CT-2: Voltage proportional to Wzг')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 385, N' CT-3: Voltage proportional to Wуг(х)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 386, N' CT-4: Voltage proportional to Wуг(х)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 387, N' DOPP - LANDYSH connection')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 388, N' CT-2:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 389, N'  Voltage proportional to Wхг')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 390, N'  Voltage proportional to Wzг')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 391, N' CT-4:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 392, N'  Voltage proportional to Wхг')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 393, N'  Voltage proportional to Wzг')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 394, N' CT-5: Voltage proportional to Wzг')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 395, N' CT-6:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 396, N'  Voltage proportional to W=215 km/h')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 397, N'  Voltage proportional to Wzг')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 398, N'DOPP - ДВС-24 sensor connection')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 399, N' Unit 8:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 400, N'  distance flown FORWARD')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 401, N'  distance flown SIDE')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 402, N' Unit 10:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 403, N'  cursor movement UP')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 404, N'  cursor movement SIDE ')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 405, N' MAP BOUNDARY light illumination')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 408, N'Unit 10 cursor position')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 409, N'Power voltages:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 410, N' direct current')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 411, N' alternating current')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 412, N'Voltage of secondary power sources:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 413, N' +12,6 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 414, N' -12,6 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 415, N' +6,3 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 416, N' -6,3 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 417, N' -20 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 420, N' +5 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 422, N'Reference voltage of ПМ generator:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 425, N'Reference voltage proportional to total current of ПМ')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 426, N'SHF oscillations frequency')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 427, N'Bias voltage at modulator diodes:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 428, N' MOD 1 (+)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 429, N' MOD 2 (-)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 430, N'Difference in TK bias voltages:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 431, N' Channel 1-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 432, N' Channel 2-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 433, N' Channel 1-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 434, N' Channel 2-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 435, N' Channel 1-3')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 436, N' Channel 2-3')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 437, N' scatter in pair:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 438, N'  Channel 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 439, N'  Channel 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 440, N'  Channel 3')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 441, N'Bias voltage at SD:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 442, N' Channel 1-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 443, N' Channel 2-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 444, N' Channel 1-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 445, N' Channel 2-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 446, N' Channel 1-3')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 447, N' Channel 2-3')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 448, N'Noise voltage of signal voltages at LFA outputs:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 449, N' Channel 1-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 450, N' Channel 2-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 451, N' Channel 1-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 452, N' Channel 2-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 453, N' Channel 1-3')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 454, N' Channel 2-3')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 455, N'Memory mode')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 456, N'"АС" mode')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 457, N'"Контр. БС" mode')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 458, N'"Контр. ВЧ" mode')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 459, N'Magnetron training')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 460, N'Power voltages: ')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 461, N'     direct current')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 462, N'     alternating current')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 463, N'Unit 6 error of velocity components indication in check task mode:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 464, N'          FORWARD - 17 (Wxг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 465, N'          LEFT - 17 (Wzг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 466, N'          DOWN - 3,1 (Wyг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 467, N'          BACK - 17 (Wxг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 468, N'          RIGHT - 17 (Wzг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 469, N'          UP - 3,1 (Wyг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 470, N'Error in calculation and indication of ground speed:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 475, N'Error in calculation and indication of drift angle:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 477, N'                     УС 9,5 (LEFT)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 479, N'                        УС 9,5 (LEFT)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 480, N'Increase of ground speed in SEA mode:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 483, N'Operation in MEMORY mode:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 484, N'     GS')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 485, N'     DA')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 486, N'Связь аппаратуры по углам - ? (values of longitudinal, lateral and vertical components of velocity vector) at:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 487, N'     Θ (pitch) = 12°')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 491, N'     Θ (pitch) = -12°')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 495, N'     γ (bank) = 12°')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 499, N'     γ (bank) = -12°')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 503, N'Error in calculation and indication of W during testing from AMP:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 504, N'     Forward 17, Left 17, Down 3')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 508, N'     Back 17, Right 17, Up 3')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 512, N'     Speed 127, Drift 0')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 513, N'Voltage of secondary power sources:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 514, N'     12,6 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 515, N'     -12,6 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 516, N'     6,3 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 517, N'     -6,3 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 518, N'     -20 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 519, N'     15 V ts.')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 520, N'     -15 V ref.')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 521, N'     5 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 522, N'     -27 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 523, N'Difference of secondary voltages modules    Δ15 V')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 524, N'Reference voltage of IMPATT oscillator:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 525, N'     IMPATT 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 526, N'     IMPATT 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 527, N'Reference voltage proprotional to total current of generator diodes')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 528, N'SHF oscillations frequency')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 529, N'Bias voltage at modulator diodes:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 530, N'     MOD 1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 531, N'     MOD 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 532, N'Difference of TK bias voltages:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 533, N'          Channel I-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 534, N'          Channel I-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 535, N'          Channel II-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 536, N'          Channel II-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 537, N'          Channel III-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 538, N'          Channel III-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 539, N'     scatter in pair:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 540, N'          Channel I')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 541, N'          Channel II')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 542, N'          Channel III')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 543, N'Bias voltage at SD:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 544, N'     Channel I-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 545, N'     Channel I-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 546, N'     Channel II-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 547, N'     Channel II-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 548, N'     Channel III-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 549, N'     Channel III-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 550, N'Noise voltage at LFA outputs:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 551, N'     Channel I-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 552, N'     Channel I-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 553, N'     Channel II-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 554, N'     Channel II-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 555, N'     Channel III-1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 556, N'     Channel III-2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 557, N'Output voltages of VCC unit with velocity vector components nulled:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 562, N'         GS')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 563, N'         DA')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 564, N'     Speed 258, Drift 9,5 (LEFT)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 565, N'         GS')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 566, N'         DA')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 567, N'Operation of light warning:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 568, N'     DOPP CTRL')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 569, N'     SEARCH')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 570, N'     OK')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 571, N'     FLIGHT')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 575, N'     ВЧ FAIL')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 576, N'     ВСС FAIL')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 577, N'     П light off (at GSDA indicator ) ')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 587, N'          FORWARD - 17 (Wxг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 588, N'          LEFT - 17 (Wzг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 589, N'          DOWN - 3,1 (Wyг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 590, N'          BACK - 17 (Wxг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 591, N'          RIGHT - 17 (Wzг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 592, N'          UP - 3,1 (Wyг)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 593, N'     unit 6:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 616, N'CT-6:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 617, N'  Voltage proportional to Wхг')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 618, N'  Voltage proportional to Wzг')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 622, N'Visual control of MEMORIZING mode operation:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 623, N'Delivery of Wx, Wy, Wz to aircraft systems:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 624, N'Task 1:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 631, N'  Wxг ACAS')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 632, N'  Wzг ACAS')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 633, N'  Wyг ACAS')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 634, N'  Wxг Tester')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 635, N'  Wzг Tester')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 636, N'  Wyг Tester')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 637, N'Task 2:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 644, N'  Wxг ACAS')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 645, N'  Wzг ACAS')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 646, N'  Wyг ACAS')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 647, N'  Wxг Tester')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 648, N'  Wzг Tester')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 649, N'  Wyг Tester')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 650, N'Output voltages stability')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 651, N'  485V:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 652, N'    - at increased power voltage')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 653, N'    - at decreased power voltage')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 654, N'  505V:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 655, N'    - at increased power voltage')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 656, N'    - at decreased power voltage')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 657, N'  ТК9:')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 1, 658, N'Output pulses and currents parameters')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 5, N'max 16,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 6, N'max 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 10, N'-5,67 to -6,62')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 15, N'max 0,3')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 17, N'-46 to -57')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 20, N'0,25 to 1,6')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 21, N'-0,25 to -1,6')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 22, N'0,25 to 1,6')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 23, N'-0,25 to -1,6')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 24, N'0,25 to 1,6')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 25, N'-0,25 to -1,6')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 27, N'max 0,65')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 28, N'max 0,65')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 29, N'max 0,65')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 30, N'min 0,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 42, N'max 130')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 43, N'max 130')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 44, N'max 130')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 45, N'max 130')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 46, N'max 130')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 47, N'max 130')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 56, N'max ±70')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 57, N'max ±70')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 58, N'max ±220')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 73, N'min 2,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 74, N'min 2,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 75, N'min 0,25')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 85, N'min 1,1')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 89, N'min 2,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 95, N'      reduced       by 1,5 - 3,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 96, N'      increased      by 0,8 - 1,7')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 98, N'      increased      by 1,5 - 3,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 99, N'      reduced        by 0,8 - 1,7')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 101, N'      increased      by 1,5 - 3,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 102, N'      reduced        by 0,8 - 1,7')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 104, N'      reduced        by 1,5 - 3,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 105, N'      increased      by 0,8 - 1,7')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 106, N'increased by 2±0,3')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 133, N'display OFF is lit in unit 6, shutter is open in unit 7')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 137, N'max ±4,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 138, N'max ±3,5°')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 139, N'Displays M and CONTROL shall be off in unit ПК')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 140, N'Displays  M and CONTROL shall be on and display OPERATION shall be off')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 172, N'inc')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 173, N'no change')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 174, N'dec')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 176, N'dec')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 177, N'no change')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 178, N'inc')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 180, N'no change')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 181, N'inc')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 182, N'dec')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 184, N'no change')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 185, N'dec')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 186, N'inc')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 188, N'unit 6')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 192, N'unit 6')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 196, N'as per DA GS unit')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 207, N'-35 to +35')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 209, N'2,4 to 3,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 210, N'2,4 to 3,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 211, N'-1,7 to -2,9')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 214, N'+2,3 to +4,9')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 215, N'-2,3 to -4,9')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 224, N'max 45')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 225, N'max 45')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 226, N'max 45')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 228, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 229, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 230, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 231, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 232, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 233, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 235, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 236, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 237, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 238, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 239, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 240, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 242, N'max ±10')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 243, N'max ±15')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 244, N'max ±30')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 245, N'max ±10')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 248, N'as per DA GS unit')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 252, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 253, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 254, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 255, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 259, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 260, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 261, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 263, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 264, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 265, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 278, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 280, N'min 260')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 281, N'2,1 to 2,9')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 294, N'min 4')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 295, N'min 4')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 296, N'min 4')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 298, N'max 7%')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 299, N'max 7%')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 300, N'max 7%')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 302, N'max 10')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 303, N'max 10')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 304, N'max 10')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 309, N'off')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 316, N'40 - 80 or off-scale')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 320, N'not measured')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 327, N'on')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 329, N'ok')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 330, N'ok')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 362, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 375, N'min -5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 379, N'min 5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 381, N'min -8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 405, N'shall come on')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 408, N'shall displace to area 2')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 423, N'2,4 to 3,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 424, N'2,4 to 3,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 425, N'-1,7 to -2,9')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 428, N'+2,7 to +4,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 429, N'-2,7 to -4,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 438, N'max 45')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 439, N'max 45')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 440, N'max 45')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 442, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 443, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 444, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 445, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 446, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 447, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 449, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 450, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 451, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 452, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 453, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 454, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 455, N'As per PNV')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 456, N'As per PNV')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 457, N'As per PNV')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 458, N'As per PNV')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 488, N'inc')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 489, N'no change')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 490, N'dec')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 492, N'dec')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 493, N'no change')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 494, N'inc')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 496, N'no change')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 497, N'inc')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 498, N'dec')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 500, N'no change')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 501, N'dec')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 502, N'inc')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 504, N'unit 6')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 508, N'unit 6')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 512, N'as per DA GS unit')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 523, N'-35 to +35')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 525, N'2,4 to 3,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 526, N'2,4 to 3,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 527, N'-1,7 to -2,9')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 530, N'+2,7 to +4,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 531, N'-2,7 to -4,5')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 540, N'max 45')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 541, N'max 45')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 542, N'max 45')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 544, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 545, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 546, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 547, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 548, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 549, N'0,6 to 1,8')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 551, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 552, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 553, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 554, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 555, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 556, N'30 to 100')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 558, N'max ±40')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 559, N'max ±80')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 560, N'max ±300')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 564, N'as per DA GS unit')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 568, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 569, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 570, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 571, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 575, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 576, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 577, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 619, N'max ±330')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 620, N'max ±230')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 621, N'max ±300')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 2, 622, N'As per OM')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 3, 274, N'Speed (task 2)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 3, 275, N'Drift angle (task 2)')
INSERT [dbo].[Translate] ([LangId], [TableId], [PropId], [Text]) VALUES (2, 3, 278, N'Speed (task 1)')
GO
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'В', N'V')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'Гц', N'Hz')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'мВ', N'mV')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'Ом', N'Ohm')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'мм/сек', N'mm/sec')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'пробл/мин', N'flash/min')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'ед.', N'units')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'пФ', N'pF')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'не более', N'less than')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'исправен', N'serviceable')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'Uвых', N'Uout')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'вращается', N'rotates')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'все каналы исправны', N'all channels are serviceable')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'нет прерываний', N'no breaks')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'соотв пасп', N'pasp value')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'ход стрелки без рывков', N'pointer moving gradually')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'Присутствует', N'Available')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'ОД введены', N'ID is entered')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'Исправно', N'serviceable')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'исправна', N'serviceable')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'испр', N'serviceable')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'Соблюдается', N'correct')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'Исправны', N'serviceable')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'ед. перегрузки', N'g')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'В', N'V')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'Гц', N'Hz')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'мВ', N'mV')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'Ом', N'Ω')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'мм/сек', N'mm/seg')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'пробл/мин', N'centelleo/min')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'ед.', N'un.')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'пФ', N'pF')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'не более', N'máx')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'исправен', N'Operativo')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'Uвых', N'Usal')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'вращается', N'Está girando')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'все каналы исправны', N'todos los canales operativos')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'нет прерываний', N'No hay interrupciones')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'соотв пасп', N'esp.')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'ход стрелки без рывков', N'movimiento suave')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'Присутствует', N'Disponible')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'ОД введены', N'Intr. los distintivo')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'Исправно', N'Operativo')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'исправна', N'Operativo')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'испр', N'Operativo')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'Соблюдается', N'Correcta')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'Исправны', N'Operativo')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'ед. перегрузки', N'un. de aceleración')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'от', N'entre')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'до', N'y')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'от', N'')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'до', N'to')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'мА', N'mA')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'мА', N'mA')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'мкА', N'µA')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (3, N'мкА', N'µA')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'МГц', N'MHz')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'Вт', N'Wh')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'сек', N'sec')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'км/ч', N'km/h')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'м/с', N'm/s')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'км/ч /с', N'km/h /s')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'м/с /с', N'm/s /s')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'град', N'°')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'град/с', N'°/s')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'км', N'km')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'с', N's')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'мкс', N'mcs')
INSERT [dbo].[TranslateEquals] ([LangId], [OriginalText], [TranslatedText]) VALUES (2, N'дел', N'div')
GO
ALTER TABLE [dbo].[AdditionalParameter] ADD  CONSTRAINT [DF_AdditionalParameter_ParamId]  DEFAULT ((1)) FOR [ParamId]
GO
ALTER TABLE [dbo].[Testing] ADD  CONSTRAINT [DF_Testing_IsManualResult]  DEFAULT ((0)) FOR [IsManualResult]
GO
ALTER TABLE [dbo].[AdditionalParameter]  WITH NOCHECK ADD  CONSTRAINT [FK_AdditionalParameter_Testing] FOREIGN KEY([TestId])
REFERENCES [dbo].[Testing] ([Id])
ON DELETE CASCADE
NOT FOR REPLICATION 
GO
ALTER TABLE [dbo].[AdditionalParameter] CHECK CONSTRAINT [FK_AdditionalParameter_Testing]
GO
ALTER TABLE [dbo].[Testing]  WITH CHECK ADD  CONSTRAINT [FK_Block_Executors] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([EmployeeId])
GO
ALTER TABLE [dbo].[Testing] CHECK CONSTRAINT [FK_Block_Executors]
GO
ALTER TABLE [dbo].[Translate]  WITH CHECK ADD  CONSTRAINT [FK_Translate_Lang] FOREIGN KEY([LangId])
REFERENCES [dbo].[Lang] ([Id])
GO
ALTER TABLE [dbo].[Translate] CHECK CONSTRAINT [FK_Translate_Lang]
GO
ALTER TABLE [dbo].[Translate]  WITH CHECK ADD  CONSTRAINT [FK_Translate_TablesToTranslate] FOREIGN KEY([TableId])
REFERENCES [dbo].[TablesToTranslate] ([TableId])
GO
ALTER TABLE [dbo].[Translate] CHECK CONSTRAINT [FK_Translate_TablesToTranslate]
GO
/****** Object:  StoredProcedure [dbo].[BackupDatabase]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin Oleg
-- Create date: 03/05/2012
-- Description:	Делает резервную копию базы
-- =============================================
CREATE PROCEDURE [dbo].[BackupDatabase]
	-- Add the parameters for the stored procedure here
	@Path varchar(254)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	BACKUP DATABASE KPK2 TO DISK = @Path WITH STATS = 10, FORMAT, SKIP, INIT
END



GO
/****** Object:  StoredProcedure [dbo].[DeleteTesting]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin Oleg
-- Create date: 22.10.2013
-- Description:	Удаляет проверку из БД
-- =============================================
CREATE PROCEDURE [dbo].[DeleteTesting]
	-- Add the parameters for the stored procedure here
	@TestId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	delete from MeasuringParam where TestId = @TestId
	delete from Composition where TestId = @TestId
	delete from Testing where Id = @TestId	
	
END

GO
/****** Object:  StoredProcedure [dbo].[GetModulePort]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin Oleg
-- Create date: 03/05/2012
-- Description:	Возвращает порт для устройства
-- =============================================
CREATE PROCEDURE [dbo].[GetModulePort]
	-- Add the parameters for the stored procedure here
	@ModuleName varchar(50)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select Port.PortName from Module join Port ON Module.PortId = Port.PortId where Module.ModuleName = @ModuleName
END


GO
/****** Object:  StoredProcedure [dbo].[GetReportValues]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin Oleg
-- Create date: 03/12/2012
-- Description:	Выдает параметры отчёта по Id проверки
-- Modified: 23/12/2012 by Subbotin Oleg
-- =============================================
CREATE PROCEDURE [dbo].[GetReportValues]
	@TestId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @DeviceTypeId int
	set @DeviceTypeId = (select DeviceTypeId from Testing where Id = @TestId)

	select Id, DeviceTypeId, Item, SubItem, OrderNumber, CheckName, Units, Requirement, ParamId into #ResultReport from ReportLang where DeviceTypeId = @DeviceTypeId

	IF(@DeviceTypeId = 2)
	 IF(Exists(select 1 from dbo.AdditionalParameter where TestId = @TestId and ParamId = 7))
	  begin
		declare @TypeVSS varchar(100)
		select @TypeVSS = Value from dbo.AdditionalParameter where ParamId = 7 and TestId = @TestId --параметр TypeVSS 
		declare @TypeVSSInt int
		select  @TypeVSSInt = cast(@TypeVSS as int)
		If (@TypeVSSInt = 1)
		 begin
		  update #ResultReport
		  set ParamId = 385, Requirement = REPLACE(Requirement, '2,5', '3,5')
		  where ParamId = 183
		  update #ResultReport
		  set ParamId = 386, Requirement = REPLACE(Requirement, '3,5', '5,0')
		  where ParamId = 187		  
	      update #ResultReport
		  set ParamId = 387, Requirement = REPLACE(Requirement, '2,5', '3,5')
		  where ParamId = 223
		  update #ResultReport
		  set ParamId = 388, Requirement = REPLACE(Requirement, '3,5', '5,0')
		  where ParamId = 225		  
		  update #ResultReport
		  set ParamId = 459, Requirement = REPLACE(Requirement, '2,5', '3,5')
		  where ParamId = 455
		  update #ResultReport
		  set ParamId = 460, Requirement = REPLACE(Requirement, '3,5', '5,0')
		  where ParamId = 457		  
		 end
	  end
    
	select Item,SubItem, OrderNumber, CheckName, Units, Requirement, ROUND(Result,[Precision]) as Result, ParamName, MinValue, MaxValue, dbo.IsOkSelectedParameter(@TestId, rr.ParamId) as IsOK, d.[Text], [Precision], Parameters.ParamId
	from #ResultReport rr 
	left join MeasuringParam ON rr.ParamId = MeasuringParam.ParamId AND TestId = @testId
	left join Parameters ON rr.ParamId = Parameters.ParamId 	
	left join DocumentLink d ON d.DeviceId = @DeviceTypeId And rr.Item = d.TestReferenceId
	where DeviceTypeId = @DeviceTypeId
	order by Item, SubItem, OrderNumber
END

GO
/****** Object:  StoredProcedure [dbo].[InsertComposition]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin Oleg
-- Create date: 12.12.2012
-- Description:	Добавляет блок для проверки
-- =============================================
CREATE PROCEDURE [dbo].[InsertComposition]
	@TestId int,
	@BlockId int,
	@BlockNumber nvarchar(12),
	@TotalRun int,
	@LastRun int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT Composition
	SELECT Id, BlockId, @BlockNumber, @TotalRun, @LastRun 
	from Testing t join CompositionReference c ON t.DeviceTypeId = c.DeviceId 
	where t.Id = @TestId AND c.BlockId = @BlockId
END

GO

/****** Object:  StoredProcedure [dbo].[InsertTesting]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin Oleg
-- Create date: 29.11.2012
-- Description:	Добавляет проверку в базу
-- =============================================
CREATE PROCEDURE [dbo].[InsertTesting] 
	-- Add the parameters for the stored procedure here
	@DeviceTypeId int, 
	@ProductNumber nvarchar(20),
	@MakingDate smalldatetime,
	@HelicopterMark nvarchar(20),
	@HelicopterSign nvarchar(20),
	@HelicopterNumber nvarchar(20),
	@TestDate smalldatetime,
	@EmployeeId int,
	@Note varchar(MAX),
	@TOFormName varchar(150),
	@TotalRun int,
	@LastRun int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @TOFormId int
	set @TOFormId = (select TOFormId from TOForm where TOFormName = @TOFormName)
	IF (@TOFormName is not NULL)	
		IF (@ToFormId is NULL)
		BEGIN
			insert TOForm (TOFormName) values (@TOFormName)
			set @TOFormId = (select TOFormId from TOForm where TOFormName = @TOFormName)
		END
	
    -- Insert statements for procedure here
	INSERT Testing (DeviceTypeId, ProductNumber, MakingDate, HelicopterMark, HelicopterSign, HelicopterNumber, TestDate, EmployeeId, Note, TOFormId, TotalRun, LastRun)
	Values (@DeviceTypeId, @ProductNumber, @MakingDate, @HelicopterMark, @HelicopterSign, @HelicopterNumber, @TestDate, @EmployeeId, @Note, @TOFormId, @TotalRun, @LastRun)
	select @@IDENTITY
END

GO
/****** Object:  StoredProcedure [dbo].[SaveCalibration]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin Oleg
-- Create date: 20.09.2013
-- Description:	Сохраняет параметр калибровки
-- =============================================
CREATE PROCEDURE [dbo].[SaveCalibration]
	@ModuleId int,
	@Channel int,
	@IdealValue decimal(18,5),
	@Value int,
	@Check bit = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	if (@Check = 1)
begin
    if (EXISTS (select * from Calibration where ModuleId = @ModuleId AND Channel = @Channel))
		update Calibration
			set [Value] = @Value
			where ModuleId = @ModuleId AND Channel = @Channel
	else
	   insert Calibration 
		select @ModuleId, @Channel, @IdealValue, @Value
end
else
	insert Calibration 
		select @ModuleId, @Channel, @IdealValue, @Value
END

GO
/****** Object:  StoredProcedure [dbo].[ScalarPost_IsPost_PostID]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ScalarPost_IsPost_PostID]
(
	@PostID int
)
AS
	SET NOCOUNT ON;
SELECT     IsActive
FROM         Post
WHERE     (PostId = @PostID)

GO
/****** Object:  StoredProcedure [dbo].[ScalarPost_Name_PostID]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- по ID получаем нименование должности

CREATE PROCEDURE [dbo].[ScalarPost_Name_PostID]
(
	@PostID int
)
AS
	SET NOCOUNT ON;
SELECT     PostName
FROM         Post
WHERE     (PostId = @PostID)

GO
/****** Object:  StoredProcedure [dbo].[ScalarPostID_PostName]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- по названию должности возвращаем ее ID

CREATE PROCEDURE [dbo].[ScalarPostID_PostName]
(
	@PostName varchar(50)
)
AS
	SET NOCOUNT ON;
SELECT     PostId
FROM         Post
WHERE     (PostName = @PostName)

GO
/****** Object:  StoredProcedure [dbo].[Select_Commands]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- По имени сигнала выбираем команды 

CREATE PROCEDURE [dbo].[Select_Commands]
(
	@ParamName nvarchar(50)
)
AS
	SET NOCOUNT ON;
SELECT     Commands
FROM         DescriptionParam
WHERE     (ParamName = @ParamName)

GO
/****** Object:  StoredProcedure [dbo].[Select_ComPort]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- По имени сигнала выбираем имя СОМ порта  

CREATE PROCEDURE [dbo].[Select_ComPort]
(
	@ParamName nvarchar(50)
)
AS
	SET NOCOUNT ON;
SELECT     COM_Port
FROM         DescriptionParam
WHERE     (ParamName = @ParamName)
GO
/****** Object:  StoredProcedure [dbo].[Select_MaxValue]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- выбираем Max значение для "зеленого коридора" 

CREATE PROCEDURE [dbo].[Select_MaxValue]
(
	@ParameName nvarchar(50)
)
AS
	SET NOCOUNT ON;
SELECT     SetMaxValue
FROM         DescriptionParam
WHERE     (ParamName = @ParameName)
GO
/****** Object:  StoredProcedure [dbo].[Select_MinValue]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- выбираем Min значение "зеленого коридора" 

CREATE PROCEDURE [dbo].[Select_MinValue]
(
	@ParamName nvarchar(50)
)
AS
	SET NOCOUNT ON;
SELECT     SetMinValue
FROM         DescriptionParam
WHERE     (ParamName = @ParamName)
GO
/****** Object:  StoredProcedure [dbo].[Select_ParamID]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- выбираем ID сигнала по его имени 

CREATE PROCEDURE [dbo].[Select_ParamID]
(
	@ParamName nvarchar(50)
)
AS
	SET NOCOUNT ON;
SELECT     ParamID
FROM         DescriptionParam
WHERE     (ParamName = @ParamName)
GO
/****** Object:  StoredProcedure [dbo].[Select_TimeOut]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- По имени сигнала выбираем величину тайм-аута 

CREATE PROCEDURE [dbo].[Select_TimeOut]
(
	@ParamName nvarchar(50)
)
AS
	SET NOCOUNT ON;
SELECT     TimeOut
FROM         DescriptionParam
WHERE     (ParamName = @ParamName)
GO
/****** Object:  StoredProcedure [dbo].[SelectBlock_History_Device]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- выводим все проверки для данного ДИССа с заданным заводским номером

CREATE PROCEDURE [dbo].[SelectBlock_History_Device]
(
	@TypeBlock nvarchar(20),
	@ProductionNumber nvarchar(20)
)
AS
	SET NOCOUNT ON;
SELECT     BlockID, ProductionNumber, DateMaking, HelicopterMark, SignHelicopter, NumberHelicopter, Note, DateRepair, FullName, EmployeeID, 
                      TypeBlock
FROM         ViewFullName
WHERE     (TypeBlock = @TypeBlock) AND (ProductionNumber = @ProductionNumber)
ORDER BY DateRepair

GO
/****** Object:  StoredProcedure [dbo].[SelectBlock_Number_Unique]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- получаем уникальные номера ДИССов для каждого типа

CREATE PROCEDURE [dbo].[SelectBlock_Number_Unique]
(
	@TypeBlock nvarchar(20)
)
AS
	SET NOCOUNT ON;
SELECT     BlockID, TypeBlock, ProductionNumber, DateMaking, HelicopterMark, SignHelicopter, NumberHelicopter, DateRepair, EmployeeID, Note
FROM         Block
WHERE     (TypeBlock = @TypeBlock) 
GO
/****** Object:  StoredProcedure [dbo].[SelectExecutors_All_True]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- выбираем всех доступных сотрудников

CREATE PROCEDURE [dbo].[SelectExecutors_All_True]
AS
	SET NOCOUNT ON;
SELECT     Employee.EmployeeID, Employee.EmployeeNumber, Employee.EmployeeFirstName, Employee.EmployeeSecondName, 
                      Employee.EmployeeLastName, Employee.IsActive, Employee.Image, Employee.Note, Employee.PostId, Post.PostName
FROM         Employee INNER JOIN
                      Post ON Employee.PostId = Post.PostId
WHERE     (Employee.IsActive = 1)

GO
/****** Object:  StoredProcedure [dbo].[SelectExecutors_Unique_FullName]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Возвращает полное имя (Ф.И.) и его ID 

CREATE PROCEDURE [dbo].[SelectExecutors_Unique_FullName]
AS
	SET NOCOUNT ON;
SELECT      EmployeeID, EmployeeFirstName + ' ' + SUBSTRING(EmployeeSecondName, 1, 1) + '.' 
			+ SUBSTRING(EmployeeLastName, 1, 1) + '.' AS FullName
FROM         Employee
WHERE     (IsActive = 1)
ORDER BY EmployeeFirstName

GO
/****** Object:  StoredProcedure [dbo].[SelectPassportValues]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin O.A.
-- Create date: 27.05.2013
-- Description:	Выдаёт паспортные данные прибора
-- =============================================
CREATE PROCEDURE [dbo].[SelectPassportValues]
	@TestId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select pvr.ParamId, [Name], [Value] 
	from PassportValuesReferenceLang pvr join Testing t ON pvr.DeviceTypeId = t.DeviceTypeId 
	left join PassportValues pv ON pv.ParamId = pvr.paramId and pv.testId = t.Id
	where t.Id = @TestId
END

GO
/****** Object:  StoredProcedure [dbo].[SelectPost_All_True]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Выбираем все доступные должности (IsPost = 1)

CREATE PROCEDURE [dbo].[SelectPost_All_True]
AS
	SET NOCOUNT ON;
SELECT     PostId, PostName, IsActive
FROM         Post
WHERE     (IsActive = 1)

GO
/****** Object:  StoredProcedure [dbo].[SelectPost_PostName]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- выбираем строку(строки) по названию должности 

CREATE PROCEDURE [dbo].[SelectPost_PostName]
(
	@PostName varchar(50)
)
AS
	SET NOCOUNT ON;
SELECT     PostId, PostName, IsActive
FROM         Post
WHERE     (PostName = @PostName)

GO
/****** Object:  StoredProcedure [dbo].[SelectTesting]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin Oleg
-- Create date: 11.12.2012
-- Description:	Выдаёт параметры по проверке
-- =============================================
CREATE PROCEDURE [dbo].[SelectTesting]
	@TestId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select DeviceName, DeviceShortName, ProductNumber, MakingDate, dbo.FullNameShortForm(EmployeeId), HelicopterMark, HelicopterSign, HelicopterNumber, Note, TotalRun, LastRun, TOFormName, TestDate 
	from Testing join DeviceType ON Testing.DeviceTypeId = DeviceType.DeviceTypeId 
	left join TOForm ON TOForm.TOFormId = Testing.TOFormId
	where Id = @TestId
END

GO
/****** Object:  StoredProcedure [dbo].[SetPassportValue]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin O.A.	
-- Create date: 27.05.2013
-- Description:	Устанавливает значение для паспортного параметра
-- =============================================
CREATE PROCEDURE [dbo].[SetPassportValue]
	@TestId int,
	@ParamId int,
	@Value decimal(18,5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF (NOT EXISTS(select * from PassportValuesReferenceLang pvr join Testing t ON t.DeviceTypeId = pvr.DeviceTypeId where pvr.ParamId = @ParamId))	
		return;
	

    IF (EXISTS(select * from PassportValues pv where pv.TestId = @TestId and pv.ParamId = @ParamId))
		UPDATE PassportValues
			SET [Value] = @Value
		where TestId = @TestId AND ParamId = @ParamId
	ELSE
	  INSERT PassportValues
	  SELECT @TestId, @ParamId, @Value
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateCalibration]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin Oleg
-- Create date: 20.09.2013
-- Description:	Обновляет параметры у конкретной калибровки по Id
-- =============================================
CREATE PROCEDURE [dbo].[UpdateCalibration]
	@Id int,
	@IdealValue decimal(18,5),
	@Value int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    update Calibration
		set IdealValue = @IdealValue, [Value] = @Value
	where Id = @Id
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateComposition]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin Oleg
-- Create date: 14.01.2014
-- Description:	Обновляет блок для проверки
-- =============================================
CREATE PROCEDURE [dbo].[UpdateComposition]
	@TestId int,
	@BlockId int,
	@BlockNumber nvarchar(12),
	@TotalRun int,
	@LastRun int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Composition
	SET [No] = @BlockNumber, TotalRun = @TotalRun, LastRun = @LastRun
	where BlockId = @BlockId and TestId = @TestId	
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateExecutor_ID]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- вставляем отредактированного сотрудника по его ID

CREATE PROCEDURE [dbo].[UpdateExecutor_ID]
(
	@EmployeeNomber varchar(50),
	@EmployeeFirstName varchar(50),
	@EmployeeSecondName varchar(50),
	@EmployeeLastName varchar(50),
	@IsActive bit,
	@Image image,
	@Note varchar(50),
	@PoistId int,
	@Original_PoistId int,
	@ExecutorID int
)
AS
	SET NOCOUNT OFF;
UPDATE    Employee
SET              EmployeeNumber = @EmployeeNomber, EmployeeFirstName = @EmployeeFirstName, EmployeeSecondName = @EmployeeSecondName, 
                      EmployeeLastName = @EmployeeLastName, IsActive = @IsActive, Image = @Image, Note = @Note, PostId = @PoistId
WHERE     (EmployeeID = @ExecutorID);
	 
SELECT EmployeeID, EmployeeNumber, EmployeeFirstName, EmployeeSecondName, EmployeeLastName, IsActive, Image, Note, PostId FROM Employee WHERE (EmployeeID = @ExecutorID)

GO
/****** Object:  StoredProcedure [dbo].[UpdatePost_PostID]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- вставляем после редактирования - новая должность/доступность

CREATE PROCEDURE [dbo].[UpdatePost_PostID]
(
	@PostName varchar(50),
	@IsActive bit,
	@PostId int,
	@PostId1 int	
)
AS
	SET NOCOUNT OFF;
UPDATE    Post
SET              PostName = @PostName, IsActive = @IsActive
WHERE     (PostId = @PostId);
	 
SELECT PostId, PostName, IsActive FROM Post WHERE (PostId = @PostId)

GO
/****** Object:  StoredProcedure [dbo].[UpdateTesting]    Script Date: 06.09.2021 10:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin Oleg
-- Create date: 14.01.2014
-- Description:	Обновляет проверку в базе
-- =============================================
CREATE PROCEDURE [dbo].[UpdateTesting] 
	-- Add the parameters for the stored procedure here	
	@TestId int,
	@ProductNumber nvarchar(20),
	@MakingDate smalldatetime,
	@HelicopterMark nvarchar(20),
	@HelicopterSign nvarchar(20),
	@HelicopterNumber nvarchar(20),
	@TestDate smalldatetime,
	@EmployeeId int,
	@Note varchar(MAX),
	@TOFormName varchar(150),
	@TotalRun int,
	@LastRun int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @TOFormId int
	set @TOFormId = (select TOFormId from TOForm where TOFormName = @TOFormName)
	IF (@TOFormName is not NULL)	
		IF (@ToFormId is NULL)
		BEGIN
			insert TOForm (TOFormName) values (@TOFormName)
			set @TOFormId = (select TOFormId from TOForm where TOFormName = @TOFormName)
		END
	
    -- Insert statements for procedure here
	UPDATE Testing
	SET ProductNumber = @ProductNumber, MakingDate = @MakingDate, HelicopterMark = @HelicopterMark, HelicopterSign = @HelicopterSign, HelicopterNumber = @HelicopterNumber,
	TestDate = @TestDate, EmployeeID = @EmployeeId, Note = @Note, TOFormId = @TOFormId, TotalRun = @TotalRun, LastRun = @LastRun
	Where Id = @TestId
	
END

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Уникальный идентификатор' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DescriptionParam', @level2type=N'COLUMN',@level2name=N'ParamID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Примечание, описание сигнала' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DescriptionParam', @level2type=N'COLUMN',@level2name=N'Note'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Имя сигнала' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DescriptionParam', @level2type=N'COLUMN',@level2name=N'ParamName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Min значение ТУ, "зеленый коридор"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DescriptionParam', @level2type=N'COLUMN',@level2name=N'SetMinValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Max значение ТУ, "зеленый коридор"' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DescriptionParam', @level2type=N'COLUMN',@level2name=N'SetMaxValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Единицы измерения - B, mA...' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DescriptionParam', @level2type=N'COLUMN',@level2name=N'UnitsMeasurement'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'№ СОМ порта для сигнала' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DescriptionParam', @level2type=N'COLUMN',@level2name=N'COM_Port'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Все команды для микроконтроллера через наши тэги (";"<>[]()...)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DescriptionParam', @level2type=N'COLUMN',@level2name=N'Commands'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Тайм аут для сигналов в ticks' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DescriptionParam', @level2type=N'COLUMN',@level2name=N'TimeOut'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Содержит перечень языков для перевода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Lang'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID проверки' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MeasuringParam', @level2type=N'COLUMN',@level2name=N'TestId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID измеренного сигнала' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MeasuringParam', @level2type=N'COLUMN',@level2name=N'ParamId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Измеренное значение' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MeasuringParam', @level2type=N'COLUMN',@level2name=N'Result'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Для выделение Параграфов жирным шрифтом' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ParamPrint', @level2type=N'COLUMN',@level2name=N'FontBold'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Имя должности' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Post', @level2type=N'COLUMN',@level2name=N'PostName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Доступность должности для других' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Post', @level2type=N'COLUMN',@level2name=N'IsActive'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Содержит id и названия таблиц для перевода' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TablesToTranslate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Уникальный идентификатор' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Testing', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Тип ДИСС-а (15, 32, 32-90, 32-92А и т.д.)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Testing', @level2type=N'COLUMN',@level2name=N'DeviceTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Заводской номер ДИСС' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Testing', @level2type=N'COLUMN',@level2name=N'ProductNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата изготовления ДИСС' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Testing', @level2type=N'COLUMN',@level2name=N'MakingDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Тип вертолета' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Testing', @level2type=N'COLUMN',@level2name=N'HelicopterMark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Опозновательный знак вертолета' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Testing', @level2type=N'COLUMN',@level2name=N'HelicopterSign'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'№ вертолета' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Testing', @level2type=N'COLUMN',@level2name=N'HelicopterNumber'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Дата проведения проверки ДИСС' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Testing', @level2type=N'COLUMN',@level2name=N'TestDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID сотрудника, который делал запись' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Testing', @level2type=N'COLUMN',@level2name=N'EmployeeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Примечание' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Testing', @level2type=N'COLUMN',@level2name=N'Note'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Содержит перевод строк разных таблиц на различные языки' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Translate'

GO
USE [master]
GO
ALTER DATABASE [KPK2] SET READ_WRITE 
GO
-- Переключаемся обратно в KPK2 для выполнения всех изменений
USE [KPK2]
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TestModes]') AND type = N'U')
BEGIN
    CREATE TABLE [dbo].[TestModes] (
        Id INT PRIMARY KEY,
        Name NVARCHAR(100)
    );
    
    INSERT [TestModes] (Id, Name) VALUES (1, 'NormalTest'), (2, 'WorkTest');
END
ELSE
BEGIN
    -- Если таблица уже есть, добавляем только отсутствующие строки
    IF NOT EXISTS (SELECT * FROM [TestModes] WHERE Id = 1)
        INSERT [TestModes] (Id, Name) VALUES (1, 'NormalTest');
    IF NOT EXISTS (SELECT * FROM [TestModes] WHERE Id = 2)
        INSERT [TestModes] (Id, Name) VALUES (2, 'WorkTest');
END

-- Добавление колонок с проверкой на существование
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Testing') AND name = 'IdTestMode')
BEGIN
    ALTER TABLE [Testing] ADD IdTestMode INT NOT NULL DEFAULT 1;
    ALTER TABLE [Testing] ADD CONSTRAINT FK_Testing_To_TestModes FOREIGN KEY (IdTestMode) REFERENCES TestModes(Id);
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Testing') AND name = 'MestreNameId')
    ALTER TABLE [Testing] ADD MestreNameId INT NULL DEFAULT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Testing') AND name = 'MestreBTKNameId')
    ALTER TABLE [Testing] ADD MestreBTKNameId INT NULL DEFAULT NULL;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('DeviceType') AND name = 'IdMode')
BEGIN
    ALTER TABLE [dbo].[DeviceType] ADD IdMode INT NOT NULL DEFAULT 1;
    ALTER TABLE [dbo].[DeviceType] ADD CONSTRAINT FK_DeviceType_To_TestModes FOREIGN KEY (IdMode) REFERENCES TestModes(Id);
END

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin Oleg
-- Create date: 29.11.2012
-- Description:	Добавляет проверку в базу
-- =============================================
ALTER PROCEDURE [dbo].[InsertTesting] 
	-- Add the parameters for the stored procedure here
	@DeviceTypeId int, 
	@ProductNumber nvarchar(20),
	@MakingDate smalldatetime,
	@HelicopterMark nvarchar(20),
	@HelicopterSign nvarchar(20),
	@HelicopterNumber nvarchar(20),
	@TestDate smalldatetime,
	@EmployeeId int,
	@Note varchar(MAX),
	@TOFormName varchar(150),
	@TotalRun int,
	@LastRun int,
	@MestreName int = null,
	@MestreBTKName int = null,
	@TestModeId int = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @TOFormId int
	set @TOFormId = (select TOFormId from TOForm where TOFormName = @TOFormName)
	IF (@TOFormName is not NULL)	
		IF (@ToFormId is NULL)
		BEGIN
			insert TOForm (TOFormName) values (@TOFormName)
			set @TOFormId = (select TOFormId from TOForm where TOFormName = @TOFormName)
		END
	
    -- Insert statements for procedure here
	INSERT Testing (DeviceTypeId, ProductNumber, MakingDate, HelicopterMark, HelicopterSign, HelicopterNumber, TestDate, EmployeeId, Note, TOFormId, TotalRun, LastRun, MestreNameId, MestreBTKNameId,IdTestMode)
	Values (@DeviceTypeId, @ProductNumber, @MakingDate, @HelicopterMark, @HelicopterSign, @HelicopterNumber, @TestDate, @EmployeeId, @Note, @TOFormId, @TotalRun, @LastRun, @MestreName, @MestreBTKName, @TestModeId)
	select @@IDENTITY
END

GO
/****** Object:  StoredProcedure [dbo].[SelectTesting]    Script Date: 25.06.2024 14:49:47 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin Oleg
-- Create date: 11.12.2012
-- Description:	Выдаёт параметры по проверке
-- =============================================
ALTER PROCEDURE [dbo].[SelectTesting]
	@TestId int,
	@TestModeId int = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    select 
		DeviceName, 
		DeviceShortName, 
		ProductNumber, 
		MakingDate, 
		dbo.FullNameShortForm(EmployeeId), 
		HelicopterMark, 
		HelicopterSign, 
		HelicopterNumber, 
		Note, 
		TotalRun, 
		LastRun, 
		TOFormName, 
		TestDate,
		ISNULL(dbo.FullNameShortForm(MestreNameId), null) as MestreName,
		ISNULL(dbo.FullNameShortForm(MestreBTKNameId), null) as MestreBTKName
	from Testing 
		inner join DeviceType ON Testing.DeviceTypeId = DeviceType.DeviceTypeId 
		left join TOForm ON TOForm.TOFormId = Testing.TOFormId
	where Id = @TestId and IdTestMode = @TestModeId
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateTesting]    Script Date: 29.10.2024 14:02:04 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Subbotin Oleg
-- Create date: 14.01.2014
-- Description:	Обновляет проверку в базе
-- =============================================
ALTER PROCEDURE [dbo].[UpdateTesting] 
	-- Add the parameters for the stored procedure here	
	@TestId int,
	@ProductNumber nvarchar(20),
	@MakingDate smalldatetime,
	@HelicopterMark nvarchar(20),
	@HelicopterSign nvarchar(20),
	@HelicopterNumber nvarchar(20),
	@TestDate smalldatetime,
	@EmployeeId int,
	@Note varchar(MAX),
	@TOFormName varchar(150),
	@TotalRun int,
	@LastRun int,
	@MestreName int = null,
	@MestreNameBTK int = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @TOFormId int
	set @TOFormId = (select TOFormId from TOForm where TOFormName = @TOFormName)
	IF (@TOFormName is not NULL)	
		IF (@ToFormId is NULL)
		BEGIN
			insert TOForm (TOFormName) values (@TOFormName)
			set @TOFormId = (select TOFormId from TOForm where TOFormName = @TOFormName)
		END
	
    -- Insert statements for procedure here
	UPDATE Testing
	SET ProductNumber = @ProductNumber, 
		MakingDate = @MakingDate, 
		HelicopterMark = @HelicopterMark, 
		HelicopterSign = @HelicopterSign, 
		HelicopterNumber = @HelicopterNumber,
		TestDate = @TestDate, 
		EmployeeID = @EmployeeId, 
		Note = @Note, 
		TOFormId = @TOFormId, 
		TotalRun = @TotalRun, 
		LastRun = @LastRun,
		MestreNameId = @MestreName,
		MestreBTKNameId = @MestreNameBTK
	Where Id = @TestId
	
END

GO
/****** Object:  StoredProcedure [dbo].[SelectTesting]    Script Date: 08.10.2024 15:59:39 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dorin Dmitry
-- Create date: 08.10.2024
-- Description:	Выдает тип ВС последней созданной проверки.
-- =============================================
CREATE PROCEDURE [dbo].[SelectLastTestingType]
	@TestModeId int = 1,
	@ParamName varchar(max) = 'Modification'
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		DeviceTypeId,
		ap.Value 
	FROM Testing 
		left join AdditionalParameter ap ON ap.TestId = Testing.Id AND 
			ap.ParamId = (SELECT ParamId 
							FROM AdditionalParameterReference 
							WHERE ParamName = @ParamName)  
	WHERE Id = (SELECT max(id) 
				FROM Testing 
				WHERE IdTestMode = @TestModeId)
END

GO
SET IDENTITY_INSERT [dbo].[DeviceType] ON
INSERT [dbo].[DeviceType] ([DeviceTypeId], [DeviceName], [DeviceShortName], [IdMode]) VALUES (7, N'доплеровский измеритель ДИСС-32', N'ДИСС-32 (РП)', 2)
INSERT [dbo].[DeviceType] ([DeviceTypeId], [DeviceName], [DeviceShortName], [IdMode]) VALUES (8, N'доплеровский измеритель ДИСС-32-90', N'ДИСС-32-90 (РП)', 2)
SET IDENTITY_INSERT [dbo].[DeviceType] OFF

GO
SET IDENTITY_INSERT [dbo].[Report] ON

GO
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (689, 7, 1, 1, 1,		N'Потребляемый ток от источников питания:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (690, 7, 1, 2, NULL,	N'	по цепи 27 В', N'А', N'не более 7', 467)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (691, 7, 1, 3, NULL,	N'	по цепи 115 В, 400Гц', N'А', N'не более 4,2', 469)

--Напряжения вторичных источников питания, В: 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (692, 7, 2, 1, 2,		N'Напряжения вторичных источников питания: ', N'', N'', NULL) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (693, 7, 2, 2, NULL,	N'	+12,6 В', N'В', N'от +11,95 до +13,25', 502) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (694, 7, 2, 3, NULL,	N'	-12,6 В', N'В', N'от -11,95 до -13,25', 503) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (695, 7, 2, 4, NULL,	N'	-20 В', N'В', N'от -19 до -21', 11) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (696, 7, 2, 5, NULL,	N'	+6,3 В', N'В', N'от +6,0 до +6,6', 8) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (697, 7, 2, 6, NULL,	N'	-6,3 В', N'В', N'от -6,0 до -6,6', 10) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (698, 7, 2, 7, NULL,	N'	+15 В', N'В', N'от +14,25 до +15,75', 12) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (699, 7, 2, 8, NULL,	N'	-15 В', N'В', N'от -14,25 до -15,75', 13) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (700, 7, 2, 9, NULL,	N'	+5 В', N'В', N'от +4,75 до + 5,25', 15) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (701, 7, 2, 10, NULL, N'	-27 В', N'В', N'от -25,65 до -28,35', 16) 

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (702, 7, 3, 1, 3,		N'Контрольное напряжение генератора М55204 (ГЛПД):', N'', N'', NULL) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (703, 7, 3, 2, NULL,	N'	ГЛПД1', N'В', N'от 2,4 до 3,5', 18) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (704, 7, 3, 3, NULL,	N'	ГЛПД2', N'В', N'от 2,4 до 3,5', 19) 

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (705, 7, 4, 1, 4,		N'Контрольное напряжение, пропорциональное общему току генераторных диодов', N'В', N'от -1,7 до -2,9', 20)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (706, 7, 5, 1, 5,		N'Напряжение смещения на модуляторных диодах:', N'', N'', NULL) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (707, 7, 5, 2, NULL,	N'	МОД 1', N'В', N'от +2,9 до +4,3', 21) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (708, 7, 5, 3, NULL,	N'	МОД 2', N'В', N'от -2,9 до -4,3', 22) 

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (709, 7, 6, 1, 6,		N'Разность напряжений смещения на диодах смесителей «Ток К» каналов I, II, III до включения и после выключения СВЧ-мощности:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (710, 7, 6, 2, NULL,	N'	Канал I-1', N'мВ', N'от 50 до 120', 461)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (711, 7, 6, 3, NULL,	N'	Канал I-2', N'мВ', N'от 50 до 120', 462)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (712, 7, 6, 4, NULL,	N'	Канал II-1', N'мВ', N'от 50 до 120', 463)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (713, 7, 6, 5, NULL,	N'	Канал II-2', N'мВ', N'от 50 до 120', 464)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (714, 7, 6, 6, NULL,	N'	Канал III-1', N'мВ', N'от 50 до 120', 465)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (715, 7, 6, 7, NULL,	N'	Канал III-2', N'мВ', N'от 50 до 120', 466)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (716, 7, 7, 1, 7,		N'Разброс абсолютных значений разности напряжений в паре: ', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (717, 7, 7, 2, NULL,	N'	Канал I', N'мВ', N'не более 45', 198)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (718, 7, 7, 3, NULL,	N'	Канал II', N'мВ', N'не более 45', 199)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (719, 7, 7, 4, NULL,	N'	Канал III', N'мВ', N'не более 45', 200)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (720, 7, 8, 1, 8,		N'Напряжения смещения на синхронных детекторах (СД) каждого канала:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (721, 7, 8, 2, NULL,	N'	Канал I-1', N'В', N'от +0,6 до +1,8', 32)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (722, 7, 8, 3, NULL,	N'	Канал I-2', N'В', N'от +0,6 до +1,8', 33)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (723, 7, 8, 4, NULL,	N'	Канал II-1', N'В', N'от +0,6 до +1,8', 34)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (724, 7, 8, 5, NULL,	N'	Канал II-2', N'В', N'от +0,6 до +1,8', 35)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (725, 7, 8, 6, NULL,	N'	Канал III-1', N'В', N'от +0,6 до +1,8', 36)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (726, 7, 8, 7, NULL,	N'	Канал III-2', N'В', N'от +0,6 до +1,8', 37)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (727, 7, 9, 1, 9,		N'Напряжение шумов на контрольных выходах УНЧ каждого канала:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (728, 7, 9, 2, NULL,	N'	Канал I-1', N'мВ', N'от 30 до 100', 38)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (729, 7, 9, 3, NULL,	N'	Канал I-2', N'мВ', N'от 30 до 100', 39)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (730, 7, 9, 4, NULL,	N'	Канал II-1', N'мВ', N'от 30 до 100', 40)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (731, 7, 9, 5, NULL,	N'	Канал II-2', N'мВ', N'от 30 до 100', 41)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (732, 7, 9, 6, NULL,	N'	Канал III-1', N'мВ', N'от 30 до 100', 42)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (733, 7, 9, 7, NULL,	N'	Канал III-2', N'мВ', N'от 30 до 100', 43)

--Величина опорного напряжения +10 В опор., выдаваемого блоком ВСС внешним потребителям
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (734, 7, 10, 1, 10,	N'Величина опорного напряжения +10 В опор. выдаваемого блоком ВСС внешним потребителям', N'В', N'+10,0 ± 0,5', 482) 

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (735, 7, 11, 1, 11,	N'Погрешность изделия по импульсному выходу', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (736, 7, 11, 2, NULL, N'	+Fд1 (номинал 250 Гц)', N'%', N'не более 0,6', 471)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (737, 7, 11, 3, NULL, N'	+Fд2 (номинал 500 Гц)', N'%', N'не более 0,6', 472)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (738, 7, 11, 4, NULL, N'	+Fд3 (номинал 250 Гц)', N'%', N'не более 0,6', 473)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (739, 7, 11, 5, NULL, N'	-Fд1 (номинал 250 Гц)', N'%', N'не более 0,6', 474)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (740, 7, 11, 6, NULL, N'	-Fд2 (номинал 500 Гц)', N'%', N'не более 0,6', 475)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (741, 7, 11, 7, NULL, N'	-Fд3 (номинал 250 Гц)', N'%', N'не более 0,6', 476)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (742, 7, 12, 1, 13,	N'Выходные напряжения блока ВСС при обнулении составляющих вектора скорости:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (743, 7, 12, 2, 0,	N'по выходам НКВ', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (744, 7, 12, 3, NULL, N'	Wxr', N'мВ', N'не более ±40', 391)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (745, 7, 12, 4, NULL, N'	Wzr', N'мВ', N'не более ±80', 392)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (746, 7, 12, 5, NULL, N'	Wyr', N'мВ', N'не более ±300', 393)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (747, 7, 12, 6, 0,	N'по выходам ПКВ', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (748, 7, 12, 7, NULL, N'	Wxr', N'мВ', N'не более ±330', 477)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (749, 7, 12, 8, NULL, N'	Wzr', N'мВ', N'не более ±230', 478)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (750, 7, 12, 9, NULL, N'	Wyr', N'мВ', N'не более ±300', 479)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (751, 7, 13, 1, 13,	N'Погрешность вычисления и индикации (на блоке 6) составляющих вектора полной скорости в режиме контрольных задач:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (752, 7, 13, 2, NULL, N'	ВПЕРЕД (Wxr, расчетное значение 17 км/ч)', N'км/ч', N'не более ±2,5', 515)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (753, 7, 13, 3, NULL, N'	ВЛЕВО (Wzr, расчетное значение 17 км/ч)', N'км/ч', N'не более ±2,5', 516)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (754, 7, 13, 4, NULL, N'	ВНИЗ (Wyr, расчетное значение 3.1 м/c)', N'м/c', N'не более ±0,5', 517)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (755, 7, 13, 5, NULL, N'	НАЗАД (Wxr, расчетное значение 17 км/ч)', N'км/ч', N'не более ±2,5', 518)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (756, 7, 13, 6, NULL, N'	ВПРАВО (Wzr, расчетное значение 17 км/ч)', N'км/ч', N'не более ±2,5', 519)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (757, 7, 13, 7, NULL, N'	ВВЕРХ (Wyr, расчетное значение 3.1 м/c)', N'м/c', N'не более ±0,5', 520)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (758, 7, 14, 1, 14,	N'Погрешность вычисления путевой скорости в режиме контрольных задач:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (759, 7, 14, 2, NULL, N'	ПС1 (расчетное значение 127 км/ч)', N'км/ч', N'не более ±2,0', 521)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (760, 7, 14, 3, NULL, N'	ПС2 (расчетное значение 258 км/ч)', N'км/ч', N'не более ±3,0', 522)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (761, 7, 15, 1, 15,	N'Погрешность вычисления и индикации (на индикаторе УСПС) путевой скорости в режиме контрольных задач:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (762, 7, 15, 2, NULL, N'	ПС1 (расчетное значение 127 км/ч)', N'км/ч', N'не более ±2,5', 525)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (763, 7, 15, 3, NULL, N'	ПС2 (расчетное значение 258 км/ч)', N'км/ч', N'не более ±3,5', 526)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (764, 7, 16, 1, 16,	N'Погрешность вычисления угла сноса в режиме контрольных задач:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (765, 7, 16, 2, NULL, N'	УС 0', N'град', N'не более ±0,5', 523)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (766, 7, 16, 3, NULL, N'	УС 9,5', N'град', N'не более ±0,5', 524)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (767, 7, 17, 1, 17,	N'Погрешность вычисления и индикации (на индикаторе УСПС) угла сноса в режиме контрольных задач:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (768, 7, 17, 2, NULL, N'	УС 0', N'град', N'не более ± 1,0', 527)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (769, 7, 17, 3, NULL, N'	УС 9,5', N'град', N'не более ± 1,0', 528)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (770, 7, 18, 1, 18,	N'При переходе изделия в режим «Память» :', N'-', N'-', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (771, 7, 18, 2, NULL, N'	изменение значения путевой скорости', N'км/ч', N'не более ± 3', 504)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (772, 7, 18, 3, NULL, N'	изменение значения угла сноса', N'град', N'не более ± 3', 505)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (773, 7, 19, 1, 19,	N'Вычисленное значение путевой скорости в режиме «Море» должно увеличиваться относительно значения, измеренного в режиме «Суша»:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (774, 7, 19, 2, NULL, N'	для подрежима «М-С»', N'км/ч', N'от 4,5 до 6,0', 483)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (775, 7, 19, 3, NULL, N'	для пожрежима «М-Б»', N'км/ч', N'от 2,0 до 3,0', 484)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (776, 7, 20, 1, 20,		N'Погрешность вычисления и индикации (на блоке 6 или индикаторе УСПС) в режиме контрольных задач при проверке изделия прибором БПК:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (777, 7, 20, 2, NULL,		N'	ВПЕРЕД (Wxr, расчетное значение 17 км/ч)', N'км/ч', N'не более ±2,5', 529)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (778, 7, 20, 3, NULL,		N'	ВЛЕВО (Wzr, расчетное значение 17 км/ч)', N'км/ч', N'не более ±2,5', 530)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (779, 7, 20, 4, NULL,		N'	ВНИЗ (Wyr, расчетное значение 3.1 м/c)', N'м/с', N'не более ±0,5', 531)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (780, 7, 20, 5, NULL,		N'	НАЗАД (Wxr, расчетное значение 17 км/ч)', N'км/ч', N'не более ±2,5', 532)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (781, 7, 20, 6, NULL,		N'	ВПРАВО (Wzr, расчетное значение 17 км/ч)', N'км/ч', N'не более ±2,5', 533)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (782, 7, 20, 7, NULL,		N'	ВВЕРХ (Wyr, расчетное значение 3.1 м/c)', N'м/с', N'не более ±0,5', 534)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (783, 7, 20, 8, 0,		N'при проверке с технологическим прибором коррекции', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (784, 7, 20, 9, NULL,		N'	ПС 1 (расчетное значение 127 км/ч)', N'км/ч', N'не более ±2,5', 535)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (785, 7, 20, 10, NULL,	N'	ПС 2 (расчетное значение 258 км/ч)', N'км/ч', N'не более ±3,5', 536)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (786, 7, 20, 11, 0,		N'при проверке со штатным прибором коррекции ', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (787, 7, 20, 12, NULL,	N'	ПС 1 (расчетное значение 127 км/ч)', N'км/ч', N'не более ±3,5', 539)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (788, 7, 20, 13, NULL,	N'	ПС 2 (расчетное значение 258 км/ч)', N'км/ч', N'не более ±5,0', 540)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (789, 7, 20, 14, NULL,	N'	УС 0 (расчетное значение 0°)', N'град', N'не более ±1,0', 537)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (790, 7, 20, 15, NULL,	N'	УС 9,5 (расчетное значение 9,5°)', N'град', N'не более ±1,0', 538)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (791, 7, 21, 1, 21,		N'Должна обеспечиваться световая сигнализация и выдача сигналов внешним потребителям:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (792, 7, 21, 2, NULL,		N'	режима «Навигация»', NULL, N'обеспечивается', 488)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (793, 7, 21, 3, NULL,		N'	режима «Контроль ДИСС»', NULL, N'обеспечивается', 211)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (794, 7, 21, 4, 0,		N'	режима «Память»:', NULL, N'', 487)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (795, 7, 21, 5, NULL,		N'		сигнал «Память 1»', NULL, N'обеспечивается', 510)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (796, 7, 21, 6, NULL,		N'		сигнал «Память 2»', NULL, N'обеспечивается', 511)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (797, 7, 21, 7, NULL,		N'	сигнала «Поиск»', NULL, N'обеспечивается', 209)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (798, 7, 21, 8, NULL,		N'	сигнала «Исправность»', NULL, N'обеспечивается', 212)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (799, 7, 21, 9, NULL,		N'	сигнала «Полет»', NULL, N'обеспечивается', 210)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (800, 7, 21, 10, NULL,	N'	сигнала «Отказ ВЧ»', NULL, N'обеспечивается', 206)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (801, 7, 21, 11, NULL,	N'	сигнала «Отказ ВСС»', NULL, N'обеспечивается', 207)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (802, 7, 22, 1, 22,	N'Время перехода из режима «Память» в режим «Захват»', N'сек', N' не более 15', 486)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (803, 7, 23, 1, 23,	N'Изделие не должно переходить из режима «Память» в режим «Захват» не по одному из каналов от собственных шумов приемного устройства', NULL, N'выполняется', 485)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (804, 7, 24, 1, 24, N'Время готовности к работе', N'мин', N' не более 3', 489)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (805, 7, 25, 1, 25, N'Частота излучаемой СВЧ энергии', N'МГц', N'13325,0 ± 7,5', 490)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (806, 7, 26, 1, 26, N'Мощность излучаемой СВЧ энергии', N'мВт', N'не менее 100', 491)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (807, 7, 27, 1, 27,	N'Электрическое сопротивление изоляции между токоведущими цепями и корпусом: ', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (808, 7, 27, 2, NULL, N'	Блок ВЧ.Ш34:38', N'МОм', N'не менее 20', 492)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (809, 7, 27, 3, NULL, N'	Блок ВЧ.Ш34:39', N'МОм', N'не менее 20', 493)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (810, 7, 27, 4, NULL, N'	Блок ВЧ.Ш35:1', N'МОм', N'не менее 20', 494)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (811, 7, 27, 5, NULL, N'	Блок ВЧ.Ш35:4', N'МОм', N'не менее 20', 495)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (812, 7, 27, 6, NULL, N'	Блок ВСС.Ш4:38', N'МОм', N'не менее 20', 498)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (813, 7, 27, 7, NULL, N'	Блок ВСС.Ш4:39', N'МОм', N'не менее 20', 499)
--DeviceType 8 (Diss32-90A)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (814, 8, 1, 1, 1,		N'Потребляемый ток от источников питания:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (815, 8, 1, 2, NULL,	N'	по цепи 27 В', N'А', N'не более 7', 468)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (816, 8, 1, 3, NULL,	N'	по цепи 115 Вб 400Гц', N'А', N'не более 4,2', 470)

--Напряжения вторичных источников питания, В: 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (817, 8, 2, 1, 2,		N'Напряжения вторичных источников питания: ', N'', N'', NULL) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (818, 8, 2, 2, NULL,	N'	+12,6 В', N'В', N'от +11,95 до +13,25', 502) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (819, 8, 2, 3, NULL,	N'	-12,6 В', N'В', N'от -11,95 до -13,25', 503) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (820, 8, 2, 4, NULL,	N'	-20 В', N'В', N'от -19 до -21', 11) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (821, 8, 2, 5, NULL,	N'	+6,3 В', N'В', N'от +6,0 до +6,6', 8) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (822, 8, 2, 6, NULL,	N'	-6,3 В', N'В', N'от -6,0 до -6,6', 10) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (823, 8, 2, 7, NULL,	N'	+15 В', N'В', N'от +14,25 до +15,75', 12) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (824, 8, 2, 8, NULL,	N'	-15 В', N'В', N'от -14,25 до -15,75', 13) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (825, 8, 2, 9, NULL,	N'	+5 В', N'В', N'от +4,75 до + 5,25', 15) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (826, 8, 2, 10, NULL, N'	-27 В', N'В', N'от -25,65 до -28,35', 16) 

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (827, 8, 3, 1, 3,		N'Контрольное напряжение генератора М55204 (ГЛПД):', N'', N'', NULL) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (828, 8, 3, 2, NULL,	N'	ГЛПД1', N'В', N'от 2,4 до 3,5', 18) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (829, 8, 3, 3, NULL,	N'	ГЛПД2', N'В', N'от 2,4 до 3,5', 19) 

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (830, 8, 4, 1, 4,		N'Контрольное напряжение, пропорциональное общему току генераторных диодов', N'В', N'от -1,7 до -2,9', 20)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (831, 8, 5, 1, 5,		N'Напряжение смещения на модуляторных диодах:', N'', N'', NULL) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (832, 8, 5, 2, NULL,	N'	МОД 1', N'В', N'от +2,9 до +4,3', 21) 
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (833, 8, 5, 3, NULL,	N'	МОД 2', N'В', N'от -2,9 до -4,3', 22) 

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (834, 8, 6, 1, 6,		N'Разность напряжений смещения на диодах смесителей «Ток К» каналов I, II, III до включения и после выключения СВЧ-мощности:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (835, 8, 6, 2, NULL,	N'	Канал I-1', N'мВ', N'от 50 до 120', 461)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (836, 8, 6, 3, NULL,	N'	Канал I-2', N'мВ', N'от 50 до 120', 462)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (837, 8, 6, 4, NULL,	N'	Канал II-1', N'мВ', N'от 50 до 120', 463)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (838, 8, 6, 5, NULL,	N'	Канал II-2', N'мВ', N'от 50 до 120', 464)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (839, 8, 6, 6, NULL,	N'	Канал III-1', N'мВ', N'от 50 до 120', 465)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (840, 8, 6, 7, NULL,	N'	Канал III-2', N'мВ', N'от 50 до 120', 466)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (841, 8, 7, 1, 7,		N'Разброс абсолютных значений разности напряжений в паре: ', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (842, 8, 7, 2, NULL,	N'	Канал I', N'мВ', N'не более 45', 198)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (843, 8, 7, 3, NULL,	N'	Канал II', N'мВ', N'не более 45', 199)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (844, 8, 7, 4, NULL,	N'	Канал III', N'мВ', N'не более 45', 200)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (845, 8, 8, 1, 8,		N'Напряжения смещения на синхронных детекторах (СД) каждого канала:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (846, 8, 8, 2, NULL,	N'	Канал I-1', N'В', N'от +0,6 до +1,8', 32)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (847, 8, 8, 3, NULL,	N'	Канал I-2', N'В', N'от +0,6 до +1,8', 33)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (848, 8, 8, 4, NULL,	N'	Канал II-1', N'В', N'от +0,6 до +1,8', 34)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (849, 8, 8, 5, NULL,	N'	Канал II-2', N'В', N'от +0,6 до +1,8', 35)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (850, 8, 8, 6, NULL,	N'	Канал III-1', N'В', N'от +0,6 до +1,8', 36)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (851, 8, 8, 7, NULL,	N'	Канал III-2', N'В', N'от +0,6 до +1,8', 37)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (852, 8, 9, 1, 9,		N'Напряжение шумов на контрольных выходах УНЧ каждого канала:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (853, 8, 9, 2, NULL,	N'	Канал I-1', N'мВ', N'от 30 до 100', 38)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (854, 8, 9, 3, NULL,	N'	Канал I-2', N'мВ', N'от 30 до 100', 39)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (855, 8, 9, 4, NULL,	N'	Канал II-1', N'мВ', N'от 30 до 100', 40)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (856, 8, 9, 5, NULL,	N'	Канал II-2', N'мВ', N'от 30 до 100', 41)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (857, 8, 9, 6, NULL,	N'	Канал III-1', N'мВ', N'от 30 до 100', 42)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (858, 8, 9, 7, NULL,	N'	Канал III-2', N'мВ', N'от 30 до 100', 43)

--Величина опорного напряжения +10 В опор., выдаваемого блоком ВСС внешним потребителям
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (859, 8, 10, 1, 10,	N'Величина опорного напряжения +10 В опор. выдаваемого блоком ВСС внешним потребителям', N'В', N'+10,0 ± 0,5', 482) 

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (860, 8, 11, 1, 11,	N'Погрешность изделия по импульсному выходу', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (861, 8, 11, 2, NULL, N'	+Fд1 (номинал 250 Гц)', N'%', N'не более 0,6', 471)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (862, 8, 11, 3, NULL, N'	+Fд2 (номинал 500 Гц)', N'%', N'не более 0,6', 472)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (863, 8, 11, 4, NULL, N'	+Fд3 (номинал 250 Гц)', N'%', N'не более 0,6', 473)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (864, 8, 11, 5, NULL, N'	-Fд1 (номинал 250 Гц)', N'%', N'не более 0,6', 474)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (865, 8, 11, 6, NULL, N'	-Fд2 (номинал 500 Гц)', N'%', N'не более 0,6', 475)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (866, 8, 11, 7, NULL, N'	-Fд3 (номинал 250 Гц)', N'%', N'не более 0,6', 476)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (867, 8, 12, 1, 13,	N'Выходные напряжения блока ВСС при обнулении составляющих вектора скорости:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (868, 8, 12, 2, 0,	N'по выходам НКВ', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (869, 8, 12, 3, NULL, N'	Wxr', N'мВ', N'не более ±40', 391)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (870, 8, 12, 4, NULL, N'	Wzr', N'мВ', N'не более ±80', 392)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (871, 8, 12, 5, NULL, N'	Wyr', N'мВ', N'не более ±300', 393)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (872, 8, 12, 6, 0,	N'по выходам ПКВ', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (873, 8, 12, 7, NULL, N'	Wxr', N'мВ', N'не более ±330', 477)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (874, 8, 12, 8, NULL, N'	Wzr', N'мВ', N'не более ±230', 478)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (875, 8, 12, 9, NULL, N'	Wyr', N'мВ', N'не более ±300', 479)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (876, 8, 13, 1, 13,	N'Погрешность вычисления и индикации (на блоке 6) составляющих вектора полной скорости в режиме контрольных задач:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (877, 8, 13, 2, NULL, N'	ВПЕРЕД (Wxr, расчетное значение 17 км/ч)', N'км/ч', N'не более ±2,5', 515)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (878, 8, 13, 3, NULL, N'	ВЛЕВО (Wzr, расчетное значение 17 км/ч)', N'км/ч', N'не более ±2,5', 516)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (879, 8, 13, 4, NULL, N'	ВНИЗ (Wyr, расчетное значение 3.1 м/c)', N'м/c', N'не более ±0,5', 517)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (880, 8, 13, 5, NULL, N'	НАЗАД (Wxr, расчетное значение 17 км/ч)', N'км/ч', N'не более ±2,5', 518)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (881, 8, 13, 6, NULL, N'	ВПРАВО (Wzr, расчетное значение 17 км/ч)', N'км/ч', N'не более ±2,5', 519)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (882, 8, 13, 7, NULL, N'	ВВЕРХ (Wyr, расчетное значение 3.1 м/c)', N'м/c', N'не более ±0,5', 520)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (883, 8, 14, 1, 14,	N'Погрешность вычисления путевой скорости в режиме контрольных задач:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (884, 8, 14, 2, NULL, N'	ПС1 (расчетное значение 127 км/ч)', N'км/ч', N'не более ±2,0', 521)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (885, 8, 14, 3, NULL, N'	ПС2 (расчетное значение 258 км/ч)', N'км/ч', N'не более ±3,0', 522)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (886, 8, 15, 1, 15,	N'Погрешность вычисления и индикации (на индикаторе УСПС) путевой скорости в режиме контрольных задач:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (887, 8, 15, 2, NULL, N'	ПС1 (расчетное значение 127 км/ч)', N'км/ч', N'не более ±2,5', 525)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (888, 8, 15, 3, NULL, N'	ПС2 (расчетное значение 258 км/ч)', N'км/ч', N'не более ±3,5', 526)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (889, 8, 16, 1, 16,	N'Погрешность вычисления угла сноса в режиме контрольных задач:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (890, 8, 16, 2, NULL, N'	УС 0', N'град', N'не более ±0,5', 523)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (891, 8, 16, 3, NULL, N'	УС 9,5', N'град', N'не более ±0,5', 524)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (892, 8, 17, 1, 17,	N'Погрешность вычисления и индикации (на индикаторе УСПС) угла сноса в режиме контрольных задач:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (893, 8, 17, 2, NULL, N'	УС 0', N'град', N'не более ± 1,0', 527)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (894, 8, 17, 3, NULL, N'	УС 9,5', N'град', N'не более ± 1,0', 528)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (895, 8, 18, 1, 18,	N'При переходе изделия в режим «Память» :', N'-', N'-', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (896, 8, 18, 2, NULL, N'	изменение значения путевой скорости', N'км/ч', N'не более ± 3', 504)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (897, 8, 18, 3, NULL, N'	изменение значения угла сноса', N'град', N'не более ± 3', 505)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (898, 8, 19, 1, 19,	N'Вычисленное значение путевой скорости в режиме «Море» должно увеличиваться относительно значения, измеренного в режиме «Суша»:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (899, 8, 19, 2, NULL, N'	для подрежима «М-С»', N'км/ч', N'от 4,5 до 6,0', 483)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (900, 8, 19, 3, NULL, N'	для пожрежима «М-Б»', N'км/ч', N'от 2,0 до 3,0', 484)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (901, 8, 20, 1, 20,		N'Погрешность вычисления и индикации (на блоке  6 или индикаторе УСПС) в режиме контрольных задач при проверке изделия прибором БПК:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (902, 8, 20, 2, NULL,		N'	ВПЕРЕД (Wxr, расчетное значение 17 км/ч)', N'км/ч', N'не более ±2,5', 529)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (903, 8, 20, 3, NULL,		N'	ВЛЕВО (Wzr, расчетное значение 17 км/ч)', N'км/ч', N'не более ±2,5', 530)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (904, 8, 20, 4, NULL,		N'	ВНИЗ (Wyr, расчетное значение 3.1 м/c)', N'м/с', N'не более ±0,5', 531)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (905, 8, 20, 5, NULL,		N'	НАЗАД (Wxr, расчетное значение 17 км/ч)', N'км/ч', N'не более ±2,5', 532)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (906, 8, 20, 6, NULL,		N'	ВПРАВО (Wzr, расчетное значение 17 км/ч)', N'км/ч', N'не более ±2,5', 533)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (907, 8, 20, 7, NULL,		N'	ВВЕРХ (Wyr, расчетное значение 3.1 м/c)', N'м/с', N'не более ±0,5', 534)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (908, 8, 20, 8, 0,		N'при проверке с технологическим прибором коррекции', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (909, 8, 20, 9, NULL,		N'	ПС 1 (расчетное значение 127 км/ч)', N'км/ч', N'не более ±2,5', 535)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (910, 8, 20, 10, NULL,	N'	ПС 2 (расчетное значение 258 км/ч)', N'км/ч', N'не более ±3,5', 536)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (911, 8, 20, 11, 0,		N'при проверке со штатным прибором коррекции ', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (912, 8, 20, 12, NULL,	N'	ПС 1 (расчетное значение 127 км/ч)', N'км/ч', N'не более ±3,5', 539)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (913, 8, 20, 13, NULL,	N'	ПС 2 (расчетное значение 258 км/ч)', N'км/ч', N'не более ±5,0', 540)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (914, 8, 20, 14, NULL,	N'	УС 0 (расчетное значение 0°)', N'град', N'не более ±1,0', 537)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (915, 8, 20, 15, NULL,	N'	УС 9,5 (расчетное значение 9,5°)', N'град', N'не более ±1,0', 538)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (916, 8, 21, 1, 21,		N'Должна обеспечиваться световая сигнализация и выдача сигналов внешним потребителям:', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (917, 8, 21, 2, NULL,		N'	режима «Навигация»', NULL, N'обеспечивается', 488)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (918, 8, 21, 3, NULL,		N'	режима «Контроль ДИСС»', NULL, N'обеспечивается', 211)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (919, 8, 21, 4, 0,		N'	режима «Память»:', NULL, N'', 487)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (920, 8, 21, 5, NULL,		N'		сигнал «Память 1»', NULL, N'обеспечивается', 510)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (921, 8, 21, 6, NULL,		N'		сигнал «Память 2»', NULL, N'обеспечивается', 511)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (922, 8, 21, 7, NULL,		N'	сигнала «Поиск»', NULL, N'обеспечивается', 209)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (923, 8, 21, 8, NULL,		N'	сигнала «Исправность»', NULL, N'обеспечивается', 212)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (924, 8, 21, 9, NULL,		N'	сигнала «Полет»', NULL, N'обеспечивается', 210)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (925, 8, 21, 10, NULL,	N'	сигнала «Отказ ВЧ»', NULL, N'обеспечивается', 206)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (926, 8, 21, 11, NULL,	N'	сигнала «Отказ ВСС»', NULL, N'обеспечивается', 207)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (927, 8, 22, 1, 22,	N'Время перехода из режима «Память» в режим «Захват»', N'сек', N' не более 15', 486)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (928, 8, 23, 1, 23,	N'Изделие не должно переходить из режима «Память» в режим «Захват» не по одному из каналов от собственных шумов приемного устройства', NULL, N'выполняется', 485)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (929, 8, 24, 1, 24, N'Время готовности к работе', N'мин', N' не более 3', 489)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (930, 8, 25, 1, 25, N'Частота излучаемой СВЧ энергии', N'МГц', N'13325,0 ± 7,5', 490)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (931, 8, 26, 1, 26, N'Мощность излучаемой СВЧ энергии', N'мВт', N'не менее 100', 491)

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (932, 8, 27, 1, 27, N'Электрическое сопротивление изоляции между токоведущими цепями и корпусом ', N'', N'', NULL)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (933, 8, 27, 2, NULL, N'	Блок ВЧ.Ш34:38', N'МОм', N'не менее 20', 492)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (934, 8, 27, 3, NULL, N'	Блок ВЧ.Ш34:39', N'МОм', N'не менее 20', 493)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (935, 8, 27, 4, NULL, N'	Блок ВЧ.Ш35:1', N'МОм', N'не менее 20', 494)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (936, 8, 27, 5, NULL, N'	Блок ВЧ.Ш35:4', N'МОм', N'не менее 20', 495)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (937, 8, 27, 6, NULL, N'	Блок ВСС.Ш4:38', N'МОм', N'не менее 20', 498)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (938, 8, 27, 7, NULL, N'	Блок ВСС.Ш4:39', N'МОм', N'не менее 20', 499)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (939, 8, 27, 8, NULL, N'	Блок БВК.Ш2:41', N'МОм', N'не менее 20', 496)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (940, 8, 27, 9, NULL, N'	Блок БВК.Ш2:40', N'МОм', N'не менее 20', 497)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (941, 8, 27, 10, NULL, N'	Блок 8.Ш2:49', N'МОм', N'не менее 20', 500)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (942, 8, 27, 11, NULL, N'	Блок 8.Ш2:50', N'МОм', N'не менее 20', 501)

SET IDENTITY_INSERT [dbo].[Report] OFF

GO
SET IDENTITY_INSERT [dbo].[DescriptionParam] ON
GO
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (460, N'Ток от источника притания 27В', N'Plus_27V_32_I', 0, 7, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (461, N'Ток K 1-1 (ДИСС-32) Дельта вкл-выкл', N'I_K_1_1_Delta_WorkTest', 50, 120, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (462, N'Ток K 1-2 (ДИСС-32) Дельта вкл-выкл', N'I_K_1_2_Delta_WorkTest', 50, 120, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (463, N'Ток K 2-1 (ДИСС-32) Дельта вкл-выкл', N'I_K_2_1_Delta_WorkTest', 50, 120, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (464, N'Ток K 2-1 (ДИСС-32) Дельта вкл-выкл', N'I_K_2_2_Delta_WorkTest', 50, 120, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (465, N'Ток K 3-1 (ДИСС-32) Дельта вкл-выкл', N'I_K_3_1_Delta_WorkTest', 50, 120, NULL, N'COM100', N'Команды не определены', 2)
INSERT [dbo].[DescriptionParam] ([ParamID], [Note], [ParamName], [SetMinValue], [SetMaxValue], [UnitsMeasurement], [COM_Port], [Commands], [TimeOut]) VALUES (466, N'Ток K 3-2 (ДИСС-32) Дельта вкл-выкл', N'I_K_3_2_Delta_WorkTest', 50, 120, NULL, N'COM100', N'Команды не определены', 2)
GO
SET IDENTITY_INSERT [dbo].[DescriptionParam] OFF

GO
SET IDENTITY_INSERT [dbo].[Parameters] ON
GO
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (461, N'I_K_1_1_Delta_WorkTest', 0, N'44', 1, 50, 120, 1, N'Ток К 1-1 Делта (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (462, N'I_K_1_2_Delta_WorkTest', 0, N'44', 1, 50, 120, 1, N'Ток К 1-2 Делта (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (463, N'I_K_2_1_Delta_WorkTest', 0, N'44', 1, 50, 120, 1, N'Ток К 2-1 Делта (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (464, N'I_K_2_2_Delta_WorkTest', 0, N'44', 1, 50, 120, 1, N'Ток К 2-2 Делта (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (465, N'I_K_3_1_Delta_WorkTest', 0, N'44', 1, 50, 120, 1, N'Ток К 3-1 Делта (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (466, N'I_K_3_2_Delta_WorkTest', 0, N'44', 1, 50, 120, 1, N'Ток К 3-2 Делта (ДИСС32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (467, N'Plus_27V_32_I', 7, N'', 1, 0, 7, 1, N'ток 27В', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (468, N'Plus_27V_32(90A)_I', 7, N'', 1, 0, 10.5, 1, N'ток 27В', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (469, N'Plus_115V_32_I', 7, N'', 1, 0, 4.2, 1, N'ток 115В', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (470, N'Plus_115V_32(90A)_I', 7, N'', 1, 0, 4.7, 1, N'ток 115В', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (471, N'Plus_Fd1', 0, N'246', 1, 0, 0.6, 1, N'+Fд1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (472, N'Plus_Fd2', 0, N'247', 1, 0, 0.6, 1, N'+Fд2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (473, N'Plus_Fd3', 0, N'248', 1, 0, 0.6, 1, N'+Fд3', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (474, N'Minus_Fd1', 0, N'249', 1, 0, 0.6, 1, N'-Fд1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (475, N'Minus_Fd2', 0, N'250', 1, 0, 0.6, 1, N'-Fд2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (476, N'Minus_Fd3', 0, N'251', 1, 0, 0.6, 1, N'-Fд3', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (477, N'PKV_X_Zero32_Diss32', 0, N'396', 1, -330, 330, 1, N'Wx PKV (ДИСС32) Обн. для ДИСС-32', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (478, N'PKV_Z_Zero32_Diss32', 0, N'397', 1, -230, 230, 1, N'Wz PKV (ДИСС32) Обн. для ДИСС-32', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (479, N'PKV_Y_Zero32_Diss32', 0, N'398', 1, -300, 300, 1, N'Wy PKV (ДИСС32) Обн. для ДИСС-32', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (480, N'Task3_Y20_32_WorkTest', 0, N'320', 1, -0.5, 0.5, 1, N'УС 0 КЗ-3 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (481, N'Task4_Y29_32_WorkTest', 0, N'325', 1, 9.0, 10.0, 1, N'ДИСС32.УСПС2.КЗ3.Угол 9,5', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (482, N'VSS_10V_WorkTest', 0, N'292', 1, 9.5, 10.5, 2, N'Опорное напряжение +10', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (483, N'Mode_M_C_WorkTest', 0, N'190', 1, 4.5, 6.0, 1, N'Режим "М-С" (КЗ-4 ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (484, N'Mode_M_B_WorkTest', 0, N'191', 1, 2, 3, 1, N'Режим "М-Б" (КЗ-4 ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (485, N'NotTransitioFromMemoryToCapure', 7, N'', 1, 0, 1, 1, N'Изделие не должно переходить из режима «Память» в режим «Захват» от собственных шумов', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (486, N'TimeTransitioFromMemoryToCapure', 7, N'', 1, 0, 15, 1, N'Время перехода из режима «Память» в режим «Захват»', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (487, N'ModeMemory_WorkTest', 7, N'', 1, 1, 1, 1, N'Режим "Память"', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (488, N'ModeNavigation_WorkTest', 7, N'', 1, 1, 1, 1, N'Режим "Навигация"', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (489, N'ReadyTime', 7, N'', 1, 0, 3, 2, N'Время готовности к работе"', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (490, N'Mnl_F_32_WorkTest', 7, N'', 1, 13317.5, 13332.5, 2, N'Частота СВЧ колебаний (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (491, N'Power_Mnl_F_32', 7, N'258', 1, 100, 10000, 1, N'Мощность излучаемой СВЧ', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (492, N'ElecResist_VCH_H34_38', 7, N'', 1, 20, 10000, 1, N'ВЧ.Ш34:38', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (493, N'ElecResist_VCH_H34_39', 7, N'', 1, 20, 10000, 1, N'ВЧ.Ш34:39', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (494, N'ElecResist_VCH_H35_1', 7, N'', 1, 20, 10000, 1, N'ВЧ.Ш35:1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (495, N'ElecResist_VCH_H35_4', 7, N'', 1, 20, 10000, 1, N'ВЧ.Ш35:4', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (496, N'ElecResist_BVK_H2_41', 7, N'', 1, 20, 10000, 1, N'БВК.Ш2:41', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (497, N'ElecResist_BVK_H2_40', 7, N'', 1, 20, 10000, 1, N'БВК.Ш2:40', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (498, N'ElecResist_VSS_H4_38', 7, N'', 1, 20, 10000, 1, N'ВСС.Ш4:38', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (499, N'ElecResist_VSS_H4_39', 7, N'', 1, 20, 10000, 1, N'ВСС.Ш4:39', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (500, N'ElecResist_BL_H2_49', 7, N'', 1, 20, 10000, 1, N'Бл8.Ш2:49', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (501, N'ElecResist_BL_H2_50', 7, N'', 1, 20, 10000, 1, N'Бл8.Ш2:50', N'')

INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (502, N'Plus_12V_WorkTest', 7, N'2', 2, 11.95, 13.25, 2, N'[АТТ][ДИСС32] +12В', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (503, N'Minus_12V_WorkTest', 7, N'3', 2, -13.25, -11.95, 2, N'[АТТ][ДИСС32] -12В', N'')

INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (504, N'DeltaTask4_PS_WorkTest', 7, N'', 1, -3.00, 3.00, 2, N'Дельта ПС', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (505, N'DeltaTask4_US_WorkTest', 7, N'', 1, -3.00, 3.00, 2, N'Дельта УС', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (506, N'DeltaTask4_PS_2_WorkTest', 7, N'', 1, -3.00, 3.00, 2, N'Дельта ПС', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (507, N'DeltaTask4_US_2_WorkTest', 7, N'', 1, -3.00, 3.00, 2, N'Дельта УС', N'')

INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (508, N'Mode_M_C_2_WorkTest', 0, N'326', 1, 4.5, 6.0, 1, N'Режим "М-С" (КЗ-4 ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (509, N'Mode_M_B_2_WorkTest', 0, N'327', 1, 2, 3, 1, N'Режим "М-Б" (КЗ-4 ДИСС-32)', N'')

INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (510, N'ModeMemory1_WorkTest', 7, N'', 1, 1, 1, 1, N'Сигнал ПАМЯТЬ 1', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (511, N'ModeMemory2_WorkTest', 7, N'', 1, 1, 1, 1, N'Сигнал ПАМЯТЬ 2', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (512, N'ElecResist_WorkTest', 7, N'', 1, 1, 1, 1, N'Электрическое сопротивление изоляции между токоведущими цепями и корпусом', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (513, N'ModeVelocityF_WorkTest', 7, N'', 1, 1, 1, 1, N'Электрическое сопротивление изоляции между токоведущими цепями и корпусом', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (514, N'ModeVelocity258_WorkTest', 7, N'', 1, 1, 1, 1, N'Электрическое сопротивление изоляции между токоведущими цепями и корпусом', N'')

INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (515, N'Mnl_Forward_32_RepairTest', 0, N'', 1, -2.5, 2.5, 2, N'Погрешность индикации Вперед-17 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (516, N'Mnl_Left_32_RepairTest', 0, N'', 1, -2.5, 2.5, 2, N'Погрешность индикации Влево-17 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (517, N'Mnl_Down_32_RepairTest', 0, N'', 1, -0.5, 0.5, 2, N'Погрешность индикации Вниз-3,1 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (518, N'Mnl_Back_32_RepairTest', 0, N'', 1, -2.5, 2.5, 2, N'Погрешность индикации Назад-17 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (519, N'Mnl_Right_32_RepairTest', 0, N'', 1, -2.5, 2.5, 2, N'Погрешность индикации Вправо-17 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (520, N'Mnl_Up_32_RepairTest', 0, N'', 1, -0.5, 0.5, 2, N'Погрешность индикации Вверх-3,1 (ДИСС-32)', N'')

INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (521, N'Task3_PC_32_RepairTest', 9, N'47', -39.8406374501992, -2.0, 2.0, 1, N'ПС 127 КЗ-3 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (522, N'Task4_PC_32_RepairTest', 9, N'47', -39.8406374501992, -3.0, 3.0, 1, N'ПС 258 КЗ-4 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (523, N'Task3_Y0_32_RepairTest', 9, N'51', 2.2, -0.5, 0.5, 1, N'УС 0 КЗ-3 (ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (524, N'Task4_Y9_32_RepairTest', 9, N'51', -2.2, -0.5, 0.5, 1, N'УС 9,5 КЗ-3 (ДИСС-32)', N'')


INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (525, N'Mnl_Spd_Task3_32_RepairTest', 0, N'', 1, -2.5, 2.5, 2, N'Вручную ПС1(-127) КЗ-3 (УС ПС №1)(ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (526, N'Mnl_Spd_Task4_32_RepairTest', 0, N'', 1, -3.5, 3.5, 2, N'Вручную ПС2(-258) КЗ-4 (УС ПС №1)(ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (527, N'Mnl_Alfa_0_32_RepairTest', 0, N'', 1, -1, 1, 2, N'Вручную УС 0 КЗ-3 (УС ПС №1)(ДИСС-32)', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (528, N'Mnl_Alfa_9_32_RepairTest', 0, N'', 1, -1, 1, 2, N'Вручную УС 9,5 КЗ-4 (УС ПС №1)(ДИСС-32)', N'')


INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (529, N'BPK_Forward_32_RepairTest', 0, N'', 1, -2.5, 2.5, 1, N'БПК (ДИСС-32) Вперед-17', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (530, N'BPK_Left_32_RepairTest', 0, N'', 1, -2.5, 2.5, 1, N'БПК (ДИСС-32) Влево-17', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (531, N'BPK_Down_32_RepairTest', 0, N'', 1, -0.5, 0.5, 1, N'БПК (ДИСС-32) Вниз-3', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (532, N'BPK_Back_32_RepairTest', 0, N'', 1, -2.5, 2.5, 1, N'БПК (ДИСС-32) Назад-17', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (533, N'BPK_Right_32_RepairTest', 0, N'', 1, -2.5, 2.5, 1, N'БПК (ДИСС-32) Вправо-17', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (534, N'BPK_Up_32_RepairTest', 0, N'', 1, -0.5, 0.5, 1, N'БПК (ДИСС-32) Вверх-3', N'')

INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (535, N'BPK_PC1_32_RepairTest', 0, N'', 1, -2.5, 2.5, 1, N'БПК (ДИСС-32) ПС1-127', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (536, N'BPK_PC2_32_RepairTest', 0, N'', 1, -3.5, 3.5, 1, N'БПК (ДИСС-32) ПС2-258', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (537, N'BPK_YC0_32_RepairTest', 0, N'', 1, -1, 1, 1, N'БПК (ДИСС-32) УС0', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (538, N'BPK_YC9_32_RepairTest', 0, N'', 1, -1, 1, 1, N'БПК (ДИСС-32) УС 9,5', N'')

INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (539, N'BPK_PC1_32_FlightVSS_RepairTest', 0, N'', 1, -3.5, 3.5, 1, N'БПК (ДИСС-32) ПС1-127 Штатный ВСС', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (540, N'BPK_PC2_32_FlightVSS_RepairTest', 0, N'', 1, -5.0, 5.0, 1, N'БПК (ДИСС-32) ПС2-258 Штатный ВСС', N'')


GO
SET IDENTITY_INSERT [dbo].[Parameters] OFF


-- =============================================
-- Author:		Kruglikov Igor
-- Create date: 16.04.2026
-- Description:	В таблицу [AdditionalParameterReference] добавлена новая переменная [NV400V]
-- для определения стабильности напряжения 400 В относительно фактического измеренного напряжения
-- =============================================
GO
USE [KPK2]
GO
SET IDENTITY_INSERT [dbo].[AdditionalParameterReference] ON 

IF NOT EXISTS (SELECT 1 FROM dbo.AdditionalParameterReference WHERE ParamId = 10)
BEGIN
INSERT [dbo].[AdditionalParameterReference] ([ParamId], [ParamName]) VALUES (10, N'NV400V')
END

SET IDENTITY_INSERT [dbo].[AdditionalParameterReference] OFF
UPDATE [dbo].[Report]
SET [CheckName] = REPLACE(REPLACE(REPLACE(REPLACE(
    [CheckName],
    '    при 400В:', 'источника 400 В:'),
    '    при 485В:', 'источника 500 В при установке 485 В:'),
    '    при 515В:', 'источника 500 В при установке 515 В:'),
    '    при 6,3В:', 'источника 6,3 В')
WHERE [CheckName] LIKE '%    при 400В:%'
   OR [CheckName] LIKE '%    при 485В:%'
   OR [CheckName] LIKE '%    при 515В:%'
   OR [CheckName] LIKE '%    при 6,3В:%';

GO
UPDATE [dbo].[Version]
SET DBVersion = '2.0.8'
WHERE Id = 1
