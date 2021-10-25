using Xunit;

namespace SimpleNotes.Models.Tests
{
    public class NotesTest
    {
        #region Constructor_Test
        [Fact]
        public void Constructor_WithAllParams_AssignsValues()
        {
            var id = 1;
            var title = "title";
            var description = "description";
                
            var note = new Note(id, title, description);
            
            Assert.Equal(id, note.Id);
            Assert.Equal(title, note.Title);
            Assert.Equal(description, note.Description);
        }
        
        [Fact]
        public void Constructor_WithOnlyRequiredParams_AssignsValues()
        {
            var id = 1;
                
            var note = new Note(id);
            
            Assert.Equal(id, note.Id);
            Assert.Null(note.Title);
            Assert.Null(note.Description);
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
            
            Assert.True(wasChanged);
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
            
            Assert.True(wasChanged);
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
            
            Assert.True(wasChanged);
        }
        #endregion
    }
}