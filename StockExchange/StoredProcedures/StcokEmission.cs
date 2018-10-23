using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.Model;

namespace StockExchange.StoredProcedures
{
    public interface IStockEmission
    {
        void createStockEmission(int id_stock, int setNumberOfStock, decimal setStockPrice);
    }
    public class StcokEmission : IStockEmission
    {
        public void createStockEmission(int id_stock, int setNumberOfStock, decimal setStockPrice)
        {
            using (var db = new StockExchangeEntities())
            {
                db.pStockEmission(id_stock,setNumberOfStock,setStockPrice);
            }
        }
    }
}
