using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleNotes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoteForm : ContentView
    {
        public NoteForm() => this.InitializeComponent();

        public Action SaveAction { get; set; }

        private void SaveButtonPressed(object sender, EventArgs e) => this.SaveAction?.Invoke();
    }
}