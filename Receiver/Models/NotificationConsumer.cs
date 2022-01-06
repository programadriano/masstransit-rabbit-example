using MassTransit;

namespace Receiver.Models
{
    public interface INotificationConsumer : IConsumer<NotificationBodyMessage> { }

    public class NotificationConsumer : INotificationConsumer
    {
        public Task Consume(ConsumeContext<NotificationBodyMessage> context)
        {
            throw new NotImplementedException();
        }
    }
}
