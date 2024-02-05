using Common.EventBus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EventBus.Events
{
    public class TestEvent : IntegrationEvent
    {
        public Guid TestId { get; set; }
        public string Name { get; set; }
    }
}
