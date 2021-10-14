using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shared;
using Xunit;

namespace SimpleNotes.Models.Tests
{
    public class NotesRepositoryTest
    {
        #region Constructor_Tests
        [Fact]
        public void Constructor_WithValidData_AssignsValues()
        {
            var mockIData = Substitute.For<IData>();
            var noteData = "[{\"id\": 1, \"title\": \"Title1\", \"description\": \"Description1\"}]";
            var notes = new List<Note>() { new Note(1, "Title1", "Description1") };
            mockIData.Retrieve("notes").Returns(noteData);
            
            var notesRepository = new NotesRepository(mockIData);
            
            notesRepository.Notes.Should().BeEquivalentTo(notes);
            notesRepository.NotesExist.Should().BeTrue();
        }

        [Fact]
        public void Constructor_NoData_DoesNotAssignsValues()
        {
            var mockIData = Substitute.For<IData>();
            mockIData.Retrieve("notes").ReturnsNull();
            
            var notesRepository = new NotesRepository(mockIData);

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
            var mockIData = Substitute.For<IData>();
            
            var notesRepository = new NotesRepository(mockIData);

            notesRepository.Save(note);

            notesRepository.Notes.Should().BeEquivalentTo(notes);
        }

        [Fact]
        public void Save_Called_SavesDataToLocalStorage()
        {
            var note = new Note(1);
            var mockIData = Substitute.For<IData>();
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            var notesRepository = new NotesRepository(mockIData);

            notesRepository.Save(note);

            mockIData.Received().SaveAsync(Arg.Is<string>("notes"), Arg.Is(serializedNotes));
        }
        
        [Fact]
        public void Save_Called_NotesPropertyChangeEvent()
        {
            var mockIData = Substitute.For<IData>();
            var noteRepository = new NotesRepository(mockIData);
            var note = new Note(1);
            var wasChanged = false;
            noteRepository.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(NotesRepository.Notes))
                {
                    wasChanged = true;
                }
            };

            noteRepository.Save(note);
        
            wasChanged.Should().BeTrue();
        }
        #endregion

        #region SaveEdits
        [Fact]
        public void SaveEdits_Called_SetsTitle()
        {
            var note = new Note(1);
            var title = "my title";
            var mockIData = Substitute.For<IData>();
            var notesRepository = new NotesRepository(mockIData)
            {
                Notes = new List<Note>
                {
                    new Note(1, title: title)
                }
            };

            notesRepository.Save(note);

            notesRepository.Notes.First().Title.Should().Be(title);
        }
        
        [Fact]
        public void SaveEdits_Called_SetsDescription()
        {
            var note = new Note(1);
            var description = "my description";
            var mockIData = Substitute.For<IData>();
            var notesRepository = new NotesRepository(mockIData)
            {
                Notes = new List<Note>
                {
                    new Note(1, "title", description: description)
                }
            };

            notesRepository.Save(note);

            notesRepository.Notes.First().Description.Should().Be(description);
        }

        [Fact]
        public void SaveEdits_Called_SavesNote()
        {
            var note = new Note(1);
            var mockIData = Substitute.For<IData>();
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            var notesRepository = new NotesRepository(mockIData)
            {
                Notes = new List<Note>
                {
                    note
                }
            };

            notesRepository.SaveEdits(note);

            mockIData.Received().SaveAsync(Arg.Is("notes"), Arg.Is(serializedNotes));
        }

        [Fact]
        public void SaveEdits_Called_NotesPropertyChangedEvent()
        {
            var mockIData = Substitute.For<IData>();
            var note = new Note(1);
            var wasChanged = false;
            var notesRepository = new NotesRepository(mockIData)
            {
                Notes = new List<Note>
                {
                    note
                }
            };
            notesRepository.PropertyChanged += (sender, args) =>
            {
                wasChanged = true;
            };

            notesRepository.SaveEdits(note);
        
            wasChanged.Should().BeTrue();
        }
        #endregion

        #region Delete_Tests
        [Fact]
        public void Delete_NoteIndexNegativeOne_Returns()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            var note = new Note(2);
            var mockIData = Substitute.For<IData>();
            mockIData.Retrieve("notes").Returns(serializedNotes);
            var notesRepository = new NotesRepository(mockIData);

            notesRepository.Delete(note);

            mockIData.DidNotReceive().SaveAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void Delete_CalledWithNote_NoteRemoveFromList()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            var note = new Note(1);
            var mockIData = Substitute.For<IData>();
            mockIData.Retrieve("notes").Returns(serializedNotes);
            var notesRepository = new NotesRepository(mockIData);

            notesRepository.Delete(note);

            notesRepository.Notes.Should().BeEmpty();
        }

        [Fact]
        public void Delete_CalledWithNotes_SavesData()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null},{\"Id\":2,\"Title\":null,\"Description\":null}]";
            var serializedAfterDeletion = "[{\"Id\":2,\"Title\":null,\"Description\":null}]";
            var note = new Note(1);
            var mockIData = Substitute.For<IData>();
            mockIData.Retrieve("notes").Returns(serializedNotes);
            var notesRepository = new NotesRepository(mockIData);

            notesRepository.Delete(note);

            mockIData.Received().SaveAsync(Arg.Is<string>("notes"), Arg.Is<string>(serializedAfterDeletion));
        }

        [Fact]
        public void Delete_CalledWithNotes_PropertyChangedEventForNotes()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            var note = new Note(1);
            var wasChanged = false;
            var mockIData = Substitute.For<IData>();
            mockIData.Retrieve("notes").Returns(serializedNotes);
            var notesRepository = new NotesRepository(mockIData)
            {
                Notes = new List<Note>
                {
                    note
                }
            };
            notesRepository.PropertyChanged += (sender, args) =>
            {
                wasChanged = true;
            };

            notesRepository.Delete(note);

            wasChanged.Should().BeTrue();
        }
        #endregion
    }
}