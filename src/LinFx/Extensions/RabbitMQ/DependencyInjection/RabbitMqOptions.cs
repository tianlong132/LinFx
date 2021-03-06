﻿namespace LinFx.Extensions.RabbitMQ
{
    public class RabbitMqOptions
    {
        public string ConnectionName { get; set; }

        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
    }
}
