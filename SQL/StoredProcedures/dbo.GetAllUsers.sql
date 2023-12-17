SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/13/2023
-- Description:	Gets all users
-- =============================================
CREATE PROCEDURE dbo.GetAllUsers 
	@UserId nvarchar(450)
AS
BEGIN
	SELECT 
		u.Id as 'UserId',
		u.UserName,
		ac.FirstName,
		ac.LastName,
		ac.ProfilePictureUrl,
		CASE
			WHEN EXISTS (
				SELECT 1 
				FROM dbo.FriendPairs F 
				WHERE f.Person1Id = u.Id AND F.Person2Id = @UserId
					OR F.Person2Id = u.Id AND F.Person1Id = @UserId
			) THEN 1
			ELSE 0
		END AS IsFriend,
		CASE
			WHEN EXISTS (
				SELECT 1 
				FROM dbo.FriendRequests fr
				WHERE fr.RequesterUserId = u.Id AND fr.ReceiverUserId = @UserId
			) THEN 1
			ELSE 0
		END AS SentFriendRequestToYou,
		CASE
			WHEN EXISTS (
				SELECT 1 
				FROM dbo.FriendRequests fr
				WHERE fr.ReceiverUserId = u.Id AND fr.RequesterUserId = @UserId
			) THEN 1
			ELSE 0
		END AS ReceivedFriendRequestFromYou
	 FROM dbo.AspNetUsers u
	 JOIN dbo.Accounts ac on ac.User_id = u.Id
	 WHERE u.Id <> @UserId;
END
GO
