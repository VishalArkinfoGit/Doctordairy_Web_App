USE [ddiarydb]
GO

/****** Object:  Table [dbo].[Treatment_Image_Master]    Script Date: 26-Jul-20 12:28:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Treatment_Image_Master](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Treat_crno] [int] NOT NULL,
	[Image_Path] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Treatment_Image_Master]  WITH CHECK ADD  CONSTRAINT [FK_Treatment_Image_Master_Treatment_Master] FOREIGN KEY([Treat_crno])
REFERENCES [dbo].[Treatment_Master] ([Treat_crno])
GO

ALTER TABLE [dbo].[Treatment_Image_Master] CHECK CONSTRAINT [FK_Treatment_Image_Master_Treatment_Master]
GO


