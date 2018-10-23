using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Model;

namespace StockExchange.StoredProcedures
{
    public interface IBuyTransaction
    {
        void createBuyTransaction(int user_id, int numberBuyStock, string stockName);
    }
    public class BuyTransaction : IBuyTransaction
    {
        public void createBuyTransaction(int user_id, int numberBuyStock, string stockName)
        {
            using (var db =  new StockExchangeEntities())
            {
                db.pCreateBuyTransaction(user_id, numberBuyStock, stockName);
            }
        }
    }
}
