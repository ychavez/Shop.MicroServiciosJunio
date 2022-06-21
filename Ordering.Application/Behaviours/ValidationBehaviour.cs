using FluentValidation;
using MediatR;

namespace Ordering.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            this.validators = validators;
        }
        
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (validators.Any())
            {
                ///validamos si es que existe alguna validacion
                var context = new ValidationContext<TRequest>(request);

                // vamos a ejecutar las validaciones correpondientes al contexto
                var valitarionResults =
                    await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                // vamos a ver si hubo problemas de validacion
                var failures = valitarionResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Any())
                    throw new ValidationException(failures);
            }
            return await next();
        }
    }
}
