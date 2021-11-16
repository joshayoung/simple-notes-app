using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared;

namespace SimpleNotes.Models
{
    public class NoteDataService
    {
        private const string LocalStorageString = "notes";

        private readonly IData data;

        public NoteDataService(IData data)
        {
            this.data = data;
        }

        public virtual List<Note> RetrieveNotes()
        {
            string? notes = this.data.RetrieveValue(LocalStorageString);

            if (notes == null)
            {
                return new List<Note>();
            }

            List<Note>? deserializedNotes;
            try
            {
                deserializedNotes = JsonConvert.DeserializeObject<List<Note>>(notes);
            }
            catch (Exception e)
            {
                // TODO: Add log here
                throw;
            }

            return deserializedNotes ?? new List<Note>();
        }

        public virtual async Task SaveNotesAsync(List<Note> notes)
        {
            string serializeNotes = JsonConvert.SerializeObject(notes);
            try
            {
                await this.data.SaveValueAsync(LocalStorageString, serializeNotes);
            }
            catch (Exception e)
            {
                // TODO: Improve error message
                // TODO: Log the error here too?
                throw new Exception(ErrorMessages.NotesSaveError, e);
            }
        }

        // TODO: Consider using the SaveAsync method and passing a list with the note removed instead of this method
        public virtual async Task DeleteNotesAsync(List<Note> notes, Note note)
        {
            string? lsNotes = this.data.RetrieveValue(LocalStorageString);

            if (lsNotes == null)
            {
                return;
            }

            // TODO: Wrap in try-catch and add test:
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

            notes.RemoveAt(noteIndex);
            deserializeNotes?.RemoveAt(noteIndex);
            string serializeNotes = JsonConvert.SerializeObject(deserializeNotes);
            await this.data.SaveValueAsync(LocalStorageString, serializeNotes);
        }
    }
}