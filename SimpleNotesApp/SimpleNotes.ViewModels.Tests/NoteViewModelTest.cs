using System.ComponentModel;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ClearExtensions;
using NSubstitute.Extensions;
using Shared;
using SimpleNotes.Models;
using Xunit;

namespace SimpleNotes.ViewModels.Tests
{
    public class NoteViewModelTest
    {
        private readonly NoteRepository mockNoteRepository;

        public NoteViewModelTest()
        {
            var mockIData = Substitute.For<IData>();
            this.mockNoteRepository = Substitute.ForPartsOf<NoteRepository>(mockIData);
        }
        
        [Fact]
        public void Constructor_Params_Assignment()
        {
            var id = 1;
            var title = "title";
            var description = "description";
            var note = new Note(id, title, description);
            
            var noteViewModel = new NoteViewModel(note, this.mockNoteRepository);

            noteViewModel.NoteAction.Should().BeNull();
            noteViewModel.Id.Should().Be(id);
            noteViewModel.Title.Should().Be(title);
            noteViewModel.Description.Should().Be(description);
        }

        #region Property_Changes
        [Fact]
        public void NoteTitle_Changes_PropertyChangedEvent()
        {
            var noteMock = Substitute.ForPartsOf<Note>(1, "title", "description");
            var noteViewModelMonitored = new NoteViewModel(noteMock, this.mockNoteRepository).Monitor();
            
            noteMock.Configure().PropertyChanged += Raise.Event<PropertyChangedEventHandler>(this, new PropertyChangedEventArgs(nameof(Note.Title)));

            noteViewModelMonitored.Should()
                         .Raise("PropertyChanged")
                         .WithSender(noteViewModelMonitored.Subject)
                         .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == nameof(NoteViewModel.Title));
        }
        
        [Fact]
        public void NoteDescription_Changes_PropertyChangedEvent()
        {
            var noteMock = Substitute.ForPartsOf<Note>(1, "title", "description");
            var noteViewModelMonitored = new NoteViewModel(noteMock, this.mockNoteRepository).Monitor();
            
            noteMock.Configure().PropertyChanged += Raise.Event<PropertyChangedEventHandler>(this, new PropertyChangedEventArgs(nameof(Note.Description)));

            noteViewModelMonitored.Should()
                         .Raise("PropertyChanged")
                         .WithSender(noteViewModelMonitored.Subject)
                         .WithArgs<PropertyChangedEventArgs>(args => args.PropertyName == nameof(NoteViewModel.Description));
        }
        #endregion

        [Fact]
        public void EditNoteCopy_Called_ReturnsNewVMWithModelData()
        {
            var note = new Note(1);
            var newNote = new Note(note.Id, note.Title, note.Description);
            var noteViewModel = new NoteViewModel(note, this.mockNoteRepository);
            var newVm = new NoteViewModel(newNote, this.mockNoteRepository);

            var results = noteViewModel.EditNoteCopy();

            results.Should().BeEquivalentTo(newVm);
            results.Should().NotBeSameAs(noteViewModel);
        }
        
        [Fact]
        public async Task SaveAsync_Called_CallsSaveOnRepository()
        {
            var note = new Note(1);
            var noteViewModel = new NoteViewModel(note, this.mockNoteRepository);
            this.mockNoteRepository.Configure().Save(note).Returns(Task.FromResult(1));

            await noteViewModel.SaveAsync();

            await this.mockNoteRepository.Received().Save(Arg.Is(note));
        }
        
        [Fact]
        public async Task DeleteAsync_Called_CallsDeleteOnRepository()
        {
            var note = new Note(1);
            var noteViewModel = new NoteViewModel(note, this.mockNoteRepository);
            this.mockNoteRepository.Configure().Delete(note).Returns(Task.FromResult);

            await noteViewModel.DeleteAsync();

            await this.mockNoteRepository.Received().Delete(Arg.Is(note));
        }
        
        [Fact]
        public async Task SaveEditsAsync_Called_CallsSaveEditsOnRepository()
        {
            var note = new Note(1);
            var noteViewModel = new NoteViewModel(note, this.mockNoteRepository);
            this.mockNoteRepository.Configure().SaveEdits(note).Returns(Task.FromResult(1));
            
            await noteViewModel.SaveEditsAsync();

            await this.mockNoteRepository.Configure().Received().SaveEdits(Arg.Is(note));
        }
    }
}