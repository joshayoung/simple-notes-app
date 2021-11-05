using FluentAssertions;
using Xunit;

namespace SimpleNotes.Models.Tests
{
    public class NoteFormatterTest
    {
        [Fact]
        public void TrimWhitespace_Called_ReturnsCorrectValue()
        {
            var title = "   title   ";
            var description = "    description  ";
            var note = new Note(1, title, description);

            var result = note.TrimWhitespace();

            result.Title.Should().Be("title");
            result.Description.Should().Be("description");
        }
        
    }
}