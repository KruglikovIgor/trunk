-- KPK-2 DB Patch
-- From:    2.0.1
-- To:      2.0.2

USE KPK2
GO
SET IDENTITY_INSERT [dbo].[Parameters] ON 

GO
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (455, N'BPK_PC1_2_32', 0, N'', 1, 124.5, 129.5, 1, N'БПК (ДИСС-32) ПС1-127', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (456, N'BPK_YC0_2_32', 0, N'', 1, -1, 1, 1, N'БПК (ДИСС-32) УС0', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (457, N'BPK_PC2_2_32', 0, N'', 1, 254.5, 261.5, 1, N'БПК (ДИСС-32) ПС2-258', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (458, N'BPK_YC9_2_32', 0, N'', 1, -1, 1, 1, N'БПК (ДИСС-32) УС 9,5', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (459, N'BPK_PC1_2_32_FlightVSS', 0, N'', 1, 124.5, 129.5, 1, N'БПК (ДИСС-32) ПС1-127 Штатный ВСС', N'')
INSERT [dbo].[Parameters] ([ParamId], [ParamName], [DeviceId], [Command], [Koef], [MinValue], [MaxValue], [Precision], [Note], [AddCommand]) VALUES (460, N'BPK_PC2_2_32_FlightVSS', 0, N'', 1, 253, 263, 1, N'БПК (ДИСС-32) ПС2-258 Штатный ВСС', N'')
GO
SET IDENTITY_INSERT [dbo].[Parameters] OFF 
GO

SET IDENTITY_INSERT [dbo].[Report] ON 
GO

update [dbo].[Report]
set SubItem = SubItem+2
where Id IN (248, 249, 250)
GO

update [dbo].[Report]
set CheckName = CheckName+' №1'
where Id IN (246, 247, 249, 250)
GO

INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (685, 2, 16, 13, NULL, N'         ПС №2', N'км/ч', N'127 ± 2,5', 455)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (686, 2, 16, 14, NULL, N'         УС №2', N'град', N'0 ± 1', 456)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (687, 2, 16, 18, NULL, N'         ПС №2', N'км/ч', N'258 ± 3,5', 457)
INSERT [dbo].[Report] ([Id], [DeviceTypeId], [Item], [SubItem], [OrderNumber], [CheckName], [Units], [Requirement], [ParamId]) VALUES (688, 2, 16, 19, NULL, N'         УС №2', N'град', N'9.5 ± 1', 458)
GO

SET IDENTITY_INSERT [dbo].[Report] OFF
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
ALTER PROCEDURE [dbo].[GetReportValues]
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

update [Report]
set CheckName = REPLACE(CheckName, N'Погрешность', N'Точность')
where Id IN (147, 154, 159, 256, 359, 266, 269, 463, 470, 475)

update [Report]
set CheckName = N'Точность вычисления и индикации выходных параметров при проверке от БПК:'
where Id = 187

update [Report]
set CheckName = N'Точность вычисления и индикации выходных параметров при проверке от БПК:'
where Id = 503

update [Parameters]
set MinValue = 14.5, MaxValue = 19.5
where [ParamId] IN (217, 218, 220, 221)

update [Parameters]
set MinValue = 2.5, MaxValue = 3.5
where [ParamId] IN (219, 222)

update [Parameters]
set MinValue = 124.5, MaxValue = 129.5
where [ParamId] IN (223, 455)

update [Parameters]
set MinValue = 254.5, MaxValue = 261.5
where [ParamId] IN (225, 457)

update [Parameters]
set MinValue = 8.5, MaxValue = 10.5
where [ParamId] IN (226, 458)

update [Parameters]
set MinValue = 123.5, MaxValue = 130.5
where [ParamId] IN (387, 459)

update [Parameters]
set MinValue = 253, MaxValue = 263
where [ParamId] IN (388, 460)

update [Report]
set Requirement = N'17 ± 2,5'
where [Id] IN (189, 190, 193, 194, 505, 506, 509, 510)

update [Report]
set Requirement = N'3,1 ± 0,5'
where [Id] IN (191,195, 507, 511)

update [Report]
set Requirement = N'127 ± 2,5'
where [Id] IN (246,562)

update [Report]
set Requirement = N'258 ± 3,5'
where [Id] IN (249, 565)

update [Report]
set Requirement = N'9,5 ± 1'
where [Id] IN (250, 566)

GO

UPDATE [dbo].[Version]
SET DBVersion = '2.0.2'
WHERE Id = 1