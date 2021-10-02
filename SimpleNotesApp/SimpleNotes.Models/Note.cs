using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleNotes.Models
{
    public class Note : INotifyPropertyChanged
    {
        private string description;
        private string id;
        private readonly NotesRepository repository;
        
        public string Id
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

        public Note(NotesRepository notesRepository, string description)
        {
            this.repository = notesRepository;
            Description = description;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Save()
        {
            repository.Save(this);
        }
    }
}