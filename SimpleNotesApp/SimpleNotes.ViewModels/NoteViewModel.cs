using SimpleNotes.Models;

namespace SimpleNotes.ViewModels
{
    public class NoteViewModel
    {
        private readonly Note note;

        public string Description => note.Description;

        public NoteViewModel(Note note)
        {
            this.note = note;
        }
    }
}