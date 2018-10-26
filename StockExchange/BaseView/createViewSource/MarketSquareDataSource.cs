using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Model;

namespace StockExchange.BaseView.createViewSource
{
    
    public sealed class MarketSquareDataSource
    {
        public string stockName;
        public int stockNumber;
        public decimal? stockPriceBuy;
        public decimal? stockPriceSell;
        public string transactionStatus;
        public int marketS_id;
        public bool transactionActive;
        public string stockSeria;
        public int stockId;
        public int walId;
        public int statusId;

    }
}
