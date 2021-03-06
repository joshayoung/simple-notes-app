using System.Collections.Generic;
using System.ComponentModel;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Extensions;
using Shared;
using SimpleNotes.Models;
using Xunit;

namespace SimpleNotes.ViewModels.Tests
{
    public class NoteRepositoryViewModelTest
    {
        private readonly NoteRepository mockNoteRepository;
        
        public NoteRepositoryViewModelTest()
        {
            var mockIData = Substitute.For<IData>();
            var noteDataService = Substitute.ForPartsOf<NoteDataService>(mockIData);
            this.mockNoteRepository = Substitute.ForPartsOf<NoteRepository>(noteDataService);
        }
        
        #region Constructor_tests
        [Fact]
        public void Constructor_SetsNotes()
        {
            var note = new Note(1);
            var notes = new List<Note>() { note, };
            this.mockNoteRepository.Notes = new List<Note>(notes);
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.mockNoteRepository);
            var noteViewModelList = new List<NoteViewModel>()
            {
                new NoteViewModel(note, this.mockNoteRepository),
            };

            noteRepositoryViewModel.Notes.Should().BeEquivalentTo(noteViewModelList);
        }
        
        [Fact]
        public void Constructor_CallsCorrectMethod()
        {
            var note = new Note(1);
            var notes = new List<Note>() { note, };
            this.mockNoteRepository.Notes = new List<Note>(notes);
            
            new NoteRepositoryViewModel(this.mockNoteRepository);

            this.mockNoteRepository.Received().UpdateNotesExist();
        }
        #endregion

        #region Property_Change_Tests
        [Theory]
        [InlineData("NoteSaved")]
        [InlineData("NoteEdited")]
        [InlineData("NoteDeleted")]
        public void RepositoryEvent_Emitted_UpdatedNotes(string eventName)
        {
            var note = new Note(1);
            var note2 = new Note(3);
            var notes = new List<Note>() { note, };
            this.mockNoteRepository.Notes = new List<Note>(notes);
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.mockNoteRepository).Monitor();
            var noteVMs = new List<NoteViewModel>()
            {
                new NoteViewModel(note, this.mockNoteRepository),
                new NoteViewModel(note2, this.mockNoteRepository),
            };
            this.mockNoteRepository.Notes.Returns(new List<Note>() { note, note2, });
            
            this.mockNoteRepository.Configure().PropertyChanged += 
                Raise.Event<PropertyChangedEventHandler>(this, new PropertyChangedEventArgs(eventName));

            noteRepositoryViewModel.Subject.Notes.Should().BeEquivalentTo(noteVMs);
        }
        
        [Fact]
        public void RepositoryNotes_Changes_CallsCorrectMethod()
        {
            var note = new Note(1);
            var notes = new List<Note>() { note, };
            this.mockNoteRepository.Notes = new List<Note>(notes);
            new NoteRepositoryViewModel(this.mockNoteRepository);
            
            this.mockNoteRepository.Configure().PropertyChanged += Raise.Event<PropertyChangedEventHandler>(this, new PropertyChangedEventArgs(nameof(NoteRepository.Notes)));

            this.mockNoteRepository.Received().UpdateNotesExist();
        }
        
        [Theory]
        [InlineData("NoteSaved")]
        [InlineData("NoteEdited")]
        [InlineData("NoteDeleted")]
        public void RepositoryEvents_EventsRaised_PropertyChangeForNotes(string eventName)
        {
            var note = new Note(1);
            var notes = new List<Note> { note, };
            this.mockNoteRepository.Notes = new List<Note>(notes);
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.mockNoteRepository).Monitor();
            this.mockNoteRepository.Configure().PropertyChanged += Raise.Event<PropertyChangedEventHandler>(this, new PropertyChangedEventArgs(eventName));

            this.mockNoteRepository.Notes.Add(new Note(2));
            
            noteRepositoryViewModel.Should().RaisePropertyChangeFor(n => n.Notes);
        }
        
        [Fact]
        public void RepositoryNotesExist_Changes_PropertyChangeForNotesExist()
        {
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.mockNoteRepository).Monitor();
            this.mockNoteRepository.Configure().PropertyChanged += Raise.Event<PropertyChangedEventHandler>(this, new PropertyChangedEventArgs(nameof(NoteRepository.NotesExist)));
            
            noteRepositoryViewModel.Should().RaisePropertyChangeFor(n => n.NotesExist);
        }
        #endregion
        
        #region GetInitialNote_Tests
        [Fact]
        public void GetInitialNote_Called_ReturnsNoteVMWithIncrementedNoteId()
        {
            this.mockNoteRepository.Notes.Returns(new List<Note> { new Note(1) });
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.mockNoteRepository);
            var noteRepositoryVM = new NoteViewModel(new Note(2), this.mockNoteRepository);

            var result = noteRepositoryViewModel.GetInitialNote();

            result.Should().BeEquivalentTo(noteRepositoryVM);
        }
        #endregion
    }
}