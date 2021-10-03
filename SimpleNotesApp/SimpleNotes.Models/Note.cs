using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleNotes.Models
{
    public class Note : INotifyPropertyChanged
    {
        private int id;
        private string description;
        
        public int Id
        {
            get => id;
            set
            {
                this.id = value;
                NotifyPropertyChanged();
            }
        }
        
        public string Description
        {
            get => description;
            set
            {
                this.description = value;
                NotifyPropertyChanged();
            }
        }

        public Note(int id, string description)
        {
            this.Id = id;
            this.description = description;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}