SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/13/2023
-- Description:	Creates a friend request
-- =============================================
CREATE PROCEDURE dbo.CreateFriendRequest
	@RequesterUserId nvarchar(450),
	@ReceiverUserId nvarchar(450),
	@SentTimestamp datetime2(7)
AS
BEGIN
	INSERT INTO dbo.FriendRequests
		(RequesterUserId, ReceiverUserId, SentTimestamp)
	VALUES
		(@RequesterUserId, @ReceiverUserId, @SentTimestamp);
END
GO
