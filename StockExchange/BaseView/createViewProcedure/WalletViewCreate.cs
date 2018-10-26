using StockExchange.BaseView.createViewSource;
using StockExchange.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchange.BaseView.createViewProcedure
{
    public interface IWalletDataSource
    {
        IEnumerable<WalletDataSource> getAllStockOfWallet();
    }
    public class WalletViewCreate : IWalletDataSource
    {
        private IMarketSquareDataSource _imarket;

        public WalletViewCreate(IMarketSquareDataSource imarket)
        {
            _imarket = imarket;
        }
        public IEnumerable<WalletDataSource> getAllStockOfWallet()
        {
            var stockInMarketSquare = _imarket.viewAllStockInMarketSquare().Where(x => x.statusId == 1);

            using (var db = new StockExchangeEntities())
            {
                var result = db.Wallet.Join(db.UserTable,
                    W => W.wal_id,
                    UT => UT.wal_id,
                    (W, UT) => new
                    {
                        userLogin = UT.usr_login,
                        walletStock = W.wal_numberOfShares,
                        walletMoney = W.wal_MoneyLimit,
                        walId = W.wal_id
                    }).Join(stockInMarketSquare,
                    WUT => WUT.walId,
                    SIMS => SIMS.walId,
                    (WUT, SIMS) => new
                    {
                        userLogin = WUT.userLogin,
                        walletStock = WUT.walletStock,
                        walletMoney = WUT.walletMoney,
                        walId = WUT.walId,
                        stockName = SIMS.stockName,
                        stockNumber = SIMS.stockNumber,
                        stockId = SIMS.stockId,
                        wallId = SIMS.walId
                    }).AsEnumerable();
                var result2 = result.Select(x => new WalletDataSource()
                {
                    userLogin = x.userLogin,
                    walletStock = x.walletStock == null ? 0 : (int)x.walletStock,
                    walletMoney = x.walletMoney == null ? 0 : (decimal)x.walletMoney,
                    wallId = x.walId,
                    stockName = x.stockName,
                    stockNumber = x.stockNumber,
                    stockId = x.stockId
                });
                return result2;
            }
        }
    }
}
