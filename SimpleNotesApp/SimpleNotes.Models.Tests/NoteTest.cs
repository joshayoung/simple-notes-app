using FluentAssertions;
using Xunit;

namespace SimpleNotes.Models.Tests
{
    public class NoteTest
    {
        [Fact]
        public void Title_Changes_PropertyChangedEvent()
        {
            var noteMonitored = new Note(1).Monitor();

            noteMonitored.Subject.Title = "new title";

            noteMonitored.Should().RaisePropertyChangeFor(n => n.Title);
        }
        
        [Fact]
        public void Description_Changed_PropertyChangedEvent()
        {
            var noteMonitored = new Note(1, "title", "description").Monitor();

            noteMonitored.Subject.Description = "new description";
            
            noteMonitored.Should().RaisePropertyChangeFor(n => n.Description);
        }
    }
}