using System.Threading.Tasks;
using Xamarin.Forms;

namespace SimpleNotes
{
    public static class ViewErrorHandling
    {
        private static bool noErrors = true;

        public static async Task DisplayAndSetError(string errorMessage)
        {
            noErrors = false;
            await Application.Current.MainPage.DisplayAlert("Alert", errorMessage, "OK");
        }

        public static async Task NavigateBackWhenSuccessful()
        {
            if (noErrors)
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
        }
    }
}