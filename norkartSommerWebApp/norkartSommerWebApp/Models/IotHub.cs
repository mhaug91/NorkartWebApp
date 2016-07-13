using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Devices;
using System.Threading.Tasks;
using System.Text;
using System.Diagnostics;

namespace norkartSommerWebApp.Models
{
    public class IotHub
    {


        ServiceClient serviceClient;
        string connectionString;
        static string feedback = "";

        private async Task SendCloudToDeviceMessageAsync(string message)
        {
            Debug.WriteLine("Message: " + message);
            var commandMessage = new Message(Encoding.ASCII.GetBytes(message));
            commandMessage.Ack = DeliveryAcknowledgement.Full;
            await serviceClient.SendAsync("Pi1", commandMessage);
        }


        private static async Task ReceiveFeedbackAsync(ServiceClient serviceClient)
        {
            var feedbackReceiver = serviceClient.GetFeedbackReceiver();
            Debug.WriteLine("TEST");


            while (true)
            {
                Debug.WriteLine("TEST2");
                var feedbackBatch = await feedbackReceiver.ReceiveAsync();
                Debug.WriteLine("TEST3");
                if (feedbackBatch == null)
                {
                    Debug.WriteLine("FEEDBACK: " + "Empty");
                }

                feedback = "Received feedback: {0}" + string.Join(", ", feedbackBatch.Records.Select(f => f.StatusCode));
                Debug.WriteLine("FEEDBACK: " + feedback);
                await feedbackReceiver.CompleteAsync(feedbackBatch);
            }
            
        }

        public static async Task Main(string message)
        {

            IotHub hub = new IotHub();
            
            hub.connectionString = "HostName=norkartiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=Vwd858FmfIZRhe9qqPap4sl5B7joqhSyYSrdk9A1XGU=";
            hub.serviceClient = ServiceClient.CreateFromConnectionString(hub.connectionString);
            
            await hub.SendCloudToDeviceMessageAsync(message);

            ReceiveFeedbackAsync(hub.serviceClient);

        }
        
    }
}