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
            Navigation.PushModalAsync(new AddPage(noteRepositoryViewModel.GetInitialNote()));
        }

        private void EditNote(object sender, EventArgs e)
        {
            var noteViewModel = (NoteViewModel)((BindableObject)sender).BindingContext;
            Navigation.PushModalAsync(new EditPage(noteViewModel.EditNoteCopy()));
        }
        
        private void DeleteNote(object sender, EventArgs e)
        {
            var noteViewModel = (NoteViewModel)((BindableObject)sender).BindingContext;
            noteViewModel.Delete();
            Navigation.PopModalAsync();
        }

        private void GoToDetails(object sender, EventArgs e)
        {
            var noteViewModel = (NoteViewModel)((BindableObject)sender).BindingContext;
            Navigation.PushModalAsync(new DetailPage(noteViewModel));
        }
    }
}