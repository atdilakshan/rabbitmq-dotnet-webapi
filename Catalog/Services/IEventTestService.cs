namespace Catalog.Services
{
    public interface IEventTestService
    {
        Task PublishViaQueue();
        Task PublishViaTopic();
        Task PublishViaFanOut();
    }
}
