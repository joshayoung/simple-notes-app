using System.Threading.Tasks;

namespace Shared
{
    public interface IData
    {
        Task SaveAsync(string id, string value);

        string? Retrieve(string id);
    }
}