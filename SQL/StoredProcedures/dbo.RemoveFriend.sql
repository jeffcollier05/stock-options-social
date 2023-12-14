SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/13/2023
-- Description:	Removes a friendship
-- =============================================
CREATE PROCEDURE dbo.RemoveFriend 
	@User1Id nvarchar(450),
	@User2Id nvarchar(450)
AS
BEGIN
	DELETE
	FROM dbo.FriendPairs
	WHERE
	  (Person1Id = @User1Id AND Person2Id = @User2Id)
	  OR
	  (Person1Id = @User2Id AND Person2Id = @User1Id);
END
GO
