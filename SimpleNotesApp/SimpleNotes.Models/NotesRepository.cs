using System;
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
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public List<Note> Notes = new List<Note>();

        public bool NotesExist { get; private set; }

        private readonly IData data;

        public NotesRepository(IData data)
        {
            this.data = data;
            PopulateNotes();
        }

        private void PopulateNotes()
        {
            var notes = data.Retrieve("notes");
            if (notes == String.Empty) return;
            
            var deserializedNotes = JsonConvert.DeserializeObject<List<Note>>(notes);
            if (deserializedNotes != null)
            {
                this.Notes = deserializedNotes;
            }
        }

        public async Task Save(Note note)
        {
            this.Notes.Add(note);
            var serializeNotes = JsonConvert.SerializeObject(Notes);
            await data.Save("notes", serializeNotes);
            NotifyPropertyChanged(nameof(Notes));
        }
        
        public async Task Delete(Note note)
        {
            var lsNotes = data.Retrieve("notes");
            var deserializeNotes = JsonConvert.DeserializeObject<List<Note>>(lsNotes);
            var noteIndex = deserializeNotes?.FindIndex(n => n.Id == note.Id);
            if (noteIndex == null) return;
            
            this.Notes.RemoveAt(noteIndex.Value);
            deserializeNotes?.RemoveAt(noteIndex.Value);
            var serializeNotes = JsonConvert.SerializeObject(deserializeNotes);
            await data.Save("notes", serializeNotes);
            NotifyPropertyChanged(nameof(Notes));
        }
        
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateNotesExist()
        {
            this.NotesExist = Notes.Count > 0;
            NotifyPropertyChanged(nameof(NotesExist));
        }
    }
}