using Comum;
using GreenPipes;
using MassTransit;
using MassTransit.MultiBus;
using Receiver.Models;

namespace Receiver.Infra
{
    public interface ISecondBus :
        IBus
    {
    }

    public interface IThirdBus :
        IBus
    {
    }


    public static class MessagingExtensions
    {
        public static void AddMessaging(this IServiceCollection services, BusConfiguration rabbitMQConfig)
        {
            services.AddMassTransit(x =>
            {

                x.AddConsumer<OptionExecutedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMQConfig.Connection[0].Servers[0],
                    rabbitMQConfig.Connection[0].Port,
                    rabbitMQConfig.Connection[0].VirtualHost,
                        h =>
                        {
                            h.Username(rabbitMQConfig.Connection[0].Username);
                            h.Password(rabbitMQConfig.Connection[0].Password);
                            h.UseCluster(c =>
                            {
                                rabbitMQConfig.Connection[0].Servers.ToList()
                                    .ForEach(server => c.Node(server));
                            });
                        }
                    );

                    cfg.ReceiveEndpoint(rabbitMQConfig.Consumer[0].RabbitMQQueue, e =>
                    {
                        e.UseRetry(r =>
                           r.Incremental(
                               rabbitMQConfig.Consumer[0].RetryLimit,
                               TimeSpan.FromSeconds(rabbitMQConfig.Consumer[0].InitialInterval),
                               TimeSpan.FromSeconds(rabbitMQConfig.Consumer[0].IntervalIncrement)
                           )
                       );

                        e.Bind(rabbitMQConfig.Consumer[0].RabbitMQExchange, x =>
                        {
                            e.ConcurrentMessageLimit = 1;
                            e.PrefetchCount = 100;
                            e.DiscardFaultedMessages();
                            e.ClearMessageDeserializers();
                            e.UseRawJsonSerializer();
                            e.ConfigureConsumer<OptionExecutedConsumer>(context);
                        });
                    });
                });
            });

            services.AddMassTransit<ISecondBus>(x =>
            {
                x.AddConsumer<NotificationConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMQConfig.Connection[1].Servers[0],
                    rabbitMQConfig.Connection[1].Port,
                    rabbitMQConfig.Connection[1].VirtualHost,
                        h =>
                        {
                            h.Username(rabbitMQConfig.Connection[1].Username);
                            h.Password(rabbitMQConfig.Connection[1].Password);
                            h.UseCluster(c =>
                            {
                                rabbitMQConfig.Connection[1].Servers.ToList()
                                    .ForEach(server => c.Node(server));
                            });
                        }
                    );

                    cfg.ReceiveEndpoint(rabbitMQConfig.Consumer[1].RabbitMQQueue, e =>
                    {
                        e.UseRetry(r =>
                           r.Incremental(
                               rabbitMQConfig.Consumer[1].RetryLimit,
                               TimeSpan.FromSeconds(rabbitMQConfig.Consumer[1].InitialInterval),
                               TimeSpan.FromSeconds(rabbitMQConfig.Consumer[1].IntervalIncrement)
                           )
                       );

                        e.Bind(rabbitMQConfig.Consumer[1].RabbitMQExchange, x =>
                        {
                            e.ConcurrentMessageLimit = 1;
                            e.PrefetchCount = 100;
                            e.DiscardFaultedMessages();
                            e.ConfigureConsumer<NotificationConsumer>(context);
                        });
                    });
                });
            });

            services.AddMassTransit<IThirdBus>(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumer<QueueVideoConsumer>();

                var connectionString = "Endpoint=sb://alphaxp.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=AMORAS_VERMELHAS";

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(connectionString);

                    cfg.UseServiceBusMessageScheduler();                

                    // Subscribe to OrderSubmitted directly on the topic, instead of configuring a queue
                    cfg.SubscriptionEndpoint<Video>("Sua Subscribe", e =>
                    {
                        e.ConfigureConsumer<QueueVideoConsumer>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });

            });

            services.AddMassTransitHostedService(true);

            ////need to always start the bus, so it behaves correctly
             services.AddSingleton<IHostedService, BusHostedService>();
        }
    }
}


