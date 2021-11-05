using System.ComponentModel;
using FluentAssertions;
using Xunit;

namespace SimpleNotes.Models.Tests
{
    public class NoteTest
    {
        #region Constructor_Test
        [Fact]
        public void Constructor_WithAllParams_AssignsValues()
        {
            var id = 1;
            var title = "title";
            var description = "description";
                
            var note = new Note(id, title, description);
            
            note.Id.Should().Be(id);
            note.Title.Should().Be(title);
            note.Description.Should().Be(description);
        }
        
        [Fact]
        public void Constructor_WithOnlyRequiredParams_AssignsValues()
        {
            var note = new Note(1);
            
            note.Id.Should().Be(1);
            note.Title.Should().BeNull();
            note.Description.Should().BeNull();
        }
        #endregion

        #region PropertyChanged_Tests
        [Fact]
        public void Id_Changed_PropertyChangedEvent()
        {
            var noteMonitored = new Note(1, "title", "description").Monitor();

            noteMonitored.Subject.Id = 2;
            
            noteMonitored.Should()
                         .Raise("PropertyChanged")
                         .WithSender(noteMonitored.Subject)
                         .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == nameof(Note.Id));
        }
        
        [Fact]
        public void Title_Changes_PropertyChangedEvent()
        {
            var noteMonitored = new Note(1).Monitor();

            noteMonitored.Subject.Title = "new title";

            noteMonitored.Should()
                         .Raise("PropertyChanged")
                         .WithSender(noteMonitored.Subject)
                         .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == nameof(Note.Title));
        }
        
        [Fact]
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