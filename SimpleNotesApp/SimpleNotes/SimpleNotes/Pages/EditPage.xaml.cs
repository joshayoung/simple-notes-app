using System;
using System.Threading.Tasks;
using SimpleNotes.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
#pragma warning disable 168

namespace SimpleNotes.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPage : ContentPage
    {
        private readonly NoteViewModel noteViewModel;

        public EditPage(NoteViewModel noteViewModel, string pageTitle)
        {
            this.InitializeComponent();
            this.noteViewModel = noteViewModel;

            // TODO: Find a better way to do this:
            this.noteViewModel.PageTitle = pageTitle;

            this.BindingContext = this.noteViewModel;

            this.NoteForm.SaveAction = async () => await this.SaveNote();
        }

        public string PageTitle { get; set; }

        private async Task SaveNote()
        {
            try
            {
                await this.noteViewModel.SaveEditsAsync();
                await this.Navigation.PopModalAsync();
            }
            catch (Exception exception)
            {
                // TODO: Add Logging
            }
        }
    }
}