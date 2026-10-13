-- KPK-2 DB Patch
-- From:    1.0.0
-- To:      2.0.0

-- !!!Неполный скрипт обновления, только добавляет таблицу с версиями и ставит версию 2.0.0
USE KPK2
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
INSERT [dbo].[Version] ([Id], [DBVersion]) VALUES (1, N'2.0.0')
GO
SET IDENTITY_INSERT [dbo].[Version] OFF
