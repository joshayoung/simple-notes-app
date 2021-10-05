using SimpleNotes.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleNotes.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailPage : ContentPage
    {
        private readonly NoteViewModel noteViewModel;
        
        public DetailPage(NoteViewModel noteViewModel)
        {
            InitializeComponent();
            this.BindingContext = this.noteViewModel = noteViewModel;
        }
    }
}