using System.Collections.Generic;
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
    public class TodosRequestHandler
        : IRequestHandler<Models.Requests.AlleTodos, IEnumerable<Models.Responses.Todo>>
    {
        private readonly IMapper mapper;
        private readonly DataContext db;

        public TodosRequestHandler(
            IMapper mapper,
            DataContext db)
        {
            this.mapper = mapper;
            this.db = db;
        }

        public async Task<IEnumerable<Models.Responses.Todo>> Handle(Models.Requests.AlleTodos request, CancellationToken cancellationToken)
        {
            var data = await this.db.Todos
                .Include(x => x.Typ)
                .OrderByDescending(x => x.ErstellZeitpunkt)
                .ToListAsync();

            return data.Select(x =>
            {
                var f = this.mapper.CreateFrom<Models.Responses.Todo>(x);
                f.Art = x.Typ.Name;

                return f;
            });
        }
    }
}
