USE [arkinfos_ddiaryRestapi]
GO

/****** Object:  Table [dbo].[Patient_Master]    Script Date: 18-Aug-20 10:53:40 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Patient_Master](
	[Patient_state] [varchar](30) NULL,
	[Patient_photo] [varchar](200) NULL,
	[Patient_name] [varchar](50) NULL,
	[Patient_email] [varchar](100) NULL,
	[Patient_contact] [varchar](20) NULL,
	[Patient_city] [varchar](30) NULL,
	[Patient_address] [varchar](500) NULL,
	[Patient_Id] [int] IDENTITY(1,1) NOT NULL,
	[Patient_Country] [varchar](30) NULL,
	[Reg_Date] [date] NULL,
	[User_Id] [int] NULL,
	[note] [varchar](500) NULL,
	[age] [numeric](16, 2) NULL,
	[gender] [varchar](5) NULL,
	[relation] [nvarchar](max) NULL,
 CONSTRAINT [pk_Patient_Master] PRIMARY KEY CLUSTERED 
(
	[Patient_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Patient_Master]  WITH CHECK ADD  CONSTRAINT [FK_Patient_Master_usr] FOREIGN KEY([User_Id])
REFERENCES [dbo].[usr] ([Id])
GO

ALTER TABLE [dbo].[Patient_Master] CHECK CONSTRAINT [FK_Patient_Master_usr]
GO


