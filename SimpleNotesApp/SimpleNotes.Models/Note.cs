using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleNotes.Models
{
    public class Note : INotifyPropertyChanged
    {
        private int id;
        private string? title;
        private string? description;

        public Note(int id, string? title = null, string? description = null)
        {
            this.Id = id;
            this.title = title;
            this.description = description;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id
        {
            get => this.id;
            set
            {
                this.id = value;
                this.NotifyPropertyChanged();
            }
        }

        public string? Title
        {
            get => this.title;
            set
            {
                this.title = value;
                this.NotifyPropertyChanged();
            }
        }

        public string? Description
        {
            get => this.description;
            set
            {
                this.description = value;
                this.NotifyPropertyChanged();
            }
        }

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}