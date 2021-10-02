using SimpleNotes.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleNotes.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsNotePage : ContentPage
    {
        public DetailsNotePage(NoteViewModel noteViewModel)
        {
            InitializeComponent();
            this.BindingContext = noteViewModel;
        }
    }
}