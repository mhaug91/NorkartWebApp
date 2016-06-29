using Microsoft.Azure.Devices.Client;
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
using System.Linq.Expressions;
using System.Threading.Tasks;
using norkartSommerWebApp.Models;
using Newtonsoft.Json;

namespace WebApiController.Controllers
{

    public class ValuesController : ApiController
    {

        // GET api/<controller>

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(string id)
        {
            return "A value";
        }

        // POST api/<controller>
        public void PostTempAndHum([FromBody]SendToDocDB.JsonValues s)
        {

            System.Diagnostics.Debug.WriteLine("APPBLOB EXISTS: " + s);
            SendToDocDB.Main(s, "Telemetry", "TempAndHum");






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


        }
        

    }

    }