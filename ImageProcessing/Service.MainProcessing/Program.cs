using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Started: Main Processing.");

const string connectionUri = "amqp://guest:guest@localhost:5672";
const string exchangeName = "Exchange.DataCaptureService";
const string routingKey = "";
const string queueName = "Queue.DataCaptureService";

var factory = new ConnectionFactory();
factory.Uri = new Uri(connectionUri);

var connection = factory.CreateConnection();
var channel = connection.CreateModel();
channel.ExchangeDeclare(exchangeName
    , ExchangeType.Fanout
    , true);

channel.QueueDeclare(queueName, true, false, false);
channel.QueueBind(queueName, exchangeName, routingKey);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, eventArgs) => 
{
    var message = System.Text.Encoding.UTF8.GetString(eventArgs.Body.ToArray());
    Console.WriteLine(message);
};

channel.BasicConsume(queueName, true, consumer);

Console.WriteLine("'Enter' to stop receiving.");
Console.ReadLine();

channel.Close();
connection.Close();
