using System.Collections.Generic;
using SimpleNotes.Models;

namespace SimpleNotes.ViewModels
{
    public class NoteRepositoryViewModel
    {
        private readonly NotesRepository notesRepository;

        public List<NoteViewModel> Notes { get; set; } = new List<NoteViewModel>();

        public NoteRepositoryViewModel(NotesRepository notesRepository)
        {
            this.notesRepository = notesRepository;
        }

        public void Refresh()
        {
            foreach (var note in notesRepository.Notes)
            {
                Notes.Add(new NoteViewModel(note));
            }
        }

        public Note GetInitialNote()
        {
            return new Note(notesRepository, "first test");
        }
    }
}