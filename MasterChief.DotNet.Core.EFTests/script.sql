USE [Sample]
GO

/****** Object:  Table [dbo].[EFSample]    Script Date: 2019/3/9 22:04:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EFSample](
	[ID] [uniqueidentifier] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[ModifyTime] [datetime] NOT NULL,
	[Available] [bit] NOT NULL,
	[UserName] [nvarchar](20) NOT NULL,
 CONSTRAINT [EFSamle_PK] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EFSample', @level2type=N'COLUMN',@level2name=N'UserName'
GO


