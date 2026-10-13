-- KPK-2 DB Patch
-- From:    2.0.0
-- To:      2.0.1

-- !!!Тестовый пример для обновления
USE KPK2
GO
UPDATE [dbo].[Version]
SET DBVersion = '2.0.1'
WHERE Id = 1