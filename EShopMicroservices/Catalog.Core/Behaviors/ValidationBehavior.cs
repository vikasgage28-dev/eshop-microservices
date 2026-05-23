using FluentValidation;
using MediatR;

namespace Catalog.Core.Behaviors
{
    /// <summary>
    /// MediatR Pipeline Behavior — runs BEFORE every command handler!
    /// If any validator fails → throws ValidationException → never reaches handler!
    /// No validation code needed in controllers — fully automatic! ✅
    /// </summary>
    public class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest                          request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken                 cancellationToken)
        {
            // No validator registered for this request type → skip validation!
            if (!_validators.Any())
                return await next();

            // Run ALL validators in parallel
            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f is not null)
                .ToList();

            // Any failures → throw! Handler never runs!
            if (failures.Count != 0)
                throw new ValidationException(failures);

            // All valid → continue to the actual handler!
            return await next();
        }
    }
}
