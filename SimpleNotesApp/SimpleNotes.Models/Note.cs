namespace SimpleNotes.Models
{
    public class Note
    {
        public string Description { get; set; }

        public Note(string description)
        {
            Description = description;
        }
    }
}