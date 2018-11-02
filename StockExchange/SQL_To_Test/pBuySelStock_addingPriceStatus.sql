
 
 -- realizacja transakcji kupna w statusie kCU i sprzedazy sCU
alter PROCEDURE [dbo].[pBuySell_kCU](
@mark_id int
)
AS
BEGIN
--pomocnicze
declare @numberOfStock int
declare @stockPrice money
declare @stockCountMax int
--potrzebuje
declare @wall_id int
declare @stock_id int
declare @stockCountBuy int 
declare @transactionBuyPriceStock money
declare @transactionPricesStatus int
--zlecenie kupna
--zlecenia których termin wazonosci przead³y przestaj¹ byc aktywne
select 
		@wall_id = wal_id, @stock_id=stock_id , @stockCountBuy = mark_numberOfShares, 
		@transactionBuyPriceStock = mark_sharePriceBuy, @transactionPricesStatus = priceStatus_id    
from MarketSquare 
where
	 mark_id = @mark_id and DATEDIFF(hour,GETDATE(),mark_dataEnd) >= 0 and mark_aktywny = 1 and bs_id = 2


--akcje ktore sa na rynku, w tym ilosciowo
--cena akcji
--warunek kCU - kupuj cena ustalona, jezeli cena zakupu = cena sprzedazy
if(@transactionPricesStatus = 1)
begin
	--wyszukanie ofert sprzedazy gdzie stock_id = @stock_id, ilosc_akcji > 0, cena_akcji_jest = @transactionPriceStatus oraz zlecenie aktywne jest
	--tutaj kolejny cursor, niekoniecznie bo zapytanie te mozna podpiac pod juz istniejacy
	-- powyzsze zapytanie wrzucic do tymczasowj tabeli w ten sposob przygotuje sobie dane do kursora bez koniecznosci powtarzania zapytan
	-- jest tu uwaga nie tylko cena akcji w zakupu i sprzedazy musi byc rowna ale rowniez cena rynkowa akcji musi byc rowna,
	-- dlaczego bo moze zdarzy
	print 'to w kursorze bedzie'
end
--kupuj po kazdej cenie (kPKC)
if(@transactionPricesStatus = 3)
begin
	--analogicznie jak powyzej tylko szukam zlecen sprzedazy po kazdej cenie
	--sprawdze czy (upewnie sie) cena jest rowna zero 
	if(@transactionBuyPriceStock > 0)
	begin
		print 'blad nie wlasciwie utworzone zlecenie kPKC'
	end
	else
	begin
		print 'zlecenia sPKC'
		-- wyszukanie zlecen sprzedazy wrzucenie do tabeli z tym wyjatkiem
		-- ze cene akcji z zleceni sPKC biore z tabeli stock za kazym razem jak 
		-- moglbym zrobic dwie tabele zlecenia z cena ustalona bo ona sie nie zmienia
		-- wiec ptzy dokonaniu transakcji bede bral cene z rekodru zlecenia
		-- druga tabela zlecenia z cena zerowa czyli sPKC, wtedy biore cene z tabeli stock
		-- opcja druga to jedna tabela i po napotkaniu sPKC sciagniecie ceny z stock
		-- w przeciwnym razie cena z zlecenia
		-- w czasie wykonywania proceury transakcji, nie mozna wprowadzac zlecen kupna/sprzedazy
		-- dzieki temu zabezpiecze sie przed poajwianiem sie nowych zlecen kupna i bezproblemu moge wrzucic je
		-- do tabeli tymczasowej
		-- po wykonaniu kazdej transakcji cena akcji w tabeli stock zmienia siê

	end
end
--select @stockPrice = mark_sharePriceSell from MarketSquare where stock_id = @stock_id and bs_id = 3
select @numberOfStock = sum(mark_numberOfShares) from MarketSquare where stock_id = @stock_id and bs_id = 3 or bs_id = 4
print 'ilosc akcji na rynku - ' + CAST(@numberOfStock as varchar(5))

