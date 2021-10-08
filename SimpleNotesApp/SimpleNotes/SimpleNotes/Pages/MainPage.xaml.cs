using System;
using SimpleNotes.ViewModels;
using Xamarin.Forms;
#pragma warning disable 168

namespace SimpleNotes.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly NoteRepositoryViewModel noteRepositoryViewModel;

        public MainPage(NoteRepositoryViewModel noteRepositoryViewModel)
        {
            this.InitializeComponent();
            this.BindingContext = this.noteRepositoryViewModel = noteRepositoryViewModel;
        }

        private void AddNotes(object sender, EventArgs e)
        {
            try
            {
                this.Navigation.PushModalAsync(new AddPage(this.noteRepositoryViewModel.GetInitialNote()));
            }
            catch (Exception exception)
            {
                // TODO: Add Logging
            }
        }

        private void EditNote(object sender, EventArgs e)
        {
            var noteViewModel = (NoteViewModel)((BindableObject)sender).BindingContext;
            this.Navigation.PushModalAsync(new EditPage(noteViewModel.EditNoteCopy()));
        }

        private async void DeleteNote(object sender, EventArgs e)
        {
            bool deleteNote = await this.DisplayAlert("Continue", "Permanently delete this note?", "Yes", "No");
            if (!deleteNote)
            {
                return;
            }

            var noteViewModel = (NoteViewModel)((BindableObject)sender).BindingContext;
            await noteViewModel.DeleteAsync();
        }

        private void GoToDetails(object sender, EventArgs e)
        {
            try
            {
                var noteViewModel = (NoteViewModel)((BindableObject)sender).BindingContext;
                this.Navigation.PushModalAsync(new DetailPage(noteViewModel));
            }
            catch (Exception exception)
            {
                // TODO: Add Logging
            }
        }
    }
}