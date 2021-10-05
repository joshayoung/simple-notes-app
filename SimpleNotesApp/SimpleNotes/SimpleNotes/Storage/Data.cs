using System.Threading.Tasks;
using Shared;
using Xamarin.Forms;

namespace SimpleNotes.Storage
{
    public class Data : IData
    {
        public async Task SaveAsync(string id, string value)
        {
            Application.Current.Properties[id] = value;
            await Application.Current.SavePropertiesAsync();
        }

        public string Retrieve(string id)
        {
            return Application.Current.Properties.ContainsKey(id) ? Application.Current.Properties[id].ToString() : "";
        }
    }
}