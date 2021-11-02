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
        private readonly NoteRepository NoteRepositoryMock;

        public NoteViewModelTest()
        {
            var iDataMock = Substitute.For<IData>();
            this.NoteRepositoryMock = Substitute.ForPartsOf<NoteRepository>(iDataMock);
        }
        
        [Fact]
        public void Constructor_Params_Assignment()
        {
            var id = 1;
            var title = "title";
            var description = "description";
            var note = new Note(id, title, description);
            
            var noteViewModel = new NoteViewModel(note, this.NoteRepositoryMock);

            noteViewModel.NoteAction.Should().BeNull();
            noteViewModel.Id.Should().Be(id);
            noteViewModel.Title.Should().Be(title);
            noteViewModel.Description.Should().Be(description);
        }

        #region Property_Changes
        [Fact]
        public void Title_Changes_PropertyChangedEvent()
        {
            var note = new Note(1);
            var noteViewModel = new NoteViewModel(note, this.NoteRepositoryMock);
            var titleWasChanged = false;
            noteViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(NoteViewModel.Title))
                {
                    titleWasChanged = true;
                }
            };

            noteViewModel.Title = "new title";

            titleWasChanged.Should().BeTrue();
        }
        
        [Fact]
        public void Description_Changes_PropertyChangedEvent()
        {
            var note = new Note(1);
            var noteViewModel = new NoteViewModel(note, this.NoteRepositoryMock);
            var descriptionWasChanged = false;
            noteViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(NoteViewModel.Description))
                {
                    descriptionWasChanged = true;
                }
            };

            noteViewModel.Description = "new description";

            descriptionWasChanged.Should().BeTrue();
        }
        #endregion

        [Fact]
        public void EditNoteCopy_Called_ReturnsNewVMWithModelData()
        {
            var note = new Note(1);
            var newNote = new Note(note.Id, note.Title, note.Description);
            var noteViewModel = new NoteViewModel(note, this.NoteRepositoryMock);
            var newVm = new NoteViewModel(newNote, this.NoteRepositoryMock);

            var results = noteViewModel.EditNoteCopy();

            results.Should().BeEquivalentTo(newVm);
            results.Should().NotBeSameAs(noteViewModel);
        }
        
        [Fact]
        public async Task SaveAsync_Called_CallsSaveOnRepository()
        {
            var note = new Note(1);
            var noteViewModel = new NoteViewModel(note, this.NoteRepositoryMock);
            this.NoteRepositoryMock.Configure().Save(note).Returns(Task.FromResult(1));

            await noteViewModel.SaveAsync();

            await this.NoteRepositoryMock.Received().Save(Arg.Is(note));
        }
        
        [Fact]
        public async Task DeleteAsync_Called_CallsDeleteOnRepository()
        {
            var note = new Note(1);
            var noteViewModel = new NoteViewModel(note, this.NoteRepositoryMock);
            this.NoteRepositoryMock.Configure().Delete(note).Returns(Task.FromResult);

            await noteViewModel.DeleteAsync();

            await this.NoteRepositoryMock.Received().Delete(Arg.Is(note));
        }
        
        [Fact]
        public async Task SaveEditsAsync_Called_CallsSaveEditsOnRepository()
        {
            var note = new Note(1);
            var noteViewModel = new NoteViewModel(note, this.NoteRepositoryMock);
            this.NoteRepositoryMock.Configure().SaveEdits(note).Returns(Task.FromResult(1));
            
            await noteViewModel.SaveEditsAsync();

            await this.NoteRepositoryMock.Configure().Received().SaveEdits(Arg.Is(note));
        }
    }
}