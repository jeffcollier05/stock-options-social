USE [TradeHarbor]
GO

/****** Object:  Table [dbo].[FriendRequests]    Script Date: 12/16/2023 10:56:29 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FriendRequests](
	[FriendRequestId] [int] IDENTITY(1,1) NOT NULL,
	[RequesterUserId] [nvarchar](450) NOT NULL,
	[ReceiverUserId] [nvarchar](450) NOT NULL,
	[SentTimestamp] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_FriendRequests] PRIMARY KEY CLUSTERED 
(
	[FriendRequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


