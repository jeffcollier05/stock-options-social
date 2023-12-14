SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/13/2023
-- Description:	Comments on a post
-- =============================================
CREATE PROCEDURE dbo.CommentOnPost
	@UserID nvarchar(450),
	@PostId int,
	@Comment nvarchar(1000),
	@Timestamp datetime2(7)
AS
BEGIN
	INSERT INTO dbo.PostComments
		(UserId, PostId, Comment, Timestamp)
	VALUES
		(@UserId, @PostId, @Comment, @Timestamp);
END
GO
