using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimpleNotes.Models;

namespace SimpleNotes.ViewModels
{
    public class NoteViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        private readonly Note note;
        private readonly NotesRepository repository;
        
        public int Id
        {
            get => note.Id;
            set => note.Id = value;
        }
        
        public string? Title
        {
            get => note.Title;
            set => note.Title = value;
        }

        public string? Description
        {
            get => note.Description;
            set => note.Description = value;
        }

        public NoteViewModel(Note note, NotesRepository notesRepository)
        {
            this.note = note;
            this.repository = notesRepository;

            this.note.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case nameof(Note.Title):
                        NotifyPropertyChanged(nameof(NoteViewModel.Title));
                        break;
                    case nameof(Note.Description):
                        NotifyPropertyChanged(nameof(NoteViewModel.Description));
                        break;
                }
            };
        }

        public NoteViewModel EditNoteCopy()
        {
            var newNote = new Note(note.Id, note.Title, note.Description);
            return new NoteViewModel(newNote, repository);
        }

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void Save() => await this.repository.Save(note);

        public async void Delete() => await this.repository.Delete(note);
    }
}