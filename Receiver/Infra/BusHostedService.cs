using MassTransit;

namespace Receiver.Infra
{
    public class BusHostedService
     : IHostedService
    {
        readonly IBusControl _busControl;

        public BusHostedService(
            IBusControl busControl)
        {
            _busControl = busControl;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _busControl.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync(cancellationToken);
        }
    }
}
