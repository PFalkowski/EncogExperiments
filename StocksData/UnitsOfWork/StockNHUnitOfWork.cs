﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StocksData.Contexts;
using StocksData.Models;
using StocksData.Repositories;

namespace StocksData.UnitsOfWork
{
    public class StockNhUnitOfWork : NhUnitOfWork
    {
        public StockRepository Stocks { get; }
        public StockNhUnitOfWork(INhContext context) : base(context)
        {
            Stocks = new StockRepository(new NhRepository<Company>(context.SessionFactory.OpenSession()));
        }
    }
}
