SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/13/2023
-- Description:	Gets trade posts for user
-- =============================================
CREATE PROCEDURE dbo.GetTrades 
	@UserId nvarchar(450)
AS
BEGIN
	WITH FriendIDs AS (
		SELECT Person2Id AS 'Id'
		FROM dbo.friendpairs
		WHERE Person1Id = @UserId
		UNION
		SELECT Person1Id AS 'Id'
		FROM dbo.friendpairs
		WHERE Person2Id = @UserId
		UNION
		SELECT @UserId
	)

	SELECT 
		t.Ticker,
		p.Position,
		op.[Option],
		t.Strikeprice,
		t.Comment,
		t.[Timestamp],
		t.User_id AS 'UserId',
		t.Trade_id AS 'TradeId',
		a.FirstName,
		a.LastName,
		a.ProfilePictureUrl,
		u.UserName as Username,
		(SELECT COUNT(*) FROM dbo.PostReactions WHERE PostId = t.Trade_id AND ReactionType = 'UPVOTE')
			- (SELECT COUNT(*) FROM dbo.PostReactions WHERE PostId = t.Trade_id AND ReactionType = 'DOWNVOTE') AS Votes,
		(SELECT ReactionType FROM dbo.PostReactions WHERE PostId = t.Trade_id AND UserId = @UserId) AS 'UserReaction'
	FROM dbo.trades t
	JOIN reference.[Option] op on op.Option_id = t.[Option]
	JOIN reference.[Position] p on p.Position_id = t.Position
	JOIN dbo.Accounts a on a.User_id = t.User_id
	JOIN dbo.AspNetUsers u on u.Id = t.User_id
	WHERE
		u.Id IN (SELECT Id FROM FriendIDs)
		AND t.IsActive = 1;
END
GO
