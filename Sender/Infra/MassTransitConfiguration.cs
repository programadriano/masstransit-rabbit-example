using GreenPipes;
using MassTransit;
using MassTransit.MultiBus;

namespace Sender.Infra
{   

    public static class MessagingExtensions
    {
        public static void AddMessaging(this IServiceCollection services, BusConfiguration rabbitMQConfig)
        {          

            services.AddMassTransit(x =>
            {
                x.AddConsumer<QueueVideoConsumer>();

                var connectionString = "Endpoint=sb://sirius.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=vx6Nr2/gu6lKHbOlOsaRyvgaGqWyhev0/hPbEdbA1io=";

                x.AddBus
                 (registrationContext => Bus.Factory.CreateUsingAzureServiceBus
                                                    (configurator =>
                                                    {
                                                        configurator.Host(connectionString);

                                                        configurator.ReceiveEndpoint("myqueue", configurator =>
                                                        {
                                                            configurator.Consumer<QueueVideoConsumer>(registrationContext);
                                                        });
                                                    }
                                                        ));


            });


            services.AddMassTransitHostedService();
            //need to always start the bus, so it behaves correctly
             services.AddSingleton<IHostedService, BusHostedService>();
        }
    }
}


