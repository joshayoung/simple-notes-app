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

        public virtual List<Note> GetNotes()
        {
            string? notes = this.data.Retrieve(LocalStorageString);

            if (notes == null)
            {
                return new List<Note>();
            }

            // TODO: Wrap in try-catch
            List<Note>? deserializedNotes = JsonConvert.DeserializeObject<List<Note>>(notes);

            // TODO: Do something different here maybe
            return deserializedNotes ?? new List<Note>();
        }

        public virtual async Task SaveAsync(List<Note> notes)
        {
            // TODO: wrap in try-catch
            string serializeNotes = JsonConvert.SerializeObject(notes);
            await this.data.SaveAsync(LocalStorageString, serializeNotes);
        }

        public virtual async Task DeleteAsync(List<Note> notes, Note note)
        {
            string? lsNotes = this.data.Retrieve(LocalStorageString);

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

            notes.RemoveAt(noteIndex);
            deserializeNotes?.RemoveAt(noteIndex);

            // TODO: Wrap in try-catch:
            string serializeNotes = JsonConvert.SerializeObject(deserializeNotes);
            await this.data.SaveAsync(LocalStorageString, serializeNotes);
        }
    }
}