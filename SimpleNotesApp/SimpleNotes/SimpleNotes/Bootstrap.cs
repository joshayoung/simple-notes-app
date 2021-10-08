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

        public static NotesRepository? NotesRepository =>
            ServiceProvider?.GetRequiredService<NotesRepository>();

        private static IServiceProvider? ServiceProvider { get; set; }

        public static void Init()
        {
            serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IData, Data>();
            serviceCollection.AddSingleton<NotesRepository>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}