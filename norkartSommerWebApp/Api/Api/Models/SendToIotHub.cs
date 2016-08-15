using System;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;




namespace Api.Models
{


    public class SendToIotHub
    {

        static DeviceClient deviceClient;
        static string iotHubUri = "norkartiothub.azure-devices.net";
        static string deviceKey = "Q0oYkDTG5LfnBKP3WKgN64Ea6ODIb12PGEys/iflDpI=";


        private static async Task SendDeviceToCloudMessagesAsync(JObject obj)
        {
            //var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
            var messageString = JsonConvert.SerializeObject(obj);
            var message = new Microsoft.Azure.Devices.Client.Message(Encoding.ASCII.GetBytes(messageString));

            await deviceClient.SendEventAsync(message);
            //Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);

        }

        public static async Task Main(JObject obj)
        {

            //var name = obj.Property("id");
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("Api", deviceKey));
            await SendDeviceToCloudMessagesAsync(obj);
        }
    }


}