using System;
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

        public AddPage(NoteViewModel noteViewModel)
        {
            this.InitializeComponent();
            this.BindingContext = this.noteViewModel = noteViewModel;
        }

        private async void SaveNote(object sender, EventArgs e)
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