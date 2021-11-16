using System;
using System.Threading.Tasks;
using Shared;
using Xamarin.Forms;

namespace SimpleNotes.Storage
{
    public class Data : IData
    {
        public virtual async Task SaveValueAsync(string id, string value)
        {
            try
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
            catch (Exception e)
            {
                // TODO: Improve error message
                // TODO: Add logging here
                throw new Exception("Saving Data Failed", e);
            }
        }

        public virtual string? RetrieveValue(string id)
        {
            return Application.Current.Properties.ContainsKey(id) ? Application.Current.Properties[id].ToString() : null;
        }
    }
}