using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;

namespace StocksData.Contexts
{
    public class StockCsvContext<TEntity> where TEntity : class
    {
        public FileInfo File { get; set; }
        public List<TEntity> Entities { get; set; }
        public CultureInfo Culture { get; set; }

        public StockCsvContext(FileInfo file)
        {
            File = file;
            if (file.Exists)
            {
                using (var csv = new CsvReader(file.OpenText(), false))
                {
                    Entities = csv.GetRecords<TEntity>().ToList();
                }
            }
            else
            {
                file.Create();
                Entities = new List<TEntity>();
            }
        }

        public void SaveChanges()
        {
            using (var writer = new CsvWriter(File.CreateText(), false))
            {
                writer.Configuration.SanitizeForInjection = true;
                writer.Configuration.CultureInfo = Culture ?? CultureInfo.CurrentCulture;
                writer.WriteRecords(Entities);
                writer.Flush();
            }
        }

        public Task SaveChangesAsync()
        {
            using (var writer = new CsvWriter(File.CreateText(), false))
            {
                writer.Configuration.SanitizeForInjection = true;
                writer.Configuration.CultureInfo = Culture ?? CultureInfo.CurrentCulture;
                writer.WriteRecords(Entities);
                return writer.FlushAsync();
            }
        }
    }
}