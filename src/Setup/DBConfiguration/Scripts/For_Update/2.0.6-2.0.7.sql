-- KPK-2 DB Patch
-- From:    2.0.6
-- To:      2.0.7

USE KPK2
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TestModes]') AND type = N'U')
CREATE TABLE [dbo].[TestModes]
(Id int PRIMARY KEY, Name NVARCHAR(100))

INSERT [TestModes] 
VALUES (1, 'NormalTest'),
		(2, 'WorkTest')

ALTER TABLE [Testing]
ADD IdTestMode INT NOT NULL DEFAULT 1,
	CONSTRAINT FK_Testing_To_TestModes FOREIGN KEY(IdTestMode) REFERENCES TestModes(Id);
	
ALTER TABLE [Testing]
ADD MestreNameId INT NULL DEFAULT NULL

ALTER TABLE [Testing]
ADD MestreBTKNameId INT NULL DEFAULT NULL

ALTER TABLE [dbo].[DeviceType]
ADD IdMode INT NOT NULL DEFAULT 1
	CONSTRAINT FK_DeviceType_To_TestModes FOREIGN KEY(IdMode) REFERENCES TestModes(Id);


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

GO

UPDATE [dbo].[Version]
SET DBVersion = '2.0.7'
WHERE Id = 1