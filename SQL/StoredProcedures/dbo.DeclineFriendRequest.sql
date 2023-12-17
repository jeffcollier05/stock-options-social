SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/13/2023
-- Description:	Declines a friend request
-- =============================================
CREATE PROCEDURE dbo.DeclineFriendRequest
	@RequesterUserId nvarchar(450),
	@ReceiverUserId nvarchar(450)
AS
BEGIN
	DELETE
	FROM dbo.FriendRequests
	WHERE
	  RequesterUserId = @RequesterUserId AND ReceiverUserId = @ReceiverUserId;
END
GO
