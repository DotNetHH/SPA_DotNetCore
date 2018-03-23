using System;
using Xunit;
using System.Linq;
using WebAPI.Controllers;
using WebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Tests.Helpers
{
    public class InitDb
    {
        [Fact(Skip="nur manuell")]
        public void DoIt()
        {
            var connectionString = "Data Source=.\\SQLEXPRESS;Database=Todos;Integrated Security=True;";
            var builder = new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(connectionString);

            using (var db = new DataContext(builder.Options))
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                AppDataSeed.Init(db);
            }

            Assert.True(true);
        }
    }
}
