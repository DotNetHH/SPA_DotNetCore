using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using WebAPI.Data;
using WebAPI.Helpers;
using WebAPI.Models.Requests;

namespace WebAPI.Handlers
{
    public class TodoAnlegenHandler
        : IRequestHandler<TodoAnlegen, Models.Responses.Todo>
    {
        private readonly IMapper mapper;
        private readonly DataContext db;

        public TodoAnlegenHandler(
            IMapper mapper,
            DataContext db)
        {
            this.mapper = mapper;
            this.db = db;
        }

        public async Task<Models.Responses.Todo> Handle(TodoAnlegen request, CancellationToken cancellationToken)
        {
            var dbTodo = new Data.Todo
            {
                ErstellZeitpunkt = DateTime.UtcNow,
                Typ = this.db.TodoArten.SingleOrDefault(x => x.Name == "Eingang")
            };
            this.mapper.CopyProperties(request, dbTodo);
            this.db.Add(dbTodo);

            await this.db.SaveChangesAsync();

            var response = new Models.Responses.Todo
            {
                Art = "Eingang",
            };
            this.mapper.CopyProperties(dbTodo, response);

            return response;
        }
    }
}
