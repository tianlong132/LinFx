﻿using LinFx;
using LinFx.Extensions.RabbitMQ;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMqServiceCollectionExtensions
    {
        public static ILinFxBuilder AddRabbitMQ(this ILinFxBuilder builder, Action<RabbitMqOptions> optionsAction)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(optionsAction, nameof(optionsAction));

            var options = new RabbitMqOptions();
            optionsAction?.Invoke(options);
            builder.Services.Configure(optionsAction);

            builder.Services.AddSingleton(sp =>
            {
                return new ConnectionFactoryWrapper(new ConnectionConfiguration
                {
                    Host = options.Host,
                    UserName = options.UserName,
                    Password = options.Password,
                });
            });
            builder.Services.AddSingleton<IConnectionPool, DefaultConnectionPool>();
            builder.Services.AddSingleton<IChannelPool, DefaultChannelPool>();
            builder.Services.AddSingleton<IConsumerFactory, ConsumerFactory>();
            builder.Services.AddSingleton<IRabbitMqSerializer, DefaultRabbitMqSerializer>();
            builder.Services.AddTransient<Consumer>();

            return builder;
        }

        public static ILinFxBuilder AddRabbitMQPersistentConnection(this ILinFxBuilder builder, Action<RabbitMqOptions> optionsAction)
        {
            var options = new RabbitMqOptions();
            optionsAction?.Invoke(options);

            builder.Services.AddSingleton((Func<IServiceProvider, IRabbitMQPersistentConnection>)(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
                var factory = new ConnectionFactory
                {
                    HostName = options.Host,
                    UserName = options.UserName,
                    Password = options.Password,
                };
                return new DefaultRabbitMQPersistentConnection(factory, logger);
            }));

            return builder;
        }
    }
}
