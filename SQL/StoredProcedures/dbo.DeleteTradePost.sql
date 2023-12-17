SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/13/2023
-- Description:	Deletes the trade post
-- =============================================
CREATE PROCEDURE dbo.DeleteTradePost 
	@UserId nvarchar(450),
	@TradeId int
AS
BEGIN
	UPDATE dbo.Trades
	SET IsActive = 0
	WHERE Trade_id = @TradeId AND User_id = @UserId;
END
GO
