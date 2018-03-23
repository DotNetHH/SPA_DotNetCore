using System.Collections.Generic;
using MediatR;

namespace WebAPI.Models.Requests
{
    public class GetTodoArten : IRequest<IEnumerable<Responses.TodoArt>>
    {
        public const string Name = "gettodoarten";
    }
}
