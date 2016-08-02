using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;




namespace norkartSommerWebApp.Models
{


    public class SendToIotHub
    {

        static DeviceClient deviceClient;
        static string iotHubUri = "norkartiothub.azure-devices.net";
        static string deviceKey = "FIxa0952VFY3HLmlqERe+YZu8gFNBZPcdENX1rMKfvk=";


        private static async Task SendDeviceToCloudMessagesAsync(JObject obj)
        {
            double avgWindSpeed = 10; // m/s
            Random rand = new Random();            
                double currentWindSpeed = avgWindSpeed + rand.NextDouble() * 4 - 2;

                var telemetryDataPoint = new
                {
                    deviceId = "myFirstDevice",
                    windSpeed = currentWindSpeed
                };
            //var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                var messageString = JsonConvert.SerializeObject(obj);
                var message = new Message(Encoding.ASCII.GetBytes(messageString));

                await deviceClient.SendEventAsync(message);
                //Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

        }

        public static async Task Main(JObject obj)
        {
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("myFirstDevice", deviceKey));
            await SendDeviceToCloudMessagesAsync(obj);
        }
    }

  
}