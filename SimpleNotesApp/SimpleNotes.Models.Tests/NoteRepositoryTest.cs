using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Extensions;
using Shared;
using Xunit;

namespace SimpleNotes.Models.Tests
{
    public class NoteRepositoryTest
    {
        private readonly NoteDataService mockNoteDataService;
        
        public NoteRepositoryTest()
        {
            this.mockNoteDataService = Substitute.ForPartsOf<NoteDataService>(Substitute.For<IData>());
        }
        
        #region Constructor_Tests
        [Fact]
        public void Constructor_WithValidData_AssignsValues()
        {
            var notes = new List<Note>() { new Note(1, "title", "description") };
            this.mockNoteDataService.Configure().RetrieveNotes().Returns(notes);
            
            var notesRepository = new NoteRepository(this.mockNoteDataService);
            
            notesRepository.Notes.Should().BeEquivalentTo(notes);
            notesRepository.NotesExist.Should().BeTrue();
        }

        [Fact]
        public void Constructor_NoData_DoesNotAssignsValues()
        {
            this.mockNoteDataService.Configure().RetrieveNotes().Returns(new List<Note>());
            
            var notesRepository = new NoteRepository(this.mockNoteDataService);
            
            notesRepository.Notes.Should().BeEmpty();
            notesRepository.NotesExist.Should().BeFalse();
        }
        #endregion
        
        #region SaveAsync_Tests
        [Fact]
        public void SaveAsync_Called_AddsNoteToList()
        {
            var note = new Note(1);
            var notes = new List<Note> { note };
            var notesRepository = new NoteRepository(this.mockNoteDataService);

            notesRepository.SaveAsync(note);

            notesRepository.Notes.Should().BeEquivalentTo(notes);
        }

        [Fact]
        public void SaveAsync_Called_CallsDataService()
        {
            var noteRepository = new NoteRepository(this.mockNoteDataService);
            var note = new Note(1);

            noteRepository.SaveAsync(note);

            this.mockNoteDataService.Configure().Received().SaveNotesAsync(noteRepository.Notes);
        }

        [Fact]
        public void SaveAsync_Called_NotesPropertyChangeEvent()
        {
            var noteRepositoryMonitored = new NoteRepository(this.mockNoteDataService).Monitor();
            var note = new Note(1);

            noteRepositoryMonitored.Subject.SaveAsync(note);
        
            noteRepositoryMonitored.Should()
                           .Raise("PropertyChanged")
                           .WithSender(noteRepositoryMonitored.Subject)
                           .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == "NoteSaved");
        }
        #endregion

        #region SaveEditsAsync
        [Fact]
        public void SaveEditsAsync_Called_SetsTitle()
        {
            var title = "my title";
            var note = new Note(1, title);
            var notesRepository = new NoteRepository(this.mockNoteDataService)
            {
                Notes = new List<Note> { new Note(1) }
            };

            notesRepository.SaveEditsAsync(note);

            notesRepository.Notes.First().Title.Should().Be(title);
        }
        
        [Fact]
        public void SaveEditsAsync_Called_SetsDescription()
        {
            var description = "my description";
            var note = new Note(1, "title", description);
            var notesRepository = new NoteRepository(this.mockNoteDataService)
            {
                Notes = new List<Note> { new Note(1) }
            };

            notesRepository.SaveEditsAsync(note);

            notesRepository.Notes.First().Description.Should().Be(description);
        }

        [Fact]
        public void SaveEditsAsync_Called_SavesNote()
        {
            var note = new Note(1);
            var notes = new List<Note> { note, };
            var notesRepository = new NoteRepository(this.mockNoteDataService)
            {
                Notes = notes
            };

            notesRepository.SaveEditsAsync(note);

            this.mockNoteDataService.Received().SaveNotesAsync(Arg.Is(notes));
        }

        [Fact]
        public void SaveEditsAsync_Called_NotesPropertyChangedEvent()
        {
            var note = new Note(1);
            var noteRepositoryMonitored = new NoteRepository(this.mockNoteDataService)
            {
                Notes = new List<Note> { note, }
            }.Monitor();

            noteRepositoryMonitored.Subject.SaveEditsAsync(note);
        
            noteRepositoryMonitored.Should()
                                   .Raise("PropertyChanged")
                                   .WithSender(noteRepositoryMonitored.Subject)
                                   .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == "NoteEdited");
        }
        #endregion

        #region DeleteAsync_Tests
        [Fact]
        public void DeleteAsync_CalledWithNotes_CallsDeleteMethod()
        {
            var note = new Note(1);
            var notesRepository = new NoteRepository(this.mockNoteDataService);

            notesRepository.DeleteAsync(note);

            this.mockNoteDataService.Configure().Received().DeleteNotesAsync(Arg.Is(notesRepository.Notes), Arg.Is(note));
        }

        [Fact]
        public void DeleteAsync_CalledWithNotes_PropertyChangedEventForNotes()
        {
            var note = new Note(1);
            var noteRepositoryMonitored = new NoteRepository(this.mockNoteDataService)
            {
                Notes = new List<Note> { note, }
            }.Monitor();

            noteRepositoryMonitored.Subject.DeleteAsync(note);

            noteRepositoryMonitored.Should()
                                   .Raise("PropertyChanged")
                                   .WithSender(noteRepositoryMonitored.Subject)
                                   .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == "NoteDeleted");
        }
        #endregion
        
        [Fact]
        public void UpdateNotesExist_Called_PropertyChangedEventForNotesExist()
        {
            var noteRepositoryMonitored = new NoteRepository(this.mockNoteDataService).Monitor();

            noteRepositoryMonitored.Subject.UpdateNotesExist();

            noteRepositoryMonitored.Should()
                                   .Raise("PropertyChanged")
                                   .WithSender(noteRepositoryMonitored.Subject)
                                   .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == nameof(NoteRepository.NotesExist));
        }
    }
}