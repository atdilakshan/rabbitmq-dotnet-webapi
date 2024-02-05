using Common.EventBus.Events;
using Common.EventBus.Handler;

namespace Notification.Events.Handlers
{
    public class EventTestHandler : IIntegrationEventHandler<TestEvent>
    {
        public Task Handle(TestEvent @event)
        {
            Console.WriteLine($"Received event: {@event.Id}");
            return Task.CompletedTask;
        }
    }
}
