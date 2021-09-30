using System;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using SimpleNotes.Models;
using SimpleNotes.Storage;

namespace SimpleNotes
{
    public class Dependencies
    {
        private static IServiceCollection serviceCollection;
        private static IServiceProvider serviceProvider { get; set; }
        
        public static NotesRepository NotesRepository =>
            serviceProvider.GetService<NotesRepository>();

        public static void Init()
        {
            serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IData, Data>();
            serviceCollection.AddSingleton<NotesRepository>();
            
            serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}