using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SimpleNotes.Models;

namespace SimpleNotes.ViewModels
{
    public class NoteViewModel : INotifyPropertyChanged
    {
        private readonly Note note;
        private readonly NotesRepository repository;

        public NoteActionType? NoteAction { get; set; }

        public NoteViewModel(Note note, NotesRepository notesRepository)
        {
            this.note = note;
            this.repository = notesRepository;

            this.note.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case nameof(Note.Title):
                        this.NotifyPropertyChanged(nameof(this.Title));
                        break;
                    case nameof(Note.Description):
                        this.NotifyPropertyChanged(nameof(this.Description));
                        break;
                }
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public int Id
        {
            get => this.note.Id;
            set => this.note.Id = value;
        }

        public string? Title
        {
            get => this.note.Title;
            set => this.note.Title = value;
        }

        public string? Description
        {
            get => this.note.Description;
            set => this.note.Description = value;
        }

        public NoteViewModel EditNoteCopy()
        {
            var newNote = new Note(this.note.Id, this.note.Title, this.note.Description);
            return new NoteViewModel(newNote, this.repository);
        }

        public async Task SaveAsync()
        {
            await this.repository.Save(this.note);
        }

        public async Task DeleteAsync()
        {
            await this.repository.Delete(this.note);
        }

        public async Task SaveEditsAsync()
        {
            await this.repository.SaveEdits(this.note);
        }

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}