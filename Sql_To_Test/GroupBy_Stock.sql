select  SN.stockN_name,UT.usr_id,Sum(MS.mark_numberOfShares) as ilosc from MarketSquare MS
inner join Wallet W on W.wal_id = MS.wal_id
inner join UserTable UT on UT.wal_id = W.wal_id
inner join Stock S on S.stock_id = MS.stock_id
inner join StockName SN on SN.stockN_id = S.stockN_id 
where 
	bs_id = 2
group by SN.stockN_name,UT.usr_id