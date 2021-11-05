using System;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using SimpleNotes.Models;
using SimpleNotes.Storage;

namespace SimpleNotes
{
    public static class Bootstrap
    {
        private static IServiceCollection? serviceCollection;

        public static NoteRepository? NoteRepository =>
            ServiceProvider?.GetRequiredService<NoteRepository>();

        private static IServiceProvider? ServiceProvider { get; set; }

        public static void Init()
        {
            serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IData, Data>();
            serviceCollection.AddSingleton<NoteDataService>();
            serviceCollection.AddSingleton<NoteRepository>();
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}