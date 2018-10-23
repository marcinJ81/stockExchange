using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Model;

namespace StockExchange.StoredProcedures
{
    public interface ISellTransaction
    {
        void createSellTransaction(int user_id, int numberSellStock, string stockName);
    }

    public class SellTransaction : ISellTransaction
    {
        public void createSellTransaction(int user_id, int numberSellStock, string stockName)
        {
            using (var db = new StockExchangeEntities())
            {
                db.pCreateSellTransaction(user_id, numberSellStock, stockName);
            }
        }
    }
}
