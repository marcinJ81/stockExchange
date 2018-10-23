using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Model;

namespace StockExchange.View.createView
{
    public interface IMarketSquareDataSource
    {
        IEnumerable<MarketSquareDataSource> viewAllStockInMarketSquare();
    }

    public class MarketSquareDataSource : IMarketSquareDataSource
    {
        private string stockName;
        private int stockNumber;
        private decimal? stockPriceBuy;
        private decimal? stockPriceSell;
        private string transactionStatus;
        private int marketS_id;
        private bool transactionActive;
        private string stockSeria;
        public IEnumerable<MarketSquareDataSource> viewAllStockInMarketSquare()
        {
            using (var db = new StockExchangeEntities())
            {
                var result = db.MarketSquare.Join(db.BuyingSelling,
                    MS => MS.bs_id,
                    BS => BS.bs_id,
                    (MS, BS) => new
                    {
                        transactionStatus = BS.bs_action,
                        marketS_id = MS.mark_id,
                        stockPriceBuy = MS.mark_sharePriceBuy,
                        stockPriceSell = MS.mark_sharePriceSell,
                        stock_id = MS.stock_id,
                        stockNumber = MS.mark_numberOfShares,
                        transactionActive = MS.mark_aktywny
                    }).Join(db.Stock,
                    MS => MS.stock_id,
                    St => St.stock_id,
                    (MS, St) => new
                    {
                        transactionStatus = MS.transactionStatus,
                        marketS_id = MS.marketS_id,
                        stockPriceBuy = MS.stockPriceBuy,
                        stockPriceSell = MS.stockPriceSell,
                        stock_id = MS.stock_id,
                        stockNumber = MS.stockNumber,
                        transactionActive = MS.transactionActive,
                        stockN_id = St.stockN_id,
                        stockSeria = St.stock_seria
                    }).Join(db.StockName,
                    MS => MS.marketS_id,
                    SN => SN.stockN_id,
                    (MS,SN) => new MarketSquareDataSource
                    {
                        transactionStatus = MS.transactionStatus,
                        marketS_id = MS.marketS_id,
                        stockPriceBuy = MS.stockPriceBuy,
                        stockPriceSell = MS.stockPriceSell,
                        stockNumber = MS.stockNumber == null ? 0 : (int)MS.stockNumber,
                        transactionActive = MS.transactionActive == null ? false : (bool)MS.transactionActive,
                        stockName = SN.stockN_name,
                        stockSeria = MS.stockSeria
                    }).ToList();
                return result;
            }
        }
    }
}
