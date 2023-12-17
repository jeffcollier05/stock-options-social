USE [TradeHarbor]
GO

/****** Object:  Table [dbo].[Trades]    Script Date: 12/16/2023 10:56:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Trades](
	[Trade_id] [int] IDENTITY(1,1) NOT NULL,
	[User_id] [nvarchar](450) NOT NULL,
	[Ticker] [varchar](4) NOT NULL,
	[Position] [int] NOT NULL,
	[Option] [int] NOT NULL,
	[Strikeprice] [decimal](18, 0) NOT NULL,
	[Comment] [varchar](1000) NOT NULL,
	[Timestamp] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Trades] PRIMARY KEY CLUSTERED 
(
	[Trade_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Trades]  WITH CHECK ADD  CONSTRAINT [FK_Trades_Option] FOREIGN KEY([Option])
REFERENCES [reference].[Option] ([Option_id])
GO

ALTER TABLE [dbo].[Trades] CHECK CONSTRAINT [FK_Trades_Option]
GO

ALTER TABLE [dbo].[Trades]  WITH CHECK ADD  CONSTRAINT [FK_Trades_Position] FOREIGN KEY([Position])
REFERENCES [reference].[Position] ([Position_id])
GO

ALTER TABLE [dbo].[Trades] CHECK CONSTRAINT [FK_Trades_Position]
GO

ALTER TABLE [dbo].[Trades]  WITH CHECK ADD  CONSTRAINT [fk_User_id] FOREIGN KEY([User_id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[Trades] CHECK CONSTRAINT [fk_User_id]
GO


