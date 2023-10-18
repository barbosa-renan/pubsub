using RabbitMQ.Client;

namespace GenericPublisher
{
    public static class Extensions
    {
        public static void ResilientSend(this IModel model, string exchangeName, string routingKey, IBasicProperties prop, object objectToSend)
        {
            if (prop == null)
            {
                prop = model.CreateBasicProperties();
            }
            prop.DeliveryMode = 2;

            var serialized = System.Text.Json.JsonSerializer.Serialize(objectToSend);

            var bytesToSend = System.Text.Encoding.UTF8.GetBytes(serialized);

            if (string.IsNullOrEmpty(routingKey))
                routingKey = string.Empty;

            model.BasicPublish(exchangeName, routingKey, prop, bytesToSend);
        }
    }
}