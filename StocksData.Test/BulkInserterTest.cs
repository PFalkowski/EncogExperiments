﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StocksData.Services;
using StocksData.UnitsOfWork;
using StocksData.Test.Mocks;
using Xunit;
using StocksData.Model;

namespace StocksData.Test
{
    public class BulkInserterTest
    {
        [Theory]
        [ClassData(typeof(MbankMock))]
        public void BulkInsertOneStock(Company company)
        {
            string connectionStr = $"server=(localdb)\\MSSQLLocalDB;Initial Catalog={nameof(BulkInsertOneStock)};Integrated Security=True;";

            var context = new StockEfTestContext(connectionStr);
            context.DropDbIfExists();
            context.CreateDbIfNotExists();

            // Act

            var tested = new CompanyBulkInserter(connectionStr);
            tested.BulkInsert(company);

            //Assert

            using (var connection = new SqlConnection(connectionStr))
            using (var command = new SqlCommand("Select Top(1) Ticker from [Companies]", connection))
            {
                connection.Open();
                var result = (string)command.ExecuteScalar();
                Assert.Equal(company.Ticker, result);
            }
        }
    }
}
