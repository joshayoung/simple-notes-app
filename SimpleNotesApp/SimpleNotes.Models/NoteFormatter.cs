namespace SimpleNotes.Models
{
    public static class NoteFormatter
    {
        public static Note TrimWhitespace(this Note note)
        {
            note.Title = note.Title?.Trim();
            note.Description = note.Description?.Trim();

            return note;
        }
    }
}