

Declare @stockName varchar(50); -- zmienna do wczytywania kolumny z cursora
Declare @userid int
declare @stockCount int
declare @markId int

Declare c1 Cursor For

select MS.mark_id,SN.stockN_name,UT.usr_id,MS.mark_numberOfShares from MarketSquare MS
inner join Wallet W on W.wal_id = MS.wal_id
inner join UserTable UT on UT.wal_id = W.wal_id
inner join Stock S on S.stock_id = MS.stock_id
inner join StockName SN on SN.stockN_id = S.stockN_id 
where 
	bs_id = 2

Open c1; --> Otwarcie
Fetch Next From c1 Into @markId,@stockName,@userid,@stockCount; -- Pobranie z wiersza cursora do zmiennej
While @@FETCH_STATUS = 0 -- test koñca kursora
Begin

    exec pBuyingSelling @stckName,@user_id,@stockCount,@markId
    Fetch Next From c1 Into @markId,@stockName,@userid,@stockCount
	; --kolejne czytanie
End
Close c1; -- zamkniêcie ( do tej chwili kursor mo¿e byæ obracany )
DeAllocate c1; -- zwolnienie cursora !!!!






