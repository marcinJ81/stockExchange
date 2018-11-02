using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Model;
using StockExchange.BaseView.createViewSource;
using StockExchange.BaseView.createViewProcedure;

namespace StockExchange.View
{
    public interface IViewMarketSquare
    {
        IEnumerable<MarketSquareDataSource> getStockFromMarketSquare(string stockName, string transactionStatus);
        IEnumerable<MarketSquareDataSource> getAllStockEnumerable();
        List<MarketSquareDataSource> getAllStockIList();
    }

    public class ViewMarketSquare : IViewMarketSquare
    {
        private IMarketSquareDataSource _imarketDS;

        public ViewMarketSquare(IMarketSquareDataSource imarketDs)
        {
            _imarketDS = imarketDs;
        }

        public IEnumerable<MarketSquareDataSource> getStockFromMarketSquare(string stockName, string transactionStatus)
        {
            var result = _imarketDS.viewAllStockInMarketSquare();
            if(!string.IsNullOrWhiteSpace(stockName))
                result = result.Where(x => x.stockName.Contains(stockName));
            if (!string.IsNullOrWhiteSpace(transactionStatus))
                result = result.Where(x => x.transactionStatus == transactionStatus);
            return result;
        }

        public IEnumerable<MarketSquareDataSource> getAllStockEnumerable()
        {
            var result = _imarketDS.viewAllStockInMarketSquare();
            return result;
        }

        public List<MarketSquareDataSource> getAllStockIList()
        {
            var result = _imarketDS.viewAllStockInMarketSquareList();
            return result;
        }
    }
}
