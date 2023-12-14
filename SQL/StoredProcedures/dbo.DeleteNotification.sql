SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/13/2023
-- Description:	Deletes a notification for user
-- =============================================
CREATE PROCEDURE dbo.DeleteNotification
	@UserId nvarchar(450),
	@NotificationId int
AS
BEGIN
	UPDATE dbo.Notifications
	SET IsActive = 0
	WHERE NotificationId = @NotificationId AND UserId = @UserId;
END
GO
