using StocksData.Model;

namespace StocksData.Services
{
    public interface IStocksDeserializer
    {
        Company Deserialize(string FileContents);
    }
}