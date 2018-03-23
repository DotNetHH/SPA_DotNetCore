using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Helpers;

namespace WebAPI.Handlers
{
    public class GetTodoArtenHandler
        : IRequestHandler<Models.Requests.GetTodoArten, IEnumerable<Models.Responses.TodoArt>>
    {
        private readonly IMapper mapper;
        private readonly DataContext db;

        public GetTodoArtenHandler(
            IMapper mapper,
            DataContext db)
        {
            this.mapper = mapper;
            this.db = db;
        }

        public async Task<IEnumerable<Models.Responses.TodoArt>> Handle(Models.Requests.GetTodoArten request, CancellationToken cancellationToken)
        {
            return await this.db.TodoArten
                .Select(x => this.mapper.CreateFrom<Models.Responses.TodoArt>(x))
                .ToListAsync();
        }
    }
}
