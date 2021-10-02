using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Shared;

namespace SimpleNotes.Models
{
    public class NotesRepository : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        public List<Note> Notes = new List<Note>();
        
        private readonly IData data;

        public NotesRepository(IData data)
        {
            this.data = data;
            PopulateNotes();
        }

        private void PopulateNotes()
        {
            var notes = data.Retrieve("notes");
            var deserializedNotes = JsonConvert.DeserializeObject<List<Note>>(notes);
            this.Notes = deserializedNotes;
        }

        public void Save(Note note)
        {
            this.Notes.Add(note);
            var serializeNotes = JsonConvert.SerializeObject(Notes);
            data.Save("notes", serializeNotes);
            NotifyPropertyChanged(nameof(Notes));
        }
        
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}