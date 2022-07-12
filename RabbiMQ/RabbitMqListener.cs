using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Diagnostics;
using System;
using SendReportService.DB;
using SendReportService.Models;
using SendReportService.ReportSamples;

namespace SendReportService.RabbiMQ
{
	public class RabbitMqListener : BackgroundService
	{
		private IConnection _connection;
		private IModel _channel;
		private bool disposed = false;
		private ConnectDB ConnectDB = new ConnectDB("User ID=postgres;Password=k1t2i3f4;Host=localhost;Port=5432;Database=HighFive;");

		public RabbitMqListener()
		{
			// Не забудьте вынести значения "localhost" и "MyQueue"
			// в файл конфигурации
			var factory = new ConnectionFactory() { Uri = new Uri("amqps://rdkgouxx:z6v6gcrnpR-B5yE_KQYWK-A9xEGGiofn@rattlesnake.rmq.cloudamqp.com/rdkgouxx") };
			_connection = factory.CreateConnection();
			_channel = _connection.CreateModel();
			_channel.QueueDeclare(queue: "SendReport", durable: true, exclusive: false, autoDelete: false, arguments: null);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += (ch, ea) =>
			{
				var content = Encoding.UTF8.GetString(ea.Body.ToArray());

				// Каким-то образом обрабатываем полученное сообщение
				//Debug.WriteLine($"Получено сообщение: {content}");
				var mail = new MailData() { mailAdress = content};
				var rateData = ConnectDB.GetTopPlaces();
				rateData = ConnectDB.GetPlacesInfo(rateData);
				//var orgIDs = String.Join(",", rateData.Select(x => x.organization_id).ToList());
				//var placeData = ConnectDB.GetPlacesInfo(orgIDs);
				mail = CommomReport.SendReport(rateData, mail);
				SendMail.Send(mail);

				_channel.BasicAck(ea.DeliveryTag, false);

			};

			_channel.BasicConsume("SendReport", false, consumer);

			return Task.CompletedTask;
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					_channel.Close();
					_connection.Close();
					ConnectDB.Dispose();
					base.Dispose();
				}
				// освобождаем неуправляемые объекты
				disposed = true;
			}
		}
		public override void Dispose()
		{
			Dispose(true);
		}
	}
}
