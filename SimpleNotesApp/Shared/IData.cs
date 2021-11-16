using System.Threading.Tasks;

namespace Shared
{
    public interface IData
    {
        Task SaveValueAsync(string id, string value);

        string? RetrieveValue(string id);
    }
}