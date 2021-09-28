using System.Collections.Generic;

namespace SimpleNotes.Models
{
    public class NotesRepository
    {
        public List<Note> Notes = new List<Note>();

        public void  Refresh()
        {
            Notes.Add(new Note { Description = "First Note" });
            Notes.Add(new Note { Description = "Second Note" });
            Notes.Add(new Note { Description = "Third Note" });
            Notes.Add(new Note { Description = "Fourth Note" });
        }
    }
}