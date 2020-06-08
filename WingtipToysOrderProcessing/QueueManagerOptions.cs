using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WingtipToys.OrderManager
{
    public class QueueManagerOptions
    {
        public String QueueName { get; set; } = "rabbit-queue";

        public String ExchangeName { get; set; } = "EXCHANGE3";

        public String ChannelName { get; set; } = "rabbit-test";
    }
}
