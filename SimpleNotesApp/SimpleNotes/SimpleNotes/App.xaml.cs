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
            Bootstrap.Init();

            var noteRepositoryViewModel = new NoteRepositoryViewModel(Bootstrap.NotesRepository);
            noteRepositoryViewModel.Refresh();
            MainPage = new NavigationPage(new MainPage(noteRepositoryViewModel));
        }

        protected override void OnStart() { }
        protected override void OnSleep() { }
        protected override void OnResume() { }
    }
}