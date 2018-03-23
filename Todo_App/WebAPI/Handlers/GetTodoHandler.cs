using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Helpers;

namespace WebAPI.Handlers
{
    public class GetTodoHandler
        : IRequestHandler<Models.Requests.GetTodo, Models.Responses.Todo>
    {
        private readonly IMapper mapper;
        private readonly DataContext db;

        public GetTodoHandler(
            IMapper mapper,
            DataContext db)
        {
            this.mapper = mapper;
            this.db = db;
        }

        public async Task<Models.Responses.Todo> Handle(Models.Requests.GetTodo request, CancellationToken cancellationToken)
        {
            var dbTodo = await this.db.Todos
                .Include(x => x.Typ)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            var todo = this.mapper.CreateFrom<Models.Responses.Todo>(dbTodo);
            todo.Art = dbTodo.Typ.Name;

            return todo;
        }
    }
}
