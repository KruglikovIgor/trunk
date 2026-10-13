-- KPK-2 DB Patch
-- From:    2.0.5
-- To:      2.0.6

USE KPK2
GO

UPDATE dbo.Report
SET OrderNumber = NULL, SubItem = 3 where Id = 385

UPDATE dbo.Report
SET OrderNumber = NULL, SubItem = 4 where Id = 383

UPDATE dbo.Report
SET OrderNumber = 0, SubItem = 2 where Id = 659
GO


UPDATE [dbo].[Version]
SET DBVersion = '2.0.6'
WHERE Id = 1
