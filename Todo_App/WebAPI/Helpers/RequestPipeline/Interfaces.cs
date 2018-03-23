using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;

namespace WebAPI.RequestPipeline
{
    // public interface IRequest<out TResponse> { }

    // public interface IRequestHandler<TRequest, TResponse>
    //     where TRequest : IRequest<TResponse>
    // { }

    public interface IMessageAuthorizer<TRequest>
        where TRequest : class
    {
        void Evaluate(TRequest request);
    }

    public interface IMessageValidator<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<IEnumerable<Error>> Validate(TRequest message);
    }

}