SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/13/2023
-- Description:	Reacts to a post
-- =============================================
CREATE PROCEDURE dbo.ReactToPost
	@UserID nvarchar(450),
	@PostId int,
	@ReactionType nvarchar(15),
	@Timestamp datetime2(7)
AS
BEGIN
	DELETE
	FROM dbo.PostReactions
	WHERE UserId = @UserID AND PostId = @PostId

	IF @ReactionType != 'NO-VOTE'
	INSERT INTO dbo.PostReactions
		(UserId, PostId, ReactionType, Timestamp)
	VALUES
		(@UserId, @PostId, @ReactionType, @Timestamp);
END
GO
