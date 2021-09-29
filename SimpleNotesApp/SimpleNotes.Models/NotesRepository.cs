using System.Collections.Generic;

namespace SimpleNotes.Models
{
    public class NotesRepository
    {
        public List<Note> Notes = new List<Note>();

        public NotesRepository()
        {
            Notes.Add(new Note("First Note"));
            Notes.Add(new Note("Second Note"));
            Notes.Add(new Note("Third Note"));
            Notes.Add(new Note("Fourth Note"));
        }
    }
}