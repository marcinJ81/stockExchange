using StockExchange.BaseView.createViewProcedure;
using StockExchange.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockExchangeMVC.Controllers
{
    public class MarketSquareController : Controller
    {
        private readonly IViewMarketSquare _viewMarket;
        private readonly IMarketSquareDataSource _MarketDS;

        public MarketSquareController(IViewMarketSquare viewMarket, IMarketSquareDataSource marketDs)
        {
            _viewMarket = viewMarket;
            _MarketDS = marketDs;
        }

        //public MarketSquareController(IViewMarketSquare viewMarket)
        //{
        //    _viewMarket = viewMarket;
        //}

        // GET: MarketSquare
        public ActionResult MarketSquare()
        {
            try
            {


                var result = _viewMarket.getAllStockIList();

            return View(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}