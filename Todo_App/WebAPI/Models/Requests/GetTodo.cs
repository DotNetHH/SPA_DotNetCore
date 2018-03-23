using MediatR;

namespace WebAPI.Models.Requests
{
    public class GetTodo : IRequest<Responses.Todo>
    {
        public const string Name = "gettodo";
        public int Id { get; set; }
    }
}
