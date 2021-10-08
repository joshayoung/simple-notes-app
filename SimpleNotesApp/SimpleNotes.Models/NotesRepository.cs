using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared;

namespace SimpleNotes.Models
{
    public class NotesRepository : INotifyPropertyChanged
    {
        public List<Note> Notes { get; set; } = new List<Note>();

        private readonly IData data;

        public NotesRepository(IData data)
        {
            this.data = data;
            this.PopulateNotes();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool NotesExist { get; private set; }

        public async Task Save(Note note)
        {
            this.Notes.Add(note);
            string? serializeNotes = JsonConvert.SerializeObject(this.Notes);
            await this.data.SaveAsync("notes", serializeNotes);
            this.NotifyPropertyChanged(nameof(this.Notes));
        }

        public async Task SaveEdits(Note note)
        {
            var nt = this.Notes.Find(n => n.Id == note.Id);
            nt.Title = note.Title;
            nt.Description = note.Description;
            string? serializeNotes = JsonConvert.SerializeObject(this.Notes);
            await this.data.SaveAsync("notes", serializeNotes);
            this.NotifyPropertyChanged(nameof(this.Notes));
        }

        public async Task Delete(Note note)
        {
            string? lsNotes = this.data.Retrieve("notes");
            var deserializeNotes = JsonConvert.DeserializeObject<List<Note>>(lsNotes);
            int? noteIndex = deserializeNotes?.FindIndex(n => n.Id == note.Id);
            if (noteIndex == null)
            {
                return;
            }

            this.Notes.RemoveAt(noteIndex.Value);
            deserializeNotes?.RemoveAt(noteIndex.Value);
            string? serializeNotes = JsonConvert.SerializeObject(deserializeNotes);
            await this.data.SaveAsync("notes", serializeNotes);
            this.NotifyPropertyChanged(nameof(this.Notes));
        }

        public void UpdateNotesExist()
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
            if (notes == string.Empty)
            {
                return;
            }

            var deserializedNotes = JsonConvert.DeserializeObject<List<Note>>(notes);
            if (deserializedNotes != null)
            {
                this.Notes = deserializedNotes;
            }
        }
    }
}