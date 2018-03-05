using CsvHelper.Configuration;
using Extensions.Serialization;
using StocksData.Mappings;
using StocksData.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace StocksData.Services
{
    public class StocksDeserializer : IStocksDeserializer
    {
        public readonly CultureInfo Culture;
        public readonly ClassMap<StockQuote> Map;

        public StocksDeserializer(ClassMap<StockQuote> map, CultureInfo culture = null)
        {
            Culture = culture ?? CultureInfo.InvariantCulture;
            Map = map;
        }

        public Company Deserialize(string FileContents)
        {
            var deserializedQuotes = FileContents.DeserializeFromCsv(Map, Culture).ToList();
            var companyName = deserializedQuotes.First().Ticker;

            return new Company
            {
                Ticker = companyName,
                Quotes = deserializedQuotes
            };
        }
    }
}