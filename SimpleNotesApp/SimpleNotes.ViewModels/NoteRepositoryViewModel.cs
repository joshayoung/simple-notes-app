using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool NotesExist => this.noteRepository.NotesExist;

        public List<NoteViewModel> Notes { get; set; } = new List<NoteViewModel>();

        public NoteViewModel GetInitialNote()
        {
            int id = this.noteRepository.Notes.Count + 1;
            return new NoteViewModel(new Note(id), this.noteRepository);
        }

        private void NotesRepositoryOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteRepository.Notes))
            {
                this.Refresh();
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Notes)));
            }

            if (e.PropertyName == nameof(NoteRepository.NotesExist))
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.NotesExist)));
            }
        }

        private void Refresh()
        {
            this.Notes = this.noteRepository.Notes.Select(n => new NoteViewModel(n, this.noteRepository)).ToList();
            this.noteRepository.UpdateNotesExist();
        }
    }
}