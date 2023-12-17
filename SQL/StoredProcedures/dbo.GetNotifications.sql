SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/13/2023
-- Description:	Gets notifications for user
-- =============================================
CREATE PROCEDURE dbo.GetNotifications 
	@UserId nvarchar(450)
AS
BEGIN
	SELECT Message, CreatedTimestamp, NotificationId
	FROM dbo.Notifications
	WHERE UserId = @UserId
		AND IsActive = 1;
END
GO
