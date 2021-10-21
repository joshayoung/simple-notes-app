using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using SimpleNotes.Models;

namespace SimpleNotes.ViewModels
{
    public class NoteRepositoryViewModel : INotifyPropertyChanged
    {
        private readonly NoteRepository noteRepository;

        public NoteRepositoryViewModel(NoteRepository noteRepository)
        {
            this.noteRepository = noteRepository;

            this.Refresh();

            noteRepository.PropertyChanged += this.NotesRepositoryOnPropertyChanged;
            noteRepository.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(NoteRepository.Notes))
                {
                    this.NotifyPropertyChanged(nameof(this.Notes));
                }
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool NotesExist => this.noteRepository.NotesExist;

        public List<NoteViewModel> Notes { get; set; } = new List<NoteViewModel>();

        public NoteViewModel GetInitialNote()
        {
            int id = this.noteRepository.Notes.Count + 1;
            return new NoteViewModel(new Note(id), this.noteRepository);
        }

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NotesRepositoryOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteRepository.Notes))
            {
                this.Refresh();
            }

            if (e.PropertyName == nameof(NoteRepository.NotesExist))
            {
                this.NotifyPropertyChanged(nameof(this.NotesExist));
            }
        }

        private void Refresh()
        {
            var noteList = this.noteRepository.Notes.Select(
                note => new NoteViewModel(note, this.noteRepository))
                               .ToList();

            this.Notes = noteList;
            // this.noteRepository.UpdateNotesExist();
        }
    }
}