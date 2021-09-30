namespace Shared
{
    public interface IData
    {
        void Save(string id, string value);
        string Retrieve(string id);
    }
}