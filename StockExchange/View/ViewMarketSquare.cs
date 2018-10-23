using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Model;
using StockExchange.View.createView;

namespace StockExchange.View
{
    public interface IViewMarketSquare
    {
        IEnumerable<MarketSquareDataSource> getDataFromMarketSquare(string stockName, string transactionStatus);
        IEnumerable<MarketSquareDataSource> getAllStock();
    }

    public class ViewMarketSquare : IViewMarketSquare
    {
        private IMarketSquareDataSource _imarketDS;

        public ViewMarketSquare(IMarketSquareDataSource imarketDs)
        {
            _imarketDS = imarketDs;
        }

        public IEnumerable<MarketSquareDataSource> getAllStock()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MarketSquareDataSource> getDataFromMarketSquare(string stockName, string transactionStatus)
        {
            throw new NotImplementedException();
        }
    }
}
