using FluentValidation;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrderListQueryValidator : AbstractValidator<GetOrdersListQuery>
    {

        public GetOrderListQueryValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("El username no puede ser vacio no manche XD")
                .MinimumLength(4).WithMessage("El username no puede ser menor de 4 caracteres XD")
                .MaximumLength(10);
        }

    }
}
