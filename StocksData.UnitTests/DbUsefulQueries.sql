select count(*) from StockQuotes
select distinct (ticker) from StockQuotes
select distinct (ticker) from Companies
select * from StockQuotes where Ticker like '%MBANK%'
select * from Companies where Ticker like '%MBANK%'
truncate table StockQuotes
insert into [Companies] (Ticker) values ('test')
delete from [Companies] where Ticker = 'Test'
DBCC SHRINKFILE ('StockMarketDb_log', 1)

alter database StockMarketDb set offline with rollback immediate
drop database StockMarketDb 

Select Top(1) Ticker from [Companies]