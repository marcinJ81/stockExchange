USE [StockExchange]
GO

DECLARE	@return_value int

EXEC	@return_value = [dbo].[pCreateBuyTransaction]
		@user_id = 1,
		@numberBuyStock = 3,
		@stock_name = N'PGB'

SELECT	'Return Value' = @return_value

GO
