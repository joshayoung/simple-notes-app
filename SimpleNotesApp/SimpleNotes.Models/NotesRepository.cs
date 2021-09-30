using System;
using System.Collections.Generic;
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
        }

        // TODO: Pass the necessary values in.
        // NOTE: Just a proof of concept at this point.
        public void Save()
        {
            data.Save("test", "val");
        }
    }
}