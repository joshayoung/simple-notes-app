using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Shared;
using Xunit;

namespace SimpleNotes.Models.Tests
{
    public class NoteDataServiceTest
    {
        private readonly IData data;

        public NoteDataServiceTest()
        {
            this.data = Substitute.For<IData>();
        }
        
        [Fact]
        public void GetNotes_LocalStorageReturnsNull_ReturnsNewNotesList()
        {
            this.data.Retrieve("notes").ReturnsNull();
            var noteDataService = new NoteDataService(this.data);

            var result = noteDataService.GetNotes();

            result.Should().BeEquivalentTo(new List<Note>());
        }
        
        // TODO: GetNotes - finish implementing logging
        // [Fact]
        // public void GetNotes_ExceptionThrown_LogsError()
        // {
        //     var serializedNotes = "[{\"Idddds\"}]";
        //     this.data.Retrieve("notes").Returns(serializedNotes);
        //     var noteDataService = new NoteDataService(this.data);
        //
        //     noteDataService.GetNotes();
        //
        //     // TODO: Add log assertion
        // }
        
        [Fact]
        public void GetNotes_DeserializationReturnsNull_ReturnsNewNotesList()
        {
            this.data.Retrieve("notes").Returns("");
            var noteDataService = new NoteDataService(this.data);

            var result = noteDataService.GetNotes();

            result.Should().BeEquivalentTo(new List<Note>());
        }
        
        [Fact]
        public void GetNotes_Called_ReturnsDeserializedListOfNotes()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            this.data.Retrieve("notes").Returns(serializedNotes);
            var noteDataService = new NoteDataService(this.data);

            var result = noteDataService.GetNotes();

            result.Should().BeEquivalentTo(new List<Note> { new Note(1)});
        }
        
        [Fact]
        public void SaveAsync_Called_ReturnsDeserializedListOfNotes()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null},{\"Id\":2,\"Title\":null,\"Description\":null}]";
            this.data.Retrieve("notes").Returns(serializedNotes);
            var noteDataService = new NoteDataService(this.data);
            var notes = new List<Note>()
            {
                new Note(1),
                new Note(2),
            };

            noteDataService.SaveAsync(notes);

            this.data.Received().SaveAsync("notes", serializedNotes);
        }
        
        [Fact]
        public void DeleteAsync_NoNotesFound_Returns()
        {
            this.data.Retrieve("notes").ReturnsNull();
            var noteDataService = new NoteDataService(this.data);

            noteDataService.DeleteAsync(new List<Note>(), new Note(1));

            this.data.DidNotReceive().SaveAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void DeleteAsync_DeserializationReturnsNull_Returns()
        {
            this.data.Retrieve("notes").Returns("");
            var noteDataService = new NoteDataService(this.data);

            noteDataService.DeleteAsync(new List<Note>(), new Note(1));

            this.data.DidNotReceive().SaveAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void DeleteAsync_NoteNotFound_Returns()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            this.data.Retrieve("notes").Returns(serializedNotes);
            var noteDataService = new NoteDataService(this.data);

            noteDataService.DeleteAsync(new List<Note>(), new Note(2));

            this.data.DidNotReceive().SaveAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void DeleteAsync_Called_SavesWithNoteRemoved()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null},{\"Id\":2,\"Title\":null,\"Description\":null}]";
            var serializedNotes2 = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            this.data.Retrieve("notes").Returns(serializedNotes);
            var noteDataService = new NoteDataService(this.data);
            var notes = new List<Note>()
            {
                new Note(1),
                new Note(2),
            };

            noteDataService.DeleteAsync(notes, new Note(2));

            this.data.Received().SaveAsync(Arg.Is<string>("notes"), Arg.Is<string>(serializedNotes2));
        }
    }
}