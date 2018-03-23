using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Handlers;
using WebAPI.Helpers;
using Xunit;

namespace WebAPI.Tests.Handlers
{
    public class TodosRequestHandler_Tests
    {
        [Fact]
        public async void Sortiert_Todos_nach_Erstellzeitpunkt()
        {
            // Seed DB
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseInMemoryDatabase("test");
            var db = new DataContext(optionsBuilder.Options);

            var todoArt = new Data.TodoArt { Name = "ArtA" };
            db.Add(todoArt);
            db.SaveChanges();

            db.AddRange(
                new Data.Todo { Titel = "A", ErstellZeitpunkt = DateTimeOffset.Now, Typ = todoArt },
                new Data.Todo { Titel = "B", ErstellZeitpunkt = DateTimeOffset.Now.AddHours(1), Typ = todoArt }
            );
            db.SaveChanges();

            // Handler und Request
            var handler = new TodosRequestHandler(new Mapper(), db);
            var request = new Models.Requests.AlleTodos();

            // Action
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(result.First().Titel, "B");
        }

    }
}
