using Common.EventBus.Handler;
using Common.EventBus.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Common.EventBus.Services
{
    public class EventBus : IEventBus
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public EventBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        #region Queue

        public void PublishViaQueue<TEvent>(TEvent @event) where TEvent : IntegrationEvent
        {
            _channel.QueueDeclare(queue: @event.GetType().Name, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));

            _channel.BasicPublish(exchange: "", routingKey: @event.GetType().Name, basicProperties: null, body: messageBody);
            Console.WriteLine($"Queue Event Published: {typeof(TEvent).Name}-{@event.Id}");
        }

        public void SubscribeViaQueue<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            // declare queue
            var queueName = $"{typeof(TEvent).Name}_{Guid.NewGuid()}";
            _channel.QueueDeclare(typeof(TEvent).Name, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                Console.WriteLine($"Queue Event Subsribed: {typeof(TEvent).Name}");
                Console.WriteLine(queueName);

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var @event = JsonSerializer.Deserialize<TEvent>(message);

                var handler = (IIntegrationEventHandler<TEvent>)_serviceProvider.GetService(typeof(THandler));

                handler.Handle(@event);

                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: typeof(TEvent).Name, autoAck: false, consumer: consumer);
        }

        #endregion

        #region Topic

        public void PublishViaTopic<TEvent>(TEvent @event) where TEvent : IntegrationEvent
        {
            var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));

            _channel.BasicPublish(exchange: $"Topic-{typeof(TEvent).Name}", routingKey: typeof(TEvent).Name, basicProperties: null, body: messageBody);
            Console.WriteLine($"Topic Event Published: {typeof(TEvent).Name}-{@event.Id}");
        }

        public void SubscribeViaTopic<TEvent, THandler>() where TEvent : IntegrationEvent where THandler : IIntegrationEventHandler<TEvent>
        {
            // declare exchange
            _channel.ExchangeDeclare(exchange: $"Topic-{typeof(TEvent).Name}", type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);

            // declare queue
            var queueName = $"{typeof(TEvent).Name}_{Guid.NewGuid()}";
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: queueName, exchange: $"Topic-{typeof(TEvent).Name}", routingKey: typeof(TEvent).Name);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                Console.WriteLine($"Topic Event Subsribed: {typeof(TEvent).Name}");
                Console.WriteLine(queueName);

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var @event = JsonSerializer.Deserialize<TEvent>(message);

                var handler = (IIntegrationEventHandler<TEvent>)_serviceProvider.GetService(typeof(THandler));

                handler.Handle(@event);

                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        #endregion

        #region Fanout

        public void PublishViaFanout<TEvent>(TEvent @event) where TEvent : IntegrationEvent
        {
            // declare exchange
            _channel.ExchangeDeclare(exchange: typeof(TEvent).Name, type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);

            var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));

            _channel.BasicPublish(exchange: typeof(TEvent).Name, routingKey: "", basicProperties: null, body: messageBody);
            Console.WriteLine($"Fanout Event Published: {typeof(TEvent).Name}-{@event.Id}");
        }

        public void SubscribeViaFanout<TEvent, THandler>()
            where TEvent : IntegrationEvent
            where THandler : IIntegrationEventHandler<TEvent>
        {
            // declare exchange
            _channel.ExchangeDeclare(exchange: typeof(TEvent).Name, type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);

            // declare queue
            var queueName = $"{typeof(TEvent).Name}_{Guid.NewGuid()}";
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: queueName, exchange: typeof(TEvent).Name, routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                Console.WriteLine($"Fanout Event Subsribed: {typeof(TEvent).Name}");
                Console.WriteLine(queueName);

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var @event = JsonSerializer.Deserialize<TEvent>(message);

                var handler = (IIntegrationEventHandler<TEvent>)_serviceProvider.GetService(typeof(THandler));

                handler.Handle(@event);

                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            };
            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        #endregion
    }
}
