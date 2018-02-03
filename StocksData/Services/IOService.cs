using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksData.Services
{
    public class IOService
    {
        public Dictionary<string, string> ReadDirectory(string path, string pattern = "*.*")
        {
            var directory = new DirectoryInfo(path);
            var dataTemp = new Dictionary<string, string>();

            var filesInDir = directory.EnumerateFiles(pattern).ToList();
            if (filesInDir.Count != 0)
            {
                foreach (var file in filesInDir)
                {
                    if (!dataTemp.ContainsKey(file.Name))
                    {
                        dataTemp.Add(file.Name, File.ReadAllText(file.FullName));
                    }
                }
                return dataTemp;
            }
            return null;
        }
    }
}
