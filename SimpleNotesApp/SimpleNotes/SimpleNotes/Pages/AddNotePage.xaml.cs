using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleNotes.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNotePage : ContentPage
    {
        public AddNotePage()
        {
            InitializeComponent();
        }

        private void SaveNote(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}