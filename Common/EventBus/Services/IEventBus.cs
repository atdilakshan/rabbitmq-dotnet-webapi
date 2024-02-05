using Common.EventBus.Handler;
using Common.EventBus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EventBus.Services
{
    public interface IEventBus
    {
        void PublishViaQueue<TEvent>(TEvent @event) where TEvent : IntegrationEvent;
        void SubscribeViaQueue<TEvent, THandler>() where TEvent : IntegrationEvent where THandler : IIntegrationEventHandler<TEvent>;
        void PublishViaTopic<TEvent>(TEvent @event) where TEvent : IntegrationEvent;
        void SubscribeViaTopic<TEvent, THandler>() where TEvent : IntegrationEvent where THandler : IIntegrationEventHandler<TEvent>;
        void PublishViaFanout<TEvent>(TEvent @event) where TEvent : IntegrationEvent;
        void SubscribeViaFanout<TEvent, THandler>() where TEvent : IntegrationEvent where THandler : IIntegrationEventHandler<TEvent>;
    }
}
