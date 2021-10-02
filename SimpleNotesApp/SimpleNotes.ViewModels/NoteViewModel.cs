using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimpleNotes.Models;

namespace SimpleNotes.ViewModels
{
    public class NoteViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        private readonly Note note;
        
        public string Id
        {
            get => note.Id;
            set => note.Id = value;
        }

        public string Description
        {
            get => note.Description;
            set => note.Description = value;
        }

        public NoteViewModel(Note note)
        {
            this.note = note;

            note.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Note.Description))
                {
                    NotifyPropertyChanged(nameof(NoteViewModel.Description));
                }
            };
        }

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Save() => note.Save();
    }
}