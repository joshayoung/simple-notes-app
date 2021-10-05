using System.Threading.Tasks;

namespace Shared
{
    public interface IData
    {
        Task Save(string id, string value);
        string Retrieve(string id);
    }
}