using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Data
{
    public static class AppDataSeed
    {
        public static void Init(DataContext context)
        {
            // Seed bereits durchgeführt?
            if (context.TodoArten.Any()) return;

            context.TodoArten.AddRange(new[] {
                new TodoArt { Name = "Eingang" },
                new TodoArt { Name = "Information" },
                new TodoArt { Name = "Aufgabe" },
            });

            context.SaveChanges();
        }
    }
}
