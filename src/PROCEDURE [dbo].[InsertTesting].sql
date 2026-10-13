CREATE TABLE [dbo].[TestModes]
(Id int PRIMARY KEY,
Name NVARCHAR(100))

INSERT [TestModes] 
VALUES (1, 'NormalTest'),
		(2, 'WorkTest')

ALTER TABLE [Testing]
ADD IdTestMode INT NOT NULL DEFAULT 1,
	CONSTRAINT FK_Testing_To_TestModes FOREIGN KEY(IdTestMode) REFERENCES TestModes(Id);

ALTER TABLE [dbo].[DeviceType]
ADD IdMode INT NOT NULL DEFAULT 1
	CONSTRAINT FK_DeviceType_To_TestModes FOREIGN KEY(IdMode) REFERENCES TestModes(Id);

	select * from [dbo].[DeviceType]
