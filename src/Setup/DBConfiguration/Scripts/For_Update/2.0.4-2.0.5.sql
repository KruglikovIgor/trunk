-- KPK-2 DB Patch
-- From:    2.0.4
-- To:      2.0.5

USE KPK2
GO

update [Report]
set Requirement = 'от 0,4 до 1,0'
where Id IN (20,22,24)


update [Report]
set Requirement = 'от -0,4 до -1,0'
where Id IN (21,23,25)


update [Report]
set Requirement = 'не более 0,4'
where Id IN (27,28,29)


update [Report]
set Requirement = '13 312 - 13 336'
where Id = 31


update [Report]
set Requirement = '0,8 - 1,7'
where Id IN (49,50,51,52,53,54)


update [Report]
set Requirement = '16,9 ± 1,1'
where Id = 92


update [Report]
set Requirement = '±3,0°'
where Id = 117

GO

update [Parameters]
set MinValue = 0.4, MaxValue = 1.0
where ParamId IN (121,122,123)

update [Parameters]
set MinValue = -1.0, MaxValue = -0.4
where ParamId IN (124,125,126)

update [Parameters]
set MaxValue = 0.4
where ParamId IN (97,118,117)

update [Parameters]
set MinValue = 0.8, MaxValue = 1.7
where ParamId IN (76,77,78,79,80,81)

update [Parameters]
set MinValue = 13312, MaxValue = 13336
where ParamId = 119

update [Parameters]
set MinValue = 15.8, MaxValue = 18.0
where ParamId = 142

update [Parameters]
set MinValue = -3.0, MaxValue = 3.0
where ParamId = 158

UPDATE [dbo].[Version]
SET DBVersion = '2.0.5'
WHERE Id = 1
GO
