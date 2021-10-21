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
        private readonly IData iData;
        private readonly NoteRepository NoteRepository;

        public NoteViewModelTest()
        {
            this.iData = Substitute.For<IData>();
            this.NoteRepository = Substitute.ForPartsOf<NoteRepository>(this.iData);
        }
        
        [Fact]
        public void Constructor_OnlyRequiredParams_Assignment()
        {
            var id = 1;
            var title = "title";
            var description = "description";
            var note = new Note(id, title, description);
            
            var noteViewModel = new NoteViewModel(note, this.NoteRepository);

            noteViewModel.NoteAction.Should().BeNull();
            noteViewModel.Id.Should().Be(id);
            noteViewModel.Title.Should().Be(title);
            noteViewModel.Description.Should().Be(description);
        }

        [Fact]
        public void Title_Changes_PropertyChangedEvent()
        {
            var note = new Note(1);
            var noteViewModel = new NoteViewModel(note, this.NoteRepository);
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
            var noteViewModel = new NoteViewModel(note, this.NoteRepository);
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

        [Fact]
        public void EditNoteCopy_Called_ReturnsNewVMWithModelData()
        {
            var note = new Note(1);
            var newNote = new Note(note.Id, note.Title, note.Description);
            var noteViewModel = new NoteViewModel(note, this.NoteRepository);
            var newVm = new NoteViewModel(newNote, this.NoteRepository);

            var results = noteViewModel.EditNoteCopy();

            results.Should().BeEquivalentTo(newVm);
            results.Should().NotBeSameAs(noteViewModel);
        }
        
        [Fact]
        public async Task SaveAsync_Called_CallsSaveOnRepository()
        {
            var note = new Note(1);
            var noteViewModel = new NoteViewModel(note, this.NoteRepository);
            this.NoteRepository.Configure().Save(note).Returns(Task.FromResult(1));

            await noteViewModel.SaveAsync();

            await this.NoteRepository.Received().Save(Arg.Is(note));
        }
        
        [Fact]
        public async Task DeleteAsync_Called_CallsSaveOnRepository()
        {
            var note = new Note(1);
            var noteViewModel = new NoteViewModel(note, this.NoteRepository);
            this.NoteRepository.Configure().Delete(note).Returns(Task.FromResult);

            await noteViewModel.DeleteAsync();

            await this.NoteRepository.Received().Delete(Arg.Is(note));
        }
        
        [Fact]
        public async Task SaveEditsAsync_Called_CallsSaveEditsOnRepository()
        {
            var note = new Note(1);
            var noteViewModel = new NoteViewModel(note, this.NoteRepository);
            this.NoteRepository.Configure().SaveEdits(note).Returns(Task.FromResult(1));
            
            await noteViewModel.SaveEditsAsync();

            await this.NoteRepository.Configure().Received().SaveEdits(note);
        }
    }
}