using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net.Security;
using System.Security.Authentication;
using System.Text;
using System.Threading;

namespace WingtipToys.OrderManager
{
    public class QueueManager
    {
        private ILogger<QueueManager> _logger;
        private IConfiguration Config { get; set; }
        private ConnectionFactory ConnectionFactory { get; set; }
        private IOptionsSnapshot<QueueManagerOptions> _options { get; set; } 

        public QueueManagerOptions Options
        {
            get { return _options.Value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueManager"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="configApp">The configuration application.</param>
        /// <param name="connectionFactory">The connection factory.</param>
        public QueueManager(ILogger<QueueManager> logger, IOptionsSnapshot<QueueManagerOptions> configApp, [FromServices] ConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
            SslOption opt = ConnectionFactory.Ssl;
            if (opt != null && opt.Enabled)
            {
                opt.Version = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12;

                // Only needed if want to disable certificate validations
                opt.AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateChainErrors |
                    SslPolicyErrors.RemoteCertificateNameMismatch | SslPolicyErrors.RemoteCertificateNotAvailable;
            }

            _options = configApp;
            _logger = logger;
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            try
            {                
                _logger?.LogInformation($"Starting Service");
                _logger?.LogInformation($"Logging Startup");

                //queueClient = new QueueClient(ServiceBusConnectionString, QueueName, ReceiveMode.PeekLock);

                _logger?.LogInformation($"Processing started");

                ReceiveMessages();

                _logger?.LogInformation($"Processing Stopped");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.ToString());
            }
        }

        // Receives messages from the queue in a loop
        /// <summary>
        /// Receives the messages.
        /// </summary>
        private void ReceiveMessages()
        {
            try
            {
                _logger?.LogInformation($"Starting Listener for {Options.QueueName}");
                

                using (IConnection connection = ConnectionFactory.CreateConnection())
                {
                    using (IModel channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(Options.ExchangeName, ExchangeType.Topic, false, true, null);
                        
                        var queueDeclareOk = channel.QueueDeclare(Options.QueueName, true, false, true, null);

                        EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (o, e) =>
                        {
                            string data = Encoding.ASCII.GetString(e.Body);
                            _logger?.LogInformation($"Received message: {data}");
                            Thread.Sleep(1000);
                            channel.BasicAck(e.DeliveryTag, true);
                        };

                        string consumerTag = channel.BasicConsume(consumer, Options.QueueName, false, "Queue", true, false);

                        channel.QueueBind(Options.QueueName, Options.ExchangeName, Options.ChannelName);

                        _logger?.LogInformation("Listening...");

                        Console.ReadLine();
                        _logger?.LogInformation("Listerner Stopped");

                        channel.QueueUnbind(Options.QueueName, Options.ExchangeName, Options.ChannelName, null);
                    }
                }
            }
            catch (Exception exception)
            {
                _logger?.LogInformation($"{DateTime.Now} > Exception: {exception.Message}");
            }
        }
    }
}
