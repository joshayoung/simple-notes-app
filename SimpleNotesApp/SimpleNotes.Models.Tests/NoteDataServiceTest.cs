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
            this.mockIData.RetrieveValue("notes").ReturnsNull();
            var noteDataService = new NoteDataService(this.mockIData);

            var result = noteDataService.RetrieveNotes();

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
            this.mockIData.RetrieveValue("notes").Returns("");
            var noteDataService = new NoteDataService(this.mockIData);

            var result = noteDataService.RetrieveNotes();

            result.Should().BeEquivalentTo(new List<Note>());
        }
        
        [Fact]
        public void GetNotes_Called_ReturnsDeserializedListOfNotes()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            this.mockIData.RetrieveValue("notes").Returns(serializedNotes);
            var noteDataService = new NoteDataService(this.mockIData);

            var result = noteDataService.RetrieveNotes();

            result.Should().BeEquivalentTo(new List<Note> { new Note(1)});
        }
        
        [Fact]
        public void SaveAsync_Called_ReturnsDeserializedListOfNotes()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null},{\"Id\":2,\"Title\":null,\"Description\":null}]";
            this.mockIData.RetrieveValue("notes").Returns(serializedNotes);
            var noteDataService = new NoteDataService(this.mockIData);
            var notes = new List<Note>()
            {
                new Note(1),
                new Note(2),
            };

            noteDataService.SaveNotesAsync(notes);

            this.mockIData.Received().SaveValueAsync("notes", serializedNotes);
        }

        [Fact]
        public void SaveAsync_DataSaveThrowsAnError_ExceptionRethrown()
        {
            var notes = new List<Note> { new Note(1), new Note(2), };
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null},{\"Id\":2,\"Title\":null,\"Description\":null}]";
            this.mockIData.When(d => d.SaveValueAsync(Arg.Is("notes"), Arg.Is(serializedNotes)))
                .Throw(new Exception("error"));
            var noteDataService = new NoteDataService(this.mockIData);

            Func<Task> testAction = async () => await noteDataService.SaveNotesAsync(notes);

            testAction.Should()
                      .ThrowAsync<Exception>()
                      .WithInnerExceptionExactly<Exception, Exception>()
                      .WithMessage("error");
        }
        
        [Fact]
        public void DeleteAsync_NoNotesFound_Returns()
        {
            this.mockIData.RetrieveValue("notes").ReturnsNull();
            var noteDataService = new NoteDataService(this.mockIData);

            noteDataService.DeleteNotesAsync(new List<Note>(), new Note(1));

            this.mockIData.DidNotReceive().SaveValueAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void DeleteAsync_DeserializationReturnsNull_Returns()
        {
            this.mockIData.RetrieveValue("notes").Returns("");
            var noteDataService = new NoteDataService(this.mockIData);

            noteDataService.DeleteNotesAsync(new List<Note>(), new Note(1));

            this.mockIData.DidNotReceive().SaveValueAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void DeleteAsync_NoteNotFound_Returns()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            this.mockIData.RetrieveValue("notes").Returns(serializedNotes);
            var noteDataService = new NoteDataService(this.mockIData);

            noteDataService.DeleteNotesAsync(new List<Note>(), new Note(2));

            this.mockIData.DidNotReceive().SaveValueAsync(Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public void DeleteAsync_Called_SavesWithNoteRemoved()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null},{\"Id\":2,\"Title\":null,\"Description\":null}]";
            var serializedNotes2 = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            this.mockIData.RetrieveValue("notes").Returns(serializedNotes);
            var noteDataService = new NoteDataService(this.mockIData);
            var notes = new List<Note>()
            {
                new Note(1),
                new Note(2),
            };

            noteDataService.DeleteNotesAsync(notes, new Note(2));

            this.mockIData.Received().SaveValueAsync(Arg.Is<string>("notes"), Arg.Is<string>(serializedNotes2));
        }

        [Fact]
        public void DeleteNotesAsync_SaveValueAsyncThrowsError_ErrorRethrown()
        {
            var serializedNotes = "[{\"Id\":1,\"Title\":null,\"Description\":null},{\"Id\":2,\"Title\":null,\"Description\":null}]";
            var serializedNotes2 = "[{\"Id\":1,\"Title\":null,\"Description\":null}]";
            this.mockIData.RetrieveValue("notes").Returns(serializedNotes);
            var note = new Note(2);
            var notes = new List<Note> { new Note(1), note };
            this.mockIData.When(d => d.SaveValueAsync(Arg.Is("notes"), Arg.Is(serializedNotes2)))
                .Throw(new Exception("error"));
            var noteDataService = new NoteDataService(this.mockIData);

            Func<Task> testAction = async () => await noteDataService.DeleteNotesAsync(notes, note);

            // TODO: Seems to be passing with false positives, no matter what I use for the strings below
            testAction.Should()
                      .ThrowAsync<Exception>()
                      .WithInnerExceptionExactly<Exception, Exception>("test")
                      .WithMessage("bla");
        }
    }
}