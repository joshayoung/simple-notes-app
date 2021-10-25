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
        private readonly NoteRepository NoteRepository;
        
        public NoteRepositoryViewModelTest()
        {
            var mockIData = Substitute.For<IData>();
            this.NoteRepository = Substitute.ForPartsOf<NoteRepository>(mockIData);
        }
        
        [Fact]
        public void Constructor_WithParams_SetsValues()
        {
            var note1 = new Note(1);
            var note2 = new Note(2);
            var notes = new List<Note>()
            {
                note1,
                note2,
            };
            this.NoteRepository.Notes = new List<Note>(notes);
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.NoteRepository);
            var noteVMs = new List<NoteViewModel>()
            {
                new NoteViewModel(note1, this.NoteRepository),
                new NoteViewModel(note2, this.NoteRepository),
            };

            noteRepositoryViewModel.Notes.Should().BeEquivalentTo(noteVMs);
        }

        // Test the property change
        [Fact]
        public void RepositoryNotes_Changes_UpdatedNotes()
        {
            var note1 = new Note(1);
            var note2 = new Note(2);
            var note3 = new Note(3);
            var notes = new List<Note>()
            {
                note1,
                note2,
            };
            this.NoteRepository.Notes = new List<Note>(notes);
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.NoteRepository);
            var noteVMs = new List<NoteViewModel>()
            {
                new NoteViewModel(note1, this.NoteRepository),
                new NoteViewModel(note2, this.NoteRepository),
                new NoteViewModel(note3, this.NoteRepository),
            };
            
            this.NoteRepository.Save(note3);

            noteRepositoryViewModel.Notes.Should().BeEquivalentTo(noteVMs);
        }
    }
}