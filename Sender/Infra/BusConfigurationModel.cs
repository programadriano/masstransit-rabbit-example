using System.Reflection;

namespace Receiver.Infra
{
    public class BusConfiguration
    {
        public MQConnection[] Connection { get; set; }

        public MQConsumer[] Consumer { get; set; }

        public class MQConnection
        {
            public string[] Servers { get; set; }
            public ushort Port { get; set; }
            public string VirtualHost { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class MQConsumer
        {
            public int RetryLimit { get; set; }
            public int InitialInterval { get; set; }
            public int IntervalIncrement { get; set; }
            public int TrackingPeriod { get; set; }
            public int TripThreshold { get; set; }
            public int ActiveThreshold { get; set; }
            public int ResetInterval { get; set; }
            public string RabbitMQExchange { get; set; }
            public string RabbitMQQueue { get; set; }
        }
    }
}
