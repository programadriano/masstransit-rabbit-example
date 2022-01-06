using MassTransit;

namespace Receiver.Models
{

    public interface IOptionExecutedConsumer : IConsumer<OptionExecuted>
    {
    }


    public class OptionExecutedConsumer : IOptionExecutedConsumer
    {

        private readonly ILogger<OptionExecutedConsumer> _logger;

        public OptionExecutedConsumer(ILogger<OptionExecutedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<OptionExecuted> context)
        {
            try
            {
                var option = context.Message;

                _logger.LogInformation($"Order received: {option}");

            }
            catch (Exception ex)
            {
                _logger.LogError("Error on try to consume order", ex);
            }

            return Task.CompletedTask;
        }
    }
}
