using MediatR;

namespace WebAPI.Models.Requests
{
    public class TodoAendern : IRequest<Responses.Todo>
    {
        public const string Name = "todoaendern";

        public int Id { get; set; }
        public string Titel { get; set; }
        public string Text { get; set; }
        public string ArtName { get; set; }
    }
}
