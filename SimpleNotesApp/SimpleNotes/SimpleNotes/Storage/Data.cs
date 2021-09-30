using Shared;
using Xamarin.Forms;

namespace SimpleNotes.Storage
{
    public class Data : IData
    {
        public void Save(string id, string value)
        {
            Application.Current.Properties[id] = value;
        }

        public string Retrieve(string id)
        {
            return Application.Current.Properties.ContainsKey(id) ? Application.Current.Properties[id].ToString() : "";
        }
    }
}