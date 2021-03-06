using System;
using Shared;
using SimpleNotes.Models;
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

        private void LoadModifyPage(object sender, EventArgs e)
        {
            try
            {
                this.Navigation.PushModalAsync(new ModifyPage(this.noteRepositoryViewModel.GetInitialNote(), NoteActionType.AddNote));
            }
            catch (Exception exception)
            {
                // TODO: Add Logging
            }
        }

        private void EditNote(object sender, EventArgs e)
        {
            var noteViewModel = (NoteViewModel)((BindableObject)sender).BindingContext;
            this.Navigation.PushModalAsync(new ModifyPage(noteViewModel.EditNoteCopy(), NoteActionType.EditNote));
        }

        private async void DeleteNote(object sender, EventArgs e)
        {
            bool deleteNote = await this.DisplayAlert("Continue", "Permanently delete this note?", "Yes", "No");
            if (!deleteNote)
            {
                return;
            }

            var noteViewModel = (NoteViewModel)((BindableObject)sender).BindingContext;

            try
            {
                await noteViewModel.DeleteAsync();
            }
            catch (Exception exception)
            {
                await this.DisplayAlert("Alert", ErrorMessages.NotesDeleteError, "OK");

                // TODO: Log Exception
            }
        }

        private void GoToDetails(object sender, EventArgs e)
        {
            try
            {
                var noteViewModel = (NoteViewModel)((BindableObject)sender).BindingContext;
                this.Navigation.PushModalAsync(new ModifyPage(noteViewModel, NoteActionType.DetailNote));
            }
            catch (Exception exception)
            {
                // TODO: Add Logging
            }
        }
    }
}