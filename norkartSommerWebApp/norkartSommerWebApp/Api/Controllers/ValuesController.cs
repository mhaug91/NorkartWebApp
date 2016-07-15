
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WebApiController;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Configuration;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;
using norkartSommerWebApp.Models;
using Newtonsoft.Json;
using Microsoft.Azure.Devices;
using Newtonsoft.Json.Linq;

namespace WebApiController.Controllers
{

    public class ValuesController : ApiController
    {
        
        /*// GET api/<controller>

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(string id)
        {
            return "A value";
        }
        */


        // POST api/<controller>
        /*[ActionName("TempAndHum")]
        public void PostTempAndHum([FromBody]JObject s)
        {

            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
            telemetry.TrackTrace("Received Object In ¨PostTempAndHum¨ : " + s.ToString());
            System.Diagnostics.Debug.WriteLine("1");
            Console.WriteLine("hei");
            SendToDocDB.Main(s, "Telemetry");

            /*DeviceClient deviceClient;

            string iotHubUri = "norkartiothub.azure-devices.net";
            string deviceKey = "vQ6yFQMCW3mSx50SXmPX/PI9QHQOGCKqZip15QFo94E=";
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("Pi1", deviceKey));

            try
            {
                //  create Azure Storage
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=norkartstorageaccount;AccountKey=ywvq9KDDK/F1mqiz1NJ/vEj9lT7wuVrEqWtO1f3hi+i4vw9gOCvJG1rOjVKpjx/Ki1q6rVjG7uUalT+hWdXxCA==");


                //  create a blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                //  create a container 
                CloudBlobContainer container = blobClient.GetContainerReference("norkartstorageaccount");


                CloudAppendBlob appBlob = container.GetAppendBlobReference("Temperature&Humidity/" + DateTime.Now.ToString("yyyy-dd-M") + ".json");

                //CloudAppendBlob appBlob = container.GetAppendBlobReference("TestFile.txt");

                System.Diagnostics.Debug.WriteLine("APPBLOB EXISTS: " + appBlob.ExistsAsync().Result.ToString());
                if (appBlob.ExistsAsync().Result.ToString() != "True")
                {
                    appBlob.CreateOrReplaceAsync();
                }
                //  create a local file
                //StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"), CreationCollisionOption.GenerateUniqueName);

                //StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("TestFile.txt", CreationCollisionOption.GenerateUniqueName);


                var message = new Message(Encoding.ASCII.GetBytes(value));
                deviceClient.SendEventAsync(message);
                appBlob.AppendTextAsync(value + Environment.NewLine);


            }
            catch
            {
                //  return error
                System.Diagnostics.Debug.WriteLine("STORAGE ERROR");

            }
            */
        //        }





            


        public async Task PostAirQuality([FromBody]JObject s)
        {
            var value = s.Property("telemetry").Value;
            Debug.WriteLine(value);
            /*if (value.ToObject<int>() == 0)
            {
                SendSmsModule.SendSms(value.ToString());
            }*/
            await SendToDocDb.Main(s, "Telemetry");
            await SendToLakeStorage.Main(s);

            return;
        }

        


    }

    }