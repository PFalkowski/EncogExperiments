﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StocksData.Contexts;
using StocksData.Model;
using StocksData.Repositories;

namespace StocksData.UnitsOfWork
{
   public  class StockEfUnitOfWork : EfUnitOfWork
    {
        public StockRepository Stocks { get; }
        public StockEfUnitOfWork(StockEfContext context) : base(context)
        {
            Stocks = new StockRepository(new EfRepository<Company>(context));
        }
    }
}
