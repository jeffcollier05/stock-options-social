SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/13/2023
-- Description:	Finds linked account information
-- =============================================
CREATE PROCEDURE dbo.FindLinkedAccountInformation 
	@UserId nvarchar(450)
AS
BEGIN
	SELECT FirstName, LastName, ProfilePictureUrl
	FROM [TradeHarbor].[dbo].[Accounts]
	WHERE [User_id] = @UserId;
END
GO
