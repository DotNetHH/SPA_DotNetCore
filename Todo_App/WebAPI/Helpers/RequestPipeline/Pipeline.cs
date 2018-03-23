using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace WebAPI.RequestPipeline
{
    public class Pipeline<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> inner;
        private readonly IMessageAuthorizer<TRequest> authorizer;
        private readonly IEnumerable<IMessageValidator<TRequest, TResponse>> validators;

        public Pipeline(
            IRequestHandler<TRequest, TResponse> inner,
            IMessageAuthorizer<TRequest> authorizer,
            IEnumerable<IMessageValidator<TRequest, TResponse>> validators)
        {
            this.inner = inner;
            this.authorizer = authorizer;
            this.validators = validators;
        }

        public Task<TResponse> Handle(TRequest message, CancellationToken cancellationToken)
        {
            if (this.authorizer != null)
                this.authorizer.Evaluate(message);

            var errors = this.validators
                .Select(v => v.Validate(message))
                .SelectMany(t => t.Result)
                .ToList();

            if (errors.Any())
                throw new ValidationException(errors);

            return inner.Handle(message, cancellationToken);
        }
    }
}
