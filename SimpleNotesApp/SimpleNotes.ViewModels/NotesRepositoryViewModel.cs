using SimpleNotes.Models;

namespace SimpleNotes.ViewModels
{
    public class NotesRepositoryViewModel
    {
        private readonly NotesRepository notesRepository;

        public NotesRepositoryViewModel(NotesRepository notesRepository)
        {
            this.notesRepository = notesRepository;
        }
    }
}