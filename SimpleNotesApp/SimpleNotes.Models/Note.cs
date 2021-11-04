using System.ComponentModel;

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

        public virtual event PropertyChangedEventHandler? PropertyChanged;

        public int Id
        {
            get => this.id;
            set
            {
                this.id = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Id)));
            }
        }

        public string? Title
        {
            get => this.title;
            set
            {
                this.title = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Title)));
            }
        }

        public string? Description
        {
            get => this.description;
            set
            {
                this.description = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Description)));
            }
        }
    }
}