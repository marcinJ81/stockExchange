using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StockExchange.BaseView.createViewSource;
using StockExchange.BaseView.createViewProcedure;

namespace StockExchange.BaseView
{
    public interface IViewWallet
    {
        IEnumerable<WalletDataSource> getAllStokckFromUser(string loginUser);
        IEnumerable<WalletDataSource> getOnlyWalletInfo(string loginUser);
    }

    public class ViewWallet : IViewWallet
    {
        private IWalletDataSource _iwallDS;

        public ViewWallet(IWalletDataSource iwallDs)
        {
            _iwallDS = iwallDs;
        }

        public IEnumerable<WalletDataSource> getAllStokckFromUser(string loginUser)
        {
            var result = _iwallDS.getAllStockOfWallet();
            result = result.Where(x => x.userLogin.Contains(loginUser));
            var gruopByStock = result.GroupBy(x => x.stockName)
                .Join(result,
                    gbs => gbs.Key,
                    r => r.stockName,
                    (gbs,r) => new
                    {
                        stockN = gbs.Key,
                        uLogin = r.userLogin,
                        stockCount = r.stockNumber,
                        wId = r.wallId,
                        sId = r.stockId 
                    }
                    )
                .Select( x => new WalletDataSource()
                {
                    userLogin = x.uLogin,
                    stockName = x.stockN,
                    stockNumber = x.stockCount,
                    wallId = x.wId,
                    stockId = x.sId
                });
              
            return gruopByStock;
        }

        public IEnumerable<WalletDataSource> getOnlyWalletInfo(string loginUser)
        {
            var result = _iwallDS.getAllStockOfWallet();
            result = result.Where(x => x.userLogin.Contains(loginUser))
                .Select(y => new WalletDataSource()
                {
                    userLogin = y.userLogin,
                    walletStock = y.walletStock,
                    walletMoney = y.walletMoney,
                });

            return result;
        }
    }
}
