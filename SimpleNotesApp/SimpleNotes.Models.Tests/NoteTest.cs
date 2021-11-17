using System.ComponentModel;
using FluentAssertions;
using FluentAssertions.Events;
using Xunit;

namespace SimpleNotes.Models.Tests
{
    namespace NoteTest.ConstructorTests
    {
        public class NoteTest
        {
            [Fact(DisplayName = "Constructor accepts all params and sets the values")]
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

            [Fact(DisplayName = "Constructor accepts only required params and sets the values")]
            public void Constructor_WithOnlyRequiredParams_CorrectValues()
            {
                var note = new Note(1);

                note.Id.Should().Be(1);
                note.Title.Should().BeNull();
                note.Description.Should().BeNull();
            }
        }
    }

    namespace NoteTest.PropertyChangedTests
    {
        public class NoteTest
        {
            [Fact(DisplayName = "Note's Title changes triggers a property change event for 'Title'")]
            public void Title_Changes_PropertyChangedEvent()
            {
                IMonitor<Note> noteMonitored = new Note(1).Monitor();

                noteMonitored.Subject.Title = "new title";

                noteMonitored.Should()
                             .Raise("PropertyChanged")
                             .WithSender(noteMonitored.Subject)
                             .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == nameof(Note.Title));
            }

            [Fact(DisplayName = "Note's Description changes triggers a property change event for 'Description'")]
            public void Description_Changed_PropertyChangedEvent()
            {
                IMonitor<Note> noteMonitored = new Note(1, "title", "description").Monitor();

                noteMonitored.Subject.Description = "new description";

                noteMonitored.Should()
                             .Raise("PropertyChanged")
                             .WithSender(noteMonitored.Subject)
                             .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == nameof(Note.Description));
            }
        }
    }
}