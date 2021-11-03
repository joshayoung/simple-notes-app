using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
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
            this.mockNoteRepository = Substitute.ForPartsOf<NoteRepository>(mockIData);
        }
        
        #region Constructor_tests
        [Fact]
        public void Constructor_Params_SetsValues()
        {
            var note1 = new Note(1);
            var notes = new List<Note>() { note1, };
            this.mockNoteRepository.Notes = new List<Note>(notes);
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.mockNoteRepository);
            var noteVMs = new List<NoteViewModel>()
            {
                new NoteViewModel(note1, this.mockNoteRepository),
            };

            noteRepositoryViewModel.Notes.Should().BeEquivalentTo(noteVMs);
        }
        
        [Fact]
        public void Constructor_Params_CallsCorrectMethod()
        {
            var note = new Note(1);
            var notes = new List<Note>() { note, };
            this.mockNoteRepository.Notes = new List<Note>(notes);
            
            new NoteRepositoryViewModel(this.mockNoteRepository);

            this.mockNoteRepository.Received().UpdateNotesExist();
        }
        #endregion

        #region Property_Change_Tests
        [Fact]
        public void RepositoryNotes_Change_UpdatedNotes()
        {
            var note1 = new Note(1);
            var note3 = new Note(3);
            var notes = new List<Note>()
            {
                note1,
            };
            this.mockNoteRepository.Notes = new List<Note>(notes);
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.mockNoteRepository);
            var noteVMs = new List<NoteViewModel>()
            {
                new NoteViewModel(note1, this.mockNoteRepository),
                new NoteViewModel(note3, this.mockNoteRepository),
            };
            
            this.mockNoteRepository.Save(note3);

            noteRepositoryViewModel.Notes.Should().BeEquivalentTo(noteVMs);
        }
        
        [Fact]
        public void RepositoryNotes_Changes_CallsCorrectMethod()
        {
            var note = new Note(1);
            var note2 = new Note(2);
            var notes = new List<Note>() { note, };
            this.mockNoteRepository.Notes = new List<Note>(notes);
            new NoteRepositoryViewModel(this.mockNoteRepository);
            
            this.mockNoteRepository.Save(note2);

            this.mockNoteRepository.Received().UpdateNotesExist();
        }
        
        [Fact]
        public void RepositoryNotes_Change_PropertyChangeForNotes()
        {
            var note1 = new Note(1);
            var note2 = new Note(2);
            var notes = new List<Note>()
            {
                note1,
                note2,
            };
            this.mockNoteRepository.Notes = new List<Note>(notes);
            var wasChanged = false;
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.mockNoteRepository);
            noteRepositoryViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(NoteRepository.Notes))
                {
                    wasChanged = true;
                }
            };

            this.mockNoteRepository.Save(new Note(2));
            
            wasChanged.Should().BeTrue();
        }
        
        [Fact]
        public void RepositoryNotesExist_Changes_PropertyChangeForNotesExist()
        {
            var note = new Note(1);
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.mockNoteRepository);
            var wasChanged = false;
            noteRepositoryViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(NoteRepository.NotesExist))
                {
                    wasChanged = true;
                }
            };
            
            this.mockNoteRepository.Save(note);

            wasChanged.Should().BeTrue();
        }
        #endregion
        
        #region GetInitialNote_Tests
        [Fact]
        public void GetInitialNote_Called_ReturnsNoteVMWithIncrementedNoteId()
        {
            var note = new Note(1);
            this.mockNoteRepository.Notes.Returns(new List<Note> { note });
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.mockNoteRepository);
            var noteRepositoryVM = new NoteViewModel(new Note(2), this.mockNoteRepository);

            var result = noteRepositoryViewModel.GetInitialNote();

            result.Should().BeEquivalentTo(noteRepositoryVM);
        }
        #endregion
    }
}