
using GenericPublisher;
using RabbitMQ.Client;

ConnectionFactory connectionFactory = new ConnectionFactory()
{
    UserName = "adm",
    Password = "123",
    Port = 5672,
    HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST"),
    VirtualHost = "main"
};

using var connection = connectionFactory.CreateConnection();
using var model = connection.CreateModel();

string exchangeName = Environment.GetEnvironmentVariable("RABBITMQ_EXCHANGE");
string routingKey = Environment.GetEnvironmentVariable("RABBITMQ_ROUNTINGKEY");
int max = int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_MAX"));


for (int i = 0; i <= max; i++)
{
    var prop = model.CreateBasicProperties();
    prop.DeliveryMode = 2;
    prop.ContentType = "application/json";
    prop.Headers = new Dictionary<string, object>()
    {
        {"DATA", DateTime.UtcNow.ToString("U") }
    };

    model.ResilientSend(exchangeName, routingKey, prop, new { value = i, OS = Environment.OSVersion.ToString() });
}

Console.WriteLine("Awesome!");
