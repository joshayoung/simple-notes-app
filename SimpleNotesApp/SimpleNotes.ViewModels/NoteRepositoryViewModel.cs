using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using SimpleNotes.Models;

namespace SimpleNotes.ViewModels
{
    public class NoteRepositoryViewModel : INotifyPropertyChanged
    {
        private readonly NotesRepository notesRepository;

        public NoteRepositoryViewModel(NotesRepository notesRepository)
        {
            this.notesRepository = notesRepository;

            this.Refresh();

            notesRepository.PropertyChanged += this.NotesRepositoryOnPropertyChanged;
            notesRepository.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(NotesRepository.Notes))
                {
                    this.NotifyPropertyChanged(nameof(this.Notes));
                }
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool NotesExist => this.notesRepository.NotesExist;

        public List<NoteViewModel> Notes { get; set; } = new List<NoteViewModel>();

        public NoteViewModel GetInitialNote()
        {
            int id = this.notesRepository.Notes.Count + 1;
            return new NoteViewModel(new Note(id), this.notesRepository);
        }

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NotesRepositoryOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NotesRepository.Notes))
            {
                this.Refresh();
            }

            if (e.PropertyName == nameof(NotesRepository.NotesExist))
            {
                this.NotifyPropertyChanged(nameof(this.NotesExist));
            }
        }

        private void Refresh()
        {
            var noteList = this.notesRepository.Notes.Select(note => new NoteViewModel(note, this.notesRepository)).ToList();

            this.Notes = noteList;
            this.notesRepository.UpdateNotesExist();
        }
    }
}