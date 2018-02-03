using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;

namespace StocksData.Contexts
{
    public class StockCsvContextEager<TEntity> where TEntity : class
    {
        public FileInfo File { get; set; }
        public virtual List<TEntity> Entities { get; set; }

        public StockCsvContextEager(FileInfo file)
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
                //throw new FileNotFoundException(nameof(file));
            }
        }

        public void SaveChanges()
        {
            using (var writer = new CsvWriter(File.CreateText(), false))
            {
                writer.WriteRecords(Entities);
                writer.Flush();
            }
        }

        public Task SaveChangesAsync()
        {
            using (var writer = new CsvWriter(File.CreateText(), false))
            {
                writer.WriteRecords(Entities);
                return writer.FlushAsync();
            }
        }
    }
}