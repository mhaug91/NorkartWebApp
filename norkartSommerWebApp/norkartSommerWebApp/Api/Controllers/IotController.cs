
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
using Microsoft.Azure.Devices;
using Newtonsoft.Json.Linq;

namespace norkartSommerWebApp.Api.Controllers
{
    public class IotController : ApiController
    {


        // POST: api/Iot
        public async Task PostCloudToDeviceMsg([FromBody]string s)
        {
            await IotHub.Main(s);
            return;
        }


    }
}
