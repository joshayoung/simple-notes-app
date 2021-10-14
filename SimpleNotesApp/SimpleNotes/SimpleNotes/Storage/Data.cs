using System.Threading.Tasks;
using Shared;
using Xamarin.Forms;

namespace SimpleNotes.Storage
{
    public class Data : IData
    {
        public async Task SaveAsync(string id, string value)
        {
            if (Application.Current.Properties.ContainsKey(id))
            {
                Application.Current.Properties[id] = value;
            }
            else
            {
                Application.Current.Properties.Add(id, value);
            }

            await Application.Current.SavePropertiesAsync();
        }

        public virtual string? Retrieve(string id)
        {
            return Application.Current.Properties.ContainsKey(id) ? Application.Current.Properties[id].ToString() : null;
        }
    }
}