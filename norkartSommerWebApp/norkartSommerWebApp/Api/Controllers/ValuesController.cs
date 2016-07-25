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
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;
using norkartSommerWebApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApiController.Controllers
{

    public class ValuesController : ApiController
    {


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