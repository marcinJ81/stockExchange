using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Model;

namespace StockExchange.StoredProcedures
{
    public interface IBuySellStock
    {
        void pBuySellStock(string stockName, int user_id, int countAction, int marketSquare_id);
    }

    public class BuySellStock : IBuySellStock
    {
        public void pBuySellStock(string stockName, int user_id, int countAction, int marketSquare_id)
        {
            using (var db = new StockExchangeEntities())
            {
               int result = db.pBuyStock(stockName, user_id, countAction, marketSquare_id);
            }
        }
    }
}
