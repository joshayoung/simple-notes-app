using System.ComponentModel;
using FluentAssertions;
using Xunit;

namespace SimpleNotes.Models.Tests
{
    public class NoteTest
    {
        #region PropertyChanged_Tests
        [Fact (DisplayName = "Note's Title changes triggers a property change event for 'Title'")]
        public void Title_Changes_PropertyChangedEvent()
        {
            var noteMonitored = new Note(1).Monitor();

            noteMonitored.Subject.Title = "new title";

            noteMonitored.Should()
                         .Raise("PropertyChanged")
                         .WithSender(noteMonitored.Subject)
                         .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == nameof(Note.Title));
        }
        
        [Fact (DisplayName = "Note's Description changes triggers a property change event for 'Description'")]
        public void Description_Changed_PropertyChangedEvent()
        {
            var noteMonitored = new Note(1, "title", "description").Monitor();

            noteMonitored.Subject.Description = "new description";
            
            noteMonitored.Should()
                         .Raise("PropertyChanged")
                         .WithSender(noteMonitored.Subject)
                         .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == nameof(Note.Description));
        }
        #endregion
    }
}