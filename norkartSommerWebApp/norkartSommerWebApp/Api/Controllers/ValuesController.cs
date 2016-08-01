using System.Web.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using norkartSommerWebApp.Models;
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
            //await SendToDocDb.Main(s, "Telemetry");
            //await SendToLakeStorage.Main(s);
            await SendToIotHub.Main(s);

            return;

        }
        

    }

    }