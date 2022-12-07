using RabbitMQ.Client;

Console.WriteLine("Started: Data Capture Service.");

const string connectionUri = "amqp://guest:guest@localhost:5672";
const string exchangeName = "Exchange.DataCaptureService";
const string routingKey = "";

var factory = new ConnectionFactory();
factory.Uri = new Uri(connectionUri);

var connection = factory.CreateConnection();
var channel = connection.CreateModel();


const string infoMessage = "Please, enter your message. 'Q' to stop sending.";
Console.WriteLine(infoMessage);
var input = string.Empty;

while (input != "Q" && input != "q")
{
    input = Console.ReadLine();

    if (input == "Q" || input == "q")
    {
        break;
    }
    else 
    {
        if (!string.IsNullOrEmpty(input))
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            channel.BasicPublish(exchangeName, routingKey, null, bytes);
        }
        else
        {
            Console.WriteLine(infoMessage);
        }
    } 
}

Console.WriteLine("Terminating: Data Capture Service.");

channel.Close();
connection.Close();
