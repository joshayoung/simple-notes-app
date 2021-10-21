using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared;

namespace SimpleNotes.Models
{
    public class NoteRepository : INotifyPropertyChanged
    {
        private readonly IData data;
        public List<Note> Notes = new List<Note>();

        public NoteRepository(IData data)
        {
            this.data = data;
            this.PopulateNotes();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool NotesExist { get; private set; }

        public virtual async Task Save(Note note)
        {
            this.Notes.Add(note);
            string? serializeNotes = JsonConvert.SerializeObject(this.Notes);
            await this.data.SaveAsync("notes", serializeNotes);
            this.NotifyPropertyChanged(nameof(this.Notes));
        }

        public virtual async Task SaveEdits(Note note)
        {
            var nt = this.Notes.Find(n => n.Id == note.Id);
            nt.Title = note.Title;
            nt.Description = note.Description;
            string? serializeNotes = JsonConvert.SerializeObject(this.Notes);
            await this.data.SaveAsync("notes", serializeNotes);
            this.NotifyPropertyChanged(nameof(this.Notes));
        }

        public virtual async Task Delete(Note note)
        {
            string? lsNotes = this.data.Retrieve("notes");

            // TODO: Check for null and add a test
            var deserializeNotes = JsonConvert.DeserializeObject<List<Note>>(lsNotes);

            int noteIndex = deserializeNotes.FindIndex(n => n.Id == note.Id);
            if (noteIndex == -1)
            {
                return;
            }

            this.Notes.RemoveAt(noteIndex);
            deserializeNotes?.RemoveAt(noteIndex);
            string? serializeNotes = JsonConvert.SerializeObject(deserializeNotes);
            await this.data.SaveAsync("notes", serializeNotes);
            this.NotifyPropertyChanged(nameof(this.Notes));
        }

        private void UpdateNotesExist()
        {
            this.NotesExist = this.Notes.Count > 0;
            this.NotifyPropertyChanged(nameof(this.NotesExist));
        }

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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