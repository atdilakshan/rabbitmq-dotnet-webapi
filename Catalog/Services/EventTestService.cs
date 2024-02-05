using Common.EventBus.Events;
using Common.EventBus.Services;

namespace Catalog.Services
{
    public class EventTestService(IEventBus _eventBus) : IEventTestService
    {
        public Task PublishViaQueue()
        {
            TestEvent testEvent = new()
            {
                TestId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = "Test"
            };

            _eventBus.PublishViaQueue(testEvent);
            return Task.CompletedTask;
        }

        public Task PublishViaTopic()
        {
            TestEvent testEvent = new()
            {
                TestId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = "Test"
            };

            _eventBus.PublishViaTopic(testEvent);
            return Task.CompletedTask;
        }

        public Task PublishViaFanOut()
        {
            TestEvent testEvent = new()
            {
                TestId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = "Test"
            };

            _eventBus.PublishViaFanout(testEvent);
            return Task.CompletedTask;
        }
    }
}
