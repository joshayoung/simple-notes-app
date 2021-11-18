using System;
using System.ComponentModel;
using System.Threading.Tasks;
using SimpleNotes.Models;

namespace SimpleNotes.ViewModels
{
    public class NoteViewModel : INotifyPropertyChanged
    {
        private readonly Note note;
        private readonly NoteRepository repository;

        public NoteViewModel(Note note, NoteRepository noteRepository)
        {
            this.note = note;
            this.repository = noteRepository;

            this.note.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case nameof(Note.Title):
                        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Title)));
                        break;
                    case nameof(Note.Description):
                        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Description)));
                        break;
                }
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public NoteActionType? NoteAction { get; set; }

        public int Id => this.note.Id;

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
            await this.repository.SaveAsync(this.note);
        }

        public async Task DeleteAsync()
        {
            await this.repository.DeleteAsync(this.note);
        }

        public async Task SaveEditsAsync()
        {
            await this.repository.SaveEditsAsync(this.note);
        }
    }
}