USE [StockExchange]
GO

DECLARE	@return_value int

EXEC	@return_value = [dbo].[pStockEmission]
		@id_stock = 6,
		@setNumberStocks = 20,
		@setStockPrice = 0

SELECT	'Return Value' = @return_value

GO
