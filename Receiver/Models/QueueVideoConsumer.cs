using Comum;
using MassTransit;


namespace Comum
{
    public class Video
    {
        public string Nome { get; set; }
        public string Caminho { get; set; }
    }
}

namespace Receiver.Models
{  

    public class QueueVideoConsumer : IConsumer<Video>
    {
        public Task Consume(ConsumeContext<Video> context)
        {
            var teste = context.Message;


            return Task.CompletedTask;
        }
    }
}
