using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;
using System.Threading.Tasks;
using CsvHelper;
using Extensions.Serialization;

namespace StocksData.Contexts
{
    public class StockCsvContextLazy<TEntity> where TEntity : class
    {
        public FileInfo File { get; set; }
        public virtual IEnumerable<TEntity> Entities { get; set; }
        public CultureInfo Culture { get; set; }


        public StockCsvContextLazy(FileInfo file)
        {
            File = file; 
            //if (!file.Exists) 
            //if (!file.Exists) throw new FileNotFoundException(nameof(file));

            using (var csv = new CsvReader(file.OpenText(), true))
            {
                Entities = csv.GetRecords<TEntity>();
            }
        }

        public void SaveChanges()
        {
            using (var writer = new CsvWriter(File.CreateText(), false))
            {
                writer.Configuration.SanitizeForInjection = true;
                writer.Configuration.CultureInfo = Culture;
                writer.WriteRecords(Entities);
                writer.Flush();
            }
        }

        public Task SaveChangesAsync()
        {
            using (var writer = new CsvWriter(File.CreateText(), false))
            {
                writer.Configuration.SanitizeForInjection = true;
                writer.Configuration.CultureInfo = Culture;
                writer.WriteRecords(Entities);
                return writer.FlushAsync();
            }
        }

        public void Dispose()
        {
        }
    }
}