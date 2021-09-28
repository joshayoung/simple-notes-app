namespace SimpleNotes.Models
{
    public class Note
    {
        public string Description { get; set; }

        public Note()
        {
        }

        public Note(string description)
        {
            Description = description;
        }
    }
}