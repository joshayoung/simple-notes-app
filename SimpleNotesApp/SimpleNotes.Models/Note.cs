using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleNotes.Models
{
    public class Note : INotifyPropertyChanged
    {
        private string description;
        public string Description
        {
            get => description;
            set
            {
                this.description = value;
                NotifyPropertyChanged();
            }
        }

        public Note(string description)
        {
            Description = description;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}