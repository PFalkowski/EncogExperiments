select count(*) from StockQuotes
select distinct (ticker) from StockQuotes
select distinct (ticker) from Companies
select * from StockQuotes where Ticker like '%MBANK%'
select * from Companies where Ticker like '%MBANK%'
select Ticker from StockQuotes group by Ticker having max([date]) = 20180209 
select ticker, max(date) from StockQuotes group by Ticker having max(date) = 20180209 and count(*) > 200

truncate table StockQuotes
insert into [Companies] (Ticker) values ('test')
delete from [Companies] where Ticker = 'Test'
DBCC SHRINKFILE ('StockMarketDb_log', 1)


alter database StockMarketDb set offline with rollback immediate
drop database StockMarketDb 

Select Top(1) Ticker from [Companies]

use master
if db_id('AddingStockToNhibernateWorks') is not null select 1 else select 0