USE [TradeHarbor]
GO

/****** Object:  Table [dbo].[FriendPairs]    Script Date: 12/16/2023 10:56:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FriendPairs](
	[PairID] [int] IDENTITY(1,1) NOT NULL,
	[Person1Id] [nvarchar](450) NULL,
	[Person2Id] [nvarchar](450) NULL,
	[PairingDate] [datetime2](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[PairID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FriendPairs]  WITH CHECK ADD FOREIGN KEY([Person1Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[FriendPairs]  WITH CHECK ADD FOREIGN KEY([Person2Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO


