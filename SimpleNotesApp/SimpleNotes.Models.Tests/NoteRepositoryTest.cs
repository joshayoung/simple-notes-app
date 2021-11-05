using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shared;
using Xunit;

namespace SimpleNotes.Models.Tests
{
    public class NoteRepositoryTest
    {
        private readonly IData mockIData;
        public NoteRepositoryTest()
        {
            this.mockIData = Substitute.For<IData>();
        }
        
        #region Constructor_Tests
        [Fact]
        public void Constructor_WithValidData_AssignsValues()
        {
            var noteData = "[{\"id\": 1, \"title\": \"Title1\", \"description\": \"Description1\"}]";
            var notes = new List<Note>() { new Note(1, "Title1", "Description1") };
            this.mockIData.Retrieve("notes").Returns(noteData);
            
            var notesRepository = new NoteRepository(this.mockIData);
            
            notesRepository.Notes.Should().BeEquivalentTo(notes);
            notesRepository.NotesExist.Should().BeTrue();
        }

        [Fact]
        public void Constructor_NoData_DoesNotAssignsValues()
        {
            this.mockIData.Retrieve("notes").ReturnsNull();
            
            var notesRepository = new NoteRepository(this.mockIData); 
            
            notesRepository.Notes.Should().BeEmpty();
            notesRepository.NotesExist.Should().BeFalse();
        }
        #endregion
        
        #region Save_Tests
        [Fact]
        public void Save_Called_AddsNoteToList()
        {
            var note = new Note(1);
            var notes = new List<Note> { note };
            var notesRepository = new NoteRepository(this.mockIData);

            notesRepository.SaveAsync(note);

            notesRepository.Notes.Should().BeEquivalentTo(notes);
        }

        [Fact]
        public void Save_Called_SavesDataToLocalStorage()
        {
            var note = new Note(1);
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            var notesRepository = new NoteRepository(this.mockIData);

            notesRepository.SaveAsync(note);

            this.mockIData.Received().SaveAsync(Arg.Is<string>("notes"), Arg.Is(serializedNotes));
        }
        
        [Fact]
        public void Save_Called_NotesPropertyChangeEvent()
        {
            var noteRepositoryMonitored = new NoteRepository(this.mockIData).Monitor();
            var note = new Note(1);

            noteRepositoryMonitored.Subject.SaveAsync(note);
        
            noteRepositoryMonitored.Should()
                           .Raise("PropertyChanged")
                           .WithSender(noteRepositoryMonitored.Subject)
                           .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == nameof(NoteRepository.Notes));
        }
        #endregion

        #region SaveEdits
        [Fact]
        public void SaveEdits_Called_SetsTitle()
        {
            var note = new Note(1);
            var title = "my title";
            var notesRepository = new NoteRepository(this.mockIData)
            {
                Notes = new List<Note>
                {
                    new Note(1, title)
                }
            };

            notesRepository.SaveAsync(note);

            notesRepository.Notes.First().Title.Should().Be(title);
        }
        
        [Fact]
        public void SaveEdits_Called_SetsDescription()
        {
            var note = new Note(1);
            var description = "my description";
            var notesRepository = new NoteRepository(this.mockIData)
            {
                Notes = new List<Note>
                {
                    new Note(1, "title", description)
                }
            };

            notesRepository.SaveAsync(note);

            notesRepository.Notes.First().Description.Should().Be(description);
        }

        [Fact]
        public void SaveEdits_Called_SavesNote()
        {
            var note = new Note(1);
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            var notesRepository = new NoteRepository(this.mockIData)
            {
                Notes = new List<Note> { note, }
            };

            notesRepository.SaveEditsAsync(note);

            this.mockIData.Received().SaveAsync(Arg.Is("notes"), Arg.Is(serializedNotes));
        }

        [Fact]
        public void SaveEdits_Called_NotesPropertyChangedEvent()
        {
            var note = new Note(1);
            var wasChanged = false;
            var notesRepository = new NoteRepository(this.mockIData)
            {
                Notes = new List<Note> { note, }
            };
            notesRepository.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(NoteRepository.Notes))
                {
                    wasChanged = true;
                }
            };

            notesRepository.SaveEditsAsync(note);
        
            wasChanged.Should().BeTrue();
        }
        #endregion

        #region Delete_Tests
        [Fact]
        public void Delete_NoteRetrievalReturnsNull_Returns()
        {
            var note = new Note(2);
            this.mockIData.Retrieve("notes").ReturnsNull();
            var notesRepository = new NoteRepository(this.mockIData);

            notesRepository.DeleteAsync(note);

            this.mockIData.DidNotReceive().SaveAsync(Arg.Any<string>(), Arg.Any<string>());
        }
        
        [Fact]
        public void Delete_DeserializationReturnsNull_Returns()
        {
            var note = new Note(2);
            this.mockIData.Retrieve("notes").Returns("");
            var notesRepository = new NoteRepository(this.mockIData);

            notesRepository.DeleteAsync(note);

            this.mockIData.DidNotReceive().SaveAsync(Arg.Any<string>(), Arg.Any<string>());
        }
        
        [Fact]
        public void Delete_NoteIndexNegativeOne_Returns()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            var note = new Note(2);
            this.mockIData.Retrieve("notes").Returns(serializedNotes);
            var notesRepository = new NoteRepository(this.mockIData);

            notesRepository.DeleteAsync(note);

            this.mockIData.DidNotReceive().SaveAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void Delete_CalledWithNote_NoteRemoveFromList()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            var note = new Note(1);
            this.mockIData.Retrieve("notes").Returns(serializedNotes);
            var notesRepository = new NoteRepository(this.mockIData);

            notesRepository.DeleteAsync(note);

            notesRepository.Notes.Should().BeEmpty();
        }

        [Fact]
        public void Delete_CalledWithNotes_SavesData()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null},{\"Id\":2,\"Title\":null,\"Description\":null}]";
            var serializedAfterDeletion = "[{\"Id\":2,\"Title\":null,\"Description\":null}]";
            var note = new Note(1);
            this.mockIData.Retrieve("notes").Returns(serializedNotes);
            var notesRepository = new NoteRepository(this.mockIData);

            notesRepository.DeleteAsync(note);

            this.mockIData.Received().SaveAsync(Arg.Is<string>("notes"), Arg.Is<string>(serializedAfterDeletion));
        }

        [Fact]
        public void Delete_CalledWithNotes_PropertyChangedEventForNotes()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            var note = new Note(1);
            var wasChanged = false;
            this.mockIData.Retrieve("notes").Returns(serializedNotes);
            var notesRepository = new NoteRepository(this.mockIData)
            {
                Notes = new List<Note> { note, }
            };
            notesRepository.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(NoteRepository.Notes))
                {
                    wasChanged = true;
                }
            };

            notesRepository.DeleteAsync(note);

            wasChanged.Should().BeTrue();
        }
        #endregion
        
        #region UpdateNotesExist
        [Fact]
        public void UpdateNotesExist_Called_PropertyChangedEventForNotesExist()
        {
            var wasChanged = false;
            var notesRepository = new NoteRepository(this.mockIData);
            notesRepository.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(NoteRepository.NotesExist))
                {
                    wasChanged = true;
                }
            };

            notesRepository.UpdateNotesExist();

            wasChanged.Should().BeTrue();
        }
        #endregion
    }
}