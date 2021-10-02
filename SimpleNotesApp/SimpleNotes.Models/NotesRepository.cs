using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Shared;

namespace SimpleNotes.Models
{
    public class NotesRepository
    {
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

        // TODO: Pass the necessary values in.
        // NOTE: Just a proof of concept at this point.
        public void Save(Note note)
        {
            this.Notes.Add(note);
            var serializeNotes = JsonConvert.SerializeObject(Notes);
            data.Save("notes", serializeNotes);
            var notes = data.Retrieve("notes");
            var deserializedNotes = JsonConvert.DeserializeObject<List<Note>>(notes);
        }
    }
}