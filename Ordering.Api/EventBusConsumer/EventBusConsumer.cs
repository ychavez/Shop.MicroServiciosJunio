using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Orders.Commands.Checkout;

namespace Ordering.Api.EventBusConsumer
{
    public class EventBusConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public EventBusConsumer(IMapper mapper, IMediator mediator )
        {
            this.mapper = mapper;
            this.mediator = mediator;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = mapper.Map<CheckoutOrderCommand>(context.Message);
            var result = await mediator.Send(command);
        }
    }
}
