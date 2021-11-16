using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SimpleNotes.Models
{
    public class NoteRepository : INotifyPropertyChanged
    {
        private readonly NoteDataService noteDataService;

        public NoteRepository(NoteDataService noteDataService)
        {
            this.noteDataService = noteDataService;
            this.Notes = this.noteDataService.RetrieveNotes();
        }

        public virtual event PropertyChangedEventHandler? PropertyChanged;

        public virtual List<Note> Notes { get; set; }

        public virtual bool NotesExist => this.Notes.Count > 0;

        public virtual async Task SaveAsync(Note note)
        {
            this.Notes.Add(note.TrimWhitespace());
            await this.noteDataService.SaveNotesAsync(this.Notes);
            // TODO: Since I am not setting the Notes property, I would be better off using a different string here, like "Add" or "SyncAsync"
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Notes)));
        }

        public virtual async Task SaveEditsAsync(Note note)
        {
            var nt = this.Notes.Find(n => n.Id == note.Id);
            nt.Title = note.TrimWhitespace().Title;
            nt.Description = note.TrimWhitespace().Description;
            await this.noteDataService.SaveNotesAsync(this.Notes);
            // TODO: Since I am not setting the Notes property, I would be better off using a different string here, like "Edit" or "SaveEditsAsync"
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Notes)));
        }

        public virtual async Task DeleteAsync(Note note)
        {
            await this.noteDataService.DeleteNotesAsync(this.Notes, note);
            // TODO: Since I am not setting the Notes property, I would be better off using a different string here, like "Delete" or "DeleteAsync"
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Notes)));
        }

        public virtual void UpdateNotesExist()
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.NotesExist)));
        }
    }
}