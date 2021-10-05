using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleNotes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CloseModal : ContentView
    {
        public CloseModal() => InitializeComponent();

        private void Close(object sender, EventArgs e)
        {
            this.Navigation.PopModalAsync();
        }
    }
}