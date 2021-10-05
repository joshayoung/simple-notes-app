using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleNotes.Models
{
    public class Note : INotifyPropertyChanged
    {
        private int id;
        private string? title;
        private string? description;
        
        public int Id
        {
            get => id;
            set
            {
                this.id = value;
                NotifyPropertyChanged();
            }
        }
        
        public string? Title
        {
            get => title;
            set
            {
                this.title = value;
                NotifyPropertyChanged();
            }
        }
        
        public string? Description
        {
            get => description;
            set
            {
                this.description = value;
                NotifyPropertyChanged();
            }
        }

        public Note(int id, string? title = null, string? description = null)
        {
            this.Id = id;
            this.title = title;
            this.description = description;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}