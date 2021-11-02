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
            var id = 1;
                
            var note = new Note(id);
            
            note.Id.Should().Be(id);
            note.Title.Should().BeNull();
            note.Description.Should().BeNull();
        }
        #endregion

        #region PropertyChanged_Tests
        [Fact]
        public void Id_Changed_PropertyChangedEvent()
        {
            var wasChanged = false;
            var note = new Note(1, "title", "description");
            note.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Note.Id))
                {
                    wasChanged = true;
                }
            };

            note.Id = 2;
            
            wasChanged.Should().BeTrue();
        }
        
        [Fact]
        public void Title_Changed_PropertyChangedEvent()
        {
            var wasChanged = false;
            var note = new Note(1, "title", "description");
            note.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Note.Title))
                {
                    wasChanged = true;
                }
            };

            note.Title = "new title";
            
            wasChanged.Should().BeTrue();
        }
        
        [Fact]
        public void Description_Changed_PropertyChangedEvent()
        {
            var wasChanged = false;
            var note = new Note(1, "title", "description");
            note.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Note.Description))
                {
                    wasChanged = true;
                }
            };

            note.Description = "new description";
            
            wasChanged.Should().BeTrue();
        }
        #endregion
    }
}