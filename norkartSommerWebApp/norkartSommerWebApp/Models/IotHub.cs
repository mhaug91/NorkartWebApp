using System.Linq;
using Microsoft.Azure.Devices;
using System.Threading.Tasks;
using System.Text;
using System.Diagnostics;

namespace norkartSommerWebApp.Models
{
    public class IotHub
    {
        private ServiceClient _serviceClient;
        private string _connectionString;
        public static string Feedback = "";

        private async Task SendCloudToDeviceMessageAsync(string message)
        {
            Debug.WriteLine("Message: " + message);
            var commandMessage = new Message(Encoding.ASCII.GetBytes(message)) {Ack = DeliveryAcknowledgement.Full};
            await _serviceClient.SendAsync("Pi1", commandMessage);
        }


        private static async Task ReceiveFeedbackAsync(ServiceClient serviceClient)
        {
            var feedbackReceiver = serviceClient.GetFeedbackReceiver();

            while (true)
            {
                var feedbackBatch = await feedbackReceiver.ReceiveAsync();
                if (feedbackBatch == null)
                {
                    Debug.WriteLine("FEEDBACK: " + "Empty");
                }

                if (feedbackBatch == null) continue;
                Feedback = "Received feedback: {0}" + string.Join(", ", feedbackBatch.Records.Select(f => f.StatusCode));
                Debug.WriteLine("FEEDBACK: " + Feedback);
                await feedbackReceiver.CompleteAsync(feedbackBatch);
            }
            
        }

        public static async Task Main(string message)
        {

            var hub = new IotHub
            {
                _connectionString =
                    "HostName=norkartiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=Vwd858FmfIZRhe9qqPap4sl5B7joqhSyYSrdk9A1XGU="
            };

            hub._serviceClient = ServiceClient.CreateFromConnectionString(hub._connectionString);
            
            await hub.SendCloudToDeviceMessageAsync(message);

            ReceiveFeedbackAsync(hub._serviceClient);

        }
        
    }
}