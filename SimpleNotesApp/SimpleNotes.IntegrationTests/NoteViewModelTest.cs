using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Shared;
using SimpleNotes.Models;
using SimpleNotes.ViewModels;
using Xunit;

namespace SimpleNotes.IntegrationTests
{
    public class NoteViewModelTest
    {
        private readonly NoteRepository mockNoteRepository;
        private readonly NoteDataService noteDataService;
        private readonly IData mockIData;
        
        public NoteViewModelTest()
        {
            this.mockIData = Substitute.For<IData>();
            this.noteDataService = Substitute.ForPartsOf<NoteDataService>(this.mockIData);
            this.mockNoteRepository = Substitute.ForPartsOf<NoteRepository>(this.noteDataService);
        }
        
        [Fact]
        public void SaveAsync_Called_SavesNotesToRepositoryAndSavesToLocalStorage()
        {
            var note = new Note(1);
            var noteRepository = Substitute.ForPartsOf<NoteRepository>(this.noteDataService);
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            var noteViewModel = new NoteViewModel(note, noteRepository);

            noteViewModel.SaveAsync();

            noteRepository.Notes.Should().BeEquivalentTo(new List<Note> { note, });
            this.mockIData.Received().SaveValueAsync("notes", serializedNotes);
        }
        
        [Fact]
        public void SaveEditsAsync_Called_SavesModifiedNoteToLocalStorage()
        {
            var note = new Note(1, "new title", "new description");
            var noteRepository = Substitute.ForPartsOf<NoteRepository>(this.noteDataService);
            noteRepository.Notes.Returns(new List<Note> { new(1) });
            var serializedNotes = "[{\"Id\":1,\"Title\":\"new title\",\"Description\":\"new description\"}]";
            this.mockIData.RetrieveValue("notes").Returns(serializedNotes);
            var noteViewModel = new NoteViewModel(note, noteRepository);

            noteViewModel.SaveEditsAsync();

            this.mockIData.Received().SaveValueAsync("notes", serializedNotes);
        }
        
        [Fact]
        public void DeleteAsync_Called_SavesModifiedNotesToLocalStorage()
        {
            var note = new Note(1);
            var noteRepository = Substitute.ForPartsOf<NoteRepository>(this.noteDataService);
            noteRepository.Notes.Returns(new List<Note> { new(1) });
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            var serializedNotes2 = "[]";
            this.mockIData.RetrieveValue("notes").Returns(serializedNotes);
            var noteViewModel = new NoteViewModel(note, noteRepository);

            noteViewModel.DeleteAsync();

            this.mockIData.Received().SaveValueAsync("notes", serializedNotes2);
        }
    }
}