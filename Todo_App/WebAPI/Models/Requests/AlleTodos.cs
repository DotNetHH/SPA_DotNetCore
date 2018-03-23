using System.Collections.Generic;
using MediatR;

namespace WebAPI.Models.Requests
{
    public class AlleTodos : IRequest<IEnumerable<Responses.Todo>>
    {
        public const string Name = "alletodos";
    }
}
