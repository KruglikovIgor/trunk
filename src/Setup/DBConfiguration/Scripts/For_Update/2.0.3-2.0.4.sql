-- KPK-2 DB Patch
-- From:    2.0.3
-- To:      2.0.4

USE KPK2
GO

update [dbo].[Parameters]
set [Precision] = 1
where ParamId = 261

UPDATE [dbo].[Version]
SET DBVersion = '2.0.4'
WHERE Id = 1