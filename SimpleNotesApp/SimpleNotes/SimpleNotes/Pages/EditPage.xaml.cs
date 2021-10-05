using System;
using SimpleNotes.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleNotes.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPage : ContentPage
    {
        private readonly NoteViewModel noteViewModel;
        
        public EditPage(NoteViewModel noteViewModel)
        {
            InitializeComponent();
            this.BindingContext = this.noteViewModel = noteViewModel;
        }

        private async void SaveNote(object sender, EventArgs e)
        {
            await noteViewModel.SaveEditsAsync();
            await Navigation.PopModalAsync();
        }
    }
}