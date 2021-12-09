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
        
        // TODO: Add test for SaveEditsAsync
        
        // TODO: Add test for DeleteAsync
    }
}