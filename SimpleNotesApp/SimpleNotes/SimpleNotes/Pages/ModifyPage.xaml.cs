using System;
using System.Threading.Tasks;
using Shared;
using SimpleNotes.Models;
using SimpleNotes.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
#pragma warning disable 168

namespace SimpleNotes.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifyPage : ContentPage
    {
        private readonly NoteViewModel noteViewModel;

        public ModifyPage(NoteViewModel noteViewModel, NoteActionType noteActionType)
        {
            this.InitializeComponent();
            this.noteViewModel = noteViewModel;

            this.noteViewModel.NoteAction = noteActionType;

            // TODO: Refactor this
            this.NoteForm.SaveAction = noteActionType switch
            {
                NoteActionType.AddNote => async () => await this.AddNote(),
                NoteActionType.EditNote => async () => await this.EditNote(),
                _ => this.NoteForm.SaveAction
            };
            this.BindingContext = this.noteViewModel;
        }

        private void Close(object sender, EventArgs e) => this.Navigation.PopModalAsync();

        private async Task AddNote()
        {
            try
            {
                await this.noteViewModel.SaveAsync();
            }
            catch (Exception exception)
            {
                await ViewErrorHandling.DisplayAndSetError(ErrorMessages.NotesAddError);

                // TODO: Add Logging
            }

            await ViewErrorHandling.NavigateBackWhenSuccessful();
        }

        private async Task EditNote()
        {
            try
            {
                await this.noteViewModel.SaveEditsAsync();
                await this.Navigation.PopModalAsync();
            }
            catch (Exception exception)
            {
                await ViewErrorHandling.DisplayAndSetError(ErrorMessages.NotesEditError);

                // TODO: Add Logging
            }
        }
    }
}