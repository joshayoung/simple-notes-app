using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Specialized;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Extensions;
using NSubstitute.ReturnsExtensions;
using Shared;
using Xunit;

namespace SimpleNotes.Models.Tests
{
    public class NoteDataServiceTest
    {
        private readonly IData mockIData;

        public NoteDataServiceTest()
        {
            this.mockIData = Substitute.For<IData>();
        }
        
        [Fact]
        public void GetNotes_LocalStorageReturnsNull_ReturnsNewNotesList()
        {
            this.mockIData.Retrieve("notes").ReturnsNull();
            var noteDataService = new NoteDataService(this.mockIData);

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
            this.mockIData.Retrieve("notes").Returns("");
            var noteDataService = new NoteDataService(this.mockIData);

            var result = noteDataService.GetNotes();

            result.Should().BeEquivalentTo(new List<Note>());
        }
        
        [Fact]
        public void GetNotes_Called_ReturnsDeserializedListOfNotes()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            this.mockIData.Retrieve("notes").Returns(serializedNotes);
            var noteDataService = new NoteDataService(this.mockIData);

            var result = noteDataService.GetNotes();

            result.Should().BeEquivalentTo(new List<Note> { new Note(1)});
        }
        
        [Fact]
        public void SaveAsync_Called_ReturnsDeserializedListOfNotes()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null},{\"Id\":2,\"Title\":null,\"Description\":null}]";
            this.mockIData.Retrieve("notes").Returns(serializedNotes);
            var noteDataService = new NoteDataService(this.mockIData);
            var notes = new List<Note>()
            {
                new Note(1),
                new Note(2),
            };

            noteDataService.SaveAsync(notes);

            this.mockIData.Received().SaveAsync("notes", serializedNotes);
        }

        [Fact]
        public void SaveAsync_DataSaveThrowsAnError_ExceptionRethrown()
        {
            var notes = new List<Note> { new Note(1), new Note(2), };
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null},{\"Id\":2,\"Title\":null,\"Description\":null}]";
            this.mockIData.When(d => d.SaveAsync(Arg.Is("notes"), Arg.Is(serializedNotes)))
                .Throw(new Exception("error"));
            var noteDataService = new NoteDataService(this.mockIData);

            Func<Task> testAction = async () => await noteDataService.SaveAsync(notes);

            testAction.Should()
                      .ThrowAsync<Exception>()
                      .WithInnerExceptionExactly<Exception, Exception>()
                      .WithMessage("error");
        }
        
        [Fact]
        public void DeleteAsync_NoNotesFound_Returns()
        {
            this.mockIData.Retrieve("notes").ReturnsNull();
            var noteDataService = new NoteDataService(this.mockIData);

            noteDataService.DeleteAsync(new List<Note>(), new Note(1));

            this.mockIData.DidNotReceive().SaveAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void DeleteAsync_DeserializationReturnsNull_Returns()
        {
            this.mockIData.Retrieve("notes").Returns("");
            var noteDataService = new NoteDataService(this.mockIData);

            noteDataService.DeleteAsync(new List<Note>(), new Note(1));

            this.mockIData.DidNotReceive().SaveAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void DeleteAsync_NoteNotFound_Returns()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            this.mockIData.Retrieve("notes").Returns(serializedNotes);
            var noteDataService = new NoteDataService(this.mockIData);

            noteDataService.DeleteAsync(new List<Note>(), new Note(2));

            this.mockIData.DidNotReceive().SaveAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void DeleteAsync_Called_SavesWithNoteRemoved()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null},{\"Id\":2,\"Title\":null,\"Description\":null}]";
            var serializedNotes2 = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            this.mockIData.Retrieve("notes").Returns(serializedNotes);
            var noteDataService = new NoteDataService(this.mockIData);
            var notes = new List<Note>()
            {
                new Note(1),
                new Note(2),
            };

            noteDataService.DeleteAsync(notes, new Note(2));

            this.mockIData.Received().SaveAsync(Arg.Is<string>("notes"), Arg.Is<string>(serializedNotes2));
        }
    }
}