using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared;

namespace SimpleNotes.Models
{
    public class NoteRepository : INotifyPropertyChanged
    {
        private readonly IData data;

        public NoteRepository(IData data)
        {
            this.data = data;
            this.PopulateNotes();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<Note> Notes { get; set; } = new List<Note>();

        public bool NotesExist => this.Notes.Count > 0;

        public virtual async Task Save(Note note)
        {
            this.Notes.Add(note.TrimWhitespace());
            string? serializeNotes = JsonConvert.SerializeObject(this.Notes);
            await this.data.SaveAsync("notes", serializeNotes);
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Notes)));
        }

        public virtual async Task SaveEdits(Note note)
        {
            var nt = this.Notes.Find(n => n.Id == note.Id);
            nt.Title = note.TrimWhitespace().Title;
            nt.Description = note.TrimWhitespace().Description;
            string? serializeNotes = JsonConvert.SerializeObject(this.Notes);
            await this.data.SaveAsync("notes", serializeNotes);
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Notes)));
        }

        public virtual async Task Delete(Note note)
        {
            string? lsNotes = this.data.Retrieve("notes");

            if (lsNotes == null)
            {
                return;
            }

            var deserializeNotes = JsonConvert.DeserializeObject<List<Note>>(lsNotes);

            if (deserializeNotes == null)
            {
                return;
            }

            int noteIndex = deserializeNotes.FindIndex(n => n.Id == note.Id);

            if (noteIndex == -1)
            {
                return;
            }

            this.Notes.RemoveAt(noteIndex);
            deserializeNotes?.RemoveAt(noteIndex);
            string? serializeNotes = JsonConvert.SerializeObject(deserializeNotes);
            await this.data.SaveAsync("notes", serializeNotes);
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Notes)));
        }

        public void UpdateNotesExist()
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.NotesExist)));
        }

        private void PopulateNotes()
        {
            string? notes = this.data.Retrieve("notes");
            if (notes == null)
            {
                return;
            }

            var deserializedNotes = JsonConvert.DeserializeObject<List<Note>>(notes);
            if (deserializedNotes != null)
            {
                this.Notes = deserializedNotes;
            }

            this.UpdateNotesExist();
        }
    }
}