if(@stockCountBuy <= @numberOfStock)
begin
	declare @sumStocksUser int
		--robie petle lub kursor 
       declare @id_markCursor int
	   declare @iloscAkcji int
	   declare @wynikodejmowania int
	   declare @statusTransakcji int
	   declare @priceOfStock money
	   declare @walletToPayForStock int
	   declare @stockId_buy_kCU money
		Declare c1 Cursor For
           
			select MS.mark_id,MS.mark_numberOfShares,MS.bs_id,MS.mark_sharePriceSell,MS.wal_id,MS.stock_id
			from MarketSquare MS
			where  stock_id = @stock_id and mark_numberOfShares > 0 and mark_sharePriceSell = @transactionBuyPriceStock and bs_id = 3 or bs_id = 4

       Open c1; --> Otwarcie
       Fetch Next From c1 Into @id_markCursor,@iloscakcji,@statusTransakcji,@priceOfStock,@walletToPayForStock,@stockId_buy_kCU; -- Pobranie z wiersza cursora do zmiennej
       While @@FETCH_STATUS = 0 -- test koñca kursora
       Begin	
	   --sprwdzenie czy cena akcji w zleceniu kupna jest rowna cenie akcji w zleceniu sprzedazy
	   if(@priceOfStock = @transactionBuyPriceStock)
		   begin
		   --ilosc akcji sie zmniejsza a wartosc transakcji rosnie	
			if(@stockCountBuy <= @numberOfStock)																																																																													if(@iloscAkcji <= @stockCountBuy)
			begin
				declare @result int
				set @result = @iloscAkcji - @stockCountBuy
				print 'result ' + Cast(@result as varchar) 
				if(@result <= 0)
				begin 
					update MarketSquare
						set
							mark_numberOfShares = 0 ,
							bs_id = 1,
							mark_TransactionPrice = @iloscAkcji *  @priceOfStock
						 where
							mark_id = @id_markCursor;
						set @stockCountBuy = @stockCountBuy - @iloscAkcji
						print 'pozostala ilosc akcji w zleceniu =>' + Cast(@stockCountBuy as varchar) 
						print 'kwota po sprzedaniu akcji =>' + Cast( (@iloscAkcji *  @priceOfStock) as varchar) 
					--update ceny  sprzedazy w tabeli stock
					update Stock
						set
							stock_pricaeSell = @priceOfStock
						where
							stock_id = @stockId_buy_kCU
						 
					--przekazuje kase do portfela i koncze transakcje sprzedazy
					if(@statusTransakcji = 3)
					begin
						declare @walletUserSellMoneyLimit money
						declare @resultWalletUserSellMoneyLimit money
						select @walletUserSellMoneyLimit =  wal_MoneyLimit from Wallet where wal_id = @walletToPayForStock
						set @resultWalletUserSellMoneyLimit = @walletUserSellMoneyLimit + (@iloscAkcji *  @priceOfStock)
						update Wallet set
							wal_MoneyLimit = @resultWalletUserSellMoneyLimit
						where
							wal_id =  @walletToPayForStock
							print 'moeny limit in wallet heigh to => ' + CAST(@resultWalletUserSellMoneyLimit as varchar)	
					end
					else
					begin
						print 'emisja akcji, na razie bez zarabiania na nich czyli kasa w proznie'
					end
				end
			end
			else
			begin 
				if(@stockCountBuy > 0)
				begin
				--ilosc akcji nie zostala wyzerowana, jakies zostaly
				--zerujemy kase za akcje, teraz jest ich mniejsza ilosc
				-- nie zmieniamy statusu
				declare @countStockPozostalychInTransaction int

				set @countStockPozostalychInTransaction = @iloscAkcji - @stockCountBuy
					update MarketSquare
					set
						mark_numberOfShares = @countStockPozostalychInTransaction,
						mark_TransactionPrice = 0,
						bs_id = @statusTransakcji
					 where
						mark_id = @id_markCursor;
					
					--update ceny  sprzedazy w tabeli stock
					update Stock
						set
							stock_pricaeSell = @priceOfStock
						where
							stock_id = @stockId_buy_kCU
					if(@statusTransakcji = 3)
					begin
						-- nie wszystkie zostaly sprzedane do portfela trafiaja srodki
						-- ze sprzedazy a to co zostalo dlaej jest na sprzedaz
						-- nie zmieniam statusu
						declare @walletUserSellMoneyLimitRestOfMoney money
						declare @resultWalletUserSellMoneyLimitRestOfMoney money
						
						select @walletUserSellMoneyLimitRestOfMoney =  wal_MoneyLimit		
						 from Wallet where wal_id = @walletToPayForStock
						set @resultWalletUserSellMoneyLimit = @walletUserSellMoneyLimitRestOfMoney + (@stockCountBuy *  @priceOfStock)
						
						update Wallet set
							wal_MoneyLimit = @resultWalletUserSellMoneyLimit
						where
							wal_id =  @walletToPayForStock
							print 'money limit in wallet heigh to => ' + CAST(@resultWalletUserSellMoneyLimit as varchar)	
					end
					else
					begin
						print 'emisja akcji, na razie bez zarabiania na nich czyli kasa w proznie.'
					end
				end
			end
			
				 Fetch Next From c1 Into @id_markCursor,@iloscakcji,@statusTransakcji,@priceOfStock,@walletToPayForStock; --kolejne czytani
			end
			else
			begin
				print 'cena kupna rozna od ceny sprzedazy'
			end
       End
       Close c1; -- zamkniêcie ( do tej chwili kursor mo¿e byæ obracany )
       DeAllocate c1; -- zwolnienie cursora !!!

	   declare @ofertaKupna_id int
	   --update portfela kupujacego, dopisanie ceny transakcji kupna ilosc * cena
	   declare @countNumberStockInTransaction int
		declare @priceStocks money
		declare @TransactionPriceBuy money
		select @countNumberStockInTransaction = mark_numberOfShares, @priceStocks = mark_sharePriceBuy from MarketSquare where mark_id = @mark_id 
		set @TransactionPriceBuy = @countNumberStockInTransaction * @priceStocks
	   update MarketSquare 
	   set
			bs_id = 1,
			mark_dataEnd = GETDATE(),
			mark_TransactionPrice = @TransactionPriceBuy
		where
			mark_id = @mark_id
		
		--aktualizacjia ilosc akcji w portfelu
		declare @allStackCountPerUser int
		declare @allMoneyCountPerUser money
		select @allStackCountPerUser = wal_numberOfShares, @allMoneyCountPerUser = wal_MoneyLimit from Wallet where wal_id = @wall_id

		declare @roznicaresult money
		set @roznicaresult = @allMoneyCountPerUser - (@TransactionPriceBuy)
		declare @sumaStack int
		set @sumaStack = @allStackCountPerUser + @countNumberStockInTransaction

		update Wallet
		set
			wal_numberOfShares = @sumaStack,
			wal_MoneyLimit = @roznicaresult
		where
			wal_id = @wall_id

	
end
else
begin
	print 'niewystraczajac ilosc akcji na rynku user musi zmienic ilosc akcji do kupienia '
end
END




