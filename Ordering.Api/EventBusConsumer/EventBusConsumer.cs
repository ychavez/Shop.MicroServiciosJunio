using EventBus.Messages.Events;
using MassTransit;

namespace Ordering.Api.EventBusConsumer
{
    public class EventBusConsumer : IConsumer<BasketCheckoutEvent>
    {
        public Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
