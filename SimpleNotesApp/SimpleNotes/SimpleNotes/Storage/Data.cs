using Xamarin.Forms;

namespace SimpleNotes.Storage
{
    public static class Data
    {
        public static void Save(string id, string value)
        {
            Application.Current.Properties[id] = value;
        }

        public static string Retrieve(string id)
        {
            return Application.Current.Properties.ContainsKey(id) ? Application.Current.Properties[id].ToString() : "";
        }
    }
}