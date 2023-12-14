SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jeff Collier
-- Create date: 12/13/2023
-- Description:	Creates a trade post
-- =============================================
CREATE PROCEDURE dbo.CreateTradePost 
	@UserId nvarchar(450),
	@Position int,
	@Option int,
	@Ticker varchar(4),
	@Strikeprice decimal(18, 0),
	@Comment varchar(1000),
	@Timestamp datetime2(7)
AS
BEGIN
	DECLARE @PositionId INT;
	SET @PositionId = (select Position_id from reference.Position where Position = @Position);

	DECLARE @OptionId INT;
	SET @OptionId = (select Option_id from reference.[Option] where [Option] = @Option); 

	INSERT INTO [dbo].[Trades]
		([User_id], [Ticker], [Position], [Option], [Strikeprice], [Comment], [Timestamp], [IsActive])
	VALUES (@UserId, @Ticker, @PositionId, @OptionId, @Strikeprice, @Comment, @Timestamp, 1);

	DECLARE @PrimaryKey INT;
	SET @PrimaryKey = SCOPE_IDENTITY();

	SELECT @PrimaryKey AS 'NewPrimaryKey';
END
GO
