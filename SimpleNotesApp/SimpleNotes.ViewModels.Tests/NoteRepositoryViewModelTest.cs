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
            this.mockNoteRepository = Substitute.ForPartsOf<NoteRepository>(mockIData);
        }
        
        #region Constructor_tests
        [Fact]
        public void Constructor_Params_SetsValues()
        {
            var note = new Note(1);
            var notes = new List<Note>() { note, };
            this.mockNoteRepository.Notes = new List<Note>(notes);
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.mockNoteRepository);
            var noteVMs = new List<NoteViewModel>()
            {
                new NoteViewModel(note, this.mockNoteRepository),
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
            var note = new Note(1);
            var note2 = new Note(3);
            var notes = new List<Note>() { note, };
            this.mockNoteRepository.Notes = new List<Note>(notes);
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.mockNoteRepository);
            var noteVMs = new List<NoteViewModel>()
            {
                new NoteViewModel(note, this.mockNoteRepository),
                new NoteViewModel(note2, this.mockNoteRepository),
            };
            
            this.mockNoteRepository.SaveAsync(note2);

            noteRepositoryViewModel.Notes.Should().BeEquivalentTo(noteVMs);
        }
        
        [Fact]
        public void RepositoryNotes_Changes_CallsCorrectMethod()
        {
            var notes = new List<Note>() { new Note(1), };
            this.mockNoteRepository.Notes = new List<Note>(notes);
            new NoteRepositoryViewModel(this.mockNoteRepository);
            
            this.mockNoteRepository.SaveAsync(new Note(2));

            this.mockNoteRepository.Received().UpdateNotesExist();
        }
        
        [Fact]
        public void RepositoryNotes_Change_PropertyChangeForNotes()
        {
            var note = new Note(1);
            var notes = new List<Note> { note, };
            this.mockNoteRepository.Notes = new List<Note>(notes);
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.mockNoteRepository).Monitor();
            this.mockNoteRepository.Configure().PropertyChanged += Raise.Event<PropertyChangedEventHandler>(this, new PropertyChangedEventArgs(nameof(NoteRepository.Notes)));

            this.mockNoteRepository.Notes.Add(new Note(2));
            
            noteRepositoryViewModel.Should()
                                   .Raise("PropertyChanged")
                                   .WithSender(noteRepositoryViewModel.Subject)
                                   .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == nameof(NoteRepositoryViewModel.Notes));
        }
        
        [Fact]
        public void RepositoryNotesExist_Changes_PropertyChangeForNotesExist()
        {
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.mockNoteRepository).Monitor();
            this.mockNoteRepository.Configure().PropertyChanged += Raise.Event<PropertyChangedEventHandler>(this, new PropertyChangedEventArgs(nameof(NoteRepository.NotesExist)));
            
            noteRepositoryViewModel.Should()
                                   .Raise("PropertyChanged")
                                   .WithSender(noteRepositoryViewModel.Subject)
                                   .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == nameof(NoteRepositoryViewModel.NotesExist));
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