USE [ddiarydb]
GO

/****** Object:  Table [dbo].[OTPVerification]    Script Date: 25-Jul-20 11:28:41 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OTPVerification](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MobileNo] [varchar](20) NULL,
	[EmailId] [varchar](max) NULL,
	[OTP] [varchar](10) NOT NULL,
	[IsSMSSend] [bit] NULL,
	[IsEmailSend] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


