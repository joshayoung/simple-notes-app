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
        private readonly NoteRepository NoteRepositoryMock;
        
        public NoteRepositoryViewModelTest()
        {
            var iDataMock = Substitute.For<IData>();
            this.NoteRepositoryMock = Substitute.ForPartsOf<NoteRepository>(iDataMock);
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
            this.NoteRepositoryMock.Notes = new List<Note>(notes);
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.NoteRepositoryMock);
            var noteVMs = new List<NoteViewModel>()
            {
                new NoteViewModel(note1, this.NoteRepositoryMock),
                new NoteViewModel(note2, this.NoteRepositoryMock),
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
            this.NoteRepositoryMock.Notes = new List<Note>(notes);
            var noteRepositoryViewModel = new NoteRepositoryViewModel(this.NoteRepositoryMock);
            var noteVMs = new List<NoteViewModel>()
            {
                new NoteViewModel(note1, this.NoteRepositoryMock),
                new NoteViewModel(note2, this.NoteRepositoryMock),
                new NoteViewModel(note3, this.NoteRepositoryMock),
            };
            
            this.NoteRepositoryMock.Save(note3);

            noteRepositoryViewModel.Notes.Should().BeEquivalentTo(noteVMs);
        }
    }
}