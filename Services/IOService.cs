using Infrastracture;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Services
{
    public class IOService// : IOService
    {
        public Dictionary<string, string> ReadDirectory(DirectoryInfo dir, string pattern = "*.*", SearchOption searchDepth = SearchOption.TopDirectoryOnly)
        {
            var dataTemp = new Dictionary<string, string>();

            var filesInDir = dir.EnumerateFiles(pattern).ToList();
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
        public Dictionary<string, string> ReadDirectory(string path, string pattern = "*.*", SearchOption searchDepth = SearchOption.TopDirectoryOnly)
        {
            return ReadDirectory(new DirectoryInfo(path), pattern, searchDepth);
        }
    }
}
