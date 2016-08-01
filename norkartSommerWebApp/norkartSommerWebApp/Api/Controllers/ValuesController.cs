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

            /*if (value.ToObject<int>() == 0)
            {
                SendSmsModule.SendSms(value.ToString());
            }*/

            /*
             * Data is now sent to documentDB and Lake Storage via Azure Stream Analytics. These are therefore obsolete but have not been deleted
             * since they are examples of how it can be done via code.
             * 
             * await SendToDocDb.Main(s, "Telemetry");
             * await SendToLakeStorage.Main(s);
             */

            /*
             * IotHub sends data to Azure Stream Analytics which sends the data to documentDB & Lake Storage.
             */
            await SendToIotHub.Main(s);

            return;

        }
        

    }

    }