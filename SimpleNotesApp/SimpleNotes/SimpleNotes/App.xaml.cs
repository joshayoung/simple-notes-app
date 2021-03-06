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
            this.InitializeComponent();
            Bootstrap.Init();

            if (Bootstrap.NoteRepository != null)
            {
                var noteRepositoryViewModel = new NoteRepositoryViewModel(Bootstrap.NoteRepository);
                this.MainPage = new NavigationPage(new MainPage(noteRepositoryViewModel));
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}