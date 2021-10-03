using System;
using SimpleNotes.ViewModels;
using Xamarin.Forms;

namespace SimpleNotes.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly NoteRepositoryViewModel noteRepositoryViewModel;
        
        public MainPage(NoteRepositoryViewModel noteRepositoryViewModel)
        {
            InitializeComponent();
            this.BindingContext = this.noteRepositoryViewModel = noteRepositoryViewModel;
        }

        private void AddNotes(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddNotePage(noteRepositoryViewModel.GetInitialNote()));
        }

        private void GoToDetails(object sender, EventArgs e)
        {
            var noteViewModel = (NoteViewModel)((BindableObject)sender).BindingContext;
            Navigation.PushModalAsync(new DetailsNotePage(noteViewModel));
        }
    }
}