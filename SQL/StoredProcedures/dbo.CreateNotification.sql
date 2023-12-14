SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/13/2023
-- Description:	Creates a notification for a user
-- =============================================
CREATE PROCEDURE dbo.CreateNotification 
	@UserId nvarchar(450),
	@Message nvarchar(450),
	@CreatedTimestamp datetime2(7)
AS
BEGIN
	INSERT INTO dbo.Notifications
		(UserId, Message, CreatedTimestamp, IsActive)
	VALUES
		(@UserId, @Message, @CreatedTimestamp, 1);
END
GO
