SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/13/2023
-- Description:	Get comments for a post
-- =============================================
CREATE PROCEDURE dbo.GetCommentsForPost
	@UserID nvarchar(450),
	@PostId int
AS
BEGIN
	SELECT
		c.CommentId,
		c.PostId,
		c.Comment,
		c.Timestamp,
		u.UserName AS 'Username',
		a.ProfilePictureUrl,
		a.FirstName,
		a.LastName,
		c.UserId
	FROM dbo.PostComments c
	JOIN dbo.Accounts a on a.User_id = c.UserId
	JOIN dbo.AspNetUsers u on u.Id = c.UserId
	WHERE c.PostId = @PostId;
END
GO
