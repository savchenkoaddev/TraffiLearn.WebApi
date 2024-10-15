using FluentValidation;

namespace TraffiLearn.Application.Routes.Commands.Delete
{
    internal sealed class DeleteRouteCommandValidator
        : AbstractValidator<DeleteRouteCommand>
    {
        public DeleteRouteCommandValidator()
        {
            RuleFor(x => x.RouteId)
                .NotEmpty();
        }
    }
}
