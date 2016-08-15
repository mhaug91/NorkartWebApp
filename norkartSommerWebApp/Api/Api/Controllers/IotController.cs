using System.Web.Http;
using System.Threading.Tasks;
using Api.Models;

namespace norkartSommerWebApp.Api.Controllers
{
    public class IotController : ApiController
    {


        // POST: api/Iot
        public async Task PostCloudToDeviceMsg([FromBody]string s)
        {
            await IotHub.Main(s);
            SendSmsModule.SendSms(s);
            return;
        }


    }
}
