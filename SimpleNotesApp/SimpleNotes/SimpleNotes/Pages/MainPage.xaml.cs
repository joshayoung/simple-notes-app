using System;
using System.IO;
using Sentry;
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
            var root = Directory.GetCurrentDirectory();
            var dotenv = Path.Combine(root, ".env");
            ProjectEnvironmentalVariables.Load(dotenv);

            var dsn = System.Environment.GetEnvironmentVariable("DSN") ?? string.Empty;
            this.BindingContext = this.noteRepositoryViewModel = noteRepositoryViewModel;
        }

        private void LoadModifyPage(object sender, EventArgs e)
        {
            try
            {
                throw new Exception("test");
                this.Navigation.PushModalAsync(new ModifyPage(this.noteRepositoryViewModel.GetInitialNote(), NoteActionType.AddNote));
            }
            catch (Exception exception)
            {
                // TODO: Add Logging
                SentrySdk.CaptureException(exception);
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