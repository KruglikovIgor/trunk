-- KPK-2 DB Patch
-- From:    2.0.7
-- To:      2.0.8

-- =============================================
-- Author:		Kruglikov Igor
-- Create date: 16.04.2026
-- Description:	В таблицу [AdditionalParameterReference] добавлена новая переменная [NV400V]
-- для определения стабильности напряжения 400 В относительно фактического измеренного напряжения
-- =============================================
USE KPK2
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
