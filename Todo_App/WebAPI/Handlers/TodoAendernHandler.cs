using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Helpers;
using WebAPI.Models.Requests;

namespace WebAPI.Handlers
{
    public class TodoAendernHandler
        : IRequestHandler<TodoAendern, Models.Responses.Todo>
    {
        private readonly IMapper mapper;
        private readonly DataContext db;

        public TodoAendernHandler(
            IMapper mapper,
            DataContext db)
        {
            this.mapper = mapper;
            this.db = db;
        }

        public async Task<Models.Responses.Todo> Handle(TodoAendern request, CancellationToken cancellationToken)
        {
            var dbTodo = await this.db.Todos
                .Include(x => x.Typ)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            this.mapper.CopyProperties(request, dbTodo);
            dbTodo.Typ = await this.db.TodoArten.FirstOrDefaultAsync(x => x.Name == request.ArtName);

            await this.db.SaveChangesAsync();

            return new Models.Responses.Todo();
        }
    }
}
