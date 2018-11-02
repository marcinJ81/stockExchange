using StockExchange.BaseView.createViewSource;
using StockExchange.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.BaseView.createViewProcedure
{
    public interface IMarketSquareDataSource
    {
        IEnumerable<MarketSquareDataSource> viewAllStockInMarketSquare();
        List<MarketSquareDataSource> viewAllStockInMarketSquareList();
    }
    public class MarketSquareViewCreate : IMarketSquareDataSource
    {
        public List<MarketSquareDataSource> viewAllStockInMarketSquareList()
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
                        transactionActive = MS.mark_aktywny,
                        walId = MS.wal_id,
                        statusId = MS.bs_id
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
                        stockSeria = St.stock_seria,
                        walId = MS.walId,
                        statusId = MS.statusId
                    }).Join(db.StockName,
                    MS => MS.stock_id,
                    SN => SN.stockN_id,
                    (MS, SN) => new MarketSquareDataSource
                    {
                        transactionStatus = MS.transactionStatus,
                        marketS_id = MS.marketS_id,
                        stockPriceBuy = MS.stockPriceBuy,
                        stockPriceSell = MS.stockPriceSell,
                        stockNumber = MS.stockNumber == null ? 0 : (int)MS.stockNumber,
                        transactionActive = MS.transactionActive == null ? false : (bool)MS.transactionActive,
                        stockName = SN.stockN_name,
                        stockSeria = MS.stockSeria,
                        stockId = MS.stock_id == null ? 0 : (int)MS.stock_id,
                        walId = MS.walId == null ? 0 : (int)MS.walId,
                        statusId = MS.statusId == null ? 0 : (int)MS.statusId
                    }).ToList();

                return result;
            }
        }

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
                        transactionActive = MS.mark_aktywny,
                        walId = MS.wal_id,
                        statusId = MS.bs_id
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
                        stockSeria = St.stock_seria,
                        walId = MS.walId,
                        statusId = MS.statusId
                    }).Join(db.StockName,
                    MS => MS.stock_id,
                    SN => SN.stockN_id,
                    (MS, SN) => new MarketSquareDataSource
                    {
                        transactionStatus = MS.transactionStatus,
                        marketS_id = MS.marketS_id,
                        stockPriceBuy = MS.stockPriceBuy,
                        stockPriceSell = MS.stockPriceSell,
                        stockNumber = MS.stockNumber == null ? 0 : (int)MS.stockNumber,
                        transactionActive = MS.transactionActive == null ? false : (bool)MS.transactionActive,
                        stockName = SN.stockN_name,
                        stockSeria = MS.stockSeria,
                        stockId = MS.stock_id == null ? 0 : (int)MS.stock_id,
                        walId = MS.walId == null ? 0 : (int)MS.walId,
                        statusId = MS.statusId == null ? 0 : (int)MS.statusId
                    }).AsEnumerable();
   
                return result;
            }

        }
    }
}
