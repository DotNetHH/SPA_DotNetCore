using MediatR;

namespace WebAPI.Models.Requests
{
    public class TodoAnlegen : IRequest<Responses.Todo>
    {
        public const string Name = "todoanlegen";
        public string Titel { get; set; }
        public string Text { get; set; }
    }
}
