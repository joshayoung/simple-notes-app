using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimpleNotes.Models;

namespace SimpleNotes.ViewModels
{
    public class NoteRepositoryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        private readonly NotesRepository notesRepository;

        public List<NoteViewModel> Notes { get; set; } = new List<NoteViewModel>();

        public NoteRepositoryViewModel(NotesRepository notesRepository)
        {
            this.notesRepository = notesRepository;
            
            Refresh();
            
            notesRepository.PropertyChanged += NotesRepositoryOnPropertyChanged;
            notesRepository.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Notes)) NotifyPropertyChanged(nameof(Notes));
            };
        }

        private void NotesRepositoryOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NotesRepository.Notes)) Refresh();
        }

        public void Refresh()
        {
            var noteList = new List<NoteViewModel>();
            foreach (var note in this.notesRepository.Notes)
            {
                noteList.Add(new NoteViewModel(note));
            }
            this.Notes = noteList;
        }

        public Note GetInitialNote()
        {
            return new Note(notesRepository, "");
        }
        
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}