using System;
using SimpleNotes.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleNotes.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailsNotePage : ContentPage
    {
        private readonly NoteViewModel noteViewModel;
        public DetailsNotePage(NoteViewModel noteViewModel)
        {
            InitializeComponent();
            this.BindingContext = this.noteViewModel = noteViewModel;
        }
    }
}