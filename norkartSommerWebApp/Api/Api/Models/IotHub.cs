using System;
using System.Configuration;
using System.Linq;
using Microsoft.Azure.Devices;
using System.Threading.Tasks;
using System.Text;
using System.Diagnostics;

namespace Api.Models
{
    public class IotHub
    {

        private ServiceClient _serviceClient;
        
        private string _connectionString;
        public static string Feedback = "";

        private async Task SendCloudToDeviceMessageAsync(string message)
        {
            try
            {
                var registryManager = RegistryManager.CreateFromConnectionString(_connectionString);
                var devices = await registryManager.GetDevicesAsync(100);
                
                foreach (var device in devices)
                {
                    var commandMessage = new Message(Encoding.ASCII.GetBytes(message)) { Ack = DeliveryAcknowledgement.Full };
                    try
                    {
                        
                        if (device.Status.ToString() != "Disabled")
                        {
                            await _serviceClient.SendAsync(device.Id, commandMessage);
                        }
                        
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
                await registryManager.CloseAsync();

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                
            }
           
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

            //ReceiveFeedbackAsync(hub._serviceClient);

        }

    }
}