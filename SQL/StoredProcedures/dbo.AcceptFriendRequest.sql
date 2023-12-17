SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/13/2023
-- Description:	Accepts a friend request
-- =============================================
CREATE PROCEDURE dbo.AcceptFriendRequest
	@RequesterUserId nvarchar(450),
	@ReceiverUserId nvarchar(450)
AS
BEGIN
	DECLARE @FriendRequestId int;
	SET @FriendRequestId = (
		select FriendRequestId
		FROM [TradeHarbor].[dbo].[FriendRequests]
		WHERE RequesterUserId = @RequesterUserId AND ReceiverUserId = @ReceiverUserId
	);

	IF @FriendRequestId IS NOT NULL
		BEGIN
		INSERT INTO dbo.FriendPairs
			(Person1Id, Person2Id, PairingDate)
		VALUES
			(@RequesterUserId, @ReceiverUserId, GETDATE());

		DELETE
		FROM dbo.FriendRequests
		WHERE
			FriendRequestId = @FriendRequestId;
		END
END
GO
