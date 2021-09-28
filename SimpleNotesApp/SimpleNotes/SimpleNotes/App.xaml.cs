using System;
using SimpleNotes.Models;
using SimpleNotes.Pages;
using SimpleNotes.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace SimpleNotes
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // TODO: Refactor this and use DI:
            var notesRepository = new NotesRepository();
            var noteRepositoryViewModel = new NoteRepositoryViewModel(notesRepository);
            noteRepositoryViewModel.Refresh();
            MainPage = new NavigationPage(new MainPage(noteRepositoryViewModel));
        }

        protected override void OnStart() { }
        protected override void OnSleep() { }
        protected override void OnResume() { }
    }
}