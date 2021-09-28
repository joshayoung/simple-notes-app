using System;
using SimpleNotes.ViewModels;
using Xamarin.Forms;

namespace SimpleNotes.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(NoteRepositoryViewModel noteRepositoryViewModel)
        {
            InitializeComponent();
            this.BindingContext = noteRepositoryViewModel;
        }

        private void AddNotes(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddNotePage());
        }
    }
}