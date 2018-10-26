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

        public MarketSquareController(IViewMarketSquare viewMarket)
        {
            _viewMarket = viewMarket;
        }

        // GET: MarketSquare
        public ActionResult MarketSquare()
        {
            var result = _viewMarket.getAllStock();
            return View(result);
        }
    }
}