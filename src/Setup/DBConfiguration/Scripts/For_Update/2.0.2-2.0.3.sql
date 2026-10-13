-- KPK-2 DB Patch
-- From:    2.0.2
-- To:      2.0.3

USE KPK2
GO

update [dbo].[Report]
set Requirement = N'более Полосы I' + CHAR(10) + N'и не более 130'
where Id IN (42, 43, 44, 45, 46, 47)

update [dbo].[Report]
set CheckName = N'Разность напряжений смещения на диодах смесителей "ТОК К":'
where Id = 216

UPDATE [dbo].[Version]
SET DBVersion = '2.0.3'
WHERE Id = 1