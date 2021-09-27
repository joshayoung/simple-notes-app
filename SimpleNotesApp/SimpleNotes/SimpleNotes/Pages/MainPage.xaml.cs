using System;
using Xamarin.Forms;

namespace SimpleNotes.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void AddNotes(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddNotePage());
        }
    }
}