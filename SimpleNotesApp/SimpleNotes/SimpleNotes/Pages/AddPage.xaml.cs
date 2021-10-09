using System;
using System.Threading.Tasks;
using SimpleNotes.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
#pragma warning disable 168

namespace SimpleNotes.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPage : ContentPage
    {
        private readonly NoteViewModel noteViewModel;

        public AddPage(NoteViewModel noteViewModel, string pageTitle)
        {
            this.InitializeComponent();
            this.noteViewModel = noteViewModel;

            // TODO: Find a better way to do this:
            this.noteViewModel.PageTitle = pageTitle;

            this.BindingContext = this.noteViewModel;
            this.NoteForm.SaveAction = async () => await this.SaveNote();
        }

        private async Task SaveNote()
        {
            try
            {
                await this.noteViewModel.SaveAsync();
                await this.Navigation.PopModalAsync();
            }
            catch (Exception exception)
            {
                // TODO: Add Logging
            }
        }
    }
}