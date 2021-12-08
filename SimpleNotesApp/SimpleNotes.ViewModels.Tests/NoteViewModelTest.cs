using System.ComponentModel;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
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
            var noteDataService = Substitute.ForPartsOf<NoteDataService>(mockIData);
            this.mockNoteRepository = Substitute.ForPartsOf<NoteRepository>(noteDataService);
        }
        
        #region Setter_Tests
        [Fact]
        public void NoteAction_Set_CorrectValue()
        {
            var note = new Note(1, "title", "description");
            var noteViewModel = new NoteViewModel(note, this.mockNoteRepository)
            {
                NoteAction = NoteActionType.AddNote
            };

            noteViewModel.NoteAction.Should().Be(NoteActionType.AddNote);
        }
        
        [Fact]
        public void Title_Set_CorrectValue()
        {
            var note = new Note(1, "title", "description");
            var noteViewModel = new NoteViewModel(note, this.mockNoteRepository)
            {
                Title = "new title"
            };

            noteViewModel.Title.Should().Be("new title");
        }
        
        [Fact]
        public void Description_Set_CorrectValue()
        {
            var note = new Note(1, "title", "description");
            var noteViewModel = new NoteViewModel(note, this.mockNoteRepository)
            {
                Description = "new description"
            };

            noteViewModel.Description.Should().Be("new description");
        }
        #endregion

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

            // TODO: Is there a better way to test this
            results.Should().BeEquivalentTo(newVm);
        }
        
        [Fact]
        public async Task SaveAsync_Called_CallsSaveOnRepository()
        {
            var note = new Note(1);
            var noteViewModel = new NoteViewModel(note, this.mockNoteRepository);
            this.mockNoteRepository.Configure().SaveAsync(note).Returns(Task.FromResult(1));

            await noteViewModel.SaveAsync();

            await this.mockNoteRepository.Received().SaveAsync(Arg.Is(note));
        }
        
        [Fact]
        public async Task DeleteAsync_Called_CallsDeleteOnRepository()
        {
            var note = new Note(1);
            var noteViewModel = new NoteViewModel(note, this.mockNoteRepository);
            this.mockNoteRepository.Configure().DeleteAsync(note).Returns(Task.FromResult);

            await noteViewModel.DeleteAsync();

            await this.mockNoteRepository.Received().DeleteAsync(Arg.Is(note));
        }
        
        [Fact]
        public async Task SaveEditsAsync_Called_CallsSaveEditsOnRepository()
        {
            var note = new Note(1);
            var noteViewModel = new NoteViewModel(note, this.mockNoteRepository);
            this.mockNoteRepository.Configure().SaveEditsAsync(note).Returns(Task.FromResult(1));
            
            await noteViewModel.SaveEditsAsync();

            await this.mockNoteRepository.Configure().Received().SaveEditsAsync(Arg.Is(note));
        }
    }
}