select count(*) from StockQuotes
select distinct (ticker) from StockQuotes
select * from StockQuotes where Ticker like '%MBANK%'
truncate table StockQuotes

DBCC SHRINKFILE (  'StockMarketDb_log', 1)

drop database StockMarketDb