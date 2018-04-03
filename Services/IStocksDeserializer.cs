using StocksData.Model;

namespace Services
{
    public interface IStocksDeserializer
    {
        Company Deserialize(string FileContents);
    }
}