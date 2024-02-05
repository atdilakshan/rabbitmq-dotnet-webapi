using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EventBus.Models
{
    public class IntegrationEvent
    {
        public Guid Id { get; set; }
        public DateTime OccurredAt { get; }

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            OccurredAt = DateTime.UtcNow;
        }
    }
}